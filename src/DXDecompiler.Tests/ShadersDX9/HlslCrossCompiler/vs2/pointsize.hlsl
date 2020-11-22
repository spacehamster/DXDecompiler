#pragma FXC pointsize vs_2_0 main

void main( float4 vPos : POSITION,
		   out float4 oPos : POSITION,
		   out float oPtSize : PSIZE)
{
	oPos = vPos;
	oPtSize = 5.0;
}



