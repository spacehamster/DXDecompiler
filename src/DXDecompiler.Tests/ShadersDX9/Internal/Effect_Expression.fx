#pragma FXC Effect_Expression_FX fx_2_0

int i;
int j;
uint k;
uint l;

bool b1;
bool b2;
float4 a;
float4 b;
float4 func() {
	float4 result = 5;
	int4 temp1 = 5;
	result += a; //add
	result -= b; //neg + add
	result *= a; //mul
	result /= a; //div
	result += dot(a, b); //dot
	result = sin(result); //sin
	result = cos(result); //cos
	result = tan(result); //tan
	result = asin(result); //asin
	result = acos(result); //acos
	result = atan(result); //atan
	result = atan2(result, a); //atan2
	result = sqrt(result); //sqrt
	result = frac(result); //frc
	result = 1.0f / sqrt(result); //TODO: rsq requires optimzation?
	result = max(result, a); //max
	result = min(result, a); //min
	result = floor(result); //floor
	result = ceil(result); //ceil
	result = log(result); //log
	result = exp(result); //exp
	result = rcp(result); //rcp
	temp1 += (int)a; //ftoi
	temp1 += (int)b1; //itob & btoi

	temp1 += i; //iadd
	temp1 -= i; //ineg + iadd
	temp1 *= i; //imul
	temp1 /= i; //udiv
	//temp1 &= i; //and
	//temp1 |= i; //or
	//temp1 ^= i; //xor
	//temp1 += ~i; //not + iadd
	//temp1 <<= i; //ishl
	//temp1 >>= i; //ishr
	temp1 = min(temp1, i); //imin
	temp1 = max(temp1, i); //imax
	//temp1 = min(temp1, l); //umin
	//temp1 = max(temp1, l); //umax
	temp1 += max(i, j);
	bool temp2 = true;
	temp2 = temp2 > b1; //bult
	temp2 = temp2 == b1; //bieq
	temp2 = temp2 != b1; //bine
	result += temp1;
	result += temp2;
	return result;
}
void RenderSceneVS(
	out float4 o1 : POSITION)
{
	o1 = 0;
	o1 = func();
}

technique Tech0
{
	pass P0
	{
		VertexShader = compile vs_2_0 RenderSceneVS();
	}
}

