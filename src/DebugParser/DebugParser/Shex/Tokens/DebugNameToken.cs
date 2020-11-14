using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugNameToken
	{
		public static SystemValueName Parse(DebugBytecodeReader reader)
		{
			uint token = reader.ReadUInt32("token");
			reader.AddNote("SystemValueName", token.DecodeValue<SystemValueName>(0, 15));
			return token.DecodeValue<SystemValueName>(0, 15);
		}
	}
}
