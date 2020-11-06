SamplerState samp0;
SamplerComparisonState samp1;
struct foo {
	float4 sValue1;
	float4 sValue2;
};
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
float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	float location1 = index;
	float location3 = index;
	float location4 = index;
	int offset = 0;
	float clamp = 0;
	uint feedback = 0;
	result += tex0.Load(index); //Same as tex0[index]
	result += tex1.Load(index);
	result += tex2.Load(index);
	result += tex3.Sample(samp0, location1, offset, clamp, feedback);
	result += CheckAccessFullyMapped(feedback);
	result += tex4.Sample(samp0, location1, offset, clamp, feedback);
	result += CheckAccessFullyMapped(feedback);
	result += tex5.Sample(samp0, location1, offset, clamp, feedback);
	result += CheckAccessFullyMapped(feedback);
	result += tex6.Sample(samp0, location3, offset, feedback);
	result += CheckAccessFullyMapped(feedback);
	result += tex7.Sample(samp0, location4, offset, feedback);
	result += CheckAccessFullyMapped(feedback);
	result += tex8.Load(index);
	result += tex9.Load(index).sValue2;
	result += tex10.Load(index, index);
	result += tex11.Load(index, index);

	return result;
}