namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugTempRegisterDeclarationToken : DebugDeclarationToken
	{
		public uint TempCount;

		public static DebugTempRegisterDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugTempRegisterDeclarationToken
			{
				TempCount = reader.ReadUInt32("TempCount")
			};
		}


	}
}
