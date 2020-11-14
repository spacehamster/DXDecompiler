namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugGeometryShaderInstanceCountDeclarationToken : DebugDeclarationToken
	{
		public uint InstanceCount;

		public static DebugGeometryShaderInstanceCountDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			return new DebugGeometryShaderInstanceCountDeclarationToken
			{
				InstanceCount = reader.ReadUInt32("InstanceCount")
			};
		}
	}
}
