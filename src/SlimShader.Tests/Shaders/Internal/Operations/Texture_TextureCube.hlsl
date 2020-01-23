SamplerState samp0;
SamplerComparisonState samp1;

TextureCube<float4> tex8 : register(t8);

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	float3 if3Location = index;
	float if1CompareValue = index;
	int iu1MipLevel = index;
	float if1Clamp = index;
	float if1Bias = index;
	float3 if3DDX = index;
	float3 if3DDY = index;
	float if1LOD = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1Height;
	uint ou1NumberOfLevels;
	
	//TextureCube
	result += tex8.Gather(samp0, if3Location);
	result += tex8.Gather(samp0, if3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	
	result += tex8.GatherRed(samp0, if3Location);
	result += tex8.GatherRed(samp0, if3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherGreen(samp0, if3Location);
	result += tex8.GatherGreen(samp0, if3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherBlue(samp0, if3Location);
	result += tex8.GatherBlue(samp0, if3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherAlpha(samp0, if3Location);
	result += tex8.GatherAlpha(samp0, if3Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherCmp(samp1, if3Location, if1CompareValue);
	result += tex8.GatherCmp(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherCmpRed(samp1, if3Location, if1CompareValue);
	result += tex8.GatherCmpRed(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherCmpGreen(samp1, if3Location, if1CompareValue);
	result += tex8.GatherCmpGreen(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherCmpBlue(samp1, if3Location, if1CompareValue);
	result += tex8.GatherCmpBlue(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.GatherCmpAlpha(samp1, if3Location, if1CompareValue);
	result += tex8.GatherCmpAlpha(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	
	tex8.GetDimensions(iu1MipLevel, ou1Width, ou1Height, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Height;
	result += ou1NumberOfLevels;

	result += tex8.Sample(samp0, if3Location);
	result += tex8.Sample(samp0, if3Location, if1Clamp);
	result += tex8.Sample(samp0, if3Location, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.SampleBias (samp0, if3Location, if1Bias);
	result += tex8.SampleBias (samp0, if3Location, if1Bias, if1Clamp);
	result += tex8.SampleBias (samp0, if3Location, if1Bias, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.SampleCmp(samp1, if3Location, if1CompareValue);
	result += tex8.SampleCmp(samp1, if3Location, if1CompareValue, if1Clamp);
	result += tex8.SampleCmp(samp1, if3Location, if1CompareValue, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.SampleCmpLevelZero(samp1, if3Location, if1CompareValue);
	result += tex8.SampleCmpLevelZero(samp1, if3Location, if1CompareValue);
	result += tex8.SampleCmpLevelZero(samp1, if3Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.SampleGrad(samp0, if3Location, if3DDX, if3DDY);
	result += tex8.SampleGrad(samp0, if3Location, if3DDX, if3DDY, if1Clamp);
	result += tex8.SampleGrad(samp0, if3Location, if3DDX, if3DDY, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex8.SampleLevel(samp0, if3Location, if1LOD);
	result += tex8.SampleLevel(samp0, if3Location, if1LOD, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	return result;
}