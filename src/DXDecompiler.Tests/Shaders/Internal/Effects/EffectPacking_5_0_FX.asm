//
// FX Version: fx_5_0
//
// 12 local buffer(s)
//
cbuffer $Globals
{
    float4  array[16];                  // Offset:    0, size:  256
    float2  test[2] = { 1, 2, 3, 4 };   // Offset:  256, size:   24
}

cbuffer IE1
{
    float4  Val1;                       // Offset:    0, size:   16
    float2  Val2;                       // Offset:   16, size:    8
    float2  Val3;                       // Offset:   24, size:    8
}

cbuffer IE2
{
    float2  Val4;                       // Offset:    0, size:    8
    float4  Val5;                       // Offset:   16, size:   16
    float2  Val6;                       // Offset:   32, size:    8
}

cbuffer IE3
{
    float1  Val7;                       // Offset:    0, size:    4
    float1  Val8;                       // Offset:    4, size:    4
    float2  Val9;                       // Offset:    8, size:    8
}

cbuffer IE4
{
    float1  Val10;                      // Offset:    0, size:    4
    float2  Val11;                      // Offset:    4, size:    8
    float1  Val12;                      // Offset:   12, size:    4
}

cbuffer IE5
{
    float1  Val13;                      // Offset:    0, size:    4
    float1  Val14;                      // Offset:    4, size:    4
    float1  Val15;                      // Offset:    8, size:    4
    float2  Val16;                      // Offset:   16, size:    8
}

cbuffer IE6
{
    float3  Val17;                      // Offset:    0, size:   12
    float1  Val18;                      // Offset:   12, size:    4
}

cbuffer IE7
{
    float1  Val19;                      // Offset:    0, size:    4
    float3  Val20;                      // Offset:    4, size:   12
}

cbuffer IE8
{
    float1  Val21;                      // Offset:    0, size:    4
    float1  Val22;                      // Offset:    4, size:    4
    float3  Val23;                      // Offset:   16, size:   12
}

cbuffer IE9
{
    float1  Val24;                      // Offset:    0, size:    4
    <unnamed> Val25;                    // Offset:   16, size:   20
}

cbuffer IE10
{
    float1  Val26;                      // Offset:    0, size:    4
    <unnamed> Val27;                    // Offset:   16, size:   32
}

cbuffer IE11
{
    <unnamed> Val28;                    // Offset:    0, size:   20
    float1  Val29;                      // Offset:   20, size:    4
}

