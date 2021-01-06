namespace DXDecompiler.DebugParser.Hash
{
	public class DebugHashChunk : DebugBytecodeChunk
	{
		const int DigestSize = 16;
		public uint Flags { get; private set; }
		public byte[] Digest { get; private set; }
		public static DebugHashChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugHashChunk();
			result.Flags = reader.ReadUInt32("Flags");
			result.Digest = reader.ReadBytes("Digest", DigestSize);
			return result;
		}
	}
}
