using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	class DebugRawUnorderedAccessViewDeclarationToken : DebugUnorderedAccessViewDeclarationTokenBase
	{
		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static DebugRawUnorderedAccessViewDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("Coherency", token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16));
			reader.AddNote("IsRasterOrderedAccess", token0.DecodeValue<bool>(17, 17));
			reader.AddNote("OpcodeType", token0.DecodeValue<OpcodeType>(0, 10));

			var result = new DebugRawUnorderedAccessViewDeclarationToken
			{
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				IsRasterOrderedAccess = token0.DecodeValue<bool>(17, 17),
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
			if(version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
