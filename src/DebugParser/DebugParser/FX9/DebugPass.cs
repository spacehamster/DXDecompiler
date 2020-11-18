using System.Collections.Generic;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugPass
	{
		public string Name { get; private set; }
		public uint NameOffset;
		public uint AnnotationCount;
		public uint AssignmentCount;
		List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		List<DebugAssignment> Assignments = new List<DebugAssignment>();
		public static DebugPass Parse(DebugBytecodeReader reader, DebugBytecodeReader passReader)
		{
			var result = new DebugPass();
			result.NameOffset = passReader.ReadUInt32("NameOffset");
			result.AnnotationCount = passReader.ReadUInt32("AnnoationCount");
			result.AssignmentCount = passReader.ReadUInt32("AssignmentCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				passReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugAnnotation.Parse(reader, passReader));
				passReader.RemoveIndent();
			}
			for (int i = 0; i < result.AssignmentCount; i++)
			{
				passReader.AddIndent($"Assignment {i}");
				result.Assignments.Add(DebugAssignment.Parse(reader, passReader));
				passReader.RemoveIndent();
			}
			var nameReader = reader.CopyAtOffset("NameReader", passReader, (int)result.NameOffset);
			result.Name = nameReader.TryReadString("Name");
			return result;
		}
	}
}