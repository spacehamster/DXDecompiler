texture tex1;
texture tex2;
texture tex3;
sampler samp1 =
sampler_state {
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    texture = <tex1>;
};
sampler samp2 =
sampler_state {
    texture = <tex1>;
};
sampler samp3[2] = {
    sampler_state {
        texture = <tex2>;
    },
    sampler_state {
        texture = <tex3>;
    }
};
technique Tech1 {
    pass P0 {
        Sampler[0] = <samp1>;
        Sampler[1] = <samp2>;
        //Not Supported
        //Sampler[2] = ( samp3[index] );
        Texture[0] = <tex1>;
        Texture[1] = <tex2>;
    }
}