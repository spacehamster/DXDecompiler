using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public enum EffectShaderType
	{
		Rasterizer = 0,
		DepthStencil = 1,
		Blend = 2,
		VertexShader= 6,
		PixelShader = 7,
		GeometryShader = 8,
		DepthStencilRef = 9,
		BlendStateBlendFactor = 10,
		BlendStateSampleMask = 11,
		ComputeShader = 58
	}
}