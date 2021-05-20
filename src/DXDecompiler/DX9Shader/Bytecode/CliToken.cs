using DXDecompiler.Util;
using System;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.Bytecode
{
	/// <summary>
	/// The CliToken stores constant literal values for Fxlc operands
	/// </summary>
	public class CliToken
	{
		List<double> Numbers = new List<double>();
		public static CliToken Parse(BytecodeReader reader)
		{
			var result = new CliToken();
			var count = reader.ReadUInt32();
			for(int i = 0; i < count; i++)
			{
				result.Numbers.Add(reader.ReadDouble());
			}
			return result;
		}

		public string GetLiteral(uint index)
		{
			var value = Numbers[(int)index];
			// TODO: Upgrade to .NET Core 3.0 or above so we can avoid this explicit check
			// https://stackoverflow.com/a/62768234/4399840
			if(BitConverter.DoubleToInt64Bits(value) == BitConverter.DoubleToInt64Bits(-0.0))
			{
				return "-0";
			}
			return value.ToString();
		}

		public float GetLiteralAsFloat(uint index) => (float)Numbers[(int)index];
	}
}
