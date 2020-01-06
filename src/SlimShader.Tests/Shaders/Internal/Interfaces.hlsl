
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
interface iInterface2
{
	float4 Func1(float4 colour);
};
class cClass3 : iInterface2
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
class cClass4 : iInterface2
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
class cClass5 : iInterface2
{
	float4 Func1(float4 colour) {
		return colour + g1;
	}
};
iInterface1 gAbstractInterface1;
iInterface2 gAbstractInterface2;
float4 main(float4 color : COLOR0) :SV_TARGET{
	float4 result = 0;
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface1.Func2(color);
	result += gAbstractInterface2.Func1(color);
	return result;
}