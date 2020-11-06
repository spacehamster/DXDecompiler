float4 g1;
float4 g2;
float4 g3;
float4 g4;
float4 g5;
float4 g6;
float4 g7;
float4 g8;
void RenderSceneVS(
	out float4 o1 : POSITION,
	out float4 o2 : COLOR0,
	out float4 o3 : COLOR1,
	out float4 o4 : TEXCOORD0,
	out float4 o5 : TEXCOORD1)
{
	o1 = 0;
	o2 = 0;
	o3 = 0;
	o4 = 0;
	o5 = 0;
	o1 += g1 + 5;
	//o2 += g2 + 6;
	o3 += g3 + 7;
	//o4 += g4 + 8;
	o5 += g5 + 9;
}

technique Tech0
{
	pass P0
	{
		VertexShader = compile vs_2_0 RenderSceneVS();
	}
}

