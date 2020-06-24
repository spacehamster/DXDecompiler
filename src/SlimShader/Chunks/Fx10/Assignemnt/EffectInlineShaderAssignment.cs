using SlimShader.Util;
using System;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInlineShaderAssignment : EffectAssignment
	{
		public string SODecl { get; private set; }
		public BytecodeContainer Shader { get; private set; }

		public new static EffectInlineShaderAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectInlineShaderAssignment();
			var shaderOffset = assignmentReader.ReadUInt32();
			var SODeclOffset = assignmentReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			var SODeclReader = reader.CopyAtOffset((int)SODeclOffset);
			result.SODecl = SODeclReader.ReadString();
			return result;
		}
		public override string ToString()
		{
			if(Shader == null)
			{
				return string.Format("{0} = NULL;", MemberType);
			}
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("{0} = asm {{", MemberType));
			sb.Append("    ");
			var shaderText = Shader.ToString()
				.Replace(Environment.NewLine, $"{Environment.NewLine}    ");
			sb.AppendLine(shaderText);
			sb.Append("};");
			return sb.ToString();
		}
	}
}
