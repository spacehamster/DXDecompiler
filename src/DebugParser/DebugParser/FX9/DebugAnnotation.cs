using System.Collections.Generic;

namespace SlimShader.DebugParser.FX9
{
	public class DebugAnnotation
	{
		public uint ParameterOffset;
		public uint ValueOffset;
		DebugParameter Parameter;
		List<Number> Values;
		public static DebugAnnotation Parse(DebugBytecodeReader reader, DebugBytecodeReader annotationReader)
		{
			var result = new DebugAnnotation();
			result.ParameterOffset = annotationReader.ReadUInt32("ParameterOffset");
			result.ValueOffset = annotationReader.ReadUInt32("ValueOffset");

			var parameterReader = reader.CopyAtOffset("AnnotationType", annotationReader, (int)result.ParameterOffset);
			result.Parameter = DebugParameter.Parse(reader, parameterReader);

			var valueReader = reader.CopyAtOffset("ValueReader", annotationReader, (int)result.ValueOffset);
			result.Values = result.Parameter.ReadParameterValue(valueReader);

			return result;
		}
	}
}