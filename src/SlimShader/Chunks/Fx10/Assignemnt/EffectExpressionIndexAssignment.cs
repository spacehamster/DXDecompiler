using SlimShader.Chunks.Fxlvm;
using SlimShader.Util;
using System;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectExpressionIndexAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public BytecodeContainer Shader { get; private set; }

		public new static EffectExpressionIndexAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectExpressionIndexAssignment();
			var arrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();

			var shaderOffset = assignmentReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(ArrayName);
			sb.Append("[");
			if (Shader != null)
			{
				var chunk = Shader.GetChunk<FxlcChunk>();
				sb.Append(chunk.ToString());
			}
			sb.Append("];");
			return sb.ToString();
		}
	}
}
