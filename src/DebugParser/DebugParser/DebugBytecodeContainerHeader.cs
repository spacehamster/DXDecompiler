using SlimShader.Util;

namespace SlimShader.DebugParser
{
	public class DebugBytecodeContainerHeader
	{
		public uint FourCc { get; private set; }
		public uint[] UniqueKey { get; private set; }
		public uint One { get; private set; }
		public uint TotalSize { get; private set; }
		public uint ChunkCount { get; private set; }

		public static DebugBytecodeContainerHeader Parse(DebugBytecodeReader reader)
		{
			var fourCc = reader.ReadUInt32("fourCC");
			if (fourCc != "DXBC".ToFourCc())
				throw new ParseException($"Invalid FourCC 0x{fourCc.ToString("X2")}");

			var uniqueKey = new uint[4];
			uniqueKey[0] = reader.ReadUInt32("uniqueKey1");
			uniqueKey[1] = reader.ReadUInt32("uniqueKey2");
			uniqueKey[2] = reader.ReadUInt32("uniqueKey3");
			uniqueKey[3] = reader.ReadUInt32("uniqueKey4");

			return new DebugBytecodeContainerHeader
			{
				FourCc = fourCc,
				UniqueKey = uniqueKey,
				One = reader.ReadUInt32("One"),
				TotalSize = reader.ReadUInt32("TotalSize"),
				ChunkCount = reader.ReadUInt32("ChunkCount")
			};
		}
	}
}
