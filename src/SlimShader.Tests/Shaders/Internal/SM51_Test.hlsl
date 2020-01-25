SamplerState samp0 : register(s0);
SamplerState samp1[5] : register(s0, space2);
SamplerState samp2[] : register(s10, space2);

Texture2D foo[5] : register(t2);
Buffer bar : register(t7);
Texture2D terrain[] : register(t8); // Unbounded array
Texture2D misc[] : register(t0,space1); // Another unbounded array 
                                        // space1 avoids overlap with above t#
Texture2D misc2[10] : register(t0,space3); // Another unbounded array 
                                        // space1 avoids overlap with above t#

RWBuffer<float4> dataLog : register(u1, space2);
RasterizerOrderedTexture2D<float4> rov1[5] : register(u3, space2);
// Are unorm and snorm SM5.1 only?
RWBuffer<float> uav0;
RWBuffer<unorm float> uav1;
RWBuffer<snorm float> uav2;

struct Data
{
    uint index;
    float4 color;
};
ConstantBuffer<Data> myData[4] : register(b0);

struct MoreData
{
    float4x4 xform;
};
ConstantBuffer<MoreData> myMoreData : register(b5);

struct Stuff
{
    float2 factor;
    uint drawID;
};
ConstantBuffer<Stuff> myStuff[][3][8]  : register(b2, space3);

ConstantBuffer<Stuff> myStuff2[3][8]  : register(b3, space4);

float4 main(
	uint index : INDEX,
	float2 uv : POSITION, 
	uint innerCoverage : SV_INNERCOVERAGE, 
	out uint stencilRef : SV_STENCILREF) : SV_TARGET 
{
	float4 result = 0;

	stencilRef = innerCoverage;

	result += dataLog.Load(index);
	result += uav0.Load(index);
	result += uav1.Load(index);
	result += uav2.Load(index);
	result += rov1[2].Load(index);
	result += rov1[index].Load(index);
	
	result += myData[3].color;
	result += myData[index].color;
	result += myMoreData.xform[0].z;
	result.xy += myStuff[3][1][2].factor;
	result.xy += myStuff[index][1][2].factor;
	result.xy += myStuff2[1][2].factor;

	result += foo[1].Sample(samp0, uv);
	result += foo[index].Sample(samp1[index], uv);
	result += bar.Load(index);
	result += terrain[1].Sample(samp1[5], uv);
	result += terrain[index].Sample(samp2[index], uv);
	result += misc[index].Sample(samp2[5], uv);
	result += misc2[index].Sample(samp0, uv);

	return result;
}