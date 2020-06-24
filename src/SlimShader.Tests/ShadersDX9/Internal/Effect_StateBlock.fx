struct VS_OUTPUT {
    float4 Pos : POSITION;
};
VS_OUTPUT Object_VS(float4 Pos : POSITION, float3 Normal : NORMAL)
{
    VS_OUTPUT Out;
    Out.Pos = Pos;    
    return Out;
}

float4 Object_PS( VS_OUTPUT In , uniform bool bVal) : COLOR0
{
    float4 Color;
    
    if(bVal){
        Color = In.Pos;
    }else{
        Color = 6;
    }
    return Color;
}
stateblock objectState = stateblock_state
{
    CullMode = NONE;
    ZWriteEnable = false;
    VertexShader = compile vs_2_0 Object_VS();
    PixelShader  = compile ps_2_0 Object_PS(false);
};
technique Tech1 {
    pass {
        StateBlock = <objectState>;
    }
}