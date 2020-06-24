//
// FX Version: fx_4_1
// Child effect (requires effect pool): false
//
// 1 local buffer(s)
//
cbuffer cb0
{
    float4x4 g_mWorldViewProj;          // Offset:    0, size:   64
}

//
// 10 local object(s)
//
Texture2D g_txDiffuse;
SamplerState g_samLinear
{
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    AddressU = uint(CLAMP /* 3 */);
    AddressV = uint(CLAMP /* 3 */);
};
BlendState OccTestBlendState
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
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
    BlendEnable[0] = bool(FALSE /* 0 */);
    RenderTargetWriteMask[0] = byte(0x00);
};
BlendState AlphaBlendState
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
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
RasterizerState DisableCulling
{
    CullMode = uint(NONE /* 1 */);
};
RasterizerState EnableCulling
{
    CullMode = uint(BACK /* 3 */);
};
DepthStencilState DisableDepthTestWrite
{
    DepthEnable = bool(FALSE /* 0 */);
    DepthWriteMask = uint(ZERO /* 0 */);
};
DepthStencilState DisableDepthWrite
{
    DepthEnable = bool(TRUE /* 1 */);
    DepthWriteMask = uint(ZERO /* 0 */);
};
DepthStencilState EnableDepthTestWrite
{
    DepthEnable = bool(TRUE /* 1 */);
    DepthWriteMask = uint(ALL /* 1 */);
};
BlendState NoBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(FALSE /* 0 */);
};

//
// 3 technique(s)
//
technique10 RenderTextured
{
    pass p0
    {
        VertexShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb0
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb0                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float       
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // TEXTURE                  0   xy          1     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[4], immediateIndexed
            dcl_input v0.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xy
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            mov o1.xy, v2.xyxx
            ret 
            // Approximately 8 instruction slots used
                    
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
            // g_samLinear                       sampler      NA          NA             s0      1 
            // g_txDiffuse                       texture  float4          2d             t0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            // TEXTURE                  0   xy          1     NONE   float   xy  
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
        DepthStencilState = EnableDepthTestWrite;
        RasterizerState = EnableCulling;
    }

}

technique10 RenderOnTop
{
    pass p0
    {
        VertexShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb0
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb0                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float       
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // TEXTURE                  0   xy          1     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[4], immediateIndexed
            dcl_input v0.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xy
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            mov o1.xy, v2.xyxx
            ret 
            // Approximately 8 instruction slots used
                    
        };
        GeometryShader = NULL;
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            // TEXTURE                  0   xy          1     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0,0,0.500000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = AlphaBlendState;
        DS_StencilRef = uint(0);
        DepthStencilState = DisableDepthTestWrite;
        RasterizerState = EnableCulling;
    }

}

technique10 RenderOccluder
{
    pass p0
    {
        VertexShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb0
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb0                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float       
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // TEXTURE                  0   xy          1     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[4], immediateIndexed
            dcl_input v0.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xy
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            mov o1.xy, v2.xyxx
            ret 
            // Approximately 8 instruction slots used
                    
        };
        GeometryShader = NULL;
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            // TEXTURE                  0   xy          1     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0,0,0.500000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = OccTestBlendState;
        DS_StencilRef = uint(0);
        DepthStencilState = DisableDepthWrite;
        RasterizerState = DisableCulling;
    }

}

