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
iInterface1 gAbstractInterface1;
iInterface1 gAbstractInterface2;
iInterface1 gAbstractInterface3[4];
iInterface2 gAbstractInterface4;
iInterface2 gAbstractInterface5[4];
cClass1 gAbstractInterface6;
cClass2 gAbstractInterface7;
cClass2 gAbstractInterface8[3];
float4 main(float4 color : COLOR0, iInterface1 testFunc1, iInterface1 testFunc2) : SV_TARGET
{
	float4 result = 0;
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface7.Func1(color);
	result += testFunc1.Func1(color);
	result += testFunc2.Func1(color);
	return result;
}
float4 PSInline(float4 color : COLOR0) : SV_TARGET
{
	float4 result = 0;
	result += gAbstractInterface1.Func1(color);
	result += gAbstractInterface7.Func1(color);
	return result;
}

struct BufType
{
	int i;
	float f;
	double d;
};

ByteAddressBuffer Buffer0 : register(t0);
ByteAddressBuffer Buffer1 : register(t1);
RWByteAddressBuffer BufferOut : register(u0);

[numthreads(1, 1, 1)]
void CSMain(uint3 DTid : SV_DispatchThreadID)
{
	int i0 = asint(Buffer0.Load(DTid.x * 8));
	float f0 = asfloat(Buffer0.Load(DTid.x * 8 + 4));
	int i1 = asint(Buffer1.Load(DTid.x * 8));
	float f1 = asfloat(Buffer1.Load(DTid.x * 8 + 4));
	f1 += gAbstractInterface1.Func1(DTid.x);
	f1 += gAbstractInterface6.Func1(DTid.y);
	BufferOut.Store(DTid.x * 8, asuint(i0 + i1));
	BufferOut.Store(DTid.x * 8 + 4, asuint(f0 + f1));
}


PixelShader PS1 = CompileShader(ps_5_0, main());
PixelShader PS2 = BindInterfaces(
	CompileShader(ps_5_0, main()),
	gAbstractInterface2,
	gAbstractInterface3[2]);
ComputeShader CS5 = CompileShader(cs_5_0, CSMain());
technique11 tech1 {
	pass p0 {
		SetPixelShader(BindInterfaces(
			PS1,
			gAbstractInterface2,
			gAbstractInterface3[1]));
	}

}

technique11 tech2 {
	pass p0 {
		SetPixelShader(PS2);
		SetComputeShader(CS5);
	}
}
technique11 tech3 {
	pass p0 {
		SetPixelShader(CompileShader(ps_5_0, PSInline()));
	}
}
