using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.FX9
{
	public class Assignment
	{
		public StateType Type { get; private set; }
		public uint ArrayIndex { get; private set; }
		public Parameter Parameter { get; private set; }
		public List<Number> Value { get; private set; }

		public static Assignment Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{
			var result = new Assignment();
			result.Type = (StateType)shaderReader.ReadUInt32();
			result.ArrayIndex = shaderReader.ReadUInt32();
			var parameterOffset = shaderReader.ReadUInt32();
			var valueOffset = shaderReader.ReadUInt32();

			var parameterReader = reader.CopyAtOffset((int)parameterOffset);
			result.Parameter = Parameter.Parse(reader, parameterReader);

			var valueReader = reader.CopyAtOffset((int)valueOffset);
			result.Value = result.Parameter.ReadParameterValue(valueReader);

			return result;
		}
	}
}
