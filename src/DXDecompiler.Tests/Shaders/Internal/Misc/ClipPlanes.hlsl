#pragma FXC ClipPlanes vs_5_0 main
//refer https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/user-clip-planes-on-10level9
cbuffer ClipPlaneConstantBuffer
{
    float4 clipPlane1;
    float4 clipPlane2;
};
struct VertexShaderOutput {
    float4 color : COLOR;
    float4 position : SV_POSITION;
};
struct VertexShaderInput {
    float4 color : COLOR;
};
[clipplanes(clipPlane1, clipPlane2)]
VertexShaderOutput main(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    return output;
}