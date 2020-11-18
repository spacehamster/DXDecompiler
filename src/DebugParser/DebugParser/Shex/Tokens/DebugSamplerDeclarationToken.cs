using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugSamplerDeclarationToken : DebugDeclarationToken
	{
		public SamplerMode SamplerMode;
		public uint SpaceIndex;
		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static DebugSamplerDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("SamplerMode", token0.DecodeValue<SamplerMode>(11, 14));
			var operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var result = new DebugSamplerDeclarationToken
			{
				SamplerMode = token0.DecodeValue<SamplerMode>(11, 14),
				Operand = operand
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;

		}
	}
}
