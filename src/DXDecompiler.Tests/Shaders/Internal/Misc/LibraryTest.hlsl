#pragma FXC LibraryTest_4_0 lib_4_0
#pragma FXC LibraryTest_4_1 lib_4_1
#pragma FXC LibraryTest_5_0 lib_5_0
#pragma FXC LibraryTest_4_Level_9_1 lib_4_0_level_9_1
#pragma FXC LibraryTest_4_Level_9_3 lib_4_0_level_9_3
#pragma FXC LibraryTest_4_Level_9_1_VS lib_4_0_level_9_1_vs_only
#pragma FXC LibraryTest_4_Level_9_1_PS lib_4_0_level_9_1_ps_only
#pragma FXC LibraryTest_4_Level_9_3_VS lib_4_0_level_9_3_vs_only
#pragma FXC LibraryTest_4_Level_9_3_PS lib_4_0_level_9_3_ps_only

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
int4 TestGlobal2 = 6;
cbuffer Foo{
	float3 TestBuffer1 = 7;
	int3 TestBuffer2 = 8;
}
export float4 TestFunction6(uint input)
{
	return input * 2.0f + TestGlobal + TestGlobal2 + TestBuffer1.xyzx + TestBuffer2.xyzx;
}