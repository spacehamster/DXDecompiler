#pragma FXC OutputRegisterPacking vs_5_0 main

struct VS_OUTPUT {
    float c0 : COLOR0;
    float c1 : COLOR1;
    float c2 : COLOR2;
    float c3 : COLOR3;
    float c4 : COLOR4;
    float c5 : COLOR5;
    float c6 : COLOR6;
    float c7 : COLOR7;
};
float4 global;
VS_OUTPUT main() {
    //Input and output registers will be combined
    //All 4 shader outputs will be assigned to with a single operation
    //mul o0.xyzw, cb0[0].xyzw, l(5.000000, 5.000000, 5.000000, 5.000000)
    //mul o1.xyzw, cb0[0].xyzw, l(10.000000, 10.000000, 10.000000, 10.000000)
    //This can be translated to hlsl with:
    //((float4[2])o)[0] = global * 5;
    //((float4[2])o)[1] = global * 10;
    VS_OUTPUT o = (VS_OUTPUT)2;
    o.c0 = global.x * 5;
    o.c1 = global.y * 5;
    o.c2 = global.z * 5;
    o.c3 = global.w * 5;
    o.c4 = global.x * 10;
    o.c5 = global.y * 10;
    o.c6 = global.z * 10;
    o.c7 = global.w * 10;
    return o;
}