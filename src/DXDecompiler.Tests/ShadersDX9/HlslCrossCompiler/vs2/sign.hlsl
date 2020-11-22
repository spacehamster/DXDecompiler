#pragma FXC sign vs_2_0 main

float factor;

void main( float4 vPos : POSITION,
		   out float4 oPos : POSITION)
{
    oPos = vPos * sign(factor);
}


