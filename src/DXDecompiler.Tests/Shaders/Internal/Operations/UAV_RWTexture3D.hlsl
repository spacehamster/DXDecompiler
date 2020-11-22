#pragma FXC UAV_RWTexture3D ps_5_0 main /Od

RWTexture3D<float4> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int3 ii3Location = index;
	uint3 iu3Pos = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1Height;
	uint ou1Depth;

	//RWTexture3D
	uav.GetDimensions(ou1Width, ou1Height, ou1Depth);
	result += ou1Width;
	result += ou1Height;
	result += ou1Depth;

	result += uav.Load(ii3Location);
	result += uav.Load(ii3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += uav[iu3Pos];

	return result;
}