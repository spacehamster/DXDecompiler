namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugImmediateConstantBufferDeclarationToken : DebugImmediateDeclarationToken
	{
		public DebugNumber[] Data;

		public static DebugImmediateConstantBufferDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("Token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var length = reader.ReadUInt32("Length") - 2;

			var result = new DebugImmediateConstantBufferDeclarationToken
			{
				DeclarationLength = length,
				Data = new DebugNumber[length]
			};

			for (int i = 0; i < length; i++)
				result.Data[i] = DebugNumber.Parse(reader);

			return result;
		}
	}
}
