#pragma FXC ps_negate_absolute ps_3_0 main

float4 main(float3 texcoord : TEXCOORD) : COLOR
{
	return float4(-abs(texcoord.z), texcoord.x, 1, 2);
}
