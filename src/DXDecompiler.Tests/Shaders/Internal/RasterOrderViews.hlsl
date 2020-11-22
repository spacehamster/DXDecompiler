#pragma FXC RasterOrderViews ps_5_1 main

struct foo {
	float4 sValue1;
	float4 sValue2;
};
RasterizerOrderedBuffer<float4> rov1;
RasterizerOrderedByteAddressBuffer rov2;
RasterizerOrderedStructuredBuffer<struct foo> rov3;
RasterizerOrderedTexture1D<float4> rov4;
RasterizerOrderedTexture1DArray<float4> rov5;
RasterizerOrderedTexture2D<float4> rov6;
RasterizerOrderedTexture2DArray<float4> rov7;
RasterizerOrderedTexture3D<float4> rov8;

RWBuffer<float4> uav1;
RWByteAddressBuffer uav2;
RWStructuredBuffer<struct foo> uav3;
RWTexture1D<float4> uav4;
RWTexture1DArray<float4> uav5;
RWTexture2D<float4> uav6;
RWTexture2DArray<float4> uav7;
RWTexture3D<float4> uav8;

float4 main(	int index : POSITION, 
				uint coverage : SV_INNERCOVERAGE,
				out uint stencilRef : SV_StencilRef ) : SV_TARGET 
{
	float4 result = 0;
	stencilRef = coverage;
	result += rov1.Load(index);
	result += rov2.Load(index);
	result += rov3.Load(index).sValue2;
	result += rov4.Load(index);
	result += rov5.Load(index);
	result += rov6.Load(index);
	result += rov7.Load(index);
	result += rov8.Load(index);

	result += uav1.Load(index);
	result += uav2.Load(index);
	result += uav3.Load(index).sValue2;
	result += uav4.Load(index);
	result += uav5.Load(index);
	result += uav6.Load(index);
	result += uav7.Load(index);
	result += uav8.Load(index);

	return result;
}