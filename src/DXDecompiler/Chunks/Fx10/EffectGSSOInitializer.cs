using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectGSSOInitializer
	{
		public BytecodeContainer Shader { get; private set; }
		public string SODecl { get; private set; }

		public static EffectGSSOInitializer Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectGSSOInitializer();
			var shaderOffset = variableReader.ReadUInt32();
			var SODeclOffset = variableReader.ReadUInt32();

			var bytecodeReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes((int)shaderSize));
			}

			var declReader = reader.CopyAtOffset((int)SODeclOffset);
			result.SODecl = declReader.ReadString();
			return result;
		}
		public override string ToString()
		{
			if(Shader == null) return "NULL";
			var sb = new StringBuilder();
			sb.AppendLine("asm {");
			sb.AppendLine(Shader.ToString());
			sb.Append("}");
			sb.AppendLine();
			sb.Append(string.Format("/* Stream out decl: \"{0}\" */", SODecl));
			return sb.ToString();
		}
	}
}
