//
// FX Version: fx_5_0
//
// 2 local buffer(s)
//
cbuffer cbConstant
{
    float3  vLightDir = { -0.577000022, 0.577000022, -0.577000022 };// Offset:    0, size:   12
}

cbuffer cbChangesEveryFrame
{
    float4x4 World;                     // Offset:    0, size:   64
    float4x4 View;                      // Offset:   64, size:   64
    float4x4 Projection;                // Offset:  128, size:   64
}

//
// 8 local object(s)
//
Texture2D g_txDiffuse;
SamplerState samLinear
{
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(WRAP /* 1 */);
};
BlendState NoBlending
{
    BlendEnable[0] = bool(FALSE /* 0 */);
};
BlendState SrcAlphaBlendingAdd
{
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[1] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[2] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[3] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[4] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[5] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[6] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[7] = uint(SRC_ALPHA /* 5 */);
    DestBlend[0] = uint(ONE /* 2 */);
    DestBlend[1] = uint(ONE /* 2 */);
    DestBlend[2] = uint(ONE /* 2 */);
    DestBlend[3] = uint(ONE /* 2 */);
    DestBlend[4] = uint(ONE /* 2 */);
    DestBlend[5] = uint(ONE /* 2 */);
    DestBlend[6] = uint(ONE /* 2 */);
    DestBlend[7] = uint(ONE /* 2 */);
    BlendOp[0] = uint(ADD /* 1 */);
    BlendOp[1] = uint(ADD /* 1 */);
    BlendOp[2] = uint(ADD /* 1 */);
    BlendOp[3] = uint(ADD /* 1 */);
    BlendOp[4] = uint(ADD /* 1 */);
    BlendOp[5] = uint(ADD /* 1 */);
    BlendOp[6] = uint(ADD /* 1 */);
    BlendOp[7] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    SrcBlendAlpha[1] = uint(ZERO /* 1 */);
    SrcBlendAlpha[2] = uint(ZERO /* 1 */);
    SrcBlendAlpha[3] = uint(ZERO /* 1 */);
    SrcBlendAlpha[4] = uint(ZERO /* 1 */);
    SrcBlendAlpha[5] = uint(ZERO /* 1 */);
    SrcBlendAlpha[6] = uint(ZERO /* 1 */);
    SrcBlendAlpha[7] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[1] = uint(ZERO /* 1 */);
    DestBlendAlpha[2] = uint(ZERO /* 1 */);
    DestBlendAlpha[3] = uint(ZERO /* 1 */);
    DestBlendAlpha[4] = uint(ZERO /* 1 */);
    DestBlendAlpha[5] = uint(ZERO /* 1 */);
    DestBlendAlpha[6] = uint(ZERO /* 1 */);
    DestBlendAlpha[7] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    BlendOpAlpha[1] = uint(ADD /* 1 */);
    BlendOpAlpha[2] = uint(ADD /* 1 */);
    BlendOpAlpha[3] = uint(ADD /* 1 */);
    BlendOpAlpha[4] = uint(ADD /* 1 */);
    BlendOpAlpha[5] = uint(ADD /* 1 */);
    BlendOpAlpha[6] = uint(ADD /* 1 */);
    BlendOpAlpha[7] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState SrcAlphaBlendingSub
{
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[1] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[2] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[3] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[4] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[5] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[6] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[7] = uint(SRC_ALPHA /* 5 */);
    DestBlend[0] = uint(ONE /* 2 */);
    DestBlend[1] = uint(ONE /* 2 */);
    DestBlend[2] = uint(ONE /* 2 */);
    DestBlend[3] = uint(ONE /* 2 */);
    DestBlend[4] = uint(ONE /* 2 */);
    DestBlend[5] = uint(ONE /* 2 */);
    DestBlend[6] = uint(ONE /* 2 */);
    DestBlend[7] = uint(ONE /* 2 */);
    BlendOp[0] = uint(SUBTRACT /* 2 */);
    BlendOp[1] = uint(SUBTRACT /* 2 */);
    BlendOp[2] = uint(SUBTRACT /* 2 */);
    BlendOp[3] = uint(SUBTRACT /* 2 */);
    BlendOp[4] = uint(SUBTRACT /* 2 */);
    BlendOp[5] = uint(SUBTRACT /* 2 */);
    BlendOp[6] = uint(SUBTRACT /* 2 */);
    BlendOp[7] = uint(SUBTRACT /* 2 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    SrcBlendAlpha[1] = uint(ZERO /* 1 */);
    SrcBlendAlpha[2] = uint(ZERO /* 1 */);
    SrcBlendAlpha[3] = uint(ZERO /* 1 */);
    SrcBlendAlpha[4] = uint(ZERO /* 1 */);
    SrcBlendAlpha[5] = uint(ZERO /* 1 */);
    SrcBlendAlpha[6] = uint(ZERO /* 1 */);
    SrcBlendAlpha[7] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[1] = uint(ZERO /* 1 */);
    DestBlendAlpha[2] = uint(ZERO /* 1 */);
    DestBlendAlpha[3] = uint(ZERO /* 1 */);
    DestBlendAlpha[4] = uint(ZERO /* 1 */);
    DestBlendAlpha[5] = uint(ZERO /* 1 */);
    DestBlendAlpha[6] = uint(ZERO /* 1 */);
    DestBlendAlpha[7] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    BlendOpAlpha[1] = uint(ADD /* 1 */);
    BlendOpAlpha[2] = uint(ADD /* 1 */);
    BlendOpAlpha[3] = uint(ADD /* 1 */);
    BlendOpAlpha[4] = uint(ADD /* 1 */);
    BlendOpAlpha[5] = uint(ADD /* 1 */);
    BlendOpAlpha[6] = uint(ADD /* 1 */);
    BlendOpAlpha[7] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState SrcColorBlendingAdd
{
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_COLOR /* 3 */);
    SrcBlend[1] = uint(SRC_COLOR /* 3 */);
    SrcBlend[2] = uint(SRC_COLOR /* 3 */);
    SrcBlend[3] = uint(SRC_COLOR /* 3 */);
    SrcBlend[4] = uint(SRC_COLOR /* 3 */);
    SrcBlend[5] = uint(SRC_COLOR /* 3 */);
    SrcBlend[6] = uint(SRC_COLOR /* 3 */);
    SrcBlend[7] = uint(SRC_COLOR /* 3 */);
    DestBlend[0] = uint(ONE /* 2 */);
    DestBlend[1] = uint(ONE /* 2 */);
    DestBlend[2] = uint(ONE /* 2 */);
    DestBlend[3] = uint(ONE /* 2 */);
    DestBlend[4] = uint(ONE /* 2 */);
    DestBlend[5] = uint(ONE /* 2 */);
    DestBlend[6] = uint(ONE /* 2 */);
    DestBlend[7] = uint(ONE /* 2 */);
    BlendOp[0] = uint(ADD /* 1 */);
    BlendOp[1] = uint(ADD /* 1 */);
    BlendOp[2] = uint(ADD /* 1 */);
    BlendOp[3] = uint(ADD /* 1 */);
    BlendOp[4] = uint(ADD /* 1 */);
    BlendOp[5] = uint(ADD /* 1 */);
    BlendOp[6] = uint(ADD /* 1 */);
    BlendOp[7] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    SrcBlendAlpha[1] = uint(ZERO /* 1 */);
    SrcBlendAlpha[2] = uint(ZERO /* 1 */);
    SrcBlendAlpha[3] = uint(ZERO /* 1 */);
    SrcBlendAlpha[4] = uint(ZERO /* 1 */);
    SrcBlendAlpha[5] = uint(ZERO /* 1 */);
    SrcBlendAlpha[6] = uint(ZERO /* 1 */);
    SrcBlendAlpha[7] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[1] = uint(ZERO /* 1 */);
    DestBlendAlpha[2] = uint(ZERO /* 1 */);
    DestBlendAlpha[3] = uint(ZERO /* 1 */);
    DestBlendAlpha[4] = uint(ZERO /* 1 */);
    DestBlendAlpha[5] = uint(ZERO /* 1 */);
    DestBlendAlpha[6] = uint(ZERO /* 1 */);
    DestBlendAlpha[7] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    BlendOpAlpha[1] = uint(ADD /* 1 */);
    BlendOpAlpha[2] = uint(ADD /* 1 */);
    BlendOpAlpha[3] = uint(ADD /* 1 */);
    BlendOpAlpha[4] = uint(ADD /* 1 */);
    BlendOpAlpha[5] = uint(ADD /* 1 */);
    BlendOpAlpha[6] = uint(ADD /* 1 */);
    BlendOpAlpha[7] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState SrcColorBlendingSub
{
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_COLOR /* 3 */);
    SrcBlend[1] = uint(SRC_COLOR /* 3 */);
    SrcBlend[2] = uint(SRC_COLOR /* 3 */);
    SrcBlend[3] = uint(SRC_COLOR /* 3 */);
    SrcBlend[4] = uint(SRC_COLOR /* 3 */);
    SrcBlend[5] = uint(SRC_COLOR /* 3 */);
    SrcBlend[6] = uint(SRC_COLOR /* 3 */);
    SrcBlend[7] = uint(SRC_COLOR /* 3 */);
    DestBlend[0] = uint(ONE /* 2 */);
    DestBlend[1] = uint(ONE /* 2 */);
    DestBlend[2] = uint(ONE /* 2 */);
    DestBlend[3] = uint(ONE /* 2 */);
    DestBlend[4] = uint(ONE /* 2 */);
    DestBlend[5] = uint(ONE /* 2 */);
    DestBlend[6] = uint(ONE /* 2 */);
    DestBlend[7] = uint(ONE /* 2 */);
    BlendOp[0] = uint(SUBTRACT /* 2 */);
    BlendOp[1] = uint(SUBTRACT /* 2 */);
    BlendOp[2] = uint(SUBTRACT /* 2 */);
    BlendOp[3] = uint(SUBTRACT /* 2 */);
    BlendOp[4] = uint(SUBTRACT /* 2 */);
    BlendOp[5] = uint(SUBTRACT /* 2 */);
    BlendOp[6] = uint(SUBTRACT /* 2 */);
    BlendOp[7] = uint(SUBTRACT /* 2 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    SrcBlendAlpha[1] = uint(ZERO /* 1 */);
    SrcBlendAlpha[2] = uint(ZERO /* 1 */);
    SrcBlendAlpha[3] = uint(ZERO /* 1 */);
    SrcBlendAlpha[4] = uint(ZERO /* 1 */);
    SrcBlendAlpha[5] = uint(ZERO /* 1 */);
    SrcBlendAlpha[6] = uint(ZERO /* 1 */);
    SrcBlendAlpha[7] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[1] = uint(ZERO /* 1 */);
    DestBlendAlpha[2] = uint(ZERO /* 1 */);
    DestBlendAlpha[3] = uint(ZERO /* 1 */);
    DestBlendAlpha[4] = uint(ZERO /* 1 */);
    DestBlendAlpha[5] = uint(ZERO /* 1 */);
    DestBlendAlpha[6] = uint(ZERO /* 1 */);
    DestBlendAlpha[7] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    BlendOpAlpha[1] = uint(ADD /* 1 */);
    BlendOpAlpha[2] = uint(ADD /* 1 */);
    BlendOpAlpha[3] = uint(ADD /* 1 */);
    BlendOpAlpha[4] = uint(ADD /* 1 */);
    BlendOpAlpha[5] = uint(ADD /* 1 */);
    BlendOpAlpha[6] = uint(ADD /* 1 */);
    BlendOpAlpha[7] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
DepthStencilState RenderWithStencilState
{
    DepthEnable = bool(FALSE /* false */);
    DepthWriteMask = uint(ZERO /* 0 */);
    DepthFunc = uint(LESS /* 2 */);
    StencilEnable = bool(TRUE /* true */);
    StencilReadMask = byte(0xff);
    StencilWriteMask = byte(0x00);
    FrontFaceStencilFunc = uint(NOT_EQUAL /* 6 */);
    FrontFaceStencilPass = uint(KEEP /* 1 */);
    FrontFaceStencilFail = uint(ZERO /* 2 */);
    BackFaceStencilFunc = uint(NOT_EQUAL /* 6 */);
    BackFaceStencilPass = uint(KEEP /* 1 */);
    BackFaceStencilFail = uint(ZERO /* 2 */);
};

//
// 1 groups(s)
//
fxgroup
{
    //
    // 7 technique(s)
    //
    technique11 RenderScene
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyz         0     NONE   float   xyz 
                // NORMAL                   0   xyz         1     NONE   float   xyz 
                // TEXCOORD                 0   xy          2     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xyz         1     NONE   float   xyz 
                // TEXCOORD                 1   xy          2     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyz
                dcl_input v1.xyz
                dcl_input v2.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xyz
                dcl_output o2.xy
                dcl_temps 2
                mov r0.xyz, v0.xyzx
                mov r0.w, l(1.000000)
                dp4 r1.x, r0.xyzw, cb0[0].xyzw
                dp4 r1.y, r0.xyzw, cb0[1].xyzw
                dp4 r1.z, r0.xyzw, cb0[2].xyzw
                dp4 r1.w, r0.xyzw, cb0[3].xyzw
                dp4 r0.x, r1.xyzw, cb0[4].xyzw
                dp4 r0.y, r1.xyzw, cb0[5].xyzw
                dp4 r0.z, r1.xyzw, cb0[6].xyzw
                dp4 r0.w, r1.xyzw, cb0[7].xyzw
                dp4 o0.x, r0.xyzw, cb0[8].xyzw
                dp4 o0.y, r0.xyzw, cb0[9].xyzw
                dp4 o0.z, r0.xyzw, cb0[10].xyzw
                dp4 o0.w, r0.xyzw, cb0[11].xyzw
                dp3 o1.x, v1.xyzx, cb0[0].xyzx
                dp3 o1.y, v1.xyzx, cb0[1].xyzx
                dp3 o1.z, v1.xyzx, cb0[2].xyzx
                mov o2.xy, v2.xyxx
                ret 
                // Approximately 19 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbConstant
                // {
                //
                //   float3 vLightDir;                  // Offset:    0 Size:    12
                //      = 0xbf13b646 0x3f13b646 0xbf13b646 
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                // cbConstant                        cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xyz         1     NONE   float   xyz 
                // TEXCOORD                 1   xy          2     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_constantbuffer CB0[1], immediateIndexed
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xyz
                dcl_input_ps linear v2.xy
                dcl_output o0.xyzw
                dcl_temps 2
                dp3_sat r0.x, v1.xyzx, cb0[0].xyzx
                sample r1.xyzw, v2.xyxx, t0.xyzw, s0
                mul o0.xyz, r0.xxxx, r1.xyzx
                mov o0.w, l(1.000000)
                ret 
                // Approximately 5 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = NoBlending;
        }

    }

    technique11 RenderWithStencil
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                mov o0.xyzw, v0.xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 3 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = NoBlending;
            DS_StencilRef = uint(0);
            DepthStencilState = RenderWithStencilState;
        }

    }

    technique11 RenderQuadSolid
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                dcl_temps 2
                dp4 r0.x, v0.xyzw, cb0[0].xyzw
                dp4 r0.y, v0.xyzw, cb0[1].xyzw
                dp4 r0.z, v0.xyzw, cb0[2].xyzw
                dp4 r0.w, v0.xyzw, cb0[3].xyzw
                dp4 r1.x, r0.xyzw, cb0[4].xyzw
                dp4 r1.y, r0.xyzw, cb0[5].xyzw
                dp4 r1.z, r0.xyzw, cb0[6].xyzw
                dp4 r1.w, r0.xyzw, cb0[7].xyzw
                dp4 o0.x, r1.xyzw, cb0[8].xyzw
                dp4 o0.y, r1.xyzw, cb0[9].xyzw
                dp4 o0.z, r1.xyzw, cb0[10].xyzw
                dp4 o0.w, r1.xyzw, cb0[11].xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 14 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = NoBlending;
        }

    }

    technique11 RenderQuadSrcAlphaAdd
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                dcl_temps 2
                dp4 r0.x, v0.xyzw, cb0[0].xyzw
                dp4 r0.y, v0.xyzw, cb0[1].xyzw
                dp4 r0.z, v0.xyzw, cb0[2].xyzw
                dp4 r0.w, v0.xyzw, cb0[3].xyzw
                dp4 r1.x, r0.xyzw, cb0[4].xyzw
                dp4 r1.y, r0.xyzw, cb0[5].xyzw
                dp4 r1.z, r0.xyzw, cb0[6].xyzw
                dp4 r1.w, r0.xyzw, cb0[7].xyzw
                dp4 o0.x, r1.xyzw, cb0[8].xyzw
                dp4 o0.y, r1.xyzw, cb0[9].xyzw
                dp4 o0.z, r1.xyzw, cb0[10].xyzw
                dp4 o0.w, r1.xyzw, cb0[11].xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 14 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = SrcAlphaBlendingAdd;
        }

    }

    technique11 RenderQuadSrcAlphaSub
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                dcl_temps 2
                dp4 r0.x, v0.xyzw, cb0[0].xyzw
                dp4 r0.y, v0.xyzw, cb0[1].xyzw
                dp4 r0.z, v0.xyzw, cb0[2].xyzw
                dp4 r0.w, v0.xyzw, cb0[3].xyzw
                dp4 r1.x, r0.xyzw, cb0[4].xyzw
                dp4 r1.y, r0.xyzw, cb0[5].xyzw
                dp4 r1.z, r0.xyzw, cb0[6].xyzw
                dp4 r1.w, r0.xyzw, cb0[7].xyzw
                dp4 o0.x, r1.xyzw, cb0[8].xyzw
                dp4 o0.y, r1.xyzw, cb0[9].xyzw
                dp4 o0.z, r1.xyzw, cb0[10].xyzw
                dp4 o0.w, r1.xyzw, cb0[11].xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 14 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = SrcAlphaBlendingSub;
        }

    }

    technique11 RenderQuadSrcColorAdd
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                dcl_temps 2
                dp4 r0.x, v0.xyzw, cb0[0].xyzw
                dp4 r0.y, v0.xyzw, cb0[1].xyzw
                dp4 r0.z, v0.xyzw, cb0[2].xyzw
                dp4 r0.w, v0.xyzw, cb0[3].xyzw
                dp4 r1.x, r0.xyzw, cb0[4].xyzw
                dp4 r1.y, r0.xyzw, cb0[5].xyzw
                dp4 r1.z, r0.xyzw, cb0[6].xyzw
                dp4 r1.w, r0.xyzw, cb0[7].xyzw
                dp4 o0.x, r1.xyzw, cb0[8].xyzw
                dp4 o0.y, r1.xyzw, cb0[9].xyzw
                dp4 o0.z, r1.xyzw, cb0[10].xyzw
                dp4 o0.w, r1.xyzw, cb0[11].xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 14 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = SrcColorBlendingAdd;
        }

    }

    technique11 RenderQuadSrcColorSub
    {
        pass P0
        {
            VertexShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Buffer Definitions: 
                //
                // cbuffer cbChangesEveryFrame
                // {
                //
                //   float4x4 World;                    // Offset:    0 Size:    64
                //   float4x4 View;                     // Offset:   64 Size:    64
                //   float4x4 Projection;               // Offset:  128 Size:    64
                //
                // }
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // cbChangesEveryFrame               cbuffer      NA          NA            cb0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // POSITION                 0   xyzw        0     NONE   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                vs_4_0
                dcl_constantbuffer CB0[12], immediateIndexed
                dcl_input v0.xyzw
                dcl_input v1.xy
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xy
                dcl_temps 2
                dp4 r0.x, v0.xyzw, cb0[0].xyzw
                dp4 r0.y, v0.xyzw, cb0[1].xyzw
                dp4 r0.z, v0.xyzw, cb0[2].xyzw
                dp4 r0.w, v0.xyzw, cb0[3].xyzw
                dp4 r1.x, r0.xyzw, cb0[4].xyzw
                dp4 r1.y, r0.xyzw, cb0[5].xyzw
                dp4 r1.z, r0.xyzw, cb0[6].xyzw
                dp4 r1.w, r0.xyzw, cb0[7].xyzw
                dp4 o0.x, r1.xyzw, cb0[8].xyzw
                dp4 o0.y, r1.xyzw, cb0[9].xyzw
                dp4 o0.z, r1.xyzw, cb0[10].xyzw
                dp4 o0.w, r1.xyzw, cb0[11].xyzw
                mov o1.xy, v1.xyxx
                ret 
                // Approximately 14 instruction slots used
                            
            };
            GeometryShader = NULL;
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // samLinear                         sampler      NA          NA             s0      1 
                // g_txDiffuse                       texture  float4          2d             t0      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // TEXCOORD                 0   xy          1     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_4_0
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t0
                dcl_input_ps linear v1.xy
                dcl_output o0.xyzw
                sample o0.xyzw, v1.xyxx, t0.xyzw, s0
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = SrcColorBlendingSub;
        }

    }

}

