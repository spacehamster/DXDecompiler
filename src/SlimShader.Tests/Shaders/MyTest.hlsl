export float3 TestFunction(float3 input)
{
      return input * 2.0f;
}
float4 main(float4 coord : COORD) : SV_Target
{
	float4 result = coord;
	return result;
}