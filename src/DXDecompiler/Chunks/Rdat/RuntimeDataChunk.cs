using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Rdat
{
	public class RuntimeDataChunk : BytecodeChunk
	{
		public byte[] RawData { get; private set; }
		public static RuntimeDataChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new RuntimeDataChunk();
			result.RawData = reader.ReadBytes((int)chunkSize);
			return result;
		}
	}
}
