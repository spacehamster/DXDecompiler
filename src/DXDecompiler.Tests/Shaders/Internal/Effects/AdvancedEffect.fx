// Global variables
float4 g_MaterialAmbientColor;      // Material's ambient color
float    g_fTime;                   // App's time in seconds
float4x4 g_mWorld;                  // World matrix for object
float4x4 g_mWorldViewProjection;    // World * View * Projection matrix

float4    defaultValue = float4(1, 2, 3, 4);
tbuffer TestTBuffer {
	float4 val1;
	float3 g_positions[4] =
	{
		float3(-1, 1, 0),
		float3(1, 1, 0),
		float3(-1, -1, 0),
		float3(1, -1, 0),
	};
	float2 g_texcoords[4] =
	{
		float2(0,1),
		float2(1,1),
		float2(0,0),
		float2(1,0),
	};
}
cbuffer TestCbuffer {
	float4 val2;
}

shared cbuffer TestShared {
	float4 sharedValue;
}
Texture2D g_MeshTexture;            // Color texture for mesh
// Texture samplers
SamplerState MeshTextureSampler
<int blabla = 27; string blacksheep = "Hello There"; >
{
	AddressU = Wrap;
	AddressV = Clamp;
	AddressW = MIrror;
	BorderColor = float4(2.0f, 3.0f, 4.0f, 5.0f);
	Filter = MIN_MAG_MIP_LINEAR;
	MaxAnisotropy = 5;
	MaxLOD = 6;
	MinLOD = 7;
	MipLODBias = 8;
	ComparisonFunc = GREATER_EQUAL;
};
BlendState NoBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable[0] = FALSE;
};

BlendState PaintBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable[0] = TRUE;
	SrcBlend = SRC_ALPHA;
	DestBlend = INV_SRC_ALPHA;
	BlendOp = ADD;
	SrcBlendAlpha = SRC_ALPHA;
	DestBlendAlpha = INV_SRC_ALPHA;
	BlendOpAlpha = ADD;
	RenderTargetWriteMask[0] = 0x0F;
};

DepthStencilState EnableDepth
{
	DepthEnable = TRUE;
	DepthWriteMask = ALL;
	DepthFunc = LESS_EQUAL;

	StencilEnable = FALSE;
	StencilReadMask = 0xFF;
	StencilWriteMask = 0xFFFFFFFF;

	BackFaceStencilFunc = Always;
	BackFaceStencilDepthFail = Incr;
	BackFaceStencilPass = Keep;
	BackFaceStencilFail = Keep;

	FrontFaceStencilFunc = Always;
	FrontFaceStencilDepthFail = Decr;
	FrontFaceStencilPass = Keep;
	FrontFaceStencilFail = Keep;
};

DepthStencilState DisableDepth
{
	DepthEnable = FALSE;
	DepthWriteMask = 0;
};

RasterizerState CullBack
{
	FillMode = WIREFRAME;
	CullMode = BACK;
	FrontCounterClockwise = TRUE;
	DepthBias = 5;
	DepthBiasClamp = 5.5;
	SlopeScaledDepthBias = 6.5;
	DepthClipEnable = TRUE;
	ScissorEnable = TRUE;
	MultiSampleEnable = FALSE;
	AntialiasedLineEnable = TRUE;
};

RasterizerState CullNone
{
	CullMode = NONE;
};
DepthStencilView TestDepthStencilView;
RenderTargetView TestRenderTargetView;

shared Texture2D sharedTexture;            // Color texture for mesh
struct VS_OUTPUT
{
	float4 Position   : SV_POSITION; // vertex position 
	float4 Diffuse    : COLOR0;      // vertex diffuse color
	float2 TextureUV  : TEXCOORD0;   // vertex texture coords 
};

VS_OUTPUT RenderSceneVS(float4 vPos : POSITION,
	float3 vNormal : NORMAL,
	float2 vTexCoord0 : TEXCOORD,
	uniform int nNumLights,
	uniform bool bTexture,
	uniform bool bAnimate)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	float3 vNormalWorldSpace;

	return Output;
}

struct PS_OUTPUT
{
	float4 RGBColor : SV_Target;  // Pixel color
};

PS_OUTPUT RenderScenePS(VS_OUTPUT In,
	uniform bool bTexture)
{
	PS_OUTPUT Output = (PS_OUTPUT)0;

	if (bTexture)
		Output.RGBColor = g_MeshTexture.Sample(MeshTextureSampler, In.TextureUV) * In.Diffuse;

	return Output;
}


struct BufType
{
	int i;
	float f;
	double d;
};

ByteAddressBuffer Buffer0 : register(t0);
ByteAddressBuffer Buffer1 : register(t1);
RWByteAddressBuffer BufferOut : register(u0);

[numthreads(1, 1, 1)]
void CSMain(uint3 DTid : SV_DispatchThreadID)
{
	int i0 = asint(Buffer0.Load(DTid.x * 8));
	float f0 = asfloat(Buffer0.Load(DTid.x * 8 + 4));
	int i1 = asint(Buffer1.Load(DTid.x * 8));
	float f1 = asfloat(Buffer1.Load(DTid.x * 8 + 4));

	BufferOut.Store(DTid.x * 8, asuint(i0 + i1));
	BufferOut.Store(DTid.x * 8 + 4, asuint(f0 + f1));
}

uint blendIndex;
BlendState BlendArray[3]
{
	{
		BlendOp = Add;
	},
	{
		BlendOp = Subtract;
	},
	{
		BlendOp = Rev_Subtract;
	}
};
RasterizerState RasterizerArray[3]
{
	{
		DepthBiasClamp = 1;
	},
	{
		DepthBiasClamp = 2;
	},
	{
		DepthBiasClamp = 3;
	}
};
DepthStencilState DepthStencilArray[3]
{
	{
		DepthFunc = 1;
	},
	{
		DepthFunc = 2;
	},
	{
		DepthFunc = 3;
	}
};

technique10 RenderSceneWithTexture1Light10_1
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, RenderSceneVS(1, true, true)));
		SetPixelShader(CompileShader(ps_4_0, RenderScenePS(true)));
		SetBlendState(NoBlending, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetRasterizerState(CullNone);
		SetDepthStencilState(DisableDepth, 5);
	}
}

technique10 RenderSceneWithTexture1Light10_2
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, RenderSceneVS(2, true, true)));
		SetPixelShader(CompileShader(ps_4_0, RenderScenePS(false)));
		SetBlendState(BlendArray[blendIndex], float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetRasterizerState(RasterizerArray[1]);
	}
}

technique11 RenderSceneWithTexture1Light11_1
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, RenderSceneVS(1, true, true)));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, RenderScenePS(true)));
		SetBlendState(NoBlending, float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetRasterizerState(CullNone);
		SetDepthStencilState(DisableDepth, 5);
	}
}
VertexShader TestVertexShader5 = CompileShader(vs_5_0, RenderSceneVS(1, true, true));
PixelShader TestPixelShader5 = CompileShader(ps_5_0, RenderScenePS(false));
ComputeShader TestComputeShader5 = CompileShader(cs_5_0, CSMain());
technique11 RenderSceneWithTexture1Light11_2
{
	pass P0
	{
		SetVertexShader(TestVertexShader5);
		SetGeometryShader(NULL);
		SetPixelShader(TestPixelShader5);
		SetComputeShader(TestComputeShader5);
		SetBlendState(BlendArray[blendIndex], float4(0.0f, 0.0f, 0.0f, 0.0f), 0xFFFFFFFF);
		SetRasterizerState(RasterizerArray[1]);
	}
}
fxgroup g0
{
	technique11 RunComputeShader
	{
		pass P0
		{
			SetComputeShader(CompileShader(cs_5_0, CSMain()));
		}
	}
}