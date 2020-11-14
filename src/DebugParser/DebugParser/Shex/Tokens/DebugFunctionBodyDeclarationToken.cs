namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugFunctionBodyDeclarationToken : DebugDeclarationToken
	{
		public uint Identifier;

		public static DebugFunctionBodyDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugFunctionBodyDeclarationToken
			{
				Identifier = reader.ReadUInt32("Identifier")
			};
		}
	}
}
