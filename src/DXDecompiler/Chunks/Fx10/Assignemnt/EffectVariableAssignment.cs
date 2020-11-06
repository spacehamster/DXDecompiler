using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10.Assignemnt
{
	public class EffectVariableAssignment : EffectAssignment
	{
		public string Value { get; private set; }

		public new static EffectVariableAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectVariableAssignment();
			result.Value = assignmentReader.ReadString();
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = ");
			sb.Append(Value);
			sb.Append(";");
			return sb.ToString();
		}
	}
}
