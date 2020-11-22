#pragma FXC ShaderWithRootSignature_1_0 ps_5_0 main /force_rootsig_ver rootsig_1_0 /DRS1_0

#define RS1 "RootFlags( ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT | DENY_VERTEX_SHADER_ROOT_ACCESS " \
                         "), " \
              "CBV(b13, space=4095), " \
              "CBV(b27, space=3567, visibility=SHADER_VISIBILITY_VERTEX), " \
              "SRV(t59, space=2748, visibility=SHADER_VISIBILITY_HULL), " \
              "UAV(u57, space=2748, visibility=SHADER_VISIBILITY_DOMAIN), " \
              "DescriptorTable( CBV(b0), " \
                               "UAV(u1, numDescriptors=2), " \
                               "SRV(t1, numDescriptors=unbounded)), " \
              "DescriptorTable(Sampler(s0, numDescriptors = 2)), " \
              "DescriptorTable(UAV(u1, space=50, numDescriptors = 2)), " \
              "RootConstants(num32BitConstants=7, b9, space=15, visibility=SHADER_VISIBILITY_GEOMETRY ), " \
              "RootConstants(num32BitConstants=51, b12, space=23,  visibility=SHADER_VISIBILITY_PIXEL ), " \
              "DescriptorTable( UAV(u3), " \
                               "UAV(u4), " \
                               "UAV(u5, offset=1)), " \
              "StaticSampler(s2)," \
              "StaticSampler(s3, " \
                            "addressU = TEXTURE_ADDRESS_MIRROR, " \
                            "addressV = TEXTURE_ADDRESS_CLAMP, " \
                            "addressW = TEXTURE_ADDRESS_BORDER, " \
                            "mipLODBias = -10.5, " \
                            "maxAnisotropy = 14, " \
                            "comparisonFunc = COMPARISON_GREATER_EQUAL, " \
                            "borderColor = STATIC_BORDER_COLOR_TRANSPARENT_BLACK, " \
                            "minLOD = -2, " \
                            "maxLOD = -50, " \
                            "space = 52, " \
                            "filter = FILTER_MIN_MAG_MIP_LINEAR )"


[RootSignature(RS1)]
float4 main(float4 coord : COORD) : SV_Target
{
	float4 result = coord;
	return result;
}