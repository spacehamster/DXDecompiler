//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 g_avSampleWeights[16];
//   sampler2D s0;
//   sampler2D s1;
//   sampler2D s2;
//   sampler2D s3;
//   sampler2D s4;
//   sampler2D s5;
//   sampler2D s6;
//   sampler2D s7;
//
//
// Registers:
//
//   Name              Reg   Size
//   ----------------- ----- ----
//   g_avSampleWeights c0       8
//   s0                s0       1
//   s1                s1       1
//   s2                s2       1
//   s3                s3       1
//   s4                s4       1
//   s5                s5       1
//   s6                s6       1
//   s7                s7       1
//

    ps_2_0
    dcl t0.xy
    dcl_2d s0
    dcl_2d s1
    dcl_2d s2
    dcl_2d s3
    dcl_2d s4
    dcl_2d s5
    dcl_2d s6
    dcl_2d s7
    texld r0, t0, s1
    texld r1, t0, s0
    texld r2, t0, s2
    texld r3, t0, s3
    texld r4, t0, s4
    texld r5, t0, s5
    texld r6, t0, s6
    texld r7, t0, s7
    mul r0, r0, c1
    mad r0, c0, r1, r0
    mad r0, c2, r2, r0
    mad r0, c3, r3, r0
    mad r0, c4, r4, r0
    mad r0, c5, r5, r0
    mad r0, c6, r6, r0
    mad r0, c7, r7, r0
    mov oC0, r0

// approximately 17 instruction slots used (8 texture, 9 arithmetic)