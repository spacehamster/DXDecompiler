namespace DXDecompiler.Chunks.Common
{
	/// <summary>
	/// Based on D3D12_BLEND_OP
	/// </summary>
	public enum BlendOp
	{
		[Description("ADD")]
		Add = 1,
		[Description("SUBTRACT")]
		Subtract = 2,
		[Description("REV_SUBTRACT")]
		RevSubtract = 3,
		[Description("MIN")]
		Min = 4,
		[Description("MAX")]
		Max = 5
	}
}
