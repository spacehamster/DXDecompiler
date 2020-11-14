using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugIndexingRangeDeclarationToken : DebugDeclarationToken
	{
		public uint RegisterCount;

		public static DebugIndexingRangeDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var registerCount = reader.ReadUInt32("RegisterCount");
			return new DebugIndexingRangeDeclarationToken
			{
				Operand = operand,
				RegisterCount = registerCount
			};
		}
	}
}
