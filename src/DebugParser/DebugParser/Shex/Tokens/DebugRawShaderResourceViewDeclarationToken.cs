using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugRawShaderResourceViewDeclarationToken : DebugDeclarationToken
	{
		public uint SpaceIndex;

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static DebugRawShaderResourceViewDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var result = new DebugRawShaderResourceViewDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
