namespace SlimShader.DebugParser.Libf
{
	public class DebugLibfChunk : DebugBytecodeChunk
	{
		public DebugBytecodeContainer LibraryContainer;
		public static DebugLibfChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugLibfChunk();
			var chunkReader = reader.CopyAtCurrentPosition("LibfChunkReader", reader);
			result.LibraryContainer = new DebugBytecodeContainer(chunkReader);
			return result;
		}
	}
}
