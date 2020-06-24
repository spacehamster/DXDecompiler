//
// FX Version: fx_5_0
//
// 4 local buffer(s)
//
cbuffer $Globals
{
    float4  g_MaterialAmbientColor;     // Offset:    0, size:   16
    float   g_fTime;                    // Offset:   16, size:    4
    float4x4 g_mWorld;                  // Offset:   32, size:   64
    float4x4 g_mWorldViewProjection;    // Offset:   96, size:   64
    float4  defaultValue = { 1, 2, 3, 4 };// Offset:  160, size:   16
    uint    blendIndex;                 // Offset:  176, size:    4
}

tbuffer TestTBuffer
{
    float4  val1;                       // Offset:    0, size:   16
    float3  g_positions[4] = { -1, 1, 0, 1, 1, 0, -1, -1, 0, 1, -1, 0 };// Offset:   16, size:   60
    float2  g_texcoords[4] = { 0, 1, 1, 1, 0, 0, 1, 0 };// Offset:   80, size:   56
}

cbuffer TestCbuffer
{
    float4  val2;                       // Offset:    0, size:   16
}

cbuffer TestShared
{
    float4  sharedValue;                // Offset:    0, size:   16
}

//
// 20 local object(s)
//
Texture2D g_MeshTexture;
SamplerState MeshTextureSampler
<
    int blabla = 27;
    String blacksheep = "Hello There";
>
{
    AddressU = uint(WRAP /* 1 */);
    AddressV = uint(CLAMP /* 3 */);
    AddressW = uint(MIRROR /* 2 */);
    BorderColor = float4(2, 3, 4, 5);
    Filter   = uint(MIN_MAG_MIP_LINEAR /* 21 */);
    MaxAnisotropy = uint(5);
    MaxLOD   = float(6);
    MinLOD   = float(7);
    MipLODBias = float(8);
    ComparisonFunc = uint(GREATER_EQUAL /* 7 */);
};
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
    SrcBlend[1] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[2] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[3] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[4] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[5] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[6] = uint(SRC_ALPHA /* 5 */);
    SrcBlend[7] = uint(SRC_ALPHA /* 5 */);
    DestBlend[0] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[1] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[2] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[3] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[4] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[5] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[6] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlend[7] = uint(INV_SRC_ALPHA /* 6 */);
    BlendOp[0] = uint(ADD /* 1 */);
    BlendOp[1] = uint(ADD /* 1 */);
    BlendOp[2] = uint(ADD /* 1 */);
    BlendOp[3] = uint(ADD /* 1 */);
    BlendOp[4] = uint(ADD /* 1 */);
    BlendOp[5] = uint(ADD /* 1 */);
    BlendOp[6] = uint(ADD /* 1 */);
    BlendOp[7] = uint(ADD /* 1 */);
    SrcBlendAlpha[0] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[1] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[2] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[3] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[4] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[5] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[6] = uint(SRC_ALPHA /* 5 */);
    SrcBlendAlpha[7] = uint(SRC_ALPHA /* 5 */);
    DestBlendAlpha[0] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[1] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[2] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[3] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[4] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[5] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[6] = uint(INV_SRC_ALPHA /* 6 */);
    DestBlendAlpha[7] = uint(INV_SRC_ALPHA /* 6 */);
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
DepthStencilState EnableDepth
{
    DepthEnable = bool(TRUE /* 1 */);
    DepthWriteMask = uint(ALL /* 1 */);
    DepthFunc = uint(LESS_EQUAL /* 4 */);
    StencilEnable = bool(FALSE /* 0 */);
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
DepthStencilState DisableDepth
{
    DepthEnable = bool(FALSE /* 0 */);
    DepthWriteMask = uint(ZERO /* 0 */);
};
RasterizerState CullBack
{
    FillMode = uint(WIREFRAME /* 2 */);
    CullMode = uint(BACK /* 3 */);
    FrontCounterClockwise = bool(TRUE /* 1 */);
    DepthBias = uint(5);
    DepthBiasClamp = float(5.5);
    SlopeScaledDepthBias = float(6.5);
    DepthClipEnable = bool(TRUE /* 1 */);
    ScissorEnable = bool(TRUE /* 1 */);
    MultisampleEnable = bool(FALSE /* 0 */);
    AntialiasedLineEnable = bool(TRUE /* 1 */);
};
RasterizerState CullNone
{
    CullMode = uint(NONE /* 1 */);
};
DepthStencilView TestDepthStencilView;
RenderTargetView TestRenderTargetView;
Texture2D sharedTexture;
ByteAddressBuffer Buffer0;
ByteAddressBuffer Buffer1;
RWByteAddressBuffer BufferOut;
BlendState BlendArray[3]
{
    {
        BlendOp[0] = uint(ADD /* 1 */);
        BlendOp[1] = uint(ADD /* 1 */);
        BlendOp[2] = uint(ADD /* 1 */);
        BlendOp[3] = uint(ADD /* 1 */);
        BlendOp[4] = uint(ADD /* 1 */);
        BlendOp[5] = uint(ADD /* 1 */);
        BlendOp[6] = uint(ADD /* 1 */);
        BlendOp[7] = uint(ADD /* 1 */);
    },
    {
        BlendOp[0] = uint(SUBTRACT /* 2 */);
        BlendOp[1] = uint(SUBTRACT /* 2 */);
        BlendOp[2] = uint(SUBTRACT /* 2 */);
        BlendOp[3] = uint(SUBTRACT /* 2 */);
        BlendOp[4] = uint(SUBTRACT /* 2 */);
        BlendOp[5] = uint(SUBTRACT /* 2 */);
        BlendOp[6] = uint(SUBTRACT /* 2 */);
        BlendOp[7] = uint(SUBTRACT /* 2 */);
    },
    {
        BlendOp[0] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[1] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[2] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[3] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[4] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[5] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[6] = uint(REV_SUBTRACT /* 3 */);
        BlendOp[7] = uint(REV_SUBTRACT /* 3 */);
    }
};
RasterizerState RasterizerArray[3]
{
    {
        DepthBiasClamp = float(1);
    },
    {
        DepthBiasClamp = float(2);
    },
    {
        DepthBiasClamp = float(3);
    }
};
DepthStencilState DepthStencilArray[3]
{
    {
        DepthFunc = uint(NEVER /* 1 */);
    },
    {
        DepthFunc = uint(LESS /* 2 */);
    },
    {
        DepthFunc = uint(EQUAL /* 3 */);
    }
};
VertexShader TestVertexShader5 = 
    asm {
        //
        // Generated by Microsoft (R) HLSL Shader Compiler 10.1
        //
        //
        //
        // Input signature:
        //
        // Name                 Index   Mask Register SysValue  Format   Used
        // -------------------- ----- ------ -------- -------- ------- ------
        // POSITION                 0   xyzw        0     NONE   float       
        // NORMAL                   0   xyz         1     NONE   float       
        // TEXCOORD                 0   xy          2     NONE   float       
        //
        //
        // Output signature:
        //
        // Name                 Index   Mask Register SysValue  Format   Used
        // -------------------- ----- ------ -------- -------- ------- ------
        // SV_POSITION              0   xyzw        0      POS   float   xyzw
        // COLOR                    0   xyzw        1     NONE   float   xyzw
        // TEXCOORD                 0   xy          2     NONE   float   xy  
        //
        vs_5_0
        dcl_globalFlags refactoringAllowed
        dcl_output_siv o0.xyzw, position
        dcl_output o1.xyzw
        dcl_output o2.xy
        mov o0.xyzw, l(0,0,0,0)
        mov o1.xyzw, l(0,0,0,0)
        mov o2.xy, l(0,0,0,0)
        ret 
        // Approximately 4 instruction slots used
            
    };
PixelShader TestPixelShader5 = 
    asm {
        //
        // Generated by Microsoft (R) HLSL Shader Compiler 10.1
        //
        //
        //
        // Input signature:
        //
        // Name                 Index   Mask Register SysValue  Format   Used
        // -------------------- ----- ------ -------- -------- ------- ------
        // SV_POSITION              0   xyzw        0      POS   float       
        // COLOR                    0   xyzw        1     NONE   float       
        // TEXCOORD                 0   xy          2     NONE   float       
        //
        //
        // Output signature:
        //
        // Name                 Index   Mask Register SysValue  Format   Used
        // -------------------- ----- ------ -------- -------- ------- ------
        // SV_Target                0   xyzw        0   TARGET   float   xyzw
        //
        ps_5_0
        dcl_globalFlags refactoringAllowed
        dcl_output o0.xyzw
        mov o0.xyzw, l(0,0,0,0)
        ret 
        // Approximately 2 instruction slots used
            
    };
ComputeShader TestComputeShader5 = 
    asm {
        //
        // Generated by Microsoft (R) HLSL Shader Compiler 10.1
        //
        //
        // Resource Bindings:
        //
        // Name                                 Type  Format         Dim      HLSL Bind  Count
        // ------------------------------ ---------- ------- ----------- -------------- ------
        // Buffer0                           texture    byte         r/o             t0      1 
        // Buffer1                           texture    byte         r/o             t1      1 
        // BufferOut                             UAV    byte         r/w             u0      1 
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
        // no Output
        cs_5_0
        dcl_globalFlags refactoringAllowed
        dcl_resource_raw t0
        dcl_resource_raw t1
        dcl_uav_raw u0
        dcl_input vThreadID.x
        dcl_temps 3
        dcl_thread_group 1, 1, 1
        ishl r0.x, vThreadID.x, l(3)
        ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.yz, r0.x, t0.xxyx
        ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r1.xy, r0.x, t1.xyxx
        iadd r2.x, r0.y, r1.x
        add r2.y, r0.z, r1.y
        store_raw u0.xy, r0.x, r2.xyxx
        ret 
        // Approximately 7 instruction slots used
            
    };

//
// 2 groups(s)
//
fxgroup
{
    //
    // 4 technique(s)
    //
    technique11 RenderSceneWithTexture1Light10_1
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
                // POSITION                 0   xyzw        0     NONE   float       
                // NORMAL                   0   xyz         1     NONE   float       
                // TEXCOORD                 0   xy          2     NONE   float       
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // COLOR                    0   xyzw        1     NONE   float   xyzw
                // TEXCOORD                 0   xy          2     NONE   float   xy  
                //
                vs_4_0
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xyzw
                dcl_output o2.xy
                mov o0.xyzw, l(0,0,0,0)
                mov o1.xyzw, l(0,0,0,0)
                mov o2.xy, l(0,0,0,0)
                ret 
                // Approximately 4 instruction slots used
                            
            };
            PixelShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // MeshTextureSampler                sampler      NA          NA             s0      1 
                // g_MeshTexture                     texture  float4          2d             t2      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // COLOR                    0   xyzw        1     NONE   float   xyzw
                // TEXCOORD                 0   xy          2     NONE   float   xy  
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
                dcl_resource_texture2d (float,float,float,float) t2
                dcl_input_ps linear v1.xyzw
                dcl_input_ps linear v2.xy
                dcl_output o0.xyzw
                dcl_temps 1
                sample r0.xyzw, v2.xyxx, t2.xyzw, s0
                mul o0.xyzw, r0.xyzw, v1.xyzw
                ret 
                // Approximately 3 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = NoBlending;
            RasterizerState = CullNone;
            DS_StencilRef = uint(5);
            DepthStencilState = DisableDepth;
        }

    }

    technique11 RenderSceneWithTexture1Light10_2
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
                // POSITION                 0   xyzw        0     NONE   float       
                // NORMAL                   0   xyz         1     NONE   float       
                // TEXCOORD                 0   xy          2     NONE   float       
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // COLOR                    0   xyzw        1     NONE   float   xyzw
                // TEXCOORD                 0   xy          2     NONE   float   xy  
                //
                vs_4_0
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xyzw
                dcl_output o2.xy
                mov o0.xyzw, l(0,0,0,0)
                mov o1.xyzw, l(0,0,0,0)
                mov o2.xy, l(0,0,0,0)
                ret 
                // Approximately 4 instruction slots used
                            
            };
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
                // SV_POSITION              0   xyzw        0      POS   float       
                // COLOR                    0   xyzw        1     NONE   float       
                // TEXCOORD                 0   xy          2     NONE   float       
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
                mov o0.xyzw, l(0,0,0,0)
                ret 
                // Approximately 2 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = BlendArray[blendIndex];
            RasterizerState = RasterizerArray[1];
        }

    }

    technique11 RenderSceneWithTexture1Light11_1
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
                // POSITION                 0   xyzw        0     NONE   float       
                // NORMAL                   0   xyz         1     NONE   float       
                // TEXCOORD                 0   xy          2     NONE   float       
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float   xyzw
                // COLOR                    0   xyzw        1     NONE   float   xyzw
                // TEXCOORD                 0   xy          2     NONE   float   xy  
                //
                vs_5_0
                dcl_globalFlags refactoringAllowed
                dcl_output_siv o0.xyzw, position
                dcl_output o1.xyzw
                dcl_output o2.xy
                mov o0.xyzw, l(0,0,0,0)
                mov o1.xyzw, l(0,0,0,0)
                mov o2.xy, l(0,0,0,0)
                ret 
                // Approximately 4 instruction slots used
                            
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
                // MeshTextureSampler                sampler      NA          NA             s0      1 
                // g_MeshTexture                     texture  float4          2d             t2      1 
                //
                //
                //
                // Input signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_POSITION              0   xyzw        0      POS   float       
                // COLOR                    0   xyzw        1     NONE   float   xyzw
                // TEXCOORD                 0   xy          2     NONE   float   xy  
                //
                //
                // Output signature:
                //
                // Name                 Index   Mask Register SysValue  Format   Used
                // -------------------- ----- ------ -------- -------- ------- ------
                // SV_Target                0   xyzw        0   TARGET   float   xyzw
                //
                ps_5_0
                dcl_globalFlags refactoringAllowed
                dcl_sampler s0, mode_default
                dcl_resource_texture2d (float,float,float,float) t2
                dcl_input_ps linear v1.xyzw
                dcl_input_ps linear v2.xy
                dcl_output o0.xyzw
                dcl_temps 1
                sample_indexable(texture2d)(float,float,float,float) r0.xyzw, v2.xyxx, t2.xyzw, s0
                mul o0.xyzw, r0.xyzw, v1.xyzw
                ret 
                // Approximately 3 instruction slots used
                            
            };
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = NoBlending;
            RasterizerState = CullNone;
            DS_StencilRef = uint(5);
            DepthStencilState = DisableDepth;
        }

    }

    technique11 RenderSceneWithTexture1Light11_2
    {
        pass P0
        {
            VertexShader = TestVertexShader5;
            GeometryShader = NULL;
            PixelShader = TestPixelShader5;
            ComputeShader = TestComputeShader5;
            AB_BlendFactor = float4(0, 0, 0, 0);
            AB_SampleMask = uint(0xffffffff);
            BlendState = BlendArray[blendIndex];
            RasterizerState = RasterizerArray[1];
        }

    }

}

fxgroup g0
{
    //
    // 1 technique(s)
    //
    technique11 RunComputeShader
    {
        pass P0
        {
            ComputeShader = asm {
                //
                // Generated by Microsoft (R) HLSL Shader Compiler 10.1
                //
                //
                // Resource Bindings:
                //
                // Name                                 Type  Format         Dim      HLSL Bind  Count
                // ------------------------------ ---------- ------- ----------- -------------- ------
                // Buffer0                           texture    byte         r/o             t0      1 
                // Buffer1                           texture    byte         r/o             t1      1 
                // BufferOut                             UAV    byte         r/w             u0      1 
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
                // no Output
                cs_5_0
                dcl_globalFlags refactoringAllowed
                dcl_resource_raw t0
                dcl_resource_raw t1
                dcl_uav_raw u0
                dcl_input vThreadID.x
                dcl_temps 3
                dcl_thread_group 1, 1, 1
                ishl r0.x, vThreadID.x, l(3)
                ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r0.yz, r0.x, t0.xxyx
                ld_raw_indexable(raw_buffer)(mixed,mixed,mixed,mixed) r1.xy, r0.x, t1.xyxx
                iadd r2.x, r0.y, r1.x
                add r2.y, r0.z, r1.y
                store_raw u0.xy, r0.x, r2.xyxx
                ret 
                // Approximately 7 instruction slots used
                            
            };
        }

    }

}

