namespace DXDecompiler.Chunks.Common
{
	/// <summary>
	/// Based on D3D12_BLEND
	/// </summary>
	public enum Blend
	{
		[Description("ZERO")]
		Zero = 1,
		[Description("ONE")]
		One = 2,
		[Description("SRC_COLOR")]
		SrcColor = 3,
		[Description("INV_SRC_COLOR")]
		InvSrcColor = 4,
		[Description("SRC_ALPHA")]
		SrcAlpha = 5,
		[Description("INV_SRC_ALPHA")]
		InvSrcAlpha = 6,
		[Description("DEST_ALPHA")]
		DestAlpha = 7,
		[Description("INV_DEST_ALPHA")]
		InvDestAlpha = 8,
		[Description("DESC_COLOR")]
		DestColor = 9,
		[Description("INV_DEST_COLOR")]
		InvDestColor = 10,
		[Description("SRC_ALPHA_SAT")]
		SrcAlphaSat = 11,
		[Description("BLEND_FACTOR")]
		BlendFactor = 14,
		[Description("INV_BLEND_FACTOR")]
		InvBlendFactor = 15,
		[Description("SRC_1_COLOR")]
		Src1Color = 16,
		[Description("INV_SRC_1_COLOR")]
		InvSrc1Color = 17,
		[Description("SRC_1_ALPHA")]
		Src1Alpha = 18,
		[Description("INV_SRC_1_ALPHA")]
		InvSrc1Alpha = 19
	}
}
