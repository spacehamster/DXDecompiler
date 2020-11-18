using DXDecompiler.Util;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.DX9Shader.FX9
{
	/*
	 * Refer https://docs.microsoft.com/en-us/windows/win32/direct3d9/parameters
	 */
	public class Variable
	{
		public uint IsShared { get; private set; }
		public Parameter Parameter { get; private set; }
		public List<Number> DefaultValue = new List<Number>();
		public List<SamplerState> SamplerStates = new List<SamplerState>();
		public List<Annotation> Annotations = new List<Annotation>();

		public static Variable Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Variable();
			var parameterOffset = variableReader.ReadUInt32();
			var defaultValueOffset = variableReader.ReadUInt32();
			result.IsShared = variableReader.ReadUInt32();
			var annotationCount = variableReader.ReadUInt32();
			for(int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			var parameterReader = reader.CopyAtOffset((int)parameterOffset);
			result.Parameter = Parameter.Parse(reader, parameterReader);
			if(result.Parameter.ParameterType.IsSampler())
			{
				var elementCount = result.Parameter.ElementCount > 0 ? result.Parameter.ElementCount : 1;
				var samplerStateReader = reader.CopyAtOffset((int)defaultValueOffset);
				for(int i = 0; i < elementCount; i++)
				{
					result.SamplerStates.Add(SamplerState.Parse(reader, samplerStateReader));
				}
			}
			else
			{
				var valueReader = reader.CopyAtOffset((int)defaultValueOffset);
				result.DefaultValue = result.Parameter.ReadParameterValue(valueReader);
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			if(IsShared == 1)
			{
				sb.Append("shared ");
			}
			sb.Append(Parameter.GetTypeName());
			sb.Append(" ");
			sb.Append(Parameter.Name);
			if(!string.IsNullOrEmpty(Parameter.Semantic))
			{
				sb.Append(string.Format(" : {0}", Parameter.Semantic));
			}
			return sb.ToString();
		}
	}
}
