using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugGeometryShaderInputPrimitiveDeclarationToken : DebugDeclarationToken
	{
		public Primitive Primitive;
		public static DebugGeometryShaderInputPrimitiveDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("Primitive", token0.DecodeValue<Primitive>(11, 16));
			return new DebugGeometryShaderInputPrimitiveDeclarationToken
			{
				Primitive = token0.DecodeValue<Primitive>(11, 16)
			};
		}
	}
}
