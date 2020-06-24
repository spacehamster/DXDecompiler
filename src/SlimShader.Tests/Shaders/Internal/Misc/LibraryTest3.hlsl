float4 TestGlobal1 = 5;
int4 TestGlobal2 = 6;
struct TestStruct {
	float3 Member1;	
	int3 Member2;	
};
TestStruct Foo;
export float4 TestFunction(uint input)
{
	return input * 2.0f +
		TestGlobal1 + 
		TestGlobal2 + 
		Foo.Member1.xyzx +
		Foo.Member2.xyzx;
}
