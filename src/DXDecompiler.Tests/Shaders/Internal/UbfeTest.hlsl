#pragma FXC UbfeTest vs_5_0 VSMain

struct TestOut
{
    float4 color1 : COLOR1;
    float4 color2 : COLOR2;
    float4 color3 : COLOR3;
    float4 color4 : COLOR4;
    float4 color5 : COLOR5;
};
float4 colorFromUInt(uint color){
    return float4(color & 255, 
        (color >> 8) & 255, 
        (color >> 16) & 255, 
        (color >> 24) & 255) * (1.0f / 255);
}
cbuffer buff {
    uint color1 : packoffset(c0);
    uint4 color2 : packoffset(c1);
}
void VSMain(out TestOut o)
{
    o = (TestOut)0;
    o.color1 = colorFromUInt(color1);
    o.color2 = colorFromUInt(color2.x);
    o.color3 = colorFromUInt(color2.y);
    o.color4 = colorFromUInt(color2.z);
    o.color5 = colorFromUInt(color2.w);
}