using DXDecompiler.Util;
using System;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectShaderData
	{
		public BytecodeContainer Shader { get; private set; }
		public static EffectShaderData Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectShaderData();
			var shaderOffset = variableReader.ReadUInt32();
			var bytecodeReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32();
			if(shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public override string ToString()
		{
			if(Shader == null) return "NULL";
			var sb = new StringBuilder();
			sb.AppendLine("    asm {");
			sb.Append("        ");
			var shaderText = Shader.ToString()
				.Replace(Environment.NewLine, $"{Environment.NewLine}        ");
			sb.AppendLine(shaderText);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
