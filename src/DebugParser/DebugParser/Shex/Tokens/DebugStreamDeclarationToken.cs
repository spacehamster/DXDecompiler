using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	class DebugStreamDeclarationToken : DebugDeclarationToken
	{
		public static DebugStreamDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("OpcodeType", token0.DecodeValue<OpcodeType>(0, 10));
			return new DebugStreamDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
		}
	}
}
