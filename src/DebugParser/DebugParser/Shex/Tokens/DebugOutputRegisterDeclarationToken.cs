
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugOutputRegisterDeclarationToken : DebugDeclarationToken
	{
		public SystemValueName SystemValueName;

		public static DebugOutputRegisterDeclarationToken Parse(DebugBytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32("token0");
			var opcodeType = token0.DecodeValue<OpcodeType>(0, 10);
			DebugOpcodeHeader.AddNotes(reader, token0);

			var result = new DebugOutputRegisterDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, opcodeType)
			};
			switch (opcodeType)
			{
				case OpcodeType.DclOutputSgv:
				case OpcodeType.DclOutputSiv:
					result.SystemValueName = DebugNameToken.Parse(reader);
					break;
			}

			return result;
		}
	}
}
