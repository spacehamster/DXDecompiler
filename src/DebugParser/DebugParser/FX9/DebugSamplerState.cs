using SlimShader.DebugParser;
using SlimShader.DebugParser.FX9;
using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugSamplerState
	{
		public List<DebugAssignment> Assignments;
		public DebugSamplerState()
		{
			Assignments = new List<DebugAssignment>();
		}
		public static DebugSamplerState Parse(DebugBytecodeReader reader, DebugBytecodeReader stateReader)
		{
			var result = new DebugSamplerState();
			var assignmentCount = stateReader.ReadUInt32("AssignmentCount");
			for (int j = 0; j < assignmentCount; j++)
			{
				stateReader.AddIndent($"Assignment {j}");
				result.Assignments.Add(DebugAssignment.Parse(reader, stateReader));
				stateReader.RemoveIndent();
			}
			return result;
		}
	}
}
