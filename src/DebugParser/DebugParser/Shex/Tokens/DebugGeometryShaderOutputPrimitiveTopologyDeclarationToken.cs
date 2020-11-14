using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugGeometryShaderOutputPrimitiveTopologyDeclarationToken : DebugDeclarationToken
	{
		public PrimitiveTopology PrimitiveTopology;

		public static DebugGeometryShaderOutputPrimitiveTopologyDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("PriomitiveTopology", token0.DecodeValue<PrimitiveTopology>(11, 17));
			return new DebugGeometryShaderOutputPrimitiveTopologyDeclarationToken
			{
				PrimitiveTopology = token0.DecodeValue<PrimitiveTopology>(11, 17)
			};
		}
	}
}
