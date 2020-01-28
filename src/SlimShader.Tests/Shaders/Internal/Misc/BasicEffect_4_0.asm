//
// FX Version: fx_4_0
// Child effect (requires effect pool): false
//
// 1 local buffer(s)
//
cbuffer $Globals
{
    float4  g_MaterialAmbientColor;     // Offset:    0, size:   16
    float   g_fTime;                    // Offset:   16, size:    4
    float4x4 g_mWorld;                  // Offset:   32, size:   64
    float4x4 g_mWorldViewProjection;    // Offset:   96, size:   64
}

//
// 2 local object(s)
//
Texture2D g_MeshTexture;
SamplerState MeshTextureSampler
{
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(WRAP /* 1 */);
};

