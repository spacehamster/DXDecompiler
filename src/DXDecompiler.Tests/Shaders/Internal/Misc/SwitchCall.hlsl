#pragma FXC SwitchCall ps_5_0 main
float4 gFloat1;
float4 gFloat2;
int gInt;

float4 main() : SV_TARGET
{
	float4 result = gFloat1;
	[call]
	switch (gInt)
	{
		case 1:
			result += sin(gFloat2);
			break;
		case 2:
			result += log(gFloat2);
			break;
		default:
			result += 5;
			break;
	}
	return result;
}