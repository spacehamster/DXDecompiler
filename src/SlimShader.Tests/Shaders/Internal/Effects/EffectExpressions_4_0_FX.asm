//
// FX Version: fx_4_0
// Child effect (requires effect pool): false
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
    MipLODBias = eval(ftoi r0.x, a.x
                    iadd r1.x, r0.x, (5)
                    movc r0.x, b1.x, (1), (0)
                    iadd r2.x, r0.x, r1.x
                    iadd r0.x, r2.x, i.x
                    ineg r0.y, i.x
                    iadd r1.x, r0.y, r0.x
                    imax r1.y, r0.y, i.x
                    imul r0.x, r1.x, i.x
                    ineg r1.x, r0.x
                    imax r2.x, r0.x, r1.x
                    xor r1.x, r0.x, i.x
                    and r0.x, r1.x, (-0.000000)
                    udiv r0.y, r2.x, r1.y
                    ineg r1.x, r0.y
                    movc r2.x, r0.x, r1.x, r0.y
                    and r0.x, r2.x, i.x
                    or r1.x, r0.x, i.x
                    xor r0.x, r1.x, i.x
                    not r0.y, i.x
                    iadd r1.x, r0.y, r0.x
                    ishl r0.x, r1.x, i.x
                    ishr r1.x, r0.x, i.x
                    imin r0.x, r1.x, i.x
                    imax r1.x, r0.x, i.x
                    umin r0.x, r1.x, l.x
                    umax r1.x, r0.x, l.x
                    imax r0.x, i.x, j.x
                    iadd r2.x, r0.x, r1.x
                    itof r0.x, r2.x
                    add r0.y, a.x, (5.000000)
                    neg r0.z, b.x
                    add r1.x, r0.z, r0.y
                    mul r0.y, r1.x, a.x
                    div r1.x, r0.y, a.x
                    dot r0.yzwx, a, b
                    add r2.x, r0.y, r1.x
                    sin r0.y, r2.x
                    cos r1.x, r0.y
                    sin r0.y, r1.x
                    cos r0.z, r1.x
                    div r1.x, r0.y, r0.z
                    asin r0.y, r1.x
                    acos r1.x, r0.y
                    atan r0.y, r1.x
                    atan2 r1.x, r0.y, a.x
                    sqrt r0.y, r1.x
                    frc r1.x, r0.y
                    sqrt r0.y, r1.x
                    rcp r1.x, r0.y
                    max r0.y, r1.x, a.x
                    min r1.x, r0.y, a.x
                    floor r0.y, r1.x
                    rcp r1.x, r0.y
                    add r2.x, r0.x, r1.x
                    bine r0.x, b1.x, (0)
                    bieq r1.x, r0.x, (0)
                    bieq r2.y, r0.x, r1.x
                    bine r1.x, r0.x, r2.y
                    and r0.x, r1.x, (1.000000)
                    add expr.x, r0.x, r2.x);
    BorderColor = eval(mul r0.x, a.x, m[3].z
                    mul r0.y, a.y, m[1].z
                    mul r0.z, a.z, m[2].z
                    mul r0.w, a.w, m[0].z
                    sin r1, r0
                    add expr.x, r1.x, bar[4].w
                    add expr0.y, r1.y, bar[4].y
                    add expr0.z, r1.z, bar[4].z
                    add expr0.w, r1.w, bar[4].x);
    MinLOD   = eval(dot r0, a, b
                    neg r0.y, c.x
                    add r1.x, r0.y, r0.x
                    mul r0.x, a.x, b.x
                    buge r0.y, l.x, k.x
                    movc expr.x, r0.y, r1.x, r0.x);
};
BlendState blend
{
    BlendEnable[0] = eval(mul r0.x, f.x, g.x
                    ftob expr.x, r0.x);
};

//
// 1 technique(s)
//
technique10 RenderSceneWithTexture1Light10_1
{
    pass P0
    {
        DS_StencilRef = uint(5);
        DepthStencilState = DepthStencilArray[eval(imul r0.x, i.x, (3)
                        iadd expr.x, r0.x, j.x)];
    }

}

