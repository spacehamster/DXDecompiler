using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Hash
{
	public class HashChunk : BytecodeChunk
	{
		const int DigestSize = 16;
		public HashFlags Flags { get; private set; }
		public byte[] Digest { get; private set; }
		public static HashChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new HashChunk()
			{
				Flags = (HashFlags)reader.ReadUInt32(),
				Digest = reader.ReadBytes(DigestSize)
			};
			return result;
		}
	}
}
