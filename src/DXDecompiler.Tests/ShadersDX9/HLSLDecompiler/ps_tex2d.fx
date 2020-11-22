#pragma FXC ps_tex2d ps_3_0 main

sampler sampler0;

float4 main(float2 texcoord : TEXCOORD) : COLOR
{
	return tex2D(sampler0, texcoord);
}
