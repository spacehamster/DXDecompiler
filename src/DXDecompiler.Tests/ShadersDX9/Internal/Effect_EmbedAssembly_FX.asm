pixelshader baz = 
asm {
    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    texld r0, t0, s0
    mul r0, r0, v0
    cmp r0, -c0.x, v0, r0
    mov oC0, r0

// approximately 4 instruction slots used (1 texture, 3 arithmetic)
};


//listing of all techniques and passes with embedded asm listings 

technique Technique1
{
    pass 
    {
        //No embedded vertex shader
        pixelshader = 
            asm {
                ps_2_0
                dcl v0
                dcl t0.xy
                dcl_2d s0
                texld r0, t0, s0
                mul r0, r0, v0
                cmp r0, -c0.x, v0, r0
                mov oC0, r0
            
            // approximately 4 instruction slots used (1 texture, 3 arithmetic)
            };
    }
}

