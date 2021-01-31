#pragma FXC DomainSignatures ds_5_0 main
struct DS_OUTPUT
{
    float3 vNormal    : NORMAL;
    float2 vUV        : TEXCOORD;
    float3 vTangent   : TANGENT;
    float3 vBiTangent : BITANGENT;
    //Vertex Shadr Semantics
    float4 color2 : COLOR2;
    float fog : FOG;
    float4 position : POSITION1;
    float psize : PSIZE1;
    float tessfactor : TESSFACTOR0;
    float4 texcoord : TEXCOORD1;
    //Pixel Shader Smantics
    float4 color3 : COLOR3;
    float4 depth : DEPTH0;
    //System-Value Semantics
    float svClipDistance : SV_ClipDistance1;
    float svCullDistance : SV_CullDistance1;
    uint svInstanceID : SV_InstanceID;
    float4 svPosition : SV_Position;
    uint SV_RenderTargetArrayIndex : SV_RenderTargetArrayIndex;
    uint svViewportArrayIndex : SV_ViewportArrayIndex;
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

// Output control point
struct BEZIER_CONTROL_POINT
{
    float3 vPosition    : BEZIERPOS;
};

[domain("quad")]
DS_OUTPUT main(HS_CONSTANT_DATA_OUTPUT input,
    float2 UV : SV_DomainLocation,
    const OutputPatch<BEZIER_CONTROL_POINT, 16> bezpatch)
{
    float4 accumulator = 0;
    [unroll]
    for (int i = 0; i < 4; i++) {
        accumulator += input.Edges[i];
        accumulator.xyz += input.vTangent[i];
        accumulator.xy += input.vUV[i];
        accumulator.xyz += input.vTanUCorner[i];
        accumulator.xyz += input.vTanUCorner[i];
        accumulator.xyz += input.vTanVCorner[i];
    }
    accumulator += input.Inside[0];
    accumulator += input.Inside[1];
    accumulator += input.vCWts;
    accumulator.xy += UV;
    DS_OUTPUT Output = (DS_OUTPUT)(accumulator.x + accumulator.y + accumulator.z + accumulator.w);
    return Output;
}