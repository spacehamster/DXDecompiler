using DXDecompiler.DX9Shader.FX9;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugVariable
	{
		public uint IsShared { get; private set; }
		public DebugParameter Parameter { get; private set; }
		public List<Number> Value { get; private set; }
		public List<DebugAnnotation> Annotations = new List<DebugAnnotation>();
		public List<DebugSamplerState> SamplerStates = new List<DebugSamplerState>();

		public uint AnnotationCount;
		public uint ParameterOffset;
		public uint DefaultValueOffset;
		public static DebugVariable Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugVariable();
			result.ParameterOffset = variableReader.ReadUInt32("ParameterOffset");
			result.DefaultValueOffset = variableReader.ReadUInt32("DefaultValueOffset");
			result.IsShared = variableReader.ReadUInt32("IsShared");
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for(int i = 0; i < result.AnnotationCount; i++)
			{
				variableReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugAnnotation.Parse(reader, variableReader));
				variableReader.RemoveIndent();
			}
			var paramterReader = reader.CopyAtOffset("ParameterReader", variableReader, (int)result.ParameterOffset);
			result.Parameter = DebugParameter.Parse(reader, paramterReader);

			if(result.Parameter.ParameterType.IsSampler())
			{
				var elementCount = result.Parameter.Elements > 0 ? result.Parameter.Elements : 1;
				var samplerStateReader = reader.CopyAtOffset("SamplerStateReader", variableReader, (int)result.DefaultValueOffset);
				for(int i = 0; i < elementCount; i++)
				{
					samplerStateReader.AddIndent($"SamplerState {i}");
					result.SamplerStates.Add(DebugSamplerState.Parse(reader, samplerStateReader));
					samplerStateReader.RemoveIndent();
				}
			}
			else
			{
				var valueReader = reader.CopyAtOffset("ValueReader", variableReader, (int)result.DefaultValueOffset);
				result.Value = result.Parameter.ReadParameterValue(valueReader);
			}
			return result;
		}
	}
}