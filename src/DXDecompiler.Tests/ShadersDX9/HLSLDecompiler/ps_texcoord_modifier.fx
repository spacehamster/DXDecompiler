#pragma FXC ps_texcoord_modifier ps_3_0 main

float4 main(float4 texcoord : TEXCOORD) : COLOR
{
	return float4(-texcoord.yx, 2, abs(texcoord.w));
}
