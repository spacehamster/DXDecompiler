#pragma FXC ps_tex2d_two_samplers ps_3_0 main

sampler sampler0;
sampler sampler1;

float4 main(float2 texcoord : TEXCOORD) : COLOR
{
	return tex2D(sampler0, 2 * tex2D(sampler1, texcoord.yx).xy + texcoord.yx).wzyx;
}
