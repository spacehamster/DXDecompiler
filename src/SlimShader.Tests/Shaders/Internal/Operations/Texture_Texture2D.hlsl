SamplerState samp0;
SamplerComparisonState samp1;

Texture2D<float4> tex;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	float2 if2Location = index;
	int2 ii2OffsetZero = 0;
	int2 ii2OffsetOne = 1;
	int3 ii3Location = index;
	uint2 iu2Pos = index;
	uint iu1MipLevel = index;
	uint iu1MipSlice = index;
	float if1Clamp = index;
	float if1Bias = index;
	float if1CompareValue = index;
	float2 if2DDX = index;
	float2 if2DDY = index;
	float if1LOD = index;
	int2 ii2Offset1 = index;
	int2 ii2Offset2 = index;
	int2 ii2Offset3 = index;
	int2 ii2Offset4 = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1NumberOfLevels;
	uint ou1Height;
	
	//Texture2D
	result += tex.Gather(samp0, if2Location);
	result += tex.Gather(samp0, if2Location, ii2OffsetZero);
	result += tex.Gather(samp0, if2Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Gather(samp0, if2Location, ii2OffsetOne);
	result += tex.Gather(samp0, if2Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	
	result += tex.GatherRed(samp0, if2Location);
	result += tex.GatherRed(samp0, if2Location, ii2OffsetZero);
	result += tex.GatherRed(samp0, if2Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherRed(samp0, if2Location, ii2OffsetOne);
	result += tex.GatherRed(samp0, if2Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherRed(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherRed(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherGreen(samp0, if2Location);
	result += tex.GatherGreen(samp0, if2Location, ii2OffsetZero);
	result += tex.GatherGreen(samp0, if2Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherGreen(samp0, if2Location, ii2OffsetOne);
	result += tex.GatherGreen(samp0, if2Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherGreen(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherGreen(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherBlue(samp0, if2Location);
	result += tex.GatherBlue(samp0, if2Location, ii2OffsetZero);
	result += tex.GatherBlue(samp0, if2Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherBlue(samp0, if2Location, ii2OffsetOne);
	result += tex.GatherBlue(samp0, if2Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherBlue(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherBlue(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherAlpha(samp0, if2Location);
	result += tex.GatherAlpha(samp0, if2Location, ii2OffsetZero);
	result += tex.GatherAlpha(samp0, if2Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherAlpha(samp0, if2Location, ii2OffsetOne);
	result += tex.GatherAlpha(samp0, if2Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherAlpha(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherAlpha(samp0, if2Location, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmp(samp1, if2Location, if1CompareValue);
	result += tex.GatherCmp(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.GatherCmp(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmp(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.GatherCmp(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherCmpRed(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherCmpGreen(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherCmpBlue(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4);
	result += tex.GatherCmpAlpha(samp1, if2Location, if1CompareValue, ii2Offset1, ii2Offset2, ii2Offset3, ii2Offset4, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	tex.GetDimensions(iu1MipLevel, ou1Width, ou1Height, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Height;
	result += ou1NumberOfLevels;

	result += tex.Load(ii3Location);
	result += tex.Load(ii3Location, ii2OffsetZero);
	result += tex.Load(ii3Location, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Load(ii3Location, ii2OffsetOne);
	result += tex.Load(ii3Location, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.mips[iu1MipSlice][iu2Pos];

	result += tex[iu2Pos];

	result += tex.Sample(samp0, if2Location);
	result += tex.Sample(samp0, if2Location, ii2OffsetZero);
	result += tex.Sample(samp0, if2Location, ii2OffsetZero, if1Clamp);
	result += tex.Sample(samp0, if2Location, ii2OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Sample(samp0, if2Location, ii2OffsetOne);
	result += tex.Sample(samp0, if2Location, ii2OffsetOne, if1Clamp);
	result += tex.Sample(samp0, if2Location, ii2OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleBias (samp0, if2Location, if1Bias);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetZero);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetZero, if1Clamp);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetOne);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetOne, if1Clamp);
	result += tex.SampleBias (samp0, if2Location, if1Bias, ii2OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);


	result += tex.SampleCmp(samp1, if2Location, if1CompareValue);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetZero, if1Clamp);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetOne, if1Clamp);
	result += tex.SampleCmp(samp1, if2Location, if1CompareValue, ii2OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleCmpLevelZero(samp1, if2Location, if1CompareValue);
	result += tex.SampleCmpLevelZero(samp1, if2Location, if1CompareValue, ii2OffsetZero);
	result += tex.SampleCmpLevelZero(samp1, if2Location, if1CompareValue, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleCmpLevelZero(samp1, if2Location, if1CompareValue, ii2OffsetOne);
	result += tex.SampleCmpLevelZero(samp1, if2Location, if1CompareValue, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetZero);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetZero, if1Clamp);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetOne);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetOne, if1Clamp);
	result += tex.SampleGrad(samp0, if2Location, if2DDX, if2DDY, ii2OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleLevel(samp0, if2Location, if1LOD);
	result += tex.SampleLevel(samp0, if2Location, if1LOD, ii2OffsetZero);
	result += tex.SampleLevel(samp0, if2Location, if1LOD, ii2OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleLevel(samp0, if2Location, if1LOD, ii2OffsetOne);
	result += tex.SampleLevel(samp0, if2Location, if1LOD, ii2OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	return result;
}