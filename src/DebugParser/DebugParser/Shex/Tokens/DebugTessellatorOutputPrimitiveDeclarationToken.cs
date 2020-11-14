using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugTessellatorOutputPrimitiveDeclarationToken : DebugDeclarationToken
	{
		public TessellatorOutputPrimitive OutputPrimitive;

		public static DebugTessellatorOutputPrimitiveDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("OutputPrimitive", token0.DecodeValue<TessellatorOutputPrimitive>(11, 13));
			return new DebugTessellatorOutputPrimitiveDeclarationToken
			{
				OutputPrimitive = token0.DecodeValue<TessellatorOutputPrimitive>(11, 13)
			};
		}
	}
}
