namespace DXDecompiler.DebugParser.Dxil
{
	public class DebugDxilReflectionChunk : DebugDxilBaseChunk
	{
		public static DebugDxilReflectionChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugDxilReflectionChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
