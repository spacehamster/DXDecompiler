#pragma FXC UAV_AppendStructuredBuffer ps_5_0 main /Od

struct foo {
	float4 sValue1;
	float4 sValue2;
};

AppendStructuredBuffer<foo> uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	foo isValue = (foo)index;
	//Out Vars
	uint ou1NumStructs;
	uint ou1Stride;
	uint ou1Status;
	//AppendStructuredBuffer
	uav.GetDimensions(ou1NumStructs, ou1Stride);
	result += ou1NumStructs;
	result += ou1Stride;

	uav.Append(isValue);

	return result;
}