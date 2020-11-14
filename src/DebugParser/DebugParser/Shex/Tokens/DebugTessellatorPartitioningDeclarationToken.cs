using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugTessellatorPartitioningDeclarationToken : DebugDeclarationToken
	{
		public TessellatorPartitioning Partitioning;

		public static DebugTessellatorPartitioningDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("Partitioning", token0.DecodeValue<TessellatorPartitioning>(11, 13));
			return new DebugTessellatorPartitioningDeclarationToken
			{
				Partitioning = token0.DecodeValue<TessellatorPartitioning>(11, 13)
			};
		}
	}
}
