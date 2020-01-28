export float3 TestFunction(float3 input)
{
      return input * 2.0f;
}
export float3 TestFunction2(float3 input, uint1 foo) : SV_TARGET
{
      return input * 2.0f;
}
export float4x4 TestFunction3(uint input, int4x3 foo : COLOR)
{    
    return input * 2.0f;
}