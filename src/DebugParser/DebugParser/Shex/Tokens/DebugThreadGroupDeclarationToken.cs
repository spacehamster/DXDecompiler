
namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugThreadGroupDeclarationToken : DebugDeclarationToken
	{
		public uint[] Dimensions;

		public DebugThreadGroupDeclarationToken()
		{
			Dimensions = new uint[3];
		}

		public static DebugThreadGroupDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var result = new DebugThreadGroupDeclarationToken();
			result.Dimensions[0] = reader.ReadUInt32("Dimensions[0]");
			result.Dimensions[1] = reader.ReadUInt32("Dimensions[1]");
			result.Dimensions[2] = reader.ReadUInt32("Dimensions[2]");

			return result;
		}
	}
}
