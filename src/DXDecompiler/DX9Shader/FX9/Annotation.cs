using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.FX9
{
	public class Annotation
	{
		public Parameter Parameter { get; private set; }
		public List<Number> Value { get; private set; }

		public static Annotation Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Annotation();
			var parameterOffset = variableReader.ReadUInt32();
			var valueOffset = variableReader.ReadUInt32();
			var parameterReader = reader.CopyAtOffset((int)parameterOffset);
			result.Parameter = Parameter.Parse(reader, parameterReader);

			var valueReader = reader.CopyAtOffset((int)valueOffset);
			result.Value = result.Parameter.ReadParameterValue(valueReader);

			return result;
		}
	}
}
