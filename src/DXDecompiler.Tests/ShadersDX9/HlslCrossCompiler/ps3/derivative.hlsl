#pragma FXC derivative ps_3_0 main

float4 main( float4 vTex0 : TEXCOORD0 ) : COLOR0
{
    return ddx(vTex0) * ddy(vTex0);
}


