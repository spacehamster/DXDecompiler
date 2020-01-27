SamplerState samp0;
SamplerComparisonState samp1;

Texture3D<float4> tex;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	float3 if3Location = index;
	int3 ii3OffsetZero = 0;
	int3 ii3OffsetOne = 1;
	int4 ii4Location = index;
	uint3 iu3Pos = index;
	uint iu1MipLevel = index;
	uint iu1MipSlice = index;
	float if1Clamp = index;
	float if1Bias = index;
	float3 if3DDX = index;
	float3 if3DDY = index;
	float if1LOD = index;
	float4 if3LOD = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1Height;
	uint ou1Depth;
	uint ou1NumberOfLevels;
	uint ou1Elements;

	//Texture3D
	result += tex.CalculateLevelOfDetail(samp0, if3LOD);
	result += tex.CalculateLevelOfDetailUnclamped(samp0, if3LOD);

	tex.GetDimensions(iu1MipLevel, ou1Width, ou1Height, ou1Depth, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Height;
	result += ou1Depth;
	result += ou1NumberOfLevels;

	result += tex.Load(ii4Location);
	result += tex.Load(ii4Location, ii3OffsetZero);
	result += tex.Load(ii4Location, ii3OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Load(ii4Location, ii3OffsetOne);
	result += tex.Load(ii4Location, ii3OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);


	result += tex.mips[iu1MipSlice][iu3Pos];

	result += tex[iu3Pos];

	result += tex.Sample(samp0, if3Location);
	result += tex.Sample(samp0, if3Location, ii3OffsetZero);
	result += tex.Sample(samp0, if3Location, ii3OffsetZero, if1Clamp);
	result += tex.Sample(samp0, if3Location, ii3OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Sample(samp0, if3Location, ii3OffsetOne);
	result += tex.Sample(samp0, if3Location, ii3OffsetOne, if1Clamp);
	result += tex.Sample(samp0, if3Location, ii3OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleBias (samp0, if3Location, if1Bias);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetZero);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetZero, if1Clamp);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetOne);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetOne, if1Clamp);
	result += tex.SampleBias (samp0, if3Location, if1Bias, ii3OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetZero);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetZero, if1Clamp);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetOne);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetOne, if1Clamp);
	result += tex.SampleGrad(samp0, if3Location, if3DDX, if3DDY, ii3OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleLevel(samp0, if3Location, if1LOD);
	result += tex.SampleLevel(samp0, if3Location, if1LOD, ii3OffsetZero);
	result += tex.SampleLevel(samp0, if3Location, if1LOD, ii3OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleLevel(samp0, if3Location, if1LOD, ii3OffsetOne);
	result += tex.SampleLevel(samp0, if3Location, if1LOD, ii3OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	return result;
}