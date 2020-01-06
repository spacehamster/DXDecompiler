cbuffer cbuffer1 {
	float4 cValue1[5];
	float4 cValue2[5];
}
tbuffer tbuffer1 {
	float4 tValue1[5];
	float4 tValue2[5];
}
struct foo {
	float4 sValue1[5];
	float4 sValue2[5];
};
StructuredBuffer<struct foo> tex;
cbuffer indexBuffer {
	int index1 : packoffset(c0);
	int index2 : packoffset(c1);
}
float4 main() : SV_Target
{
	float4 result = 0;
	result += cValue2[index1];
	result += tValue2[index1];
	result += tex.Load(index1).sValue2[index2];
	return result;
}