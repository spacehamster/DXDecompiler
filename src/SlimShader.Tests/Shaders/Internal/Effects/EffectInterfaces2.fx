float4 g1;
interface iInterface5
{
	float4 Func1(float4 colour);
	float4 Func2(float4 colour);
};

interface iInterface1
{
	float4 Func1(float4 colour);
	float4 Func2(float4 colour);
};

interface iInterface3
{
	float4 Func3(float4 colour);
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
		return 5;
	}
};
class cClass4 : iInterface2, iInterface3
{
	float4 Func1(float4 colour) {
		return 5;
	}
	float4 Func3(float4 colour) {
		return 5;
	}
};
class cClass5
{

	float4 Func1(float4 colour) {
		return 5;
	}
};
/*

Unread Memory 010F:011A[00AF:00BA] (See 010B:010F - TypeOffset) hex(00000000 01000000 4F000000), chr(........O...)
Unread Memory 0173:017E[0113:011E] (See 016F:0173 - TypeOffset) hex(00000000 01000000 4F000000), chr(........O...)
Unread Memory 01CF:01DA[016F:017A] (See 01CB:01CF - TypeOffset) hex(00000000 01000000 4F000000), chr(........O...)
Unread Memory 01F7:0226[0197:01C6] (See 01EF:01F7 - TypeName) hex(69496E74 65726661 63653200 97010000 03000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000), chr(iInterface2.....................................)
Unread Memory 0243:024E[01E3:01EE] (See 023F:0243 - MemberCount) hex(00000000 01000000 A3010000), chr(........£...)
Unread Memory 026B:029A[020B:023A] (See 0263:026B - TypeName) hex(69496E74 65726661 63653300 0B020000 03000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000), chr(iInterface3.....................................)
Unread Memory 02B7:02C6[0257:0266] (See 02B3:02B7 - MemberCount) hex(00000000 02000000 A3010000 17020000), chr(........£.......)
Unread Memory 0300:0307[02A0:02A7] (See 02FC:0300 - MemberCount) hex(00000000 00000000), chr(........)
*/
//Unread Memory 00AF:00D2[004F:0072] (See 00A3:00AF - TypeName) hex(43000000 03000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000), chr(C...................................)
//Unread Memory 00A3:00D2[0043:0072] (See 009F:00A3 - Name) hex(69496E74 65726661 63653100 43000000 03000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000), chr(iInterface1.C...................................)
//iInterface1 gAbstractInterface1;
cClass1 gAbstractInterface6; //hex(00000000 01000000 4F000000)
cClass2 gAbstractInterface7; //hex(00000000 01000000 4F000000)
cClass2 gAbstractInterface8[3]; //hex(00000000 01000000 4F000000)
cClass3 gAbstractInterface9; //hex(00000000 01000000 A3010000)
cClass4 gAbstractInterface10; //hex(00000000 02000000 A3010000 17020000)
cClass5 gAbstractInterface11; //hex(00000000 00000000)