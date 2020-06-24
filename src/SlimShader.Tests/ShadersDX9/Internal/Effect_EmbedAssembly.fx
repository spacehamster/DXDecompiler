PixelShader baz = asm {
    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    texld r0, t0, s0
    mul r0, r0, v0
    cmp r0, -c0.x, v0, r0
    mov oC0, r0
};

technique Technique1 {
    pass {
        PixelShader = baz;
    }
};