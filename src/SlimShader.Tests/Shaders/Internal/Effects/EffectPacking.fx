//  2 x 16byte elements
cbuffer IE1
{
	float4 Val1;
	float2 Val2;  // starts a new vector
	float2 Val3;
};

//  3 x 16byte elements
cbuffer IE2
{
	float2 Val4;
	float4 Val5;  // starts a new vector
	float2 Val6;  // starts a new vector
};

//  1 x 16byte elements
cbuffer IE3
{
	float1 Val7;
	float1 Val8;
	float2 Val9;
};

//  1 x 16byte elements
cbuffer IE4
{
	float1 Val10;
	float2 Val11;
	float1 Val12;
};

//  2 x 16byte elements
cbuffer IE5
{
	float1 Val13;
	float1 Val14;
	float1 Val15;
	float2 Val16;    // starts a new vector
};


//  1 x 16byte elements
cbuffer IE6
{
	float3 Val17;
	float1 Val18;
};

//  1 x 16byte elements
cbuffer IE7
{
	float1 Val19;
	float3 Val20;
};

//  2 x 16byte elements
cbuffer IE8
{
	float1 Val21;
	float1 Val22;
	float3 Val23;        // starts a new vector
};


// 3 x 16byte elements
cbuffer IE9
{
	float1 Val24;


	struct     {

		float4 SVal1;    // starts a new vector
float1 SVal2;    // starts a new vector
    } Val25;
};

// 3 x 16byte elements
cbuffer IE10
{
	float1 Val26;

	struct     {

		float1 SVal1;     // starts a new vector
float4 SVal2;     // starts a new vector
    } Val27;
};

// 3 x 16byte elements
cbuffer IE11
{

	struct     {

		float4 SVal1;
float1 SVal2;    // starts a new vector
    } Val28;

    float1 Val29;   // starts a new vector
};
float4 array[16];
static float2 aggressivePackArray[32] = (float2[32])array;
float2 test[2] = (float2[2])float4(1, 2, 3, 4);