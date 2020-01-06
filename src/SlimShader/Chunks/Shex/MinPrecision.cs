namespace SlimShader.Chunks.Shex
{
	public enum OperandMinPrecision
	{
		[Description("")]
		Default = 0,
		[Description("{min16f}")]
		Float16 = 1,
		[Description("{min2_8f}")]
		Float12 = 2,
		[Description("{min16i}")]
		SInt16 = 4,
		[Description("{min16u}")]
		Uint16 = 5,
	}
}