using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugTechnique
	{
		public string Name { get; private set; }
		public uint NameOffset;
		public uint AnnotationCount;
		public uint PassCount;
		List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		List<DebugPass> Passes = new List<DebugPass>();
		public static DebugTechnique Parse(DebugBytecodeReader reader, DebugBytecodeReader techniqueReader)
		{
			var result = new DebugTechnique();
			result.NameOffset = techniqueReader.ReadUInt32("NameOffset");
			result.AnnotationCount = techniqueReader.ReadUInt32("AnnotationCount");
			result.PassCount = techniqueReader.ReadUInt32("PassCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				techniqueReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugAnnotation.Parse(reader, techniqueReader));
				techniqueReader.RemoveIndent();
			}
			for (int i = 0; i < result.PassCount; i++)
			{
				techniqueReader.AddIndent($"Pass {i}");
				result.Passes.Add(DebugPass.Parse(reader, techniqueReader));
				techniqueReader.RemoveIndent();
			}
			var nameReader = reader.CopyAtOffset("NameReader", techniqueReader, (int)result.NameOffset);
			result.Name = nameReader.TryReadString("Name");
			return result;
		}
	}
}