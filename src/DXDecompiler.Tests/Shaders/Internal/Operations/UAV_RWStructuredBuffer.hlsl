#pragma FXC UAV_RWStructuredBuffer ps_5_0 main /Od

struct foo {
	float4 sValue1;
	float4 sValue2;
};

RWStructuredBuffer<struct foo> uav1;
RWStructuredBuffer<struct foo> uav2;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	int ii1Location = index;
	uint iu1Pos = index;
	//Out Vars
	uint ou1NumStructs;
	uint ou1Stride;
	uint ou1Status;
	//Structuredbuffer
	uav1.GetDimensions(ou1NumStructs, ou1Stride);
	result += ou1NumStructs;
	result += ou1Stride;

	foo val1 = uav1.Load(ii1Location);
	result += val1.sValue1;
	result += val1.sValue2;

	foo val2 = uav1.Load(ii1Location, ou1Status);
	result += val2.sValue1;
	result += val2.sValue2;
	result += CheckAccessFullyMapped(ou1Status);

	foo val3 = uav1[iu1Pos];
	result += val3.sValue1;
	result += val3.sValue2;

	result += uav1.IncrementCounter();
	result += uav1.IncrementCounter();
	result += uav2.DecrementCounter();
	result += uav2.DecrementCounter();
	return result;
}