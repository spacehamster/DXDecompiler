namespace SlimShader.Chunks.Shex.Tokens
{
	public abstract class UnorderedAccessViewDeclarationTokenBase : DeclarationToken
	{
		public UnorderedAccessViewCoherency Coherency { get; protected set; }
		public bool IsRasterOrderedAccess { get; protected set; }
		public uint SpaceIndex { get; protected set; }
		protected string GetRasterOrderedAccessDescription()
		{
			return IsRasterOrderedAccess ? "_rov" : "";
		}
	}
}