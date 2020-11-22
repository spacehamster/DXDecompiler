#pragma FXC Level9_Test_VS_9_0 vs_4_0_level_9_0 VSMain
#pragma FXC Level9_Test_PS_9_0 ps_4_0_level_9_0 PSMain
#pragma FXC Level9_Test_VS_9_1 vs_4_0_level_9_1 VSMain
#pragma FXC Level9_Test_PS_9_1 ps_4_0_level_9_1 PSMain
#pragma FXC Level9_Test_VS_9_3 vs_4_0_level_9_3 VSMain
#pragma FXC Level9_Test_PS_9_3 ps_4_0_level_9_3 PSMain

struct VS_INPUT
{
	float4 vPosition	: POSITION;
	float3 vNormal		: NORMAL;
    float4 vDiffuse	: COLOR0;
	float2 vTexcoord	: TEXCOORD0;
};
struct VS_OUTPUT
{
	float4 Position		: SV_POSITION;
    float3 Normal		: NORMAL;
	float4 Diffuse	: COLOR0;
	float2 TextureUV	: TEXCOORD0;
};
cbuffer MyBuffer1  : register(b11)
{
	int4 g_nNumLights0 : packoffset(c10);
	int4 g_nNumLights1 : packoffset(c15);
	int4 g_nNumLights2 : packoffset(c20);
	int4 g_nNumLights3 : packoffset(c25);
	float4 g_LightDiffuse: packoffset(c30);
}
cbuffer MyBuffer2  : register(b13)
{
	matrix		g_mWorldViewProjection	: packoffset(c0);
	matrix		g_mWorld				: packoffset(c4);
	float4		g_vObjectColor			: packoffset(c8);
}

VS_OUTPUT VSMain(VS_INPUT Input)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;

	for (int i1 = 0; i1 < g_nNumLights1.y; i1++) {
		Output.Diffuse += g_LightDiffuse;
	}
	for (int i2 = 0; i2 < g_nNumLights2.z; i2++) {
		Output.Diffuse += g_LightDiffuse;
	}
	for (int i0 = 0; i0 < g_nNumLights0.x; i0++) {
		Output.Diffuse += g_LightDiffuse;
	}
	for (int i3 = 0; i3 < g_nNumLights3.w; i3++) {
		Output.Diffuse += g_LightDiffuse;
	}

    
	Output.Position = mul(Input.vPosition, g_mWorldViewProjection);
	Output.Normal = mul(Input.vNormal, (float3x3)g_mWorld);
    Output.Diffuse = Input.vDiffuse;
	Output.TextureUV = Input.vTexcoord;

	return Output;
}

cbuffer cb1 {
	float4 val1;
};
cbuffer cb1 {
	float4 val2;
	int3 vec[5];
	uint4 val3;
};
cbuffer cb3 {
	float4 val4;
};
SamplerState samp0;
SamplerState samp1;
Texture2D tex0;
Texture2D tex1;
Texture2D tex2;
Texture2D tex3;
float4 PSMain(VS_OUTPUT input) : SV_TARGET
{
	float4 result = 0;
	float2 uv = 0;
	result += val1;
	result += val2;
	result += vec[2].xyzx;
	result += vec[4].xyzx;
	result += val3;
	result += val4;
	result += tex0.Sample(samp0, uv);
	result += tex1.Sample(samp0, uv);
	result += tex2.Sample(samp1, uv);
	result += tex3.Sample(samp1, uv);
	return result;
}