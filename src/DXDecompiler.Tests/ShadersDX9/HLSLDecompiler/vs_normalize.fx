#pragma FXC vs_normalize vs_3_0 main

float4 main(float4 position : POSITION) : POSITION
{
	return float4(normalize(position.yxz), 1);
}
