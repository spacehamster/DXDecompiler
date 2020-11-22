#pragma FXC ps_multiply_negate ps_3_0 main

float4 main(float3 texcoord : TEXCOORD) : COLOR
{
	return float4(-abs(texcoord.y * texcoord.x * texcoord.z), texcoord.y * texcoord.x * texcoord.z, 1, 2);
}
