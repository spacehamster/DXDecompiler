struct foo {
	float4 sValue1;
	float4 sValue2;
};

ConsumeStructuredBuffer<foo> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	//Out Vars
	uint ou1NumStructs;
	uint ou1Stride;
	uint ou1Status;;
	//ConsumeStructuredBuffer
	uav.GetDimensions(ou1NumStructs, ou1Stride);
	result += ou1NumStructs;
	result += ou1Stride;

	foo value1 = uav.Consume();
	result += value1.sValue1;

	foo value2 = uav.Consume();
	result += value2.sValue1;
	result += value2.sValue2;

	return result;
}