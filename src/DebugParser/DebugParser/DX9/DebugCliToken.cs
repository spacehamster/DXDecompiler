using DXDecompiler.DebugParser;
using System.Collections.Generic;
using System.Text;

namespace DebugParser.DebugParser.DX9
{
	public class DebugCliToken
	{
		List<double> Numbers = new List<double>();
		public static DebugCliToken Parse(DebugBytecodeReader reader)
		{
			var result = new DebugCliToken();
			var count = reader.ReadUInt32("Count");
			for(int i = 0; i < count; i++)
			{
				result.Numbers.Add(reader.ReadDouble($"Number {i}"));
			}
			return result;
		}
		public string GetLiteral(uint elementIndex, uint elementCount)
		{
			var sb = new StringBuilder();
			for(int i = 0; i < elementCount; i++)
			{
				var index = elementIndex + i;
				var number = Numbers[(int)index];
				sb.Append(number.ToString());
				if(i < elementCount - 1)
				{
					sb.Append(", ");
				}
			}
			return sb.ToString();
		}
	}
}
