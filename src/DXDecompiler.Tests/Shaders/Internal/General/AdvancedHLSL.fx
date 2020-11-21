cbuffer cbPerObject
{
    matrix		g_mWorldViewProjection	: packoffset(c0);
    matrix		g_mWorld				: packoffset(c4);
    float4		g_vObjectColor			: packoffset(c8);
};
cbuffer cbPerFrame
{
    float3		g_vLightDir : packoffset(c0);
    float		g_fAmbient : packoffset(c0.w);
};

float4 gFloat1;
float4 gFloat2;
float4 gFloatArr1[4] =
{
    float4(-1, 1, 0, 2),
    float4(1, 1, 0, 3),
    float4(-1, -1, 0, 4),
    float4(1, -1, 0, 5),
};
int4 gInt1;
int4 gInt2;
bool gBool1;
bool gBool2;
float4x4 gMatrix1;

struct InnerStructTest {
    float foo;
    uint bar;
    snorm float baz;
    int buz;
    float4 f4; // Aligned
    float4 g4;
    float oopsie;
    float4 m4; // Misaligned
};

struct bar {
    float foofoo;
    matrix mat;
    uint foobar;
    struct {
        float foo;
        float bar;
    } inner_struct_1;
    struct {
        float baz;
        float bug;
        struct InnerStructTest inner_struct_3;
    } inner_struct_2[2];
    snorm float foobaz;
    int foobuz[8];
    float binary_decompiler_array_size_calculation_looks_sketchy;
    int2 really[3];
    float sketchy;
    float3 did[5];
    float i;
    float4 mention[7];
    float how_sketchy;
};

cbuffer cbuffer2 {
    bar structVal1[2];
    float dummy;
    bar structVal2;
    struct {
        float val1;
        float val2;
    } anonStruct1;
    struct {
        float2 val1;
        float2 val2;
    } anonStruct2;
}

tbuffer TestTBuffer {
    float4 g_TBval1;
    float3 g_TBpositions[4] =
    {
        float3(-1, 1, 0),
        float3(1, 1, 0),
        float3(-1, -1, 0),
        float3(1, -1, 0),
    };
    float2 g_TBtexcoords[4] =
    {
        float2(0,1),
        float2(1,1),
        float2(0,0),
        float2(1,0),
    };
    bar structVal3;
}

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

Texture2D	g_txDiffuse;
SamplerState g_samLinear;

struct VS_INPUT
{
    float4 vPosition	: POSITION;
    float3 vNormal		: NORMAL;
    float2 vTexcoord	: TEXCOORD0;
};
struct VS_OUTPUT
{
    float3 vNormal		: NORMAL;
    float2 vTexcoord	: TEXCOORD0;
    float4 vPosition	: SV_POSITION;
    float4 vData	: CUSTOM;
};

float4 Instrinsics()
{
    float4 result = 0;
    result += gFloat1 + gFloat2;
    result += gFloat1 - gFloat2;
    result += gFloat1 * gFloat2;
    result += gFloat1 / gFloat2;
    result += sin(gFloat1);
    result += cos(gFloat2);
    result += tan(gFloat2);
    result += dot(gFloat1, gFloat2);
    result += length(gFloat1);
    result += log(gFloat1);
    result += log10(gFloat2);
    result += max(gFloat1, gFloat2);
    result += normalize(gFloat2);
    result += radians(gFloat2);
    result += trunc(gFloat1);
    return result;
}
float4 Structs()
{
    float4 result = 0;
    result += structVal1[1].foofoo;
    result += structVal1[1].foobar;
    result += structVal1[1].inner_struct_1.foo;
    result += structVal1[1].inner_struct_1.bar;
    result += mul(gFloat1, structVal1[1].mat);
    result += structVal1[1].inner_struct_2[1].inner_struct_3.bar;
    result += structVal1[1].inner_struct_2[1].inner_struct_3.baz;
    result += structVal1[1].inner_struct_2[1].inner_struct_3.f4;
    result += structVal1[1].inner_struct_2[1].inner_struct_3.oopsie;
    result += structVal2.inner_struct_2[0].bug;
    result += g_TBval1;
    result += float4(g_TBpositions[0], 0);
    result += float4(g_TBpositions[1], 0);
    result += float4(g_TBpositions[2], 0);
    result += float4(g_TBtexcoords[3], 0, 0);
    result += structVal3.foobuz[5];
    return result;
}
float4 DynamicIndex()
{
    float4 result = 0;
    result += structVal1[gInt1.x].foofoo;
    result += float4(g_TBpositions[gInt2.x], 0);
    result += gFloatArr1[gInt1.y];
    return result;
}

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

float4 ControlFlow()
{
    float4 result = 0;
    for (int i = 0; i < gInt1.x; i++)
    {
        result += gFloat1;
    }
    switch (gInt1.x) {
    case 1:
        result += gFloat1;
        break;
    case 2:
        result += gFloat2;
        break;
    case 3:
        result += gBool1;
        break;
    default:
        result += gBool2;
        break;
    }
    while (true)
    {
        if (result.x > gFloat1.x) break;
        result += gFloat1;
    }

    if (gInt1.y > gInt2.z)
    {
        result += gFloat1;
    }
    else {
        result += gFloat2;
    }
    do {
        result += gFloat2;
        if (gBool1 == gBool2) continue;
    } while (result.y > gFloat1.y);
    return result;
}

VS_OUTPUT VSMain(VS_INPUT Input)
{
    VS_OUTPUT Output;

    Output.vPosition = mul(Input.vPosition, g_mWorldViewProjection);
    Output.vNormal = mul(Input.vNormal, (float3x3)g_mWorld);
    Output.vTexcoord = Input.vTexcoord;
    Output.vData = 0;
    Output.vData += Instrinsics();
    Output.vData += Structs();
    Output.vData += DynamicIndex();
    Output.vData += Classes();
    Output.vData += ControlFlow();
    return Output;
}

float4 PSMain(VS_OUTPUT Input) : SV_TARGET
{
    float4 vDiffuse = g_txDiffuse.Sample(g_samLinear, Input.vTexcoord);

    float fLighting = saturate(dot(g_vLightDir, Input.vNormal));
    fLighting = max(fLighting, g_fAmbient);

    return vDiffuse * fLighting;
}

struct BufType
{
    int i;
    float f;
    double d;
};

StructuredBuffer<BufType> CSBuffer0;
StructuredBuffer<BufType> CSBuffer1;
RWStructuredBuffer<BufType> CSBuffer2;
ByteAddressBuffer CSBuffer3;
ByteAddressBuffer CSBuffer4;
RWByteAddressBuffer CSBuffer5;

[numthreads(1, 1, 1)]
void CSMain(uint3 DTid : SV_DispatchThreadID)
{
    CSBuffer2[DTid.x].i = CSBuffer0[DTid.x].i + CSBuffer1[DTid.x].i;
    CSBuffer2[DTid.x].f = CSBuffer0[DTid.x].f + CSBuffer1[DTid.x].f;
    CSBuffer2[DTid.x].d = CSBuffer0[DTid.x].d + CSBuffer1[DTid.x].d;

    int i0 = asint(CSBuffer3.Load(DTid.x * 16));
    float f0 = asfloat(CSBuffer3.Load(DTid.x * 16 + 4));
    double d0 = asdouble(CSBuffer3.Load(DTid.x * 16 + 8), CSBuffer3.Load(DTid.x * 16 + 12));
    int i1 = asint(CSBuffer4.Load(DTid.x * 16));
    float f1 = asfloat(CSBuffer4.Load(DTid.x * 16 + 4));
    double d1 = asdouble(CSBuffer4.Load(DTid.x * 16 + 8), CSBuffer4.Load(DTid.x * 16 + 12));

    CSBuffer5.Store(DTid.x * 16, asuint(i0 + i1));
    CSBuffer5.Store(DTid.x * 16 + 4, asuint(f0 + f1));

    uint dl, dh;
    asuint(d0 + d1, dl, dh);

    CSBuffer5.Store(DTid.x * 16 + 8, dl);
    CSBuffer5.Store(DTid.x * 16 + 12, dh);


}

#define BEZIER_HS_PARTITION "integer"
#define INPUT_PATCH_SIZE 16
#define OUTPUT_PATCH_SIZE 16

struct HS_CONSTANT_DATA_OUTPUT
{
    float Edges[4]             : SV_TessFactor;
    float Inside[2]            : SV_InsideTessFactor;
};
struct HS_OUTPUT
{
    float3 vPosition           : BEZIERPOS;
};
struct VS_CONTROL_POINT_OUTPUT
{
    float3 vPosition        : POSITION;
};
struct DS_OUTPUT
{
    float4 vPosition        : SV_POSITION;
    float3 vWorldPos        : WORLDPOS;
    float3 vNormal            : NORMAL;
};

float  g_fTessellationFactor;

HS_CONSTANT_DATA_OUTPUT BezierConstantHS(InputPatch<VS_CONTROL_POINT_OUTPUT, INPUT_PATCH_SIZE> ip,
    uint PatchID : SV_PrimitiveID)
{
    HS_CONSTANT_DATA_OUTPUT Output;

    float TessAmount = g_fTessellationFactor;

    Output.Edges[0] = Output.Edges[1] = Output.Edges[2] = Output.Edges[3] = TessAmount;
    Output.Inside[0] = Output.Inside[1] = TessAmount;

    return Output;
}

[domain("quad")]
[partitioning(BEZIER_HS_PARTITION)]
[outputtopology("triangle_cw")]
[outputcontrolpoints(OUTPUT_PATCH_SIZE)]
[patchconstantfunc("BezierConstantHS")]
HS_OUTPUT HSMain(InputPatch<VS_CONTROL_POINT_OUTPUT, INPUT_PATCH_SIZE> p,
    uint i : SV_OutputControlPointID,
    uint PatchID : SV_PrimitiveID)
{
    HS_OUTPUT Output;
    Output.vPosition = p[i].vPosition;
    return Output;
}

float4 BernsteinBasis(float t)
{
    float invT = 1.0f - t;

    return float4(invT * invT * invT,
        3.0f * t * invT * invT,
        3.0f * t * t * invT,
        t * t * t);
}

float4 dBernsteinBasis(float t)
{
    float invT = 1.0f - t;

    return float4(-3 * invT * invT,
        3 * invT * invT - 6 * t * invT,
        6 * t * invT - 3 * t * t,
        3 * t * t);
}

float3 EvaluateBezier(const OutputPatch<HS_OUTPUT, OUTPUT_PATCH_SIZE> bezpatch,
    float4 BasisU,
    float4 BasisV)
{
    float3 Value = float3(0, 0, 0);
    Value = BasisV.x * (bezpatch[0].vPosition * BasisU.x + bezpatch[1].vPosition * BasisU.y + bezpatch[2].vPosition * BasisU.z + bezpatch[3].vPosition * BasisU.w);
    Value += BasisV.y * (bezpatch[4].vPosition * BasisU.x + bezpatch[5].vPosition * BasisU.y + bezpatch[6].vPosition * BasisU.z + bezpatch[7].vPosition * BasisU.w);
    Value += BasisV.z * (bezpatch[8].vPosition * BasisU.x + bezpatch[9].vPosition * BasisU.y + bezpatch[10].vPosition * BasisU.z + bezpatch[11].vPosition * BasisU.w);
    Value += BasisV.w * (bezpatch[12].vPosition * BasisU.x + bezpatch[13].vPosition * BasisU.y + bezpatch[14].vPosition * BasisU.z + bezpatch[15].vPosition * BasisU.w);

    return Value;
}

[domain("quad")]
DS_OUTPUT DSMain(HS_CONSTANT_DATA_OUTPUT input,
    float2 UV : SV_DomainLocation,
    const OutputPatch<HS_OUTPUT, OUTPUT_PATCH_SIZE> bezpatch)
{
    float4 BasisU = BernsteinBasis(UV.x);
    float4 BasisV = BernsteinBasis(UV.y);
    float4 dBasisU = dBernsteinBasis(UV.x);
    float4 dBasisV = dBernsteinBasis(UV.y);

    float3 WorldPos = EvaluateBezier(bezpatch, BasisU, BasisV);
    float3 Tangent = EvaluateBezier(bezpatch, dBasisU, BasisV);
    float3 BiTangent = EvaluateBezier(bezpatch, BasisU, dBasisV);
    float3 Norm = normalize(cross(Tangent, BiTangent));

    DS_OUTPUT Output;
    Output.vPosition = mul(float4(WorldPos, 1), g_mWorldViewProjection);
    Output.vWorldPos = WorldPos;
    Output.vNormal = Norm;

    return Output;
}

struct vs2gs {
    uint idx : TEXCOORD1;
};

struct gs2ps {
    float4 pos : SV_Position0;
};

[maxvertexcount(144)]
void GSMain(lineadj vs2gs input[4], inout PointStream<gs2ps> ostream)
{
    gs2ps output = (gs2ps)0;
    for (int i = 0; i < 4; i++)
    {
        float4 position = gFloat1 * gFloat2;
        output.pos = position;
        ostream.Append(output);
    }
    ostream.RestartStrip();
    ostream.Append(output);
    ostream.RestartStrip();
    ostream.Append(output);
}


VertexShader TestVertexShader5 = CompileShader(vs_5_0, VSMain());
PixelShader TestPixelShader5 = CompileShader(ps_5_0, PSMain());
ComputeShader TestComputeShader5 = CompileShader(cs_5_0, CSMain());
DomainShader TestDomainShader5 = CompileShader(ds_5_0, DSMain());
HullShader TestHullShader5 = CompileShader(hs_5_0, HSMain());
GeometryShader TestGeometryShader = CompileShader(gs_5_0, GSMain());
technique11 RenderSceneWithTexture1Light11_2
{
    pass P0
    {
        SetVertexShader(TestVertexShader5);
        SetPixelShader(TestPixelShader5);
        SetComputeShader(TestComputeShader5);
    }
    pass P1
    {
        SetGeometryShader(TestGeometryShader);
    }
    pass P2
    {
        SetDomainShader(TestDomainShader5);
        SetHullShader(TestHullShader5);
    }
}