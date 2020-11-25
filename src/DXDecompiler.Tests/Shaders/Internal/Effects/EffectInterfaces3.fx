#pragma FXC EffectInterfaces3_5_FX fx_5_0

// Global variables
float4 g_MaterialAmbientColor;      // Material's ambient color
float4 gFloat1;
float4 gFloat2;
struct VS_OUTPUT
{
    float4 Position   : SV_POSITION; // vertex position 
};
interface iInterface1
{
    float4 Func1(float4 colour);
    float4 Func2(float4 colour);
};

class cClass1 : iInterface1
{
    float4           foo;
    float4           bar;
    float4 Func1(float4 color) {
        float4 result = foo + sin(color);
        return result;
    }
    float4 Func2(float4 color) {
        return color * 55;
    }
};

class cClass2 : iInterface1
{
    float4           foo;
    float4           bar;
    float4 Func1(float4 color) {
        return color + sqrt(foo) + gFloat1;
    }
    float4 Func2(float4 color) {
        return color + log(bar);
    }
};
iInterface1 gAbstractInterface1;
cClass1 gAbstractInterface2;
cClass2 gAbstractInterface3;

float4 Classes()
{
    float4 result = 0;
    float4 color = gFloat2;
    result += gAbstractInterface1.Func1(color);
    result += gAbstractInterface1.Func2(color);
    result += gAbstractInterface1.Func1(color);
    result += gAbstractInterface2.Func1(color);
    result += gAbstractInterface2.Func2(color);
    result += gAbstractInterface3.Func1(color);
    result += gAbstractInterface3.Func2(color);
    return result;
}
VS_OUTPUT RenderSceneVS(float4 vPos : POSITION,
    float3 vNormal : NORMAL,
    float2 vTexCoord0 : TEXCOORD)
{
    VS_OUTPUT Output = (VS_OUTPUT)0;
    Output.Position += g_MaterialAmbientColor;
    Output.Position += Classes();
    return Output;
}
float4 RenderScenePS(VS_OUTPUT In) : SV_TARGET
{
    float4 result = 0;
    result += g_MaterialAmbientColor;
    result += Classes();
    return result;
}
VertexShader vs = CompileShader(vs_5_0, RenderSceneVS());
PixelShader ps = CompileShader(ps_5_0, RenderScenePS());
technique11 RenderSceneWithTexture1Light11_1
{
    pass P0
    {
        SetVertexShader(vs);
        SetPixelShader(ps);
    }
}