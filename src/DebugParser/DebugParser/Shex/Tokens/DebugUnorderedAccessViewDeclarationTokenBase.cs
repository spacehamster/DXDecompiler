using DXDecompiler.Chunks.Shex;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugUnorderedAccessViewDeclarationTokenBase : DebugDeclarationToken
	{
		public UnorderedAccessViewCoherency Coherency;
		public bool IsRasterOrderedAccess;
		public uint SpaceIndex;
	}
}
