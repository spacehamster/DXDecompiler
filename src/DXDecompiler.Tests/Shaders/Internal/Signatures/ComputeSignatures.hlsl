#pragma FXC ComputeSignatures cs_5_0 main

RWByteAddressBuffer buf : register(u0);
struct CSInput {
    //System-Value Semantics
    uint3 svDispatchThreadID : SV_DispatchThreadID;
    uint3 svGroupID : SV_GroupID;
    uint svGroupIndex : SV_GroupIndex;
    uint3 svGroupThreadID : SV_GroupThreadID;
};
[numthreads(4, 2, 1)]
void main(CSInput input)
{
    uint3 accumulator = 0;
    accumulator += input.svDispatchThreadID;
    accumulator += input.svGroupID;
    accumulator += input.svGroupIndex;
    accumulator += input.svGroupThreadID;
    buf.InterlockedCompareStore(0, 1, accumulator.x);
    buf.InterlockedCompareStore(0, 1, accumulator.y);
    buf.InterlockedCompareStore(0, 1, accumulator.z);
}
