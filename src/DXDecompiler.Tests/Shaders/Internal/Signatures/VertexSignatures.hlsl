#pragma FXC VertexSignatures vs_5_0 main
struct VS_INPUT {
    //Vertex Shader Semantics
    float4 binomial : BINORMAL0;
    uint blendincies : BLENDINDICES0;
    float blendweights : BLENDWEIGHT0;
    float4 color : COLOR0;
    float4 normal : NORMAL0;
    float4 position : POSITION0;
    float4 positionT : POSITIONT;
    float psize : PSIZE0;
    float4 tangent : TANGENT0;
    float4 texcoord : TEXCOORD0;
    //Pixel Shader Semantics
    float4 color1 : COLOR1;
    float4 texcoord1 : TEXCOORD1;
    float vface : VFACE;
    float2 vpos : VPOS;
    //System-Value Semantics
    float svClipDistance : SV_ClipDistance0;
    float svCullDistance : SV_CullDistance0;
    uint svInstanceID : SV_InstanceID;
    float4 svPosition : SV_Position;
    uint SV_RenderTargetArrayIndex : SV_RenderTargetArrayIndex;
    uint svVertexID : SV_VertexID;
    uint svViewportArrayIndex : SV_ViewportArrayIndex;
};
struct VS_OUTPUT {
    //Vertex Shader Semantics
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
VS_OUTPUT main(VS_INPUT input)
{
    float accumulator = 0;
    accumulator += input.binomial;
    accumulator += input.blendincies;
    accumulator += input.blendweights;
    accumulator += input.color;
    accumulator += input.normal;
    accumulator += input.position;
    accumulator += input.positionT;
    accumulator += input.psize;
    accumulator += input.tangent;
    accumulator += input.texcoord;
    accumulator += input.color1;
    accumulator += input.texcoord1;
    accumulator += input.vface;
    accumulator += input.vpos;
    accumulator += input.svClipDistance;
    accumulator += input.svCullDistance;
    accumulator += input.svInstanceID;
    accumulator += input.svPosition;
    accumulator += input.SV_RenderTargetArrayIndex;
    accumulator += input.svVertexID;
    accumulator += input.svViewportArrayIndex;
    VS_OUTPUT output = (VS_OUTPUT)accumulator;
    return output;
}