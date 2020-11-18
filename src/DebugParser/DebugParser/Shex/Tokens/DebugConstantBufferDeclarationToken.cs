
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugConstantBufferDeclarationToken : DebugDeclarationToken
	{

		public ConstantBufferAccessPattern AccessPattern;
		public uint ConstantBufferID => (uint)Operand.Indices[0].Value;
		public uint ConstantBufferSize => IsSM51 ?
			m_ConstantBufferSize :
			(uint)Operand.Indices[1].Value;
		public uint SpaceIndex { get; internal set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;
		private uint m_ConstantBufferSize;

		public static DebugConstantBufferDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("Token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("AccessPattern", token0.DecodeValue<ConstantBufferAccessPattern>(11, 11));
			var result = new DebugConstantBufferDeclarationToken
			{
				AccessPattern = token0.DecodeValue<ConstantBufferAccessPattern>(11, 11),
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
			if(version.IsSM51)
			{
				result.m_ConstantBufferSize = reader.ReadUInt32("SM51_ConstantBufferSize");
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
