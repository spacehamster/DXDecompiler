using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugResourceReturnTypeToken
	{
		public ResourceReturnType X;
		public ResourceReturnType Y;
		public ResourceReturnType Z;
		public ResourceReturnType W;

		public static DebugResourceReturnTypeToken Parse(DebugBytecodeReader reader)
		{
			var token = reader.ReadUInt32("token");
			var result = new DebugResourceReturnTypeToken
			{
				X = token.DecodeValue<ResourceReturnType>(00, 03),
				Y = token.DecodeValue<ResourceReturnType>(04, 07),
				Z = token.DecodeValue<ResourceReturnType>(08, 11),
				W = token.DecodeValue<ResourceReturnType>(12, 15)
			};
			reader.AddNote("X", result.X);
			reader.AddNote("Y", result.Y);
			reader.AddNote("Z", result.Z);
			reader.AddNote("W", result.W);
			return result;
		}
	}
}
