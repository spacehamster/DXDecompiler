Buffer<float4> tex;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int ii1Location = index;
	uint iu1Pos = index;
	//Out Vars
	uint ou1Status;
	uint ou1Dim;
	//Buffer
	tex.GetDimensions(ou1Dim);
	result += ou1Dim;
	result += tex.Load(ii1Location);
	result += tex.Load(ii1Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex[iu1Pos];
	return result;
}