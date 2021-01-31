#pragma FXC GeometrySignatures gs_5_0 main
// Per-vertex data from the vertex shader.
struct GeometryShaderInput
{
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
    uint svViewportArrayIndex : SV_ViewportArrayIndex;
};

// Per-vertex data passed to the rasterizer.
struct GeometryShaderOutput
{
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
    bool svIsFrontFace : SV_IsFrontFace;
    float4 svPosition : SV_Position;
    uint SV_RenderTargetArrayIndex : SV_RenderTargetArrayIndex;
    uint svViewportArrayIndex : SV_ViewportArrayIndex;
};

// This geometry shader is a pass-through that leaves the geometry unmodified 
// and sets the render target array index.
[maxvertexcount(3)]
void main(triangle GeometryShaderInput input[3],
    inout TriangleStream<GeometryShaderOutput> outStream,
    uint svGSInstanceID : SV_GSInstanceID,
    uint svPrimitiveID : SV_PrimitiveID)
{
    GeometryShaderOutput output;
    for (int i = 0; i < 3; ++i)
    {
        output = (GeometryShaderOutput)0;
        outStream.Append(output);
    }
}