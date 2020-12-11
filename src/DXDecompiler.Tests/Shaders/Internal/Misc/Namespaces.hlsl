#pragma FXC Namespaces ps_5_0 main
namespace A {
	namespace B {
		struct Foo {
			float4 Abc;
		};
	}
}
namespace C
{
	cbuffer test
	{
		float4 Val;
	};
	A::B::Foo bar;
}
A::B::Foo foo;
float main() : SV_TARGET
{
	float result = 0;
	result += foo.Abc;
	result += C::bar.Abc;
	result += C::Val;
	return result;
}