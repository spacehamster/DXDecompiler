#pragma FXC DuplicateNames ps_5_0 main

cbuffer Test {
	float4 a[5];
}
cbuffer Test {
	float4 b[5];
}
tbuffer Test {
	float4 c[5];
}
tbuffer Test {
	float4 d[5];
}
struct Test {
	float4 e[5];
};
StructuredBuffer<Test> TestBuffer;

float4 main() : SV_TARGET
{
	float4 result = 0;

	result += a[0];
	result += b[0];
	result += c[0];
	// Doesn't support duplicate tbuffer names
	//result += d[0];
	result += TestBuffer[0].e[0];
	return result;
}