
int index;
struct Simple {
	float4 Member1;
	float2 Member2;
};
Simple simpleStruct = { 1, 2, 3, 4, 5, 6};
Simple simpleArr[5];

struct TestStruct {
	int Member1;
    Simple Member2;
    float2 Member3[2];
    float4 Member4;
    float4x4 Member5;
    matrix Member6;
};
TestStruct testStruct;

void VertScene( out float4 oDiffuse : COLOR0,
                out float4 oPos : POSITION,
                out float2 oTex0 : TEXCOORD0 )
{
    oDiffuse = 0;
    oPos = 0;
    oTex0 = 0;
    oDiffuse += simpleStruct.Member1;
    oPos += simpleStruct.Member2.xyyx;
    oDiffuse += simpleArr[1].Member1;
    oPos += simpleArr[index].Member1;
    oTex0 += simpleArr[index + 2].Member1;

    oDiffuse += testStruct.Member1;
    oDiffuse += testStruct.Member2.Member1;
    oDiffuse += testStruct.Member4;
    oDiffuse += testStruct.Member5[0];
    oDiffuse += testStruct.Member5[1];
}



float4 PixScene( float4 Diffuse : COLOR0,
                 float2 Tex0 : TEXCOORD0 ) : COLOR0
{
    float4 result = 0;
    result += simpleStruct.Member1;
    result += simpleArr[1].Member1;


    result += testStruct.Member1;
    result += testStruct.Member2.Member1;
    result += testStruct.Member4;
    result += testStruct.Member5[0];
    result += testStruct.Member5[1];
    return result;
}

technique RenderScene
{
	pass P0
    {
        VertexShader = compile vs_2_0 VertScene();
        PixelShader  = compile ps_2_0 PixScene();
    }
}