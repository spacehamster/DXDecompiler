#pragma FXC HullShader_QuadPoint hs_5_0 main

// Input control point
struct VS_CONTROL_POINT_OUTPUT
{

};
// Output control point
struct BEZIER_CONTROL_POINT
{
	float3 vPosition    : BEZIERPOS;
};
// Output patch constant data.
struct HS_CONSTANT_DATA_OUTPUT
{
	float Edges[4]        : SV_TessFactor;
	float Inside[2]       : SV_InsideTessFactor;
};

#define MAX_POINTS 32
#define MAX_CONTROL_POINTS 16

float4 gTest;

// Patch Constant Function
HS_CONSTANT_DATA_OUTPUT PatchConstantFunc(
	InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip,
	OutputPatch<BEZIER_CONTROL_POINT, MAX_CONTROL_POINTS> op,
	uint PatchID : SV_PrimitiveID)
{
	HS_CONSTANT_DATA_OUTPUT Output = (HS_CONSTANT_DATA_OUTPUT)0;
	return Output;
}
[domain("quad")]
[partitioning("integer")]
[outputtopology("point")]
[outputcontrolpoints(MAX_CONTROL_POINTS)]
[patchconstantfunc("PatchConstantFunc")]
BEZIER_CONTROL_POINT main(
	InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip,
	uint i : SV_OutputControlPointID,
	uint PatchID : SV_PrimitiveID)
{
	BEZIER_CONTROL_POINT Output = (BEZIER_CONTROL_POINT)0;
	Output.vPosition.x = PatchID;
	return Output;
}