#pragma FXC Issue34 cs_5_0 CSMain

[numthreads(1, 1, 1)]
void CSMain (uint id : SV_DispatchThreadID, RWStructuredBuffer<int> distance)
{
InterlockedMin (distance[2], 6);
}
