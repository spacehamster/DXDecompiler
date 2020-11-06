RWByteAddressBuffer uav;

float4 main(int index : POSITION) : SV_Target
{
	float4 result = 0;
	//InVars
	uint iu1Dest = index;
	uint iu1Value = index;
	uint2 iu2Value = index;
	uint3 iu3Value = index;
	uint4 iu4Value = index;
	uint iu1CompareValue = index;
	uint iuLocation = index;
	uint iu1Address = index;
	//Out Vars
	uint ou1Dim;
	uint ou1OriginalValue;
	uint ou1Status;
	//TextureCubeArray
	uav.GetDimensions(ou1Dim);
	result += ou1Dim;

	uav.InterlockedAdd(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedAdd(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedAnd(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedAnd(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedCompareExchange(iu1Dest, iu1CompareValue, iu1Value, ou1OriginalValue);
	uav.InterlockedCompareExchange(iu1Dest, iu1CompareValue, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedCompareStore (iu1Dest, iu1CompareValue, iu1Value);

	
	uav.InterlockedExchange(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedExchange(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedMax(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedMax(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedMin(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedMin(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedOr(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedOr(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	uav.InterlockedXor(iu1Dest, iu1Value, ou1OriginalValue);
	uav.InterlockedXor(iu1Dest, iu1Value, ou1OriginalValue);
	result += ou1OriginalValue;

	result += uav.Load(iuLocation);
	result += uav.Load(iuLocation, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result.xy += uav.Load2(iuLocation);
	result.xy += uav.Load2(iuLocation, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result.xyz += uav.Load3(iuLocation);
	result.xyz += uav.Load3(iuLocation, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	result += uav.Load4(iuLocation);
	result += uav.Load4(iuLocation, ou1Status);
	result += CheckAccessFullyMapped(ou1Status);

	uav.Store(iu1Address, iu1Value);
	uav.Store2(iu1Address, iu2Value);
	uav.Store3(iu1Address, iu3Value);
	uav.Store4(iu1Address, iu4Value);

	return result;
}