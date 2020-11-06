int CurNumBones;
float light;
struct VS_OUTPUT {
	float4  Pos     : POSITION;
};
VS_OUTPUT VertSkinning(uniform int iNumBones)
{
	VS_OUTPUT   o;
	o.Pos = iNumBones;
	return o;
}
VertexShader vsArray20[2] = { compile vs_2_0 VertSkinning(1),
								compile vs_2_0 VertSkinning(2) };

technique Technique1 {
	pass {
		VertexShader = (vsArray20[CurNumBones + 1]);
		LightAttenuation0[0] = light;
	}
};