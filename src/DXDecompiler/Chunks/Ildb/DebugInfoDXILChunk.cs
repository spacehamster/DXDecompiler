using DXDecompiler.Chunks.Dxil;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Ildb
{
	public class DebugInfoDXILChunk : DxilBaseChunk
	{
		public static DebugInfoDXILChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new DebugInfoDXILChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
