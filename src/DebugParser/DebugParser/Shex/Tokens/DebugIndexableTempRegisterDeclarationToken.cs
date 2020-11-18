
namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugIndexableTempRegisterDeclarationToken : DebugDeclarationToken
	{
		public uint RegisterIndex;
		public uint RegisterCount;
		public uint NumComponents;

		public static DebugIndexableTempRegisterDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugIndexableTempRegisterDeclarationToken
			{
				RegisterIndex = reader.ReadUInt32("RegisterIndex"),
				RegisterCount = reader.ReadUInt32("RegisterCount"),
				NumComponents = reader.ReadUInt32("NumComponents")
			};
		}
	}
}
