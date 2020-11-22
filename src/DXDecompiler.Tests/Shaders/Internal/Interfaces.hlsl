#pragma FXC Interfaces ps_5_0 main

float4 g1;
interface iInterface1
{
	float4 Func1(float4 colour);
	float4 Func2(float4 colour);
};
class cClass1 : iInterface1
{
	float4           foo;
	float4           bar;
	float4 Func1(float4 colour) {
		float4 result = bar;
		return result;
	}
	float4 Func2(float4 colour) {
		return colour + 2;
	}
};
class cClass2 : iInterface1
{
	float4           foo;
	float4           bar;
	float4 Func1(float4 colour) {
		return colour + bar + g1;
	}
	float4 Func2(float4 colour) {
		return colour + 1;
	}
};
interface iInterface2A
{
	float4 Func1(float4 colour);
};
class cClass3 : iInterface2A
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
class cClass4 : iInterface2A
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
class cClass5 : iInterface2A
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
interface iInterface3AB
{
	float4 Func1(float4 colour);
};
interface iInterface4ABC
{
	float4 Func2(float4 colour);
};
interface iInterface5ABCDE
{
	float4 Func3(float4 colour);
};
class cClass6 : iInterface3AB
{
	float4           foo;
	float4 Func1(float4 colour) {
		float4 result = foo;
		return result;
	}
};
class cClass7 : iInterface3AB, iInterface4ABC
{
	float4           foo;
	float4 Func1(float4 colour) {
		float4 result = foo;
		return result;
	}
	float4 Func2(float4 colour) {
		float4 result = foo;
		return result;
	}
};

class cClass8 : iInterface3AB, iInterface4ABC, iInterface5ABCDE
{
	float4           foo;
	float4 Func1(float4 colour) {
		float4 result = foo;
		return result;
	}
	float4 Func2(float4 colour) {
		float4 result = foo;
		return result;
	}
	float4 Func3(float4 colour) {
		float4 result = foo;
		return result;
	}
};


iInterface1 gAbstractInterface1;
iInterface2A gAbstractInterface2;
cClass6 gAbstractInterface3;
cClass7 gAbstractInterface4;
cClass8 gAbstractInterface5;
float4 main(float4 color : COLOR0) :SV_TARGET{
	float4 result = 0;
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func2(color);
	result += gAbstractInterface2.Func1(color);
	
	
	result += gAbstractInterface3.Func1(color);
	result += gAbstractInterface4.Func1(color);
	result += gAbstractInterface5.Func3(color);
	return result;
}