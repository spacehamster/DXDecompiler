using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectVariableIndexAssignment : EffectAssignment
	{
		public string ArrayName { get; private set; }
		public string VariableName { get; private set; }

		public new static EffectVariableIndexAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectVariableIndexAssignment();
			var arrayNameOffset = assignmentReader.ReadUInt32();
			var arrayNameReader = reader.CopyAtOffset((int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString();
			var variableNameOffset = assignmentReader.ReadUInt32();
			var variableNameReader = reader.CopyAtOffset((int)variableNameOffset);
			result.VariableName = variableNameReader.ReadString();
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(ArrayName);
			sb.Append("[");
			sb.Append(VariableName);
			sb.Append("];");
			return sb.ToString();
		}
	}
}