//
// FX Version: fx_5_0
//
// 1 local buffer(s)
//
cbuffer $Globals
{
    int     i;                          // Offset:    0, size:    4
    int     j;                          // Offset:    4, size:    4
    uint    k;                          // Offset:    8, size:    4
    uint    l;                          // Offset:   12, size:    4
    bool    b1;                         // Offset:   16, size:    4
    bool    b2;                         // Offset:   20, size:    4
    float4  a;                          // Offset:   32, size:   16
    float4  b;                          // Offset:   48, size:   16
    float4  c;                          // Offset:   64, size:   16
    float3  f;                          // Offset:   80, size:   12
    float3  g;                          // Offset:   96, size:   12
    float4x4 m;                         // Offset:  112, size:   64
    Foo     bar;                        // Offset:  176, size:  112
}

//
// 3 local object(s)
//
DepthStencilState DepthStencilArray[3]
{
    {
        DepthFunc = uint(NEVER /* 1 */);
    },
    {
        DepthFunc = uint(LESS /* 2 */);
    },
    {
        DepthFunc = eval(iadd expr.x, i.x, (2));
    }
};
SamplerState samp
{
    MipLODBias = eval(itof r0.x, (5)
                    mov r0.y, (5)
                    add r1.x, r0.x, a.x
                    neg r0.x, b.x
                    add r2.x, r0.x, r1.x
                    mul r0.x, r2.x, a.x
                    div r1.x, r0.x, a.x
                    dot r0, a, b
                    add r2.x, r0.x, r1.x
                    sin r0.x, r2.x
                    cos r1.x, r0.x
                    sin r0.x, r1.x
                    cos r0.z, r1.x
                    div r1.x, r0.x, r0.z
                    asin r0.x, r1.x
                    acos r1.x, r0.x
                    atan r0.x, r1.x
                    atan2 r1.x, r0.x, a.x
                    sqrt r0.x, r1.x
                    frc r1.x, r0.x
                    sqrt r0.x, r1.x
                    div r1.x, (1.000000), r0.x
                    max r0.x, r1.x, a.x
                    min r1.x, r0.x, a.x
                    floor r0.x, r1.x
                    ceil r1.x, r0.x
                    log r0.x, r1.x
                    mul r1.x, r0.x, (0.693147)
                    mul r0.x, r1.x, (1.442695)
                    exp r1.x, r0.x
                    rcp r0.x, r1.x
                    ftoi r0.z, a.x
                    iadd r1.x, r0.z, r0.y
                    itob r0.y, b1.x
                    btoi r1.y, r0.y
                    iadd r0.y, r1.y, r1.x
                    iadd r1.x, r0.y, i.x
                    ineg r0.y, i.x
                    iadd r2.x, r0.y, r1.x
                    imul r0.y, r2.x, i.x
                    ineg r1.x, r0.y
                    ineg r0.z, i.x
                    xor r1.y, r0.y, i.x
                    imax r2.x, r0.y, r1.x
                    imax r1.x, r0.z, i.x
                    udiv r0.y, r2.x, r1.x
                    ineg r1.x, r0.y
                    and r0.z, r1.y, (-0.000000)
                    itob r1.y, r0.z
                    movc r2.x, r1.y, r1.x, r0.y
                    and r0.y, r2.x, i.x
                    or r1.x, r0.y, i.x
                    xor r0.y, r1.x, i.x
                    not r0.z, i.x
                    iadd r1.x, r0.z, r0.y
                    ishl r0.y, r1.x, i.x
                    ishr r1.x, r0.y, i.x
                    imin r0.y, r1.x, i.x
                    imax r1.x, r0.y, i.x
                    umin r0.y, r1.x, l.x
                    umax r1.x, r0.y, l.x
                    imax r0.y, i.x, j.x
                    iadd r2.x, r0.y, r1.x
                    mov r0.y, (-1)
                    itob r0.z, b1.x
                    and r1.x, r0.z, (1)
                    and r1.y, r0.y, (1)
                    bult r0.y, r1.x, r1.y
                    itob r0.z, b1.x
                    bieq r1.x, r0.z, r0.y
                    itob r0.y, b1.x
                    bine r2.y, r0.y, r1.x
                    itof r0.y, r2.x
                    add r1.x, r0.y, r0.x
                    btof r0.x, r2.y
                    add r2.x, r0.x, r1.x
                    mov expr.x, r2.x);
    BorderColor = eval(mul r0.x, a.x, m[3].z
                    mul r0.y, a.y, m[1].z
                    mul r0.z, a.z, m[2].z
                    mul r0.w, a.w, m[0].z
                    sin r1, r0
                    add expr.x, r1.x, bar[4].w
                    add expr0.y, r1.y, bar[4].y
                    add expr0.z, r1.z, bar[4].z
                    add expr0.w, r1.w, bar[4].x);
    MinLOD   = eval(bult r0.x, l.x, k.x
                    mul r0.y, a.x, b.x
                    not r1.x, r0.x
                    dot r0, a, b
                    neg r0.z, c.x
                    add r1.y, r0.z, r0.x
                    movc expr.x, r1.x, r1.y, r0.y);
};
BlendState blend
{
    BlendEnable[0] = eval(mul r0.x, f.x, g.x
                    ftob expr.x, r0.x);
};

//
// 1 groups(s)
//
fxgroup
{
    //
    // 2 technique(s)
    //
    technique11 RenderSceneWithTexture1Light10_1
    {
        pass P0
        {
            DS_StencilRef = uint(5);
            DepthStencilState = DepthStencilArray[eval(imul r0.x, i.x, (3)
                            iadd expr.x, r0.x, j.x)];
        }

    }

    technique11 RenderSceneWithTexture1Light11_1
    {
        pass P0
        {
            DS_StencilRef = uint(5);
            DepthStencilState = DepthStencilArray[eval(imul r0.x, i.x, (3)
                            iadd expr.x, r0.x, j.x)];
        }

    }

}

