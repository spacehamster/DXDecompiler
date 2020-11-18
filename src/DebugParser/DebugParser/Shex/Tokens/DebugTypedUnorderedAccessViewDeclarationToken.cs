using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugTypedUnorderedAccessViewDeclarationToken : DebugUnorderedAccessViewDeclarationTokenBase
	{
		public ResourceDimension ResourceDimension;
		public DebugResourceReturnTypeToken ReturnType;

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static DebugTypedUnorderedAccessViewDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("ResourceDimension", token0.DecodeValue<ResourceDimension>(11, 15));
			reader.AddNote("Coherency", token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16));
			reader.AddNote("IsRasterOrderedAccess", token0.DecodeValue<bool>(17, 17));
			var result = new DebugTypedUnorderedAccessViewDeclarationToken
			{
				ResourceDimension = token0.DecodeValue<ResourceDimension>(11, 15),
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				IsRasterOrderedAccess = token0.DecodeValue<bool>(17, 17),
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				ReturnType = DebugResourceReturnTypeToken.Parse(reader)
			};
			if(version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
