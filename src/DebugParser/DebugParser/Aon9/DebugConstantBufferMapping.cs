using DXDecompiler.Chunks.Aon9;
using System;
using System.Diagnostics;

namespace DXDecompiler.DebugParser.Aon9
{
	public class DebugConstantBufferMapping
	{
		public ushort TargetReg { get; private set; }
		public ushort Buffer { get; private set; }
		public ushort StartReg { get; private set; }
		public ushort RegCount { get; private set; }
		public DataConversionType[] DataConversion = new DataConversionType[4];
		public static DebugConstantBufferMapping Parse(DebugBytecodeReader reader)
		{
			var result = new DebugConstantBufferMapping();
			result.Buffer = reader.ReadUInt16("Buffer");
			result.StartReg = reader.ReadUInt16("StartReg");
			result.RegCount = reader.ReadUInt16("RegCount");
			result.TargetReg = reader.ReadUInt16("TargetReg");
			for (int i = 0; i < 4; i++)
			{
				var type = reader.ReadEnum8<DataConversionType>("DataConversionType");
				result.DataConversion[i] = type;
			}
			return result;
		}
	}
}