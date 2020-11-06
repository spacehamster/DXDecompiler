RWTexture2D<float4> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int2 ii2Location = index;
	uint2 iu2Pos = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1Height;
	
	//Texture2D
	uav.GetDimensions(ou1Width, ou1Height);
	result += ou1Width;
	result += ou1Height;

	result += uav.Load(ii2Location);
	result += uav.Load(ii2Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += uav[iu2Pos];

	return result;
}