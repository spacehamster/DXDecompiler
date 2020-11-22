#pragma FXC BasicEffect_5_0_FX fx_5_0
#pragma FXC BasicEffect_4_0_FX fx_4_0
#pragma FXC BasicEffect_4_1_FX fx_4_1
#pragma FXC BasicEffect_5_0_Child_FX fx_5_0 /Gch
#pragma FXC BasicEffect_4_0_Child_FX fx_4_0 /Gch
#pragma FXC BasicEffect_4_1_Child_FX fx_4_1 /Gch

// Global variables
float4 g_MaterialAmbientColor;      // Material's ambient color

struct VS_OUTPUT
{
	float4 Position   : SV_POSITION; // vertex position 
};

VS_OUTPUT RenderSceneVS(float4 vPos : POSITION,
	float3 vNormal : NORMAL,
	float2 vTexCoord0 : TEXCOORD)
{
	VS_OUTPUT Output = (VS_OUTPUT)0;
	Output.Position += g_MaterialAmbientColor;
	return Output;
}
float4 RenderScenePS(VS_OUTPUT In) : SV_TARGET
{
	float4 result = 0;
	result += g_MaterialAmbientColor;
	return result;
}

technique10 RenderSceneWithTexture1Light10_1
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, RenderSceneVS()));
		SetPixelShader(CompileShader(ps_4_0, RenderScenePS()));
	}
}
technique11 RenderSceneWithTexture1Light11_1
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_5_0, RenderSceneVS()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_5_0, RenderScenePS()));
	}
}