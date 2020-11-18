using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Chunks.Libf
{
	public class DebugLibraryParameterSignatureChunk : DebugBytecodeChunk
	{
		public List<DebugLibraryParameterDescription> Parameters { get; private set; }
		public DebugLibraryParameterSignatureChunk()
		{
			Parameters = new List<DebugLibraryParameterDescription>();
		}
		public static DebugLibraryParameterSignatureChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugLibraryParameterSignatureChunk();
			var chunkReader = reader.CopyAtCurrentPosition("LibraryParameterSignatureChunkReader", reader);
			var parameterCount = chunkReader.ReadUInt32("ParameterCount");
			var parameterOffset = chunkReader.ReadUInt32("ParameterOffset");
			for(int i = 0; i < parameterCount; i++)
			{
				var parameterReader = chunkReader.CopyAtOffset($"ParameterReader{i}", chunkReader, (int)parameterOffset + 12*4*i);
				result.Parameters.Add(DebugLibraryParameterDescription.Parse(reader, parameterReader));
			}
			return result;
		}
	}
}
