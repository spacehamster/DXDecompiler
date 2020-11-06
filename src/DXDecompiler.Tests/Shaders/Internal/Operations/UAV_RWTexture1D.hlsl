RWTexture1D<float4> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int ii1Location = index;
	uint iu1Pos = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	
	//Texture2D
	uav.GetDimensions(ou1Width);
	result += ou1Width;

	result += uav.Load(ii1Location);
	result += uav.Load(ii1Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += uav[iu1Pos];

	return result;
}