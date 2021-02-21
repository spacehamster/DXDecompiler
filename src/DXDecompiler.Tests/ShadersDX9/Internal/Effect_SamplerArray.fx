#pragma FXC Effect_SamplerArray_FX fx_2_0

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
float4 RenderScenePS(float2 textureUV : TEXCOORD0) : COLOR0
{
    float4 Output = 0;
    Output += tex2D(samp3[0], textureUV);
    Output += tex2D(samp3[1], textureUV);
    //Dynamic indexing not allowed
    //Output += tex2D(samp3[index], textureUV);
    return Output;
}
technique Tech1 {
    pass P0 {
        PixelShader = compile ps_2_0 RenderScenePS();
        Sampler[0] = <samp1>;
        Sampler[1] = <samp2>;
        //Not Supported
        //Sampler[2] = ( samp3[index] );
        Texture[0] = <tex1>;
        Texture[1] = <tex2>;
    }
}