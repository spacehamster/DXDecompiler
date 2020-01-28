sampler tex0;
sampler1D tex1;
sampler2D tex2;
sampler3D tex3;
samplerCUBE tex4;

float4 main(float4 pos : POSITION) : SV_TARGET 
{
    float4 result = 0;

    //Plain sampler is implictly typed?
    result += texCUBE(tex0, pos);

    float ddx1 = pos.z;
    float ddy1 = pos.w;

    result += tex1D(tex1, pos.x);
    result += tex1D(tex1, pos.x, ddx1, ddy1);
    result += tex1Dbias(tex1, pos.xyzw);
    result += tex1Dgrad(tex1, pos.x, ddx1, ddy1);
    result += tex1Dlod(tex1, pos.xyzw);
    result += tex1Dproj(tex1, pos.xyzw);

    float2 ddx2 = pos.z;
    float2 ddy2 = pos.w;
    result += tex2D(tex2, pos.xy);
    result += tex2D(tex2, pos.xy, ddx2, ddy2);
    result += tex2Dbias(tex2, pos.xyzw);
    result += tex2Dgrad(tex2, pos.xy, ddx2, ddy2);
    result += tex2Dlod(tex2, pos.xyzw);
    result += tex2Dproj(tex2, pos.xyzw);

    float3 ddx3 = pos.z;
    float3 ddy3 = pos.w;
    result += tex3D(tex3, pos.xyz);
    result += tex3D(tex3, pos.xyz, ddx3, ddy3);
    result += tex3Dbias(tex3, pos.xyzw);
    result += tex3Dgrad(tex3, pos.xyz, ddx3, ddy3);
    result += tex3Dlod(tex3, pos.xyzw);
    result += tex3Dproj(tex3, pos.xyzw);


    result += texCUBE(tex4, pos.xyz);
    result += texCUBE(tex4, pos.xyz, ddx3, ddy3);
    result += texCUBEbias(tex4, pos.xyzw);
    result += texCUBEgrad(tex4, pos.xyz, ddx3, ddy3);
    result += texCUBElod(tex4, pos.xyzw);
    result += texCUBEproj(tex4, pos.xyzw);

    
    return result;
}