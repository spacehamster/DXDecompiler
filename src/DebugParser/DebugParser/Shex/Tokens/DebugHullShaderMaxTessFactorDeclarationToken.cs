namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugHullShaderMaxTessFactorDeclarationToken : DebugDeclarationToken
	{
		public float MaxTessFactor;

		public static DebugHullShaderMaxTessFactorDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugHullShaderMaxTessFactorDeclarationToken
			{
				MaxTessFactor = reader.ReadSingle("MaxTessFactor")
			};
		}
	}
}
