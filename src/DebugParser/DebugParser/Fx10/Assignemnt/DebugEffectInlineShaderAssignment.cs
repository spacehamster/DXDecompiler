using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectInlineShaderAssignment : DebugEffectAssignment
	{
		public string SODecl { get; private set; }
		public BytecodeContainer Shader { get; private set; }
		uint ShaderOffset;
		uint SODeclOffset;
		public new static DebugEffectInlineShaderAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectInlineShaderAssignment();
			var shaderOffset = result.ShaderOffset = assignmentReader.ReadUInt32("ShaderOffset");
			var SODeclOffset = result.SODeclOffset = assignmentReader.ReadUInt32("DODeclOffset");
			var shaderReader = reader.CopyAtOffset("ShaderReader", assignmentReader, (int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32("ShaderSize");
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes("Shader", (int)shaderSize));
			}
			var SODeclReader = reader.CopyAtOffset("SODeclReader", assignmentReader, (int)SODeclOffset);
			result.SODecl = SODeclReader.ReadString("SODecl");
			return result;
		}
	}
}
