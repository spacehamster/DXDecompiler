//
// FX Version: fx_4_0
// Child effect (requires effect pool): false
//
// 1 local buffer(s)
//
cbuffer $Globals
{
    float4  g_MaterialAmbientColor = { 0 };// Offset:    0, size:   16
    float4  g_MaterialDiffuseColor = { 1 };// Offset:   16, size:   16
    float3  g_LightDir;                 // Offset:   32, size:   12
    float4  g_LightDiffuse;             // Offset:   48, size:   16
    float4x4 g_mWorld;                  // Offset:   64, size:   64
    float4x4 g_mWorldViewProjection;    // Offset:  128, size:   64
}

//
// 2 local object(s)
//
Texture2D g_MeshTexture;
SamplerState MeshTextureSampler
{
    Texture  = g_MeshTexture;
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(WRAP /* 1 */);
};

