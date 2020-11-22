#pragma FXC ps_multiply_subtract ps_3_0 main

float4 main(float4 texcoord : TEXCOORD) : COLOR
{
	return float4(3, 2 * texcoord.xy - 1, abs(texcoord.w));
}
