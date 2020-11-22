#pragma FXC StreamOutEffect_5_FX fx_5_0

struct GSOutput
{
	float4 Pos : POSITION;
	float4 Color : COLOR;
	float4 Texcoord : TEXCOORD;
};
cbuffer cb0
{
    float g_fElapsedTime;
};


void GSGenericHandler( GSOutput input, inout PointStream<GSOutput> ParticleOutputStream )
{
    input.Pos += input.Pos*g_fElapsedTime;
    input.Color += input.Color*g_fElapsedTime;
	input.Texcoord += input.Texcoord*g_fElapsedTime;
    ParticleOutputStream.Append( input );
}

[maxvertexcount(24)]
void GSMain(triangle  GSOutput input[3], 
	inout PointStream<GSOutput> OutputStream, 
	inout PointStream<GSOutput> OutputStream1)
{
	GSGenericHandler(input[0], OutputStream);
	GSGenericHandler(input[1], OutputStream);
	GSGenericHandler(input[2], OutputStream);
	GSGenericHandler(input[0], OutputStream1);
	GSGenericHandler(input[1], OutputStream1);
	GSGenericHandler(input[2], OutputStream1);
}
GSOutput VSMain(
	float3 PosO: POSITION,
	float3 Normal: NORMAL,
	uint InstanceID: SV_VertexID)
{
	GSOutput Out;	
	Out.Pos = float4(1, 2, 3, 4);
	Out.Color = float4(5, 6, 7, 8);
	Out.Texcoord = float4(9, 10, 11, 12);
    return Out;
}

GeometryShader gsBase = CompileShader(gs_5_0, GSMain());
GeometryShader gsStreamOut1 = ConstructGSWithSO( 
	gsBase, 
	"0:POSITION.xy; 1:POSITION.zw; 2:COLOR.xy", 
	"3:TEXCOORD.xyzw; 3:$SKIP.x;", NULL, NULL, 1);

GeometryShader gsStreamOut2 = ConstructGSWithSO( CompileShader( vs_5_0, VSMain() ),
	 "POSITION.xyz;COLOR.xyz;TEXCOORD.xyz", NULL,NULL,NULL,-1 );
technique11 AdvanceParticles
{
    pass p0
    {
        SetGeometryShader( gsStreamOut1 );
    }  
}
