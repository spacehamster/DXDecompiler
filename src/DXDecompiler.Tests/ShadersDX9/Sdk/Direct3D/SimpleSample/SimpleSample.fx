CALL #pragma FXC SimpleSample_VS_2 vs_2_0 RenderSceneVS
CALL #pragma FXC SimpleSample_PS_2 ps_2_0 RenderScenePS
CALL #pragma FXC SimpleSample_VS_2a vs_2_a RenderSceneVS
CALL #pragma FXC SimpleSample_VS_2sw vs_2_sw RenderSceneVS
CALL #pragma FXC SimpleSample_VS_3 vs_3_0 RenderSceneVS
CALL #pragma FXC SimpleSample_VS_3sw vs_3_sw RenderSceneVS
CALL #pragma FXC SimpleSample_PS_2a ps_2_a RenderScenePS
CALL #pragma FXC SimpleSample_PS_2b ps_2_b RenderScenePS
CALL #pragma FXC SimpleSample_PS_2sw ps_2_sw RenderScenePS
CALL #pragma FXC SimpleSample_PS_3 ps_3_0 RenderScenePS
CALL #pragma FXC SimpleSample_PS_3sw ps_3_sw RenderScenePS
CALL #pragma FXC SimpleSample_FX fx_2_0

//--------------------------------------------------------------------------------------
// File: SimpleSample.fx
//
// The effect file for the SimpleSample sample.  
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------


//--------------------------------------------------------------------------------------
// Global variables
//--------------------------------------------------------------------------------------
float4 g_MaterialAmbientColor;      // Material's ambient color
float4 g_MaterialDiffuseColor;      // Material's diffuse color
float3 g_LightDir;                  // Light's direction in world space
float4 g_LightDiffuse;              // Light's diffuse color
texture g_MeshTexture;              // Color texture for mesh

float    g_fTime;                   // App's time in seconds
float4x4 g_mWorld;                  // World matrix for object
float4x4 g_mWorldViewProjection;    // World * View * Projection matrix



//--------------------------------------------------------------------------------------
// Texture samplers
//--------------------------------------------------------------------------------------
sampler MeshTextureSampler = 
sampler_state
{
    Texture = <g_MeshTexture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};


//--------------------------------------------------------------------------------------
// Vertex shader output structure
//--------------------------------------------------------------------------------------
struct VS_OUTPUT
{
    float4 Position   : POSITION;   // vertex position 
    float4 Diffuse    : COLOR0;     // vertex diffuse color (note that COLOR0 is clamped from 0..1)
    float2 TextureUV  : TEXCOORD0;  // vertex texture coords 
};


//--------------------------------------------------------------------------------------
// This shader computes standard transform and lighting
//--------------------------------------------------------------------------------------
VS_OUTPUT RenderSceneVS( float4 vPos : POSITION, 
                         float3 vNormal : NORMAL,
                         float2 vTexCoord0 : TEXCOORD0 )
{
    VS_OUTPUT Output;
    float3 vNormalWorldSpace;
    
    // Transform the position from object space to homogeneous projection space
    Output.Position = mul(vPos, g_mWorldViewProjection);
    
    // Transform the normal from object space to world space    
    vNormalWorldSpace = normalize(mul(vNormal, (float3x3)g_mWorld)); // normal (world space)

    // Calc diffuse color    
    Output.Diffuse.rgb = g_MaterialDiffuseColor * g_LightDiffuse * max(0,dot(vNormalWorldSpace, g_LightDir)) + 
                         g_MaterialAmbientColor;   
    Output.Diffuse.a = 1.0f; 
    
    // Just copy the texture coordinate through
    Output.TextureUV = vTexCoord0; 
    
    return Output;    
}


//--------------------------------------------------------------------------------------
// Pixel shader output structure
//--------------------------------------------------------------------------------------
struct PS_OUTPUT
{
    float4 RGBColor : COLOR0;  // Pixel color    
};


//--------------------------------------------------------------------------------------
// This shader outputs the pixel's color by modulating the texture's
// color with diffuse material color
//--------------------------------------------------------------------------------------
PS_OUTPUT RenderScenePS( VS_OUTPUT In ) 
{ 
    PS_OUTPUT Output;

    // Lookup mesh texture and modulate it with diffuse
    Output.RGBColor = tex2D(MeshTextureSampler, In.TextureUV) * In.Diffuse;

    return Output;
}


//--------------------------------------------------------------------------------------
// Renders scene 
//--------------------------------------------------------------------------------------
technique RenderScene
{
    pass P0
    {          
        VertexShader = compile vs_2_0 RenderSceneVS();
        PixelShader  = compile ps_2_0 RenderScenePS(); 
    }
}
