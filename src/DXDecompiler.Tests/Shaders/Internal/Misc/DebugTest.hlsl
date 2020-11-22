#pragma FXC DebugTest ps_5_0 PSMain /Zi
//Note: Debug info is not deterministic

cbuffer cbPerFrame : register(b1)
{
	float3		g_vLightDir				: packoffset(c0);
	float		g_fAmbient : packoffset(c0.w);
};


Texture2D	g_txDiffuse : register(t0);
SamplerState g_samLinear : register(s0);

struct VS_OUTPUT
{
	float3 vNormal		: NORMAL;
	float2 vTexcoord	: TEXCOORD0;
	float4 vPosition	: SV_POSITION;
};
float4 PSMain(VS_OUTPUT Input) : SV_TARGET
{
	float4 vDiffuse = g_txDiffuse.Sample(g_samLinear, Input.vTexcoord);

	float fLighting = saturate(dot(g_vLightDir, Input.vNormal));
	fLighting = max(fLighting, g_fAmbient);
	fLighting *= 3.5;
	return vDiffuse * fLighting;
}