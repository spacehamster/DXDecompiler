export float3 TestFunction(float3 input,
	float1 in2 : COLOR,
	inout float2x3 val3,
	out uint val4,
	float3x2 val5,
	centroid int3 val6) : SV_TARGET
{
	val4 = 5;
	return input * 2.0f;
}
export float3 TestFunction2(
	float3x4 val1, float3x4 val2,
	column_major float3x4 val3, column_major float3x4 val4,
	row_major float3x4 val5, row_major float3x4 val6,
	float2x3 val7, float2x3 val8,
	column_major float2x3 val9, column_major float2x3 val10,
	row_major float2x3 val11, row_major float2x3 val12)
{
	return 5;
}
export float3 TestFunction3(float3 input, centroid float3 testInterpolation, uint1 foo) : SV_TARGET
{
	  return input * 2.0f * testInterpolation;
}
export void TestFunction4(float input, float in2, out float3 result)
{
	result = input * 2;
}
export float4x4 TestFunction5(float input, float in2)
{
	return input * 2.0f;
}
float4 TestGlobal = 5;
export float4 TestFunction6(uint input)
{
	return input * 2.0f + TestGlobal;
}