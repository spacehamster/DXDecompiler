using SlimShader.DX9Shader.FX9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.FX9
{
	public class DebugAssignment
	{
		public StateType Type;
		public uint ArrayIndex;
		public uint TypeOffset;
		public uint ValueOffset;
		public DebugParameter Parameter;
		List<Number> Value;
		public static DebugAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugAssignment();
			result.Type = assignmentReader.ReadEnum32<StateType>("Type");
			result.ArrayIndex = assignmentReader.ReadUInt32("ArrayIndex");
			result.TypeOffset = assignmentReader.ReadUInt32("TypeOffset");
			result.ValueOffset = assignmentReader.ReadUInt32("ValueOffset");

			var variableReader = reader.CopyAtOffset("AssignmentTypeReader", assignmentReader, (int)result.TypeOffset);
			result.Parameter = DebugParameter.Parse(reader, variableReader);

			var valueReader = reader.CopyAtOffset("ValueReader", assignmentReader, (int)result.ValueOffset);
			result.Value = result.Parameter.ReadParameterValue(valueReader);
			return result;
		}
	}
}
