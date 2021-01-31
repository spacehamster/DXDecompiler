#pragma FXC PixelSignatures ps_5_0 main
struct PS_INPUT {
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
    //Not system-generated input signature parametes cannot appear after system generated values
    uint svPrimitiveID : SV_PrimitiveID;
    uint svRenderTargetArrayIndex : SV_RenderTargetArrayIndex;
    float4 svPosition : SV_Position;
    uint svViewportArrayIndex : SV_ViewportArrayIndex;
    //System-Value Semantics
    float svClipDistance : SV_ClipDistance0;
    float svCullDistance : SV_CullDistance0;
    uint svCoverage : SV_Coverage;
    uint svInstanceID : SV_InstanceID;
    bool svIsFrontFace : SV_IsFrontFace;
    uint svSampleIndex : SV_SampleIndex;

};
struct PS_OUTPUT {
    uint SV_Coverage : SV_Coverage;
    float svDepth : SV_Depth;
    uint svStencilRef : SV_StencilRef;
    float4 svTarget0 : SV_Target0;
    float4 svTarget1 : SV_Target1;
};
PS_OUTPUT main(PS_INPUT input)
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
    accumulator += input.svPrimitiveID;
    accumulator += input.svRenderTargetArrayIndex;
    accumulator += input.svPosition;
    accumulator += input.svPosition;
    accumulator += input.svViewportArrayIndex;
    accumulator += input.svClipDistance;
    accumulator += input.svCullDistance;
    accumulator += input.svCoverage;
    accumulator += input.svInstanceID;
    accumulator += input.svIsFrontFace;
    accumulator += input.svSampleIndex;
    PS_OUTPUT output = (PS_OUTPUT)accumulator;
    return output;
}