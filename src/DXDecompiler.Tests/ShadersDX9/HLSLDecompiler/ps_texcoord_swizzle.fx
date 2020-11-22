#pragma FXC ps_texcoord_swizzle ps_3_0 main

float4 main(float3 texcoord : TEXCOORD) : COLOR
{
	return float4(texcoord.yzx, 3);
}
