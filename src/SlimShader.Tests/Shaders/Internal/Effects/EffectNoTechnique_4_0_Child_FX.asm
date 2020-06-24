//
// FX Version: fx_4_0
// Child effect (requires effect pool): true
//
// 3 local buffer(s)
//
cbuffer $Globals
{
    float4  g_MaterialAmbientColor;     // Offset:    0, size:   16
    float   g_fTime;                    // Offset:   16, size:    4
    float4x4 g_mWorld;                  // Offset:   32, size:   64
    float4x4 g_mWorldViewProjection;    // Offset:   96, size:   64
}

cbuffer ShaderedBuffer : register(b5)
{
    float4  val100;                     // Offset:    0, size:   16
    uint3   val101;                     // Offset:   16, size:   12
}

tbuffer TextureBuffer
{
    float4  tVal1;                      // Offset:    0, size:   16
    float4  tVal2;                      // Offset:   16, size:   16
}

//
// 9 local object(s)
//
Texture2D g_MeshTexture;
SamplerState MeshTextureSampler
{
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(WRAP /* 1 */);
};
Buffer  g_ParticleBuffer;
BlendState NoBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(FALSE /* 0 */);
};
BlendState PaintBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_ALPHA /* 5 */);
    DestBlend[0] = uint(INV_SRC_ALPHA /* 6 */);
    BlendOp[0] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(SRC_ALPHA /* 5 */);
    DestBlendAlpha[0] = uint(INV_SRC_ALPHA /* 6 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
DepthStencilState EnableDepth
{
    DepthEnable = bool(TRUE /* 1 */);
    DepthWriteMask = uint(ALL /* 1 */);
};
DepthStencilState DisableDepth
{
    DepthEnable = bool(FALSE /* 0 */);
    DepthWriteMask = uint(ZERO /* 0 */);
};
RasterizerState CullBack
{
    CullMode = uint(BACK /* 3 */);
};
RasterizerState CullNone
{
    CullMode = uint(NONE /* 1 */);
};

//
// 1 shared buffer(s)
//
cbuffer ShaderedBuffer
{
    float4  val1;                       // Offset:    0, size:   16
    float4x4 val2;                      // Offset:   16, size:   64
    float4x4 val3;                      // Offset:   80, size:   64
    uint    val4;                       // Offset:  144, size:    4
    uint2   val5;                       // Offset:  148, size:    8
    float4  val6[4];                    // Offset:  160, size:   64
}

//
// 2 shared object(s)
//
SamplerState SharedSampler;
Texture2D SharedTexture;

