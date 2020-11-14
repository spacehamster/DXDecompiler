using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugInputRegisterDeclarationToken : DebugDeclarationToken
	{
		public SystemValueName SystemValueName;
		public static DebugInputRegisterDeclarationToken Parse(DebugBytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32("token0");
			var opcodeType = token0.DecodeValue<OpcodeType>(0, 10);
			DebugOpcodeHeader.AddNotes(reader, token0);

			DebugInputRegisterDeclarationToken result;
			switch (opcodeType)
			{
				case OpcodeType.DclInput:
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
					result = new DebugInputRegisterDeclarationToken();
					break;
				case OpcodeType.DclInputPs:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					result = new DebugPixelShaderInputRegisterDeclarationToken
					{
						InterpolationMode = token0.DecodeValue<InterpolationMode>(11, 14)
					};
					reader.AddNote("InterpolationMode", token0.DecodeValue<InterpolationMode>(11, 14));
					break;
				default:
					throw new ParseException("Unrecognised opcode type: " + opcodeType);
			}

			result.Operand = DebugOperand.Parse(reader, opcodeType);

			switch (opcodeType)
			{
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					result.SystemValueName = DebugNameToken.Parse(reader);
					break;
			}

			return result;
		}
	}
}
