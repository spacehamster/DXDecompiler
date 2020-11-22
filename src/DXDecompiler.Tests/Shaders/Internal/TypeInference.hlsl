#pragma FXC TypeInference ps_5_0 main

// FXC disassembler will interepret 1065353216 as 1.0 for mov instructions
int loop_max;
float4 main( ) : SV_Target
{
    float4 result = 0;
    for(int i = 1; i < loop_max; i++){
        result += i;
    }
    for(int i = 1065353216; i < loop_max; i++){
        result += i;
    }
    return result;
}
