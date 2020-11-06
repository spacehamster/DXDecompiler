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
#define MAX_CONTROL_POINTS 16

float4 gTest;

// Patch Constant Function
HS_CONSTANT_DATA_OUTPUT PatchConstantFunc( 
    InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip,
    OutputPatch<BEZIER_CONTROL_POINT, MAX_CONTROL_POINTS> op,
    uint PatchID : SV_PrimitiveID )
{   
    HS_CONSTANT_DATA_OUTPUT Output = (HS_CONSTANT_DATA_OUTPUT)0;

    Output.Edges[0] = op[0].vPosition.x;
    Output.Edges[1] = ip[0].vPosition.y;
    Output.Edges[2] = (ip[0].vPosition * op[0].vPosition).z;
    Output.Edges[3] = gTest.w;

    Output.vTangent[0] = op[0].vPosition;
    Output.vTangent[1] = ip[0].vPosition;
    Output.vTangent[2] = ip[0].vPosition * op[0].vPosition;
    Output.vTangent[3] = gTest.xyz;


    Output.vUV[0] = op[PatchID].vPosition.xy;
    Output.vUV[1] = ip[PatchID].vPosition.xy;
    Output.vUV[2] = (ip[PatchID].vPosition * op[PatchID].vPosition).xy;
    Output.vUV[3] = PatchID;

    return Output;
}
[domain("quad")]
[partitioning("integer")]
[outputtopology("triangle_cw")]
[outputcontrolpoints(MAX_CONTROL_POINTS)]
[maxtessfactor(64.0f)]
[patchconstantfunc("PatchConstantFunc")]
BEZIER_CONTROL_POINT main( 
    InputPatch<VS_CONTROL_POINT_OUTPUT, MAX_POINTS> ip, 
    uint i : SV_OutputControlPointID,
    uint PatchID : SV_PrimitiveID )
{
    BEZIER_CONTROL_POINT Output = (BEZIER_CONTROL_POINT) 0;

    Output.vPosition = float3(ip[i].vUV.xy * PatchID, (ip[2].vPosition * i).x);

    return Output;
}