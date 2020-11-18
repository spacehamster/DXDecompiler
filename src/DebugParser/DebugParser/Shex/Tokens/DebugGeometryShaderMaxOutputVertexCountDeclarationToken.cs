namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugGeometryShaderMaxOutputVertexCountDeclarationToken : DebugDeclarationToken
	{
		public uint MaxPrimitives;

		public static DebugGeometryShaderMaxOutputVertexCountDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugGeometryShaderMaxOutputVertexCountDeclarationToken
			{
				MaxPrimitives = reader.ReadUInt32("MaxPrimitives")
			};
		}
	}
}
