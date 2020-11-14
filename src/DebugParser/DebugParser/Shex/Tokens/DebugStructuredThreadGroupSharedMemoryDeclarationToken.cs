using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugStructuredThreadGroupSharedMemoryDeclarationToken : DebugDeclarationToken
	{
		public uint StructByteStride;
		public uint StructCount;

		public static DebugStructuredThreadGroupSharedMemoryDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);

			return new DebugStructuredThreadGroupSharedMemoryDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				StructByteStride = reader.ReadUInt32("StructByteStride"),
				StructCount = reader.ReadUInt32("StructCount")
			};
		}
	}
}
