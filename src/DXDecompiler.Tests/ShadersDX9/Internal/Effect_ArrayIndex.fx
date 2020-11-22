#pragma FXC Effect_ArrayIndex_FX fx_2_0

float4 array[10];
int index;

void VertScene(out float4 oPos : POSITION )
{
    oPos = 0;
    oPos += array[index];
}

float4 PixScene()
{
	float4 result = 0;
	result += array[index];
	return result;
}

technique RenderScene
{
	pass P0
    {
        VertexShader = compile vs_2_0 VertScene();
		PixelShader = compile ps_2_0 PixScene();
    }
}