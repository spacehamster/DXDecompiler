using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Dxil
{
	public class DxilChunk : DxilBaseChunk
	{
		public static DxilChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new DxilChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
