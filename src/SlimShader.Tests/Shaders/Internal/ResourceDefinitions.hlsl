cbuffer cbuffer1 {
	float4 cValue;
}
tbuffer tbuffer1 {
	float4 tValue;
}
struct foo {
	float4 sValue1;
	float4 sValue2;
};
struct inner_test {
	float4 sValue1[2];
	float3x4 sValue2[2];
	float4x3 sValue3[2];
	float4 sValue4[2];
};
struct bar {
	float foofoo;
	uint foobar;
	struct {
		float foo;
		float bar;
	} inner_struct_1;
	struct {
		float baz;
		float bug;
		struct inner_test inner_struct_3[2];
	} inner_struct_2[2];
	snorm float foobaz;
	int foobuz[8];
	float binary_decompiler_array_size_calculation_looks_sketchy;
	int2 really[3];
	float sketchy;
	float3 did[5];
	float i;
	float4 mention[7];
	float how_sketchy;
	float4x3 matCM43;
	column_major float3x4 matCM34;
	row_major float4x3 matRM43;
	row_major float3x4 matRM34;
	float val;
};

cbuffer cbuffer2 {
	bar structVal1[2];
	float dummy;
	bar structVal2;
	struct {
		float val1;
		float val2;
	} anonStruct1;
	struct {
		float2 val1;
		float2 val2;
	} anonStruct2;
}
cbuffer cbuffer3 {
	column_major float4x3 testCM43;
}
cbuffer cbuffer4 {
	column_major float3x4 testCM34;
}
cbuffer cbuffer5 {
	row_major float4x3 testRM43;
}
cbuffer cbuffer6 {
	row_major float3x4 testRM34;
}
cbuffer matrices {
	row_major float1x1 matRM11;
	row_major float1x2 matRM12;
	row_major float1x3 matRM13;
	row_major float1x4 matRM14;
	row_major float2x1 matRM21;
	row_major float2x2 matRM22;
	row_major float2x3 matRM23;
	row_major float2x4 matRM24;
	row_major float3x1 matRM31;
	row_major float3x2 matRM32;
	row_major float3x3 matRM33;
	row_major float3x4 matRM34;
	row_major float4x1 matRM41;
	row_major float4x2 matRM42;
	row_major float4x3 matRM43;
	row_major float4x4 matRM44;

	column_major float1x1 matCM11;
	column_major float1x2 matCM12;
	column_major float1x3 matCM13;
	column_major float1x4 matCM14;
	column_major float2x1 matCM21;
	column_major float2x2 matCM22;
	column_major float2x3 matCM23;
	column_major float2x4 matCM24;
	column_major float3x1 matCM31;
	column_major float3x2 matCM32;
	column_major float3x3 matCM33;
	column_major float3x4 matCM34;
	column_major float4x1 matCM41;
	column_major float4x2 matCM42;
	column_major float4x3 matCM43;
	column_major float4x4 matCM44;
}
SamplerState samp0;
SamplerComparisonState samp1;

Buffer<float4> tex0 : register(t0);
Texture1D<float4> tex1 : register(t1);
Texture1DArray<float4> tex2 : register(t2);
Texture2D<float4> tex3 : register(t3);
Texture2DArray<float4> tex4 : register(t4);
Texture3D<float4> tex5 : register(t5);
TextureCube<float4> tex6 : register(t6);
TextureCubeArray<float4> tex7 : register(t7);
ByteAddressBuffer tex8 : register(t8);
StructuredBuffer<struct foo> tex9 : register(t9);
Texture2DMS<float, 4> tex10 : register(t10);
Texture2DMSArray<float, 4> tex11 : register(t11);
AppendStructuredBuffer<struct foo> uav1 : register(u1);
ConsumeStructuredBuffer<struct foo> uav2 : register(u2);
RWBuffer<float4> uav3 : register(u3);
RWByteAddressBuffer uav4 : register(u4);
RWStructuredBuffer<struct foo> uav5 : register(u5);
RWTexture1D<float4> uav6 : register(u6);
RWTexture1DArray<float4> uav7 : register(u7);
RWTexture2D<float4> uav8 : register(u8);
RWTexture2DArray<float4> uav9 : register(u9);
RWTexture3D<float4> uav10 : register(u10);

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	result += cValue;
	result += tValue;
	result += structVal1[0].inner_struct_2[0].inner_struct_3[0].sValue4[0];
	result += structVal1[1].inner_struct_2[1].inner_struct_3[1].sValue4[1];
	result += structVal2.inner_struct_2[1].inner_struct_3[1].sValue2[1][2];

	result += float4(anonStruct1.val2, 0, 0, 0);
	result += float4(anonStruct2.val2, 0, 0);
	result += float4(testCM43[3], 0);
	result += testCM34[2];
	result += float4(testRM43[3], 0);
	result += testRM34[2];

	result += matRM11[0].xxxx;

	result += tex3.Sample(samp0, index);
	result += tex3.SampleCmp(samp1, index, 0.5);

	result += tex0.Load(index); //Same as tex0[index]
	result += tex1.Load(index);
	result += tex2.Load(index);
	result += tex3.Load(index);
	result += tex4.Load(index);
	result += tex5.Load(index);
	result += tex6.Sample(samp0, index);
	result += tex7.Sample(samp0, index);
	result += tex8.Load(index);
	result += tex9.Load(index).sValue2;
	result += tex10.Load(index, index);
	result += tex11.Load(index, index);

	struct foo value = uav2.Consume();
	result += value.sValue1;
	value.sValue1 += result;
	uav1.Append(value);
	result += uav3.Load(index);
	result += uav4.Load(index);
	result += uav5.Load(index).sValue2;
	result += uav6.Load(index);
	result += uav7.Load(index);
	result += uav8.Load(index);
	result += uav9.Load(index);
	result += uav10.Load(index);
	return result;
}