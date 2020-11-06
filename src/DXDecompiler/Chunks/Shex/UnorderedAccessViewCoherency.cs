namespace SlimShader.Chunks.Shex
{
	public enum UnorderedAccessViewCoherency
	{
		LocallyCoherent = 0,
		[Description("glc")]
		GloballyCoherent = 1
	}
}