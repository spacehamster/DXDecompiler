namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugControlPointCountDeclarationToken : DebugDeclarationToken
	{
		public uint ControlPointCount;

		public static DebugControlPointCountDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugControlPointCountDeclarationToken
			{
				ControlPointCount = reader.ReadUInt32("ControlPointCount")
			};
		}
	}
}
