#pragma FXC HLSLSample_VS vs_5_0 RenderSceneVS
#pragma FXC HLSLSample_PS ps_5_0 RenderScenePS
#pragma FXC HLSLSample_VS_5_1 vs_5_1 RenderSceneVS
#pragma FXC BasicHLSL_PS_5_1 ps_5_1 PSMain

//--------------------------------------------------------------------------------------
// File: BasicHLSL.fx
//
// The effect file for the BasicHLSL sample.  
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

//--------------------------------------------------------------------------------------
// Global variables
//--------------------------------------------------------------------------------------
float4 g_MaterialAmbientColor;      // Material's ambient color
float4 g_MaterialDiffuseColor;      // Material's diffuse color
int g_nNumLights;

float3 g_LightDir;               // Light's direction in world space
float4 g_LightDiffuse;           // Light's diffuse color
float4 g_LightAmbient;              // Light's ambient color

float    g_fTime;                   // App's time in seconds
float4x4 g_mWorld;                  // World matrix for object
float4x4 g_mWorldViewProjection;    // World * View * Projection matrix



//--------------------------------------------------------------------------------------
// Texture samplers
//--------------------------------------------------------------------------------------

Texture2D g_MeshTexture;              // Color texture for mesh
SamplerState g_samLinear;

//--------------------------------------------------------------------------------------
// Vertex shader output structure
//--------------------------------------------------------------------------------------
struct VS_OUTPUT
{
	float4 Position   : SV_Position;   // vertex position 
	float4 Diffuse    : COLOR0;     // vertex diffuse color (note that COLOR0 is clamped from 0..1)
	float2 TextureUV  : TEXCOORD0;  // vertex texture coords 
};


//--------------------------------------------------------------------------------------
// This shader computes standard transform and lighting
//--------------------------------------------------------------------------------------
VS_OUTPUT RenderSceneVS(float4 vPos : SV_Position,
	float3 vNormal : NORMAL,
	float2 vTexCoord0 : TEXCOORD0,
	uniform int nNumLights,
	uniform bool bTexture,
	uniform bool bAnimate)
{

	VS_OUTPUT Output = (VS_OUTPUT)0;
	float3 vNormalWorldSpace;

	// Transform the position from object space to homogeneous projection space
	Output.Position = mul(vPos, g_mWorldViewProjection);

	// Transform the normal from object space to world space    
	vNormalWorldSpace = normalize(mul(vNormal, (float3x3)g_mWorld)); // normal (world space)

	// Compute simple directional lighting equation
	float3 vTotalLightDiffuse = float3(0, 0, 0);
	for (int i = 0; i < nNumLights; i++)
		vTotalLightDiffuse += g_LightDiffuse * max(0, dot(vNormalWorldSpace, g_LightDir));

	Output.Diffuse.rgb = g_MaterialDiffuseColor * vTotalLightDiffuse +
		g_MaterialAmbientColor * g_LightAmbient;
	Output.Diffuse.a = 1.0f;

	// Just copy the texture coordinate through
	if (bTexture)
		Output.TextureUV = vTexCoord0;
	else
		Output.TextureUV = 0;

	return Output;
}


//--------------------------------------------------------------------------------------
// Pixel shader output structure
//--------------------------------------------------------------------------------------
struct PS_OUTPUT
{
	float4 RGBColor : SV_TARGET;  // Pixel color
};


//--------------------------------------------------------------------------------------
// This shader outputs the pixel's color by modulating the texture's
//       color with diffuse material color
//--------------------------------------------------------------------------------------
PS_OUTPUT RenderScenePS(VS_OUTPUT In,
	uniform bool bTexture)
{
	PS_OUTPUT Output;

	// Lookup mesh texture and modulate it with diffuse
	if (bTexture)
		Output.RGBColor = g_MeshTexture.Sample(g_samLinear, In.TextureUV) * In.Diffuse;
	else
		Output.RGBColor = In.Diffuse;

	return Output;
}