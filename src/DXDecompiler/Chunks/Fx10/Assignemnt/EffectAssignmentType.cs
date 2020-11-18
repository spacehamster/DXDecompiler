using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Fx10.Assignemnt;
using DXDecompiler.Chunks.RTS0;

namespace DXDecompiler.Chunks.Fx10
{
	public enum EffectAssignmentType
	{
		RasterizerState = 0,
		DepthStencilState = 1,
		BlendState = 2,
		VertexShader = 6,
		PixelShader = 7,
		GeometryShader = 8,
		[AssignmentType(typeof(uint))]
		DS_StencilRef = 9,
		[AssignmentType(typeof(float))]
		AB_BlendFactor = 10,
		[AssignmentType(typeof(uint))]
		AB_SampleMask = 11,
		/// <summary>
		/// Rasterizer State
		/// Based on D3D10_RASTERIZER_DESC for list of types
		/// </summary>
		[AssignmentType(typeof(FillMode))]
		FillMode = 12,
		[AssignmentType(typeof(CullMode))]
		CullMode = 13,
		[AssignmentType(typeof(bool))]
		FrontCounterClockwise = 14,
		[AssignmentType(typeof(int))]
		DepthBias = 15,
		[AssignmentType(typeof(float))]
		DepthBiasClamp = 16,
		[AssignmentType(typeof(float))]
		SlopeScaledDepthBias = 17,
		[AssignmentType(typeof(bool))]
		DepthClipEnable = 18,
		[AssignmentType(typeof(bool))]
		ScissorEnable = 19,
		[AssignmentType(typeof(bool))]
		MultisampleEnable = 20,
		[AssignmentType(typeof(bool))]
		AntialiasedLineEnable = 21,
		/// <summary>
		/// Depth Stencil State
		/// Based on D3D10_DEPTH_STENCIL_DESC for list of types
		/// </summary>
		[AssignmentType(typeof(bool))]
		DepthEnable = 22,
		[AssignmentType(typeof(DepthWriteMask))]
		DepthWriteMask = 23,
		[AssignmentType(typeof(ComparisonFunc))]
		DepthFunc = 24,
		[AssignmentType(typeof(bool))]
		StencilEnable = 25,
		[AssignmentType(typeof(byte))]
		StencilReadMask = 26,
		[AssignmentType(typeof(byte))]
		StencilWriteMask = 27,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilFail = 28,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilDepthFail = 29,
		[AssignmentType(typeof(StencilOp))]
		FrontFaceStencilPass = 30,
		[AssignmentType(typeof(ComparisonFunc))]
		FrontFaceStencilFunc = 31,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilFail = 32,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilDepthFail = 33,
		[AssignmentType(typeof(StencilOp))]
		BackFaceStencilPass = 34,
		[AssignmentType(typeof(ComparisonFunc))]
		BackFaceStencilFunc = 35,
		/// <summary>
		/// Blend State
		/// Based on D3D10_BLEND_DESC for list of types
		/// </summary>
		[AssignmentType(typeof(bool))]
		AlphaToCoverageEnable = 36,
		[AssignmentType(typeof(bool))]
		BlendEnable = 37,
		[AssignmentType(typeof(Blend))]
		SrcBlend = 38,
		[AssignmentType(typeof(Blend))]
		DestBlend = 39,
		[AssignmentType(typeof(BlendOp))]
		BlendOp = 40,
		[AssignmentType(typeof(Blend))]
		SrcBlendAlpha = 41,
		[AssignmentType(typeof(Blend))]
		DestBlendAlpha = 42,
		[AssignmentType(typeof(BlendOp))]
		BlendOpAlpha = 43,
		[AssignmentType(typeof(byte))]
		RenderTargetWriteMask = 44,
		/// <summary>
		/// SamplerState
		/// Based on D3D10_SAMPLER_DESC for list of types
		/// </summary>
		[AssignmentType(typeof(Filter))]
		Filter = 45,
		[AssignmentType(typeof(TextureAddressMode))]
		AddressU = 46,
		[AssignmentType(typeof(TextureAddressMode))]
		AddressV = 47,
		[AssignmentType(typeof(TextureAddressMode))]
		AddressW = 48,
		[AssignmentType(typeof(float))]
		MipLODBias = 49,
		[AssignmentType(typeof(uint))]
		MaxAnisotropy = 50,
		[AssignmentType(typeof(ComparisonFunc))]
		ComparisonFunc = 51,
		[AssignmentType(typeof(float))]
		BorderColor = 52,
		[AssignmentType(typeof(float))]
		MinLOD = 53,
		[AssignmentType(typeof(float))]
		MaxLOD = 54,
		Texture = 55,
		/// <summary>
		/// FX5 Shaders
		/// </summary>
		HullShader = 56,
		DomainShader = 57,
		ComputeShader = 58,
	}
}
