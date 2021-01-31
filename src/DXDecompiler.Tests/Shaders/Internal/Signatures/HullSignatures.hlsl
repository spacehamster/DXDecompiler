#pragma FXC HullSignatures hs_5_0 main
// Input control point
struct VS_CONTROL_POINT_OUTPUT
{
    float3 vPosition : WORLDPOS;
    float2 vUV       : TEXCOORD0;
    float3 vTangent  : TANGENT;
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

    float3 vTangent[4]    : TANGENT;
    float2 vUV[4]         : TEXCOORD;
    float3 vTanUCorner[4] : TANUCORNER;
    float3 vTanVCorner[4] : TANVCORNER;
    float4 vCWts          : TANWEIGHTS;
};

#define MAX_POINTS 32
// Patch Constant Function
HS_CONSTANT_DATA_OUTPUT SubDToBezierConstantsHS(
    InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip,
    uint PatchID : SV_PrimitiveID)
{
    HS_CONSTANT_DATA_OUTPUT Output = (HS_CONSTANT_DATA_OUTPUT)0;

    // Insert code to compute Output here

    return Output;
}

[domain("quad")]
[partitioning("integer")]
[outputtopology("triangle_cw")]
[outputcontrolpoints(16)]
[patchconstantfunc("SubDToBezierConstantsHS")]
BEZIER_CONTROL_POINT main(
    InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip,
    uint i : SV_OutputControlPointID,
    uint PatchID : SV_PrimitiveID)
{
    BEZIER_CONTROL_POINT Output = (BEZIER_CONTROL_POINT)0;
    // Insert code to compute Output here.
    return Output;
}