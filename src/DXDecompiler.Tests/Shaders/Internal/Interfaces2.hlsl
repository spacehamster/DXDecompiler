#pragma FXC Interfaces2 ps_5_0 RenderScenePS

float4 g_MaterialAmbientColor;
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

float4 RenderScenePS(VS_OUTPUT In) : SV_TARGET
{
    float4 result = 0;
    result += g_MaterialAmbientColor;
    result += Classes();
    return result;
}
