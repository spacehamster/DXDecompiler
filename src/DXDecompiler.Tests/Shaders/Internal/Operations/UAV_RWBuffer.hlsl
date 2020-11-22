#pragma FXC UAV_RWBuffer ps_5_0 main /Od

RWBuffer<float4> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int ii1Location = index;
	uint iu1Pos = index;
	//Out Vars
	uint ou1Dim;
	uint ou1Status;
	//TextureCubeArray
	uav.GetDimensions(ou1Dim);
	result += ou1Dim;

	result += uav.Load(ii1Location);
	result += uav.Load(ii1Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += uav[iu1Pos];

	return result;
}