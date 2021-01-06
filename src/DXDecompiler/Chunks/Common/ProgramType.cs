namespace DXDecompiler.Chunks.Common
{
	public enum ProgramType : ushort
	{
		[Description("ps")]
		PixelShader = 0,

		[Description("vs")]
		VertexShader = 1,

		[Description("gs")]
		GeometryShader = 2,

		// Below are shaders new in DX 11

		[Description("hs")]
		HullShader = 3,

		[Description("ds")]
		DomainShader = 4,

		[Description("cs")]
		ComputeShader = 5,

		[Description("lib")]
		LibraryShader = 6,

		RayGeneration = 7,
		Intersection = 8,
		AnyHit = 9,
		ClosestHit = 10,
		Miss = 11,
		Callable = 12,
		Mesh = 13,
		Amplification = 14,
		Invalid = 15,

		[Description("fx")]
		EffectsShader = 16
	}
}