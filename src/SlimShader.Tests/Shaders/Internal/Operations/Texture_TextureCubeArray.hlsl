SamplerState samp0;
SamplerComparisonState samp1;

TextureCubeArray<float4> tex : register(t8);

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	float4 if4Location = index;
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
	uint ou1Elements;
	uint ou1NumberOfLevels;
	
	//TextureCube
	result += tex.Gather(samp0, if4Location);
	result += tex.Gather(samp0, if4Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	
	result += tex.GatherRed(samp0, if4Location);
	result += tex.GatherRed(samp0, if4Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherGreen(samp0, if4Location);
	result += tex.GatherGreen(samp0, if4Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherBlue(samp0, if4Location);
	result += tex.GatherBlue(samp0, if4Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherAlpha(samp0, if4Location);
	result += tex.GatherAlpha(samp0, if4Location, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmp(samp1, if4Location, if1CompareValue);
	result += tex.GatherCmp(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpRed(samp1, if4Location, if1CompareValue);
	result += tex.GatherCmpRed(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpGreen(samp1, if4Location, if1CompareValue);
	result += tex.GatherCmpGreen(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpBlue(samp1, if4Location, if1CompareValue);
	result += tex.GatherCmpBlue(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpAlpha(samp1, if4Location, if1CompareValue);
	result += tex.GatherCmpAlpha(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	
	tex.GetDimensions(iu1MipLevel, ou1Width, ou1Height, ou1Elements, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Height;
	result += ou1Elements;
	result += ou1NumberOfLevels;

	result += tex.Sample(samp0, if4Location);
	result += tex.Sample(samp0, if4Location, if1Clamp);
	result += tex.Sample(samp0, if4Location, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleBias (samp0, if4Location, if1Bias);
	result += tex.SampleBias (samp0, if4Location, if1Bias, if1Clamp);
	result += tex.SampleBias (samp0, if4Location, if1Bias, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleCmp(samp1, if4Location, if1CompareValue);
	result += tex.SampleCmp(samp1, if4Location, if1CompareValue, if1Clamp);
	result += tex.SampleCmp(samp1, if4Location, if1CompareValue, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleCmpLevelZero(samp1, if4Location, if1CompareValue);
	result += tex.SampleCmpLevelZero(samp1, if4Location, if1CompareValue);
	result += tex.SampleCmpLevelZero(samp1, if4Location, if1CompareValue, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleGrad(samp0, if4Location, if3DDX, if3DDY);
	result += tex.SampleGrad(samp0, if4Location, if3DDX, if3DDY, if1Clamp);
	result += tex.SampleGrad(samp0, if4Location, if3DDX, if3DDY, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleLevel(samp0, if4Location, if1LOD);
	result += tex.SampleLevel(samp0, if4Location, if1LOD, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	return result;
}