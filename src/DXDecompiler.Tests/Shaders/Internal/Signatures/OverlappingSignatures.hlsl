#pragma FXC SignatureAliasing ps_5_0 main
//Note: Overlapping semantics will throw compile errors with DXC
float4 main(
    float4 color : COLOR,
    float4 colorDup : COLOR,
    float4 color0 : COLOR0,
    float4 color1 : COLOR1,
    float4 colorA : COLORA
) : SV_TARGET
{
    float4 result = 0;
    result += max(color, 1);
    result += max(colorDup, 2);
    result += max(color0, 3);
    result += max(color1, 4);
    result += max(colorA, 5);
    return result;
}