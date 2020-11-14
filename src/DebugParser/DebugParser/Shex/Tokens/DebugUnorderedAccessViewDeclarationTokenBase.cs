using SlimShader.Chunks.Shex;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugUnorderedAccessViewDeclarationTokenBase : DebugDeclarationToken
	{
		public UnorderedAccessViewCoherency Coherency;
		public bool IsRasterOrderedAccess;
		public uint SpaceIndex;
	}
}
