using DXDecompiler.Chunks.Fx10;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectConstantAssignment : DebugEffectAssignment
	{
		public List<EffectScalarType> Types { get; private set; }
		public List<DebugNumber> Values { get; private set; }
		public DebugEffectConstantAssignment()
		{
			Types = new List<EffectScalarType>();
			Values = new List<DebugNumber>();
		}
		public new static DebugEffectConstantAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectConstantAssignment();
			var assignmentCount = assignmentReader.ReadUInt32("AssignmentCount");
			for (int i = 0; i < assignmentCount; i++)
			{
				result.Types.Add((EffectScalarType)assignmentReader.ReadUInt32($"Type{i}"));
				result.Values.Add(DebugNumber.Parse(assignmentReader));
			}
			return result;
		}
	}
}
