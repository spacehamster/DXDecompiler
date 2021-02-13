#pragma FXC SM51DynamicIndexing ps_5_1 main /enable_unbounded_descriptor_tables
cbuffer CBuf0
{
	int i2;
	float f1;
	float4 farr[3];
}
struct CBStruct {
	int i2;
	float f2;
	float4 farr[3];
};
ConstantBuffer<CBStruct> CBuf1[3];
ConstantBuffer<CBStruct> CBuf2[];

Texture2D	tex1;
SamplerState samp1;
Texture2D	tex2[3];
SamplerState samp2[3];
Texture2D	tex3[];
SamplerState samp3[];

RWStructuredBuffer<struct CBStruct> tex4;
RWStructuredBuffer<struct CBStruct> tex5[3];
RWStructuredBuffer<struct CBStruct> tex6[];

float4 main(int idx1 : IDXA, int idx2 : IDXB, int idx3 : IDXC, float texIdx : IDXD) : SV_TARGET
{
	//SM5.1 allows for less constrained dynamic indexing and unbounded arrayys
	//https://docs.microsoft.com/en-us/windows/win32/direct3d12/dynamic-indexing-using-hlsl-5-1
	float4 result = 0;
	result += f1;
	result += farr[idx1];
	result += CBuf1[idx1].f2;
	result += CBuf1[idx1].farr[idx1];
	result += CBuf2[idx1].f2;
	result += CBuf2[idx1].farr[idx2];
	result += tex1.Sample(samp1, texIdx);
	result += tex2[idx1].Sample(samp2[idx2], texIdx);
	result += tex3[idx1].Sample(samp3[idx2], texIdx);

	result += tex4.Load(idx1).f2;
	result += tex4.Load(idx1).farr[idx2];
	result += tex5[idx1].Load(idx2).f2;
	result += tex5[idx1].Load(idx2).farr[idx3];
	result += tex6[idx1].Load(idx2).f2;
	result += tex6[idx1].Load(idx2).farr[idx3];

	return result;
}