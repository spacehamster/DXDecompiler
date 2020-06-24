using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class SamplerState
	{
		public List<Assignment> Assignments;
		public SamplerState()
		{
			Assignments = new List<Assignment>();
		}
		public static SamplerState Parse(BytecodeReader reader, BytecodeReader stateReader)
		{
			var result = new SamplerState();
			var assignmentCount = stateReader.ReadUInt32();
			for (int j = 0; j < assignmentCount; j++)
			{
				result.Assignments.Add(Assignment.Parse(reader, stateReader));
			}
			return result;
		}
	}
}
