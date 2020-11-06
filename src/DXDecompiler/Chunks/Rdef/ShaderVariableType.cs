using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// These flags identify various data, texture, and buffer types that can be assigned to a shader variable.
	/// Based on SHADER_VARIABLE_TYPE .
	/// </summary>
	public enum ShaderVariableType
	{
		Void = 0,

		[Description("bool")]
		Bool = 1,

		[Description("int")]
		Int = 2,

		[Description("float")]
		Float = 3,

		String = 4,
		Texture = 5,
		Texture1D = 6,
		Texture2D = 7,
		Texture3D = 8,
		TextureCube = 9,
		Sampler = 10,
		Sampler1D = 11,
		Sampler2D = 12,
		Sampler3D = 13,
		SamplerCube = 14,
		PixelShader = 15,
		VertexShader = 16,
		PixelFragment = 17,
		VertexFragment = 18,

		[Description("uint")]
		UInt = 19,

		UInt8 = 20,
		GeometryShader = 21,
		Rasterizer = 22,
		DepthStencil = 23,
		Blend = 24,
		Buffer = 25,
		CBuffer = 26,
		TBuffer = 27,
		Texture1DArray = 28,
		Texture2DArray = 29,
		RenderTargetView = 30,
		DepthStencilView = 31,
		Texture2DMultiSampled = 32,
		Texture2DMultiSampledArray = 33,
		TextureCubeArray = 34,

		// The following are new in D3D11.

		HullShader = 35,
		DomainShader = 36,

		[Description("interface")]
		InterfacePointer = 37,

		ComputeShader = 38,

		[Description("double")]
		Double = 39,

		ReadWriteTexture1D = 40,
		ReadWriteTexture1DArray = 41,
		ReadWriteTexture2D = 42,
		ReadWriteTexture2DArray = 43,
		ReadWriteTexture3D = 44,
		ReadWriteBuffer = 45,
		ByteAddressBuffer = 46,
		ReadWriteByteAddressBuffer = 47,
		StructuredBuffer = 48,
		ReadWriteStructuredBuffer = 49,
		AppendStructuredBuffer = 50,
		ConsumeStructuredBuffer = 51,
		Min8Float = 52,
		Min10Float = 53,
		Min16Float = 54,
		Min12Int = 55,
		Min16Int = 56,
		Min16Uint = 57,
	}
}