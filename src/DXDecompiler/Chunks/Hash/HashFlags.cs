namespace DXDecompiler.Chunks.Hash
{
	public enum HashFlags
	{
		None = 0,           // No flags defined.
		IncludesSource = 1, // This flag indicates that the shader hash was computed
							// taking into account source information (-Zss)
	}
}
