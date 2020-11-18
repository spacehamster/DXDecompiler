using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	class DebugStructuredShaderResourceViewDeclarationToken : DebugDeclarationToken
	{
		public uint StructByteStride;
		public uint SpaceIndex;

		public static DebugStructuredShaderResourceViewDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);

			var result = new DebugStructuredShaderResourceViewDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				StructByteStride = reader.ReadUInt32("StructByteStride")
			};
			if(version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
