using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectVariableIndexAssignment : DebugEffectAssignment
	{
		public string ArrayName { get; private set; }
		public string VariableName { get; private set; }

		public uint VariableNameOffset;
		uint ArrayNameOffset;
		public new static DebugEffectVariableIndexAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectVariableIndexAssignment();
			var arrayNameOffset = result.ArrayNameOffset = assignmentReader.ReadUInt32("ArrayNameOffset");
			var arrayNameReader = reader.CopyAtOffset("ArrayNameReader", assignmentReader, (int)arrayNameOffset);
			result.ArrayName = arrayNameReader.ReadString("ArrayName");
			var variableNameOffset = result.VariableNameOffset = assignmentReader.ReadUInt32("VariableNameOffset");
			var variableNameReader = reader.CopyAtOffset("VariableNameReader", assignmentReader, (int)variableNameOffset);
			result.VariableName = variableNameReader.ReadString("VariableName");
			return result;
		}
	}
}