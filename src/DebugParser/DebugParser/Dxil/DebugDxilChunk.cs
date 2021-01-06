namespace DXDecompiler.DebugParser.Dxil
{
	public class DebugDxilChunk : DebugDxilBaseChunk
	{
		public static DebugDxilChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugDxilChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
