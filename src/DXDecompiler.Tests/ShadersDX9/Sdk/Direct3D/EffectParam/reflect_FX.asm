
//listing of all techniques and passes with embedded asm listings 

technique RenderScene
{
    pass P0
    {
        vertexshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4x4 g_mView;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_mView      c0       4
            //
            
                preshader
                mul r0.x, c2.w, c3.z
                mul r0.y, c2.y, c3.w
                mul r0.z, c2.z, c3.y
                neg r1.xyz, r0.xyz
                mul r0.x, c2.z, c3.w
                mul r0.y, c2.w, c3.y
                mul r0.z, c2.y, c3.z
                add r2.xyz, r1.xyz, r0.xyz
                dot r0.xyz, r2.xyz, c1.yzw
                mul r1.x, c2.z, c3.w
                mul r1.y, c2.w, c3.x
                mul r1.z, c2.x, c3.z
                neg r2.xyz, r1.xyz
                mul r1.x, c2.w, c3.z
                mul r1.y, c2.x, c3.w
                mul r1.z, c2.z, c3.x
                add r3.xyz, r2.xyz, r1.xyz
                d3ds_dotswiz r0.y, r3.x, r3.y, r3.z, c1.x, c1.z, c1.w
                mul r1.x, c2.w, c3.y
                mul r1.y, c2.x, c3.w
                mul r1.z, c2.y, c3.x
                neg r2.xyz, r1.xyz
                mul r1.x, c2.y, c3.w
                mul r1.y, c2.w, c3.x
                mul r1.z, c2.x, c3.y
                add r3.xyz, r2.xyz, r1.xyz
                d3ds_dotswiz r0.z, r3.x, r3.y, r3.z, c1.x, c1.y, c1.w
                mul r1.x, c2.y, c3.z
                mul r1.y, c2.z, c3.x
                mul r1.z, c2.x, c3.y
                neg r2.xyz, r1.xyz
                mul r1.x, c2.z, c3.y
                mul r1.y, c2.x, c3.z
                mul r1.z, c2.y, c3.x
                add r3.xyz, r2.xyz, r1.xyz
                dot r0.wxy, r3.xyz, c1.xyz
                dot r1, r0, c0
                rcp r0.x, r1.x
                mul r0.y, r1.x, r1.x
                mul r0.z, c1.z, c2.y
                mul r1.x, r0.z, c3.w
                neg r0.z, r1.x
                mul r0.w, c1.y, c2.z
                mul r1.x, r0.w, c3.w
                mul r0.w, c2.y, c3.z
                mul r1.y, r0.w, c1.w
                mul r1.z, r0.w, c0.w
                neg r0.w, r1.z
                add r2.x, r1.y, r1.x
                mul r1.x, c1.z, c3.y
                mul r2.y, r1.x, c2.w
                mul r2.z, r1.x, c0.w
                neg r1.x, r2.z
                add r1.y, r2.y, r2.x
                mul r1.z, c1.y, c3.z
                mul r2.x, r1.z, c2.w
                mul r2.y, r1.z, c0.w
                neg r1.z, r2.x
                add r2.x, r1.z, r1.y
                add r1.y, r0.z, r2.x
                mul r0.z, c2.z, c3.y
                mul r1.z, r0.z, c1.w
                mul r1.w, r0.z, c0.w
                neg r0.z, r1.z
                add r3.x, r0.z, r1.y
                mul r0.z, c0.z, c2.y
                mul r1.y, r0.z, c3.w
                mul r0.z, c0.y, c3.z
                mul r1.z, r0.z, c2.w
                mul r2.x, r0.z, c1.w
                neg r0.z, r2.x
                add r2.x, r1.y, r1.z
                add r3.w, r1.w, r2.x
                mul r1.y, c0.y, c2.z
                mul r2.x, r1.y, c3.w
                neg r1.y, r2.x
                add r2.x, r1.y, r3.w
                add r1.y, r0.w, r2.x
                mul r0.w, c0.z, c3.y
                mul r1.z, r0.w, c2.w
                mul r1.w, r0.w, c1.w
                neg r0.w, r1.z
                add r3.y, r0.w, r1.y
                mul r0.w, c0.y, c1.z
                mul r1.y, r0.w, c3.w
                add r0.w, r2.y, r1.y
                add r2.x, r1.w, r0.w
                add r1.y, r0.z, r2.x
                mul r0.z, c0.z, c1.y
                mul r1.z, r0.z, c3.w
                neg r0.z, r1.z
                add r2.x, r0.z, r1.y
                add r3.z, r1.x, r2.x
                mul r1.xyz, r0.x, r3.xyz
                neg r1.w, r0.y
                lt r2.x, r1.w, r0.y
                mul c12.xyz, r2.x, r1.xyz
                mul r0.y, c1.x, c2.z
                mul r1.x, r0.y, c3.w
                neg r0.y, r1.x
                mul r0.z, c1.z, c2.x
                mul r1.x, r0.z, c3.w
                mul r0.z, c1.x, c3.z
                mul r1.y, r0.z, c2.w
                mul r1.z, r0.z, c0.w
                neg r0.z, r1.z
                add r0.w, r1.x, r1.y
                mul r1.x, c2.z, c3.x
                mul r2.y, r1.x, c1.w
                mul r2.z, r1.x, c0.w
                neg r1.x, r2.z
                add r1.y, r0.w, r2.y
                add r2.y, r0.y, r1.y
                mul r0.y, c2.x, c3.z
                mul r1.y, r0.y, c1.w
                mul r1.z, r0.y, c0.w
                neg r0.y, r1.y
                add r1.y, r0.y, r2.y
                mul r0.y, c1.z, c3.x
                mul r1.w, r0.y, c2.w
                mul r2.y, r0.y, c0.w
                neg r0.y, r1.w
                add r3.x, r0.y, r1.y
                mul r0.y, c0.x, c2.z
                mul r1.y, r0.y, c3.w
                add r0.y, r1.z, r1.y
                mul r0.w, c0.z, c3.x
                mul r1.y, r0.w, c2.w
                mul r1.z, r0.w, c1.w
                neg r0.w, r1.z
                add r2.z, r0.y, r1.y
                mul r0.y, c0.x, c3.z
                mul r1.y, r0.y, c2.w
                mul r1.z, r0.y, c1.w
                neg r0.y, r1.y
                add r1.y, r0.y, r2.z
                mul r0.y, c0.z, c2.x
                mul r1.w, r0.y, c3.w
                neg r0.y, r1.w
                add r2.z, r0.y, r1.y
                add r3.y, r1.x, r2.z
                mul r0.y, c0.z, c1.x
                mul r1.x, r0.y, c3.w
                add r0.y, r1.x, r1.z
                add r1.x, r2.y, r0.y
                mul r0.y, c0.x, c1.z
                mul r1.y, r0.y, c3.w
                neg r0.y, r1.y
                add r2.y, r0.y, r1.x
                add r1.x, r0.z, r2.y
                add r3.z, r0.w, r1.x
                mul r1.xyz, r0.x, r3.xyz
                mul c13.xyz, r2.x, r1.xyz
                mul r0.y, c1.y, c2.x
                mul r1.x, r0.y, c3.w
                neg r0.y, r1.x
                mul r0.z, c1.x, c2.y
                mul r1.x, r0.z, c3.w
                mul r0.z, c2.x, c3.y
                mul r1.y, r0.z, c1.w
                mul r1.z, r0.z, c0.w
                neg r0.z, r1.z
                add r0.w, r1.y, r1.x
                mul r1.x, c1.y, c3.x
                mul r2.y, r1.x, c2.w
                mul r2.z, r1.x, c0.w
                neg r1.x, r2.z
                add r1.y, r0.w, r2.y
                mul r0.w, c1.x, c3.y
                mul r1.z, r0.w, c2.w
                mul r1.w, r0.w, c0.w
                neg r0.w, r1.z
                add r2.y, r0.w, r1.y
                add r1.y, r0.y, r2.y
                mul r0.y, c2.y, c3.x
                mul r1.z, r0.y, c1.w
                mul r2.y, r0.y, c0.w
                neg r0.y, r1.z
                add r3.x, r0.y, r1.y
                mul r0.y, c0.y, c2.x
                mul r1.y, r0.y, c3.w
                mul r0.y, c0.x, c3.y
                mul r1.z, r0.y, c2.w
                mul r2.z, r0.y, c1.w
                neg r0.y, r2.z
                add r0.w, r1.y, r1.z
                add r1.y, r2.y, r0.w
                mul r0.w, c0.x, c2.y
                mul r1.z, r0.w, c3.w
                neg r0.w, r1.z
                add r2.y, r0.w, r1.y
                add r1.y, r0.z, r2.y
                mul r0.z, c0.y, c3.x
                mul r1.z, r0.z, c2.w
                mul r2.y, r0.z, c1.w
                neg r0.z, r1.z
                add r3.y, r0.z, r1.y
                mul r0.z, c0.x, c1.y
                mul r1.y, r0.z, c3.w
                add r0.z, r1.w, r1.y
                add r1.y, r2.y, r0.z
                add r2.y, r0.y, r1.y
                mul r0.y, c0.y, c1.x
                mul r1.y, r0.y, c3.w
                neg r0.y, r1.y
                add r1.y, r0.y, r2.y
                add r3.z, r1.x, r1.y
                mul r1.xyz, r0.x, r3.xyz
                mul c14.xyz, r2.x, r1.xyz
            
            // approximately 209 instructions used
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float4 Diffuse;
            //   float4x4 g_mProj;
            //   float4x4 g_mView;
            //   float4x4 g_mWorld;
            //   float3 g_vLight;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   g_mWorld     c0       4
            //   g_mView      c4       4
            //   g_mProj      c8       4
            //   g_vLight     c15      1
            //   Diffuse      c16      1
            //
            
                vs_2_0
                def c17, 1, 0, 0, 0
                dcl_position v0
                dcl_normal v1
                dcl_texcoord v2
                dp4 r0.w, v0, c3
                dp4 r0.x, v0, c0
                dp4 r0.y, v0, c1
                dp4 r0.z, v0, c2
                dp4 r1.x, r0, c4
                dp4 r1.y, r0, c5
                dp4 r1.z, r0, c6
                dp4 r1.w, r0, c7
                mad r0, r0.xyzx, c17.xxxy, c17.yyyx
                dp4 oPos.x, r1, c8
                dp4 oPos.y, r1, c9
                dp4 oPos.z, r1, c10
                dp4 oPos.w, r1, c11
                dp3 r1.x, v1, c0
                dp3 r1.y, v1, c1
                dp3 r1.z, v1, c2
                dp3 r2.x, r1, c4
                dp3 r2.y, r1, c5
                dp3 r2.z, r1, c6
                nrm r1.xyz, r2
                dp4 r2.x, r0, c4
                dp4 r2.y, r0, c5
                dp4 r2.z, r0, c6
                add r0.xyz, -r2, c15
                nrm r3.xyz, r0
                dp3 r0.x, r1, r3
                mul oD0, r0.x, c16
                dp3 r0.x, -r2, r1
                add r0.x, r0.x, r0.x
                mad r0.xyz, r0.x, r1, r2
                mov oT2.xyz, r1
                mov oT1.xyz, r2
                dp3 oT3.x, r0, c12
                dp3 oT3.y, r0, c13
                dp3 oT3.z, r0, c14
                mov oT0.xy, v2
            
            // approximately 40 instruction slots used
            };

        pixelshader = 
            asm {
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float Reflectivity;
            //
            //
            // Registers:
            //
            //   Name         Reg   Size
            //   ------------ ----- ----
            //   Reflectivity c0       1
            //
            
                preshader
                neg r0.x, c0.x
                add c5.x, r0.x, (1)
            
            // approximately 2 instructions used
            //
            // Generated by Microsoft (R) HLSL Shader Compiler 10.1
            //
            // Parameters:
            //
            //   float Power;
            //   float Reflectivity;
            //   float4 Specular;
            //   samplerCUBE g_samEnv;
            //   sampler2D g_samScene;
            //   float3 g_vLight;
            //   float4 g_vLightColor;
            //
            //
            // Registers:
            //
            //   Name          Reg   Size
            //   ------------- ----- ----
            //   g_vLightColor c0       1
            //   g_vLight      c1       1
            //   Specular      c2       1
            //   Reflectivity  c3       1
            //   Power         c4       1
            //   g_samScene    s0       1
            //   g_samEnv      s1       1
            //
            
                ps_2_0
                def c6, 1, 0, 0, 0
                dcl v0.xyz
                dcl t0.xy
                dcl t1.xyz
                dcl t2.xyz
                dcl t3.xyz
                dcl_2d s0
                dcl_cube s1
                texld r0, t0, s0
                texld r1, t3, s1
                dp3 r0.w, -t1, -t1
                rsq r0.w, r0.w
                add r2.xyz, -t1, c1
                nrm r3.xyz, r2
                mad r2.xyz, -t1, r0.w, r3
                nrm r3.xyz, r2
                nrm r2.xyz, t2
                dp3_sat r0.w, r3, r2
                mul r2.xyz, r0.w, c2
                log r3.x, r2.x
                log r3.y, r2.y
                log r3.z, r2.z
                mul r2.xyz, r3, c4.x
                exp r3.x, r2.x
                exp r3.y, r2.y
                exp r3.z, r2.z
                mad r0.xyz, r0, v0, r3
                mul r0.xyz, r0, c0
                mul r1.xyz, r1, c3.x
                mad r0.xyz, r0, c5.x, r1
                mov r0.w, c6.x
                mov oC0, r0
            
            // approximately 30 instruction slots used (2 texture, 28 arithmetic)
            };
    }
}
