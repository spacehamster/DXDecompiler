using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Ildb
{
	public class DebugNameChunk : BytecodeChunk
	{
		public string Name { get; private set; }
		public static DebugNameChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new DebugNameChunk();
			var flags = reader.ReadUInt16();
			var nameLength = reader.ReadUInt16();
			result.Name = reader.ReadString();
			return result;
		}
	}
}
