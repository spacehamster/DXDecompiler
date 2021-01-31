#pragma FXC InputIndexing ps_5_0 main
struct PS_INPUT {
    float2 color1 : COLOR0;
    float2 color2 : COLOR1;
    float2 color3 : COLOR2;
};
float4 main(PS_INPUT input) : SV_TARGET
{
    //Input/Output values can be accessed by register
    float4 result = 0;
    result += ((float4[1])input)[0] * 3;
    //result += ((float4[2])input)[1]; //Cannot convert Struct PS_INPUT to float4[2]
    result.xy += ((float2[3])input)[2] * 4;
    return result;
}