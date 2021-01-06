namespace DXDecompiler.DebugParser.Ildb
{
	public class DebugDebugNameChunk : DebugBytecodeChunk
	{
		public string Name { get; private set; }
		public static DebugDebugNameChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugDebugNameChunk();
			var flags = reader.ReadUInt16("Flags"); //Currently unused
			var nameLength = reader.ReadUInt16("NameLength");
			result.Name = reader.ReadString("Name");
			var padding = 4 - (nameLength + 1) % 4; //Aligned to 4 byte boundary
			if(padding > 0) reader.ReadBytes("Padding", padding);
			return result;
		}
	}
}
