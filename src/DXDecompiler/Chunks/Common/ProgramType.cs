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
		LibraryShader = 0xFFF0,

		[Description("fx")]
		EffectsShader = 6
	}
}