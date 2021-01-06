using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Dxil
{
	public class DxilReflectionChunk : DxilBaseChunk
	{
		public static DxilReflectionChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new DxilReflectionChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
