// Global variables
float4 g_MaterialAmbientColor;      // Material's ambient color

Texture2D g_MeshTexture;            // Color texture for mesh

float g_fTime;                   // App's time in seconds
float4x4 g_mWorld;                  // World matrix for object
float4x4 g_mWorldViewProjection;    // World * View * Projection matrix

shared cbuffer ShaderedBuffer {
    float4 val1;
float4x4 val2;
row_major float4x4 val3;
    uint val4;
uint2 val5;
float4 val6[4];
}
cbuffer ShaderedBuffer : register(b5)
{
	float4 val100;
	uint3 val101;
}

// Texture samplers
SamplerState MeshTextureSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

// Texture samplers
shared SamplerState SharedSampler
{
	Filter = MIN_MAG_MIP_POINT;
	AddressU = CLAMP;
	AddressV = CLAMP;
};

Buffer<float4> g_ParticleBuffer : register(t10);

tbuffer TextureBuffer
{
	float4 tVal1;
	float4 tVal2;
}
shared Texture2D SharedTexture;

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


BlendState NoBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable [0] = FALSE;
};

BlendState PaintBlending
{
	AlphaToCoverageEnable = FALSE;
	BlendEnable [0] = TRUE;
	SrcBlend = SRC_ALPHA;
	DestBlend = INV_SRC_ALPHA;
	BlendOp = ADD;
	SrcBlendAlpha = SRC_ALPHA;
	DestBlendAlpha = INV_SRC_ALPHA;
	BlendOpAlpha = ADD;
	RenderTargetWriteMask [0] = 0x0F;
};

DepthStencilState EnableDepth
{
	DepthEnable = TRUE;
	DepthWriteMask = ALL;
};

DepthStencilState DisableDepth
{
	DepthEnable = FALSE;
	DepthWriteMask = 0;
};

RasterizerState CullBack
{
	CullMode = BACK;
};

RasterizerState CullNone
{
	CullMode = NONE;
};