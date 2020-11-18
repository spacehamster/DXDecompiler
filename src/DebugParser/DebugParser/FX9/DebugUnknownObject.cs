using System.Collections.Generic;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugUnknownObject
	{
		public List<uint> Data = new List<uint>();
		public static DebugUnknownObject Parse(DebugBytecodeReader variableReader, uint count)
		{
			var result = new DebugUnknownObject();
			for (int i = 0; i < count; i++)
			{
				result.Data.Add(variableReader.ReadUInt32($"UnknownValue{i}"));
			}
			return result;
		}
	}
}