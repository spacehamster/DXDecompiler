#pragma FXC EmptyStructTest ps_5_0 main

struct Struct
{
    int2 a;
    struct
    {
        int b[2];
        int2 c;
        int2 d;
    } s;
    struct {} _;
    int e;
};

StructuredBuffer<Struct> sb;
RWStructuredBuffer<Struct> rwsb;

int main() : SV_TARGET
{
    return sb[0].e + rwsb[0].e;
}