using DXDecompiler.DebugParser.DX9;

namespace DXDecompiler.DebugParser.Chunks.Fxlvm
{
	public class DebugCtabChunk : DebugBytecodeChunk
	{
		public DebugConstantTable ConstantTable { get; private set; }
		public static DebugBytecodeChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			//TODO: Merge Ctab parsing with DX9 Ctab
			var result = new DebugCtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader, (int)chunkSize);
			result.ConstantTable = DebugConstantTable.Parse(chunkReader);
			return result;
		}
	}
}
