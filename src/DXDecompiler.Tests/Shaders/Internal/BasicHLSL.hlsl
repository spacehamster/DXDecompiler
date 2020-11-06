//--------------------------------------------------------------------------------------
// File: BasicHLSL11.hlsl
//
// The pixel shader file for the BasicHLSL11 sample.  
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

cbuffer cbPerObject : register(b0)
{
	matrix		g_mWorldViewProjection	: packoffset(c0);
	matrix		g_mWorld				: packoffset(c4);
	float4		g_vObjectColor			: packoffset(c8);
};
cbuffer cbPerFrame : register(b1)
{
	float3		g_vLightDir				: packoffset(c0);
	float		g_fAmbient : packoffset(c0.w);
};


Texture2D	g_txDiffuse : register(t0);
SamplerState g_samLinear : register(s0);

struct VS_INPUT
{
	float4 vPosition	: POSITION;
	float3 vNormal		: NORMAL;
	float2 vTexcoord	: TEXCOORD0;
};
struct VS_OUTPUT
{
	float3 vNormal		: NORMAL;
	float2 vTexcoord	: TEXCOORD0;
	float4 vPosition	: SV_POSITION;
};
VS_OUTPUT VSMain(VS_INPUT Input)
{
	VS_OUTPUT Output;

	Output.vPosition = mul(Input.vPosition, g_mWorldViewProjection);
	Output.vNormal = mul(Input.vNormal, (float3x3)g_mWorld);
	Output.vTexcoord = Input.vTexcoord;

	return Output;
}
float4 PSMain(VS_OUTPUT Input) : SV_TARGET
{
	float4 vDiffuse = g_txDiffuse.Sample(g_samLinear, Input.vTexcoord);

	float fLighting = saturate(dot(g_vLightDir, Input.vNormal));
	fLighting = max(fLighting, g_fAmbient);

	return vDiffuse * fLighting;
}