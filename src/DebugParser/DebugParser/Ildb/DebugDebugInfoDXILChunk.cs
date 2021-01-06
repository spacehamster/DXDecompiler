using DXDecompiler.DebugParser.Dxil;

namespace DXDecompiler.DebugParser.Ildb
{
	public class DebugDebugInfoDXILChunk : DebugDxilBaseChunk
	{
		public static DebugDebugInfoDXILChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugDebugInfoDXILChunk();
			result.ParseInstance(reader, chunkSize);
			return result;
		}
	}
}
