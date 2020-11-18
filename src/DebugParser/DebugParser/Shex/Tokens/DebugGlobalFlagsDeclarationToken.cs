using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugGlobalFlagsDeclarationToken : DebugDeclarationToken
	{
		public GlobalFlags Flags;
		public static DebugGlobalFlagsDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var result = new DebugGlobalFlagsDeclarationToken();
			result.Flags = token0.DecodeValue<GlobalFlags>(11, 18);
			reader.AddNote("Flags", result.Flags);
			return result;
		}
	}
}
