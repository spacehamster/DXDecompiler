namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugHullShaderForkPhaseInstanceCountDeclarationToken : DebugDeclarationToken
	{
		public uint InstanceCount;

		public static DebugHullShaderForkPhaseInstanceCountDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugHullShaderForkPhaseInstanceCountDeclarationToken
			{
				InstanceCount = reader.ReadUInt32("InstanceCount")
			};
		}
	}
}
