//
// FX Version: fx_4_0
// Child effect (requires effect pool): false
//
// 1 local buffer(s)
//
cbuffer cb1
{
    float4x4 g_mWorldViewProj;          // Offset:    0, size:   64
    float4x4 g_mViewProj;               // Offset:   64, size:   64
    float4x4 g_mWorld;                  // Offset:  128, size:   64
    float3  g_vLightPos;                // Offset:  192, size:   12
    float4  g_vLightColor;              // Offset:  208, size:   16
    float4  g_vAmbient;                 // Offset:  224, size:   16
    float   g_fExtrudeAmt;              // Offset:  240, size:    4
    float   g_fExtrudeBias;             // Offset:  244, size:    4
    float4  g_vShadowColor;             // Offset:  256, size:   16
}

//
// 15 local object(s)
//
Texture2D g_txDiffuse;
SamplerState g_samLinear
{
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(WRAP /* 1 */);
};
DepthStencilState DisableDepth
{
    DepthEnable = bool(FALSE /* 0 */);
    DepthWriteMask = uint(ZERO /* 0 */);
};
DepthStencilState EnableDepth
{
    DepthEnable = bool(TRUE /* 1 */);
    DepthWriteMask = uint(ALL /* 1 */);
};
DepthStencilState TwoSidedStencil
{
    DepthEnable = bool(TRUE /* true */);
    DepthWriteMask = uint(ZERO /* 0 */);
    DepthFunc = uint(LESS /* 2 */);
    StencilEnable = bool(TRUE /* true */);
    StencilReadMask = byte(0xff);
    StencilWriteMask = byte(0xff);
    BackFaceStencilFunc = uint(ALWAYS /* 8 */);
    BackFaceStencilDepthFail = uint(INCR /* 7 */);
    BackFaceStencilPass = uint(KEEP /* 1 */);
    BackFaceStencilFail = uint(KEEP /* 1 */);
    FrontFaceStencilFunc = uint(ALWAYS /* 8 */);
    FrontFaceStencilDepthFail = uint(DECR /* 8 */);
    FrontFaceStencilPass = uint(KEEP /* 1 */);
    FrontFaceStencilFail = uint(KEEP /* 1 */);
};
DepthStencilState VolumeComplexityStencil
{
    DepthEnable = bool(TRUE /* true */);
    DepthWriteMask = uint(ZERO /* 0 */);
    DepthFunc = uint(LESS /* 2 */);
    StencilEnable = bool(TRUE /* true */);
    StencilReadMask = byte(0xff);
    StencilWriteMask = byte(0xff);
    BackFaceStencilFunc = uint(ALWAYS /* 8 */);
    BackFaceStencilDepthFail = uint(INCR /* 7 */);
    BackFaceStencilPass = uint(INCR /* 7 */);
    BackFaceStencilFail = uint(INCR /* 7 */);
    FrontFaceStencilFunc = uint(ALWAYS /* 8 */);
    FrontFaceStencilDepthFail = uint(INCR /* 7 */);
    FrontFaceStencilPass = uint(INCR /* 7 */);
    FrontFaceStencilFail = uint(INCR /* 7 */);
};
DepthStencilState ComplexityStencil
{
    DepthEnable = bool(FALSE /* false */);
    DepthWriteMask = uint(ZERO /* 0 */);
    DepthFunc = uint(LESS /* 2 */);
    StencilEnable = bool(TRUE /* true */);
    StencilReadMask = byte(0xff);
    StencilWriteMask = byte(0xff);
    BackFaceStencilFunc = uint(ALWAYS /* 8 */);
    BackFaceStencilDepthFail = uint(INCR /* 7 */);
    BackFaceStencilPass = uint(KEEP /* 1 */);
    BackFaceStencilFail = uint(KEEP /* 1 */);
    FrontFaceStencilFunc = uint(LESS_EQUAL /* 4 */);
    FrontFaceStencilDepthFail = uint(KEEP /* 1 */);
    FrontFaceStencilPass = uint(ZERO /* 2 */);
    FrontFaceStencilFail = uint(KEEP /* 1 */);
};
DepthStencilState RenderNonShadows
{
    DepthEnable = bool(TRUE /* true */);
    DepthWriteMask = uint(ZERO /* 0 */);
    DepthFunc = uint(LESS_EQUAL /* 4 */);
    StencilEnable = bool(TRUE /* true */);
    StencilReadMask = byte(0xff);
    StencilWriteMask = byte(0x00);
    FrontFaceStencilFunc = uint(EQUAL /* 3 */);
    FrontFaceStencilPass = uint(KEEP /* 1 */);
    FrontFaceStencilFail = uint(ZERO /* 2 */);
    BackFaceStencilFunc = uint(NEVER /* 1 */);
    BackFaceStencilPass = uint(ZERO /* 2 */);
    BackFaceStencilFail = uint(ZERO /* 2 */);
};
BlendState DisableFrameBuffer
{
    BlendEnable[0] = bool(FALSE /* 0 */);
    RenderTargetWriteMask[0] = byte(0x00);
};
BlendState EnableFrameBuffer
{
    BlendEnable[0] = bool(FALSE /* 0 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState NoBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(FALSE /* 0 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState AdditiveBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(ONE /* 2 */);
    DestBlend[0] = uint(ONE /* 2 */);
    BlendOp[0] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
    RenderTargetWriteMask[0] = byte(0x0f);
};
BlendState SrcAlphaBlending
{
    AlphaToCoverageEnable = bool(FALSE /* 0 */);
    BlendEnable[0] = bool(TRUE /* 1 */);
    SrcBlend[0] = uint(SRC_ALPHA /* 5 */);
    DestBlend[0] = uint(INV_SRC_ALPHA /* 6 */);
    BlendOp[0] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(ZERO /* 1 */);
    DestBlendAlpha[0] = uint(ZERO /* 1 */);
    BlendOpAlpha[0] = uint(ADD /* 1 */);
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

//
// 7 technique(s)
//
technique10 RenderSceneTextured
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // COLOR                    0   xyzw        1     NONE   float   xyzw
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[14], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xyzw
            dcl_output o2.xy
            dcl_temps 2
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            dp3 r0.x, v0.xyzx, cb0[8].xyzx
            dp3 r0.y, v0.xyzx, cb0[9].xyzx
            dp3 r0.z, v0.xyzx, cb0[10].xyzx
            add r0.xyz, -r0.xyzx, cb0[12].xyzx
            dp3 r0.w, r0.xyzx, r0.xyzx
            rsq r0.w, r0.w
            mul r0.xyz, r0.wwww, r0.xyzx
            dp3 r1.x, v1.xyzx, cb0[8].xyzx
            dp3 r1.y, v1.xyzx, cb0[9].xyzx
            dp3 r1.z, v1.xyzx, cb0[10].xyzx
            dp3_sat r0.w, r0.xyzx, r1.xyzx
            dp3 r0.x, r0.xyzx, r0.xyzx
            mul r1.xyzw, r0.wwww, cb0[13].xyzw
            mul r1.xyzw, r1.xyzw, l(0.096000, 0.096000, 0.096000, 0.096000)
            div o1.xyzw, r1.xyzw, r0.xxxx
            mov o2.xy, v2.xyxx
            ret 
            // Approximately 23 instruction slots used
                    
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
            // COLOR                    0   xyzw        1     NONE   float       
            // TEXTURE                  0   xy          2     NONE   float   xy  
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
            dcl_input_ps linear v2.xy
            dcl_output o0.xyzw
            sample o0.xyzw, v2.xyxx, t0.xyzw, s0
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(0);
        DepthStencilState = EnableDepth;
        RasterizerState = EnableCulling;
    }

}

technique10 RenderSceneLit
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // COLOR                    0   xyzw        1     NONE   float   xyzw
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[14], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xyzw
            dcl_output o2.xy
            dcl_temps 2
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            dp3 r0.x, v0.xyzx, cb0[8].xyzx
            dp3 r0.y, v0.xyzx, cb0[9].xyzx
            dp3 r0.z, v0.xyzx, cb0[10].xyzx
            add r0.xyz, -r0.xyzx, cb0[12].xyzx
            dp3 r0.w, r0.xyzx, r0.xyzx
            rsq r0.w, r0.w
            mul r0.xyz, r0.wwww, r0.xyzx
            dp3 r1.x, v1.xyzx, cb0[8].xyzx
            dp3 r1.y, v1.xyzx, cb0[9].xyzx
            dp3 r1.z, v1.xyzx, cb0[10].xyzx
            dp3_sat r0.w, r0.xyzx, r1.xyzx
            dp3 r0.x, r0.xyzx, r0.xyzx
            mul r1.xyzw, r0.wwww, cb0[13].xyzw
            mul r1.xyzw, r1.xyzw, l(0.096000, 0.096000, 0.096000, 0.096000)
            div o1.xyzw, r1.xyzw, r0.xxxx
            mov o2.xy, v2.xyxx
            ret 
            // Approximately 23 instruction slots used
                    
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
            // COLOR                    0   xyzw        1     NONE   float   xyzw
            // TEXTURE                  0   xy          2     NONE   float   xy  
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
            dcl_input_ps linear v1.xyzw
            dcl_input_ps linear v2.xy
            dcl_output o0.xyzw
            dcl_temps 1
            sample r0.xyzw, v2.xyxx, t0.xyzw, s0
            mul o0.xyzw, r0.xyzw, v1.xyzw
            ret 
            // Approximately 3 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = AdditiveBlending;
        DS_StencilRef = uint(0);
        DepthStencilState = RenderNonShadows;
        RasterizerState = EnableCulling;
    }

}

technique10 RenderShadow
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float   xyz 
            //
            vs_4_0
            dcl_constantbuffer CB0[11], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_output o0.xyz
            dcl_output o1.xyz
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[8].xyzw
            dp4 o0.y, r0.xyzw, cb0[9].xyzw
            dp4 o0.z, r0.xyzw, cb0[10].xyzw
            dp3 o1.x, v1.xyzx, cb0[8].xyzx
            dp3 o1.y, v1.xyzx, cb0[9].xyzx
            dp3 o1.z, v1.xyzx, cb0[10].xyzx
            ret 
            // Approximately 9 instruction slots used
                    
        };
        GeometryShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            gs_4_0
            dcl_constantbuffer CB0[16], immediateIndexed
            dcl_input v[6][0].xyz
            dcl_input v[6][1].xyz
            dcl_temps 7
            dcl_inputprimitive triangleadj 
            dcl_outputtopology trianglestrip 
            dcl_output_siv o0.xyzw, position
            dcl_maxout 18
            add r0.xyz, -v[0][0].zxyz, v[2][0].zxyz
            add r1.xyz, -v[0][0].yzxy, v[4][0].yzxy
            mul r2.xyz, r0.xyzx, r1.xyzx
            mad r0.xyz, r0.zxyz, r1.yzxy, -r2.xyzx
            add r1.xyz, cb0[12].xyzx, -v[0][0].xyzx
            dp3 r0.x, r0.xyzx, r1.xyzx
            lt r0.x, l(0.000000), r0.x
            if_nz r0.x
              add r0.xyz, -cb0[12].xyzx, v[0][0].xyzx
              dp3 r0.w, r0.xyzx, r0.xyzx
              rsq r0.w, r0.w
              mul r0.xyz, r0.wwww, r0.xyzx
              add r1.xyz, -cb0[12].xyzx, v[2][0].xyzx
              dp3 r0.w, r1.xyzx, r1.xyzx
              rsq r0.w, r0.w
              mul r1.xyz, r0.wwww, r1.xyzx
              mad r2.xyz, cb0[15].yyyy, r0.xyzx, v[0][0].xyzx
              mad r0.xyz, cb0[15].xxxx, r0.xyzx, v[0][0].xyzx
              mad r3.xyz, cb0[15].yyyy, r1.xyzx, v[2][0].xyzx
              mad r1.xyz, cb0[15].xxxx, r1.xyzx, v[2][0].xyzx
              mov r2.w, l(1.000000)
              dp4 r4.x, r2.xyzw, cb0[4].xyzw
              dp4 r4.y, r2.xyzw, cb0[5].xyzw
              dp4 r4.z, r2.xyzw, cb0[6].xyzw
              dp4 r2.x, r2.xyzw, cb0[7].xyzw
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov r0.w, l(1.000000)
              dp4 r2.y, r0.xyzw, cb0[4].xyzw
              dp4 r2.z, r0.xyzw, cb0[5].xyzw
              dp4 r2.w, r0.xyzw, cb0[6].xyzw
              dp4 r0.x, r0.xyzw, cb0[7].xyzw
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              mov r3.w, l(1.000000)
              dp4 r0.y, r3.xyzw, cb0[4].xyzw
              dp4 r0.z, r3.xyzw, cb0[5].xyzw
              dp4 r0.w, r3.xyzw, cb0[6].xyzw
              dp4 r3.x, r3.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov r1.w, l(1.000000)
              dp4 r3.y, r1.xyzw, cb0[4].xyzw
              dp4 r3.z, r1.xyzw, cb0[5].xyzw
              dp4 r3.w, r1.xyzw, cb0[6].xyzw
              dp4 r1.x, r1.xyzw, cb0[7].xyzw
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              cut 
              add r1.yzw, -cb0[12].xxyz, v[4][0].xxyz
              dp3 r4.w, r1.yzwy, r1.yzwy
              rsq r4.w, r4.w
              mul r1.yzw, r1.yyzw, r4.wwww
              mad r5.xyz, cb0[15].yyyy, r1.yzwy, v[4][0].xyzx
              mad r6.xyz, cb0[15].xxxx, r1.yzwy, v[4][0].xyzx
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              mov r5.w, l(1.000000)
              dp4 r0.y, r5.xyzw, cb0[4].xyzw
              dp4 r0.z, r5.xyzw, cb0[5].xyzw
              dp4 r0.w, r5.xyzw, cb0[6].xyzw
              dp4 r1.x, r5.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov r6.w, l(1.000000)
              dp4 r1.y, r6.xyzw, cb0[4].xyzw
              dp4 r1.z, r6.xyzw, cb0[5].xyzw
              dp4 r1.w, r6.xyzw, cb0[6].xyzw
              dp4 r3.x, r6.xyzw, cb0[7].xyzw
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              cut 
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(0)
              loop 
                ige r1.y, r1.x, l(6)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].yyyy, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(2)
              endloop 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(4)
              loop 
                ilt r1.y, r1.x, l(0)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].xxxx, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(-2)
              endloop 
              cut 
            endif 
            ret 
            // Approximately 165 instruction slots used
                    
        };
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_constantbuffer CB0[17], immediateIndexed
            dcl_output o0.xyzw
            mov o0.xyz, cb0[16].xyzx
            mov o0.w, l(0.100000)
            ret 
            // Approximately 3 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = DisableFrameBuffer;
        DS_StencilRef = uint(1);
        DepthStencilState = TwoSidedStencil;
        RasterizerState = DisableCulling;
    }

}

technique10 RenderSceneAmbient
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            // COLOR                    0   xyzw        1     NONE   float   xyzw
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            vs_4_0
            dcl_constantbuffer CB0[14], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_input v2.xy
            dcl_output_siv o0.xyzw, position
            dcl_output o1.xyzw
            dcl_output o2.xy
            dcl_temps 2
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[0].xyzw
            dp4 o0.y, r0.xyzw, cb0[1].xyzw
            dp4 o0.z, r0.xyzw, cb0[2].xyzw
            dp4 o0.w, r0.xyzw, cb0[3].xyzw
            dp3 r0.x, v0.xyzx, cb0[8].xyzx
            dp3 r0.y, v0.xyzx, cb0[9].xyzx
            dp3 r0.z, v0.xyzx, cb0[10].xyzx
            add r0.xyz, -r0.xyzx, cb0[12].xyzx
            dp3 r0.w, r0.xyzx, r0.xyzx
            rsq r0.w, r0.w
            mul r0.xyz, r0.wwww, r0.xyzx
            dp3 r1.x, v1.xyzx, cb0[8].xyzx
            dp3 r1.y, v1.xyzx, cb0[9].xyzx
            dp3 r1.z, v1.xyzx, cb0[10].xyzx
            dp3_sat r0.w, r0.xyzx, r1.xyzx
            dp3 r0.x, r0.xyzx, r0.xyzx
            mul r1.xyzw, r0.wwww, cb0[13].xyzw
            mul r1.xyzw, r1.xyzw, l(0.096000, 0.096000, 0.096000, 0.096000)
            div o1.xyzw, r1.xyzw, r0.xxxx
            mov o2.xy, v2.xyxx
            ret 
            // Approximately 23 instruction slots used
                    
        };
        GeometryShader = NULL;
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // g_samLinear                       sampler      NA          NA             s0      1 
            // g_txDiffuse                       texture  float4          2d             t0      1 
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            // COLOR                    0   xyzw        1     NONE   float       
            // TEXTURE                  0   xy          2     NONE   float   xy  
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_constantbuffer CB0[15], immediateIndexed
            dcl_sampler s0, mode_default
            dcl_resource_texture2d (float,float,float,float) t0
            dcl_input_ps linear v2.xy
            dcl_output o0.xyzw
            dcl_temps 1
            sample r0.xyzw, v2.xyxx, t0.xyzw, s0
            mul o0.xyzw, r0.xyzw, cb0[14].xyzw
            ret 
            // Approximately 3 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(0);
        DepthStencilState = EnableDepth;
        RasterizerState = EnableCulling;
    }

}

technique10 ShowShadow
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float   xyz 
            //
            vs_4_0
            dcl_constantbuffer CB0[11], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_output o0.xyz
            dcl_output o1.xyz
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[8].xyzw
            dp4 o0.y, r0.xyzw, cb0[9].xyzw
            dp4 o0.z, r0.xyzw, cb0[10].xyzw
            dp3 o1.x, v1.xyzx, cb0[8].xyzx
            dp3 o1.y, v1.xyzx, cb0[9].xyzx
            dp3 o1.z, v1.xyzx, cb0[10].xyzx
            ret 
            // Approximately 9 instruction slots used
                    
        };
        GeometryShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            gs_4_0
            dcl_constantbuffer CB0[16], immediateIndexed
            dcl_input v[6][0].xyz
            dcl_input v[6][1].xyz
            dcl_temps 7
            dcl_inputprimitive triangleadj 
            dcl_outputtopology trianglestrip 
            dcl_output_siv o0.xyzw, position
            dcl_maxout 18
            add r0.xyz, -v[0][0].zxyz, v[2][0].zxyz
            add r1.xyz, -v[0][0].yzxy, v[4][0].yzxy
            mul r2.xyz, r0.xyzx, r1.xyzx
            mad r0.xyz, r0.zxyz, r1.yzxy, -r2.xyzx
            add r1.xyz, cb0[12].xyzx, -v[0][0].xyzx
            dp3 r0.x, r0.xyzx, r1.xyzx
            lt r0.x, l(0.000000), r0.x
            if_nz r0.x
              add r0.xyz, -cb0[12].xyzx, v[0][0].xyzx
              dp3 r0.w, r0.xyzx, r0.xyzx
              rsq r0.w, r0.w
              mul r0.xyz, r0.wwww, r0.xyzx
              add r1.xyz, -cb0[12].xyzx, v[2][0].xyzx
              dp3 r0.w, r1.xyzx, r1.xyzx
              rsq r0.w, r0.w
              mul r1.xyz, r0.wwww, r1.xyzx
              mad r2.xyz, cb0[15].yyyy, r0.xyzx, v[0][0].xyzx
              mad r0.xyz, cb0[15].xxxx, r0.xyzx, v[0][0].xyzx
              mad r3.xyz, cb0[15].yyyy, r1.xyzx, v[2][0].xyzx
              mad r1.xyz, cb0[15].xxxx, r1.xyzx, v[2][0].xyzx
              mov r2.w, l(1.000000)
              dp4 r4.x, r2.xyzw, cb0[4].xyzw
              dp4 r4.y, r2.xyzw, cb0[5].xyzw
              dp4 r4.z, r2.xyzw, cb0[6].xyzw
              dp4 r2.x, r2.xyzw, cb0[7].xyzw
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov r0.w, l(1.000000)
              dp4 r2.y, r0.xyzw, cb0[4].xyzw
              dp4 r2.z, r0.xyzw, cb0[5].xyzw
              dp4 r2.w, r0.xyzw, cb0[6].xyzw
              dp4 r0.x, r0.xyzw, cb0[7].xyzw
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              mov r3.w, l(1.000000)
              dp4 r0.y, r3.xyzw, cb0[4].xyzw
              dp4 r0.z, r3.xyzw, cb0[5].xyzw
              dp4 r0.w, r3.xyzw, cb0[6].xyzw
              dp4 r3.x, r3.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov r1.w, l(1.000000)
              dp4 r3.y, r1.xyzw, cb0[4].xyzw
              dp4 r3.z, r1.xyzw, cb0[5].xyzw
              dp4 r3.w, r1.xyzw, cb0[6].xyzw
              dp4 r1.x, r1.xyzw, cb0[7].xyzw
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              cut 
              add r1.yzw, -cb0[12].xxyz, v[4][0].xxyz
              dp3 r4.w, r1.yzwy, r1.yzwy
              rsq r4.w, r4.w
              mul r1.yzw, r1.yyzw, r4.wwww
              mad r5.xyz, cb0[15].yyyy, r1.yzwy, v[4][0].xyzx
              mad r6.xyz, cb0[15].xxxx, r1.yzwy, v[4][0].xyzx
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              mov r5.w, l(1.000000)
              dp4 r0.y, r5.xyzw, cb0[4].xyzw
              dp4 r0.z, r5.xyzw, cb0[5].xyzw
              dp4 r0.w, r5.xyzw, cb0[6].xyzw
              dp4 r1.x, r5.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov r6.w, l(1.000000)
              dp4 r1.y, r6.xyzw, cb0[4].xyzw
              dp4 r1.z, r6.xyzw, cb0[5].xyzw
              dp4 r1.w, r6.xyzw, cb0[6].xyzw
              dp4 r3.x, r6.xyzw, cb0[7].xyzw
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              cut 
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(0)
              loop 
                ige r1.y, r1.x, l(6)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].yyyy, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(2)
              endloop 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(4)
              loop 
                ilt r1.y, r1.x, l(0)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].xxxx, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(-2)
              endloop 
              cut 
            endif 
            ret 
            // Approximately 165 instruction slots used
                    
        };
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_constantbuffer CB0[17], immediateIndexed
            dcl_output o0.xyzw
            mov o0.xyz, cb0[16].xyzx
            mov o0.w, l(0.100000)
            ret 
            // Approximately 3 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = SrcAlphaBlending;
        DS_StencilRef = uint(1);
        DepthStencilState = TwoSidedStencil;
        RasterizerState = DisableCulling;
    }

}

technique10 RenderShadowVolumeComplexity
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
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POSITION                 0   xyz         0     NONE   float   xyz 
            // NORMAL                   0   xyz         1     NONE   float   xyz 
            // TEXTURE                  0   xy          2     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float   xyz 
            //
            vs_4_0
            dcl_constantbuffer CB0[11], immediateIndexed
            dcl_input v0.xyz
            dcl_input v1.xyz
            dcl_output o0.xyz
            dcl_output o1.xyz
            dcl_temps 1
            mov r0.xyz, v0.xyzx
            mov r0.w, l(1.000000)
            dp4 o0.x, r0.xyzw, cb0[8].xyzw
            dp4 o0.y, r0.xyzw, cb0[9].xyzw
            dp4 o0.z, r0.xyzw, cb0[10].xyzw
            dp3 o1.x, v1.xyzx, cb0[8].xyzx
            dp3 o1.y, v1.xyzx, cb0[9].xyzx
            dp3 o1.z, v1.xyzx, cb0[10].xyzx
            ret 
            // Approximately 9 instruction slots used
                    
        };
        GeometryShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16 [unused]
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // POS                      0   xyz         0     NONE   float   xyz 
            // TEXTURE                  0   xyz         1     NONE   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            gs_4_0
            dcl_constantbuffer CB0[16], immediateIndexed
            dcl_input v[6][0].xyz
            dcl_input v[6][1].xyz
            dcl_temps 7
            dcl_inputprimitive triangleadj 
            dcl_outputtopology trianglestrip 
            dcl_output_siv o0.xyzw, position
            dcl_maxout 18
            add r0.xyz, -v[0][0].zxyz, v[2][0].zxyz
            add r1.xyz, -v[0][0].yzxy, v[4][0].yzxy
            mul r2.xyz, r0.xyzx, r1.xyzx
            mad r0.xyz, r0.zxyz, r1.yzxy, -r2.xyzx
            add r1.xyz, cb0[12].xyzx, -v[0][0].xyzx
            dp3 r0.x, r0.xyzx, r1.xyzx
            lt r0.x, l(0.000000), r0.x
            if_nz r0.x
              add r0.xyz, -cb0[12].xyzx, v[0][0].xyzx
              dp3 r0.w, r0.xyzx, r0.xyzx
              rsq r0.w, r0.w
              mul r0.xyz, r0.wwww, r0.xyzx
              add r1.xyz, -cb0[12].xyzx, v[2][0].xyzx
              dp3 r0.w, r1.xyzx, r1.xyzx
              rsq r0.w, r0.w
              mul r1.xyz, r0.wwww, r1.xyzx
              mad r2.xyz, cb0[15].yyyy, r0.xyzx, v[0][0].xyzx
              mad r0.xyz, cb0[15].xxxx, r0.xyzx, v[0][0].xyzx
              mad r3.xyz, cb0[15].yyyy, r1.xyzx, v[2][0].xyzx
              mad r1.xyz, cb0[15].xxxx, r1.xyzx, v[2][0].xyzx
              mov r2.w, l(1.000000)
              dp4 r4.x, r2.xyzw, cb0[4].xyzw
              dp4 r4.y, r2.xyzw, cb0[5].xyzw
              dp4 r4.z, r2.xyzw, cb0[6].xyzw
              dp4 r2.x, r2.xyzw, cb0[7].xyzw
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov r0.w, l(1.000000)
              dp4 r2.y, r0.xyzw, cb0[4].xyzw
              dp4 r2.z, r0.xyzw, cb0[5].xyzw
              dp4 r2.w, r0.xyzw, cb0[6].xyzw
              dp4 r0.x, r0.xyzw, cb0[7].xyzw
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              mov r3.w, l(1.000000)
              dp4 r0.y, r3.xyzw, cb0[4].xyzw
              dp4 r0.z, r3.xyzw, cb0[5].xyzw
              dp4 r0.w, r3.xyzw, cb0[6].xyzw
              dp4 r3.x, r3.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov r1.w, l(1.000000)
              dp4 r3.y, r1.xyzw, cb0[4].xyzw
              dp4 r3.z, r1.xyzw, cb0[5].xyzw
              dp4 r3.w, r1.xyzw, cb0[6].xyzw
              dp4 r1.x, r1.xyzw, cb0[7].xyzw
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              cut 
              add r1.yzw, -cb0[12].xxyz, v[4][0].xxyz
              dp3 r4.w, r1.yzwy, r1.yzwy
              rsq r4.w, r4.w
              mul r1.yzw, r1.yyzw, r4.wwww
              mad r5.xyz, cb0[15].yyyy, r1.yzwy, v[4][0].xyzx
              mad r6.xyz, cb0[15].xxxx, r1.yzwy, v[4][0].xyzx
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r3.y
              mov o0.y, r3.z
              mov o0.z, r3.w
              mov o0.w, r1.x
              emit 
              mov r5.w, l(1.000000)
              dp4 r0.y, r5.xyzw, cb0[4].xyzw
              dp4 r0.z, r5.xyzw, cb0[5].xyzw
              dp4 r0.w, r5.xyzw, cb0[6].xyzw
              dp4 r1.x, r5.xyzw, cb0[7].xyzw
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov r6.w, l(1.000000)
              dp4 r1.y, r6.xyzw, cb0[4].xyzw
              dp4 r1.z, r6.xyzw, cb0[5].xyzw
              dp4 r1.w, r6.xyzw, cb0[6].xyzw
              dp4 r3.x, r6.xyzw, cb0[7].xyzw
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              cut 
              mov o0.x, r0.y
              mov o0.y, r0.z
              mov o0.z, r0.w
              mov o0.w, r1.x
              emit 
              mov o0.x, r1.y
              mov o0.y, r1.z
              mov o0.z, r1.w
              mov o0.w, r3.x
              emit 
              mov o0.x, r4.x
              mov o0.y, r4.y
              mov o0.z, r4.z
              mov o0.w, r2.x
              emit 
              mov o0.x, r2.y
              mov o0.y, r2.z
              mov o0.z, r2.w
              mov o0.w, r0.x
              emit 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(0)
              loop 
                ige r1.y, r1.x, l(6)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].yyyy, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(2)
              endloop 
              cut 
              mov r0.w, l(1.000000)
              mov r1.x, l(4)
              loop 
                ilt r1.y, r1.x, l(0)
                breakc_nz r1.y
                add r1.yzw, -cb0[12].xxyz, v[r1.x + 0][0].xxyz
                dp3 r2.x, r1.yzwy, r1.yzwy
                rsq r2.x, r2.x
                mul r1.yzw, r1.yyzw, r2.xxxx
                mad r0.xyz, cb0[15].xxxx, r1.yzwy, v[r1.x + 0][0].xyzx
                dp4 r1.y, r0.xyzw, cb0[4].xyzw
                dp4 r1.z, r0.xyzw, cb0[5].xyzw
                dp4 r1.w, r0.xyzw, cb0[6].xyzw
                dp4 r0.x, r0.xyzw, cb0[7].xyzw
                mov o0.x, r1.y
                mov o0.y, r1.z
                mov o0.z, r1.w
                mov o0.w, r0.x
                emit 
                iadd r1.x, r1.x, l(-2)
              endloop 
              cut 
            endif 
            ret 
            // Approximately 165 instruction slots used
                    
        };
        PixelShader = asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            //
            // Buffer Definitions: 
            //
            // cbuffer cb1
            // {
            //
            //   float4x4 g_mWorldViewProj;         // Offset:    0 Size:    64 [unused]
            //   float4x4 g_mViewProj;              // Offset:   64 Size:    64 [unused]
            //   float4x4 g_mWorld;                 // Offset:  128 Size:    64 [unused]
            //   float3 g_vLightPos;                // Offset:  192 Size:    12 [unused]
            //   float4 g_vLightColor;              // Offset:  208 Size:    16 [unused]
            //   float4 g_vAmbient;                 // Offset:  224 Size:    16 [unused]
            //   float g_fExtrudeAmt;               // Offset:  240 Size:     4 [unused]
            //   float g_fExtrudeBias;              // Offset:  244 Size:     4 [unused]
            //   float4 g_vShadowColor;             // Offset:  256 Size:    16
            //
            // }
            //
            //
            // Resource Bindings:
            //
            // Name                                 Type  Format         Dim      HLSL Bind  Count
            // ------------------------------ ---------- ------- ----------- -------------- ------
            // cb1                               cbuffer      NA          NA            cb0      1 
            //
            //
            //
            // Input signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float       
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_constantbuffer CB0[17], immediateIndexed
            dcl_output o0.xyzw
            mov o0.xyz, cb0[16].xyzx
            mov o0.w, l(0.100000)
            ret 
            // Approximately 3 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(1);
        DepthStencilState = VolumeComplexityStencil;
        RasterizerState = DisableCulling;
    }

}

technique10 RenderComplexity
{
    pass p0
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,1.000000,1.000000,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(71);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p1
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0,0,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(51);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p2
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0.500000,0,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(41);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p3
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,1.000000,0,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(31);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p4
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(0,1.000000,0,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(21);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p5
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(0,1.000000,1.000000,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(11);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p6
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(0,0,1.000000,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(6);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p7
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0,1.000000,1.000000)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = NoBlending;
        DS_StencilRef = uint(1);
        DepthStencilState = ComplexityStencil;
        RasterizerState = EnableCulling;
    }

    pass p8
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
            // POSITION                 0   xyz         0     NONE   float   xyz 
            //
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Position              0   xyzw        0      POS   float   xyzw
            //
            vs_4_0
            dcl_input v0.xyz
            dcl_output_siv o0.xyzw, position
            mov o0.xyz, v0.xyzx
            mov o0.w, l(1.000000)
            ret 
            // Approximately 3 instruction slots used
                    
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
            // no Input
            //
            // Output signature:
            //
            // Name                 Index   Mask Register SysValue  Format   Used
            // -------------------- ----- ------ -------- -------- ------- ------
            // SV_Target                0   xyzw        0   TARGET   float   xyzw
            //
            ps_4_0
            dcl_output o0.xyzw
            mov o0.xyzw, l(1.000000,0,0,0)
            ret 
            // Approximately 2 instruction slots used
                    
        };
        AB_BlendFactor = float4(0, 0, 0, 0);
        AB_SampleMask = uint(0xffffffff);
        BlendState = DisableFrameBuffer;
        DS_StencilRef = uint(0);
        DepthStencilState = DisableDepth;
        RasterizerState = DisableCulling;
    }

}

