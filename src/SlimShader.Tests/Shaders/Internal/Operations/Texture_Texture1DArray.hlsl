SamplerState samp0;
SamplerComparisonState samp1;

Texture1DArray<float4> tex;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	float if1Location = index;
	int ii1Location = index;
	int ii1OffsetZero = 0;
	int ii1OffsetOne = 1;
	float if1Clamp = index;
	uint2 iu2Pos = index;
	uint iu1MipLevel = index;
	uint iu1MipSlice = index;
	float if1Bias = index;
	float if1CompareValue = index;
	float if1DDX = index;
	float if1DDY = index;
	float if1LOD = index;
	//Out Vars
	uint ou1Status;
	uint ou1Width;
	uint ou1NumberOfLevels;
	uint ou1Elements;
	//Texture1DArray
	result += tex.CalculateLevelOfDetail(samp0, if1LOD);
	result += tex.CalculateLevelOfDetailUnclamped(samp0, if1LOD);

	tex.GetDimensions(iu1MipLevel, ou1Width, ou1Elements, ou1NumberOfLevels);
	result += ou1Width;
	result += ou1Elements;
	result += ou1NumberOfLevels;

	result += tex.Load(ii1Location);
	result += tex.Load(ii1Location, ii1OffsetZero);
	result += tex.Load(ii1Location, ii1OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Load(ii1Location, ii1OffsetOne);
	result += tex.Load(ii1Location, ii1OffsetOne, ou1Status);

	result += tex.mips[iu1MipSlice][iu2Pos];

	result += tex[iu2Pos];
	
	result += tex.Sample(samp0, if1Location);
	result += tex.Sample(samp0, if1Location, ii1OffsetZero);
	result += tex.Sample(samp0, if1Location, ii1OffsetZero, if1Clamp);
	result += tex.Sample(samp0, if1Location, ii1OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.Sample(samp0, if1Location, ii1OffsetOne);
	result += tex.Sample(samp0, if1Location, ii1OffsetOne, if1Clamp);
	result += tex.Sample(samp0, if1Location, ii1OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleBias(samp0, if1Location, if1Bias);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetZero);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetZero, if1Clamp);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetOne);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetOne, if1Clamp);
	result += tex.SampleBias(samp0, if1Location, if1Bias, ii1OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleCmp(samp1, if1Location, if1CompareValue);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetZero);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetZero, if1Clamp);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetOne);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetOne, if1Clamp);
	result += tex.SampleCmp(samp1, if1Location, if1CompareValue, ii1OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleCmpLevelZero(samp1, if1Location, if1CompareValue);
	result += tex.SampleCmpLevelZero(samp1, if1Location, if1CompareValue, ii1OffsetZero);
	result += tex.SampleCmpLevelZero(samp1, if1Location, if1CompareValue, ii1OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleCmpLevelZero(samp1, if1Location, if1CompareValue, ii1OffsetOne);
	result += tex.SampleCmpLevelZero(samp1, if1Location, if1CompareValue, ii1OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetZero);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetZero, if1Clamp);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetZero, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetOne);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetOne, if1Clamp);
	result += tex.SampleGrad(samp0, if1Location, if1DDX, if1DDY, ii1OffsetOne, if1Clamp, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += tex.SampleLevel(samp0, if1Location, if1LOD);
	result += tex.SampleLevel(samp0, if1Location, if1LOD, ii1OffsetZero);
	result += tex.SampleLevel(samp0, if1Location, if1LOD, ii1OffsetZero, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);
	result += tex.SampleLevel(samp0, if1Location, if1LOD, ii1OffsetOne);
	result += tex.SampleLevel(samp0, if1Location, if1LOD, ii1OffsetOne, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	return result;
}