#pragma FXC ps_conditional ps_3_0 main

float4 main(float4 texcoord : TEXCOORD) : COLOR
{
	return float4(texcoord.x >= 0 ? 3 * texcoord.yyy : 2 * texcoord.zzz, texcoord.w);
}
