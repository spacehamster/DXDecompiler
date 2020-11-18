namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Sampler filter type
	/// Based on D3D12_FILTER.
	/// </summary>
	public enum Filter
	{
		[Description("FILTER_MIN_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MIN_MAG_MIP_POINT")]
		MinMagMipPoint = 0,
		[Description("FILTER_MIN_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MIN_MAG_POINT_MIP_LINEAR")]
		MinMagPointMipLinear = 0x1,
		[Description("FILTER_MIN_POINT_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MIN_POINT_MAG_LINEAR_MIP_POINT")]
		MinPointMagLinearMipPoint = 0x4,
		[Description("FILTER_MIN_POINT_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MIN_POINT_MAG_MIP_LINEAR")]
		MinPointMagMipLinear = 0x5,
		[Description("FILTER_MIN_LINEAR_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MIN_LINEAR_MAG_MIP_POINT")]
		MinLinearMagMipPoint = 0x10,
		[Description("FILTER_MIN_LINEAR_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MIN_LINEAR_MAG_POINT_MIP_LINEAR")]
		MinLinearMagPointMipLinear = 0x11,
		[Description("FILTER_MIN_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MIN_MAG_LINEAR_MIP_POINT")]
		MinMagLinearMipPoint = 0x14,
		[Description("FILTER_MIN_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MIN_MAG_MIP_LINEAR")]
		MinMagMipLinear = 0x15,
		[Description("FILTER_ANISOTROPIC", ChunkType.Rts0)]
		[Description("ANISOTROPIC")]
		Anisotropic = 0x55,
		[Description("FILTER_COMPARISON_MIN_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_MAG_MIP_POINT")]
		ComparisonMinMagMipPoint = 0x80,
		[Description("FILTER_COMPARISON_MIN_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_MAG_POINT_MIP_LINEAR")]
		ComparisonMinMagPointMipLinear = 0x81,
		[Description("FILTER_COMPARISON_MIN_POINT_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_POINT_MAG_LINEAR_MIP_POINT")]
		ComparisonMinPointMagLinearMipPoint = 0x84,
		[Description("FILTER_COMPARISON_MIN_POINT_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_POINT_MAG_MIP_LINEAR")]
		ComparisonMinPointMagMipLinear = 0x85,
		[Description("FILTER_COMPARISON_MIN_LINEAR_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_LINEAR_MAG_MIP_POINT")]
		ComparisonMinLinearMagMipPoint = 0x90,
		[Description("FILTER_COMPARISON_MIN_LINEAR_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_LINEAR_MAG_POINT_MIP_LINEAR")]
		ComparisonMinLinearMagPointMipLinear = 0x91,
		[Description("FILTER_COMPARISON_MIN_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_MAG_LINEAR_MIP_POINT")]
		ComparisonMinMagLinearMipPoint = 0x94,
		[Description("FILTER_COMPARISON_MIN_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("COMPARISON_MIN_MAG_MIP_LINEAR")]
		ComparisonMinMagMipLinear = 0x95,
		[Description("FILTER_COMPARISON_ANISOTROPIC", ChunkType.Rts0)]
		[Description("COMPARISON_ANISOTROPIC")]
		ComparisonAnisotropic = 0xd5,
		[Description("FILTER_MINIMUM_MIN_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_MAG_MIP_POINT")]
		MinimumMinMagMipPoint = 0x100,
		[Description("FILTER_MINIMUM_MIN_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_MAG_POINT_MIP_LINEAR")]
		MinimumMinMagPointMipLinear = 0x101,
		[Description("FILTER_MINIMUM_MIN_POINT_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_POINT_MAG_LINEAR_MIP_POINT")]
		MinimumMinPointMagLinearMipPoint = 0x104,
		[Description("FILTER_MINIMUM_MIN_POINT_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_POINT_MAG_MIP_LINEAR")]
		MinimumMinPointMagMipLinear = 0x105,
		[Description("FILTER_MINIMUM_MIN_LINEAR_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_LINEAR_MAG_MIP_POINT")]
		MinimumMinLinearMagMipPoint = 0x110,
		[Description("FILTER_MINIMUM_MIN_LINEAR_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_LINEAR_MAG_POINT_MIP_LINEAR")]
		MinimumMinLinearMagPointMipLinear = 0x111,
		[Description("FILTER_MINIMUM_MIN_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_MAG_LINEAR_MIP_POINT")]
		MinimumMinMagLinearMipPoint = 0x114,
		[Description("FILTER_MINIMUM_MIN_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MINIMUM_MIN_MAG_MIP_LINEAR")]
		MinimumMinMagMipLinear = 0x115,
		[Description("FILTER_MINIMUM_ANISOTROPIC", ChunkType.Rts0)]
		[Description("MINIMUM_ANISOTROPIC")]
		MinimumAnisotropic = 0x155,
		[Description("FILTER_MAXIMUM_MIN_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_MAG_MIP_POINT")]
		MaximumMinMagMipPoint = 0x180,
		[Description("FILTER_MAXIMUM_MIN_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_MAG_POINT_MIP_LINEAR")]
		MaximumMinMagPointMipLinear = 0x181,
		[Description("FILTER_MAXIMUM_MIN_POINT_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_POINT_MAG_LINEAR_MIP_POINT")]
		MaximumMinPointMagLinearMipPoint = 0x184,
		[Description("FILTER_MAXIMUM_MIN_POINT_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_POINT_MAG_MIP_LINEAR")]
		MaximumMinPointMagMipLinear = 0x185,
		[Description("FILTER_MAXIMUM_MIN_LINEAR_MAG_MIP_POINT", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_LINEAR_MAG_MIP_POINT")]
		MaximumMinLinearMagMipPoint = 0x190,
		[Description("FILTER_MAXIMUM_MIN_LINEAR_MAG_POINT_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_LINEAR_MAG_POINT_MIP_LINEAR")]
		MaximumMinLinearMagPointMipLinear = 0x191,
		[Description("FILTER_MAXIMUM_MIN_MAG_LINEAR_MIP_POINT", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_MAG_LINEAR_MIP_POINT")]
		MaximumMinMagLinearMipPoint = 0x194,
		[Description("FILTER_MAXIMUM_MIN_MAG_MIP_LINEAR", ChunkType.Rts0)]
		[Description("MAXIMUM_MIN_MAG_MIP_LINEAR")]
		MaximumMinMagMipLinear = 0x195,
		[Description("FILTER_MAXIMUM_ANISOTROPIC", ChunkType.Rts0)]
		[Description("MAXIMUM_ANISOTROPIC")]
		MaximumAnisotropic = 0x1d5
	}
}
