Texture2DMSArray<float4, 4> tex;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int ii1SampleIndex = index;;
	int3 ii3Location = index;
	int2 ii2OffsetZero = 0;
	int2 ii2OffsetOne = 1;
	int iu1SampleSlice = index;
	uint3 iu3Pos = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1NumberOfLevels;
	uint ou1Elements;
	uint ou1Height;
	
	//Texture2DMS
	tex.GetDimensions(ou1Width, ou1Height, ou1Elements, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Height;
	result += ou1NumberOfLevels;

	result.xy += tex.GetSamplePosition(ii1SampleIndex);

	result += tex.Load(ii3Location, ii1SampleIndex);
	result += tex.Load(ii3Location, ii1SampleIndex, ii2OffsetZero);
	result += tex.Load(ii3Location, ii1SampleIndex, ii2OffsetOne);
	result += tex.Load(ii3Location, ii1SampleIndex, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Load(ii3Location, ii1SampleIndex, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.sample[iu1SampleSlice][iu3Pos];

	result += tex[iu3Pos];


	return result;
}