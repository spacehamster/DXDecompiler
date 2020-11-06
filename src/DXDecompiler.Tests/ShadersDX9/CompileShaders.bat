@ECHO OFF

CLS

ECHO Compiling shaders...
ECHO.

CALL CompileShader2.bat Internal Effect_Test.fx Effect_Test_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_EmbedAssembly.fx Effect_EmbedAssembly_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_SamplerArray.fx Effect_SamplerArray_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_ShaderArray.fx Effect_ShaderArray_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_StateBlock.fx Effect_StateBlock_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_Expression.fx Effect_Expression_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_Expression2.fx Effect_Expression2_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_Structs.fx Effect_Structs_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat Internal Effect_ArrayIndex.fx Effect_ArrayIndex_FX fx_2_0 || GOTO :error

CALL CompileShader.bat HLSLCrossCompiler/vs2 boolconst.hlsl boolconst vs_2_0 main /Od || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/vs2 intrep.hlsl intrep vs_2_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/vs2 loop.hlsl loop vs_2_0 main /Od || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/vs2 mov.hlsl mov vs_2_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/vs2 pointsize.hlsl pointsize vs_2_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/vs2 sign.hlsl sign vs_2_0 main || GOTO :error

CALL CompileShader.bat HLSLCrossCompiler/ps2 tex2d.fx tex2d ps_2_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/ps2 uniformFuncParam.fx uniformFuncParam ps_2_0 main || GOTO :error

CALL CompileShader.bat HLSLCrossCompiler/ps3 constTexCoord.hlsl constTexCoord ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/ps3 derivative.hlsl derivative ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/ps3 discard.hlsl discard ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/ps3 fxaa.hlsl fxaa ps_3_0 main "/DFXAA_HLSL_3=1" || GOTO :error
CALL CompileShader.bat HLSLCrossCompiler/ps3 ParallaxOcclusionMapping.fx ParallaxOcclusionMapping ps_3_0 RenderSceneParallaxMappingPS || GOTO :error

CALL CompileShader.bat HLSLDecompiler ps_absolute_multiply.fx ps_absolute_multiply ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_conditional.fx ps_conditional ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_constant.fx ps_constant ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_constant_struct.fx ps_constant_struct ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_dot_product2_add.fx ps_dot_product2_add ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_float4_constant.fx ps_float4_constant ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_float4_construct.fx ps_float4_construct ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_float4_construct2.fx ps_float4_construct2 ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_multiply_negate.fx ps_multiply_negate ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_multiply_subtract.fx ps_multiply_subtract ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_negate_absolute.fx ps_negate_absolute ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_tex2d.fx ps_tex2d ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_tex2d_swizzle.fx ps_tex2d_swizzle ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_tex2d_two_samplers.fx ps_tex2d_two_samplers ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_texcoord.fx ps_texcoord ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_texcoord_modifier.fx ps_texcoord_modifier ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler ps_texcoord_swizzle.fx ps_texcoord_swizzle ps_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_constant.fx vs_constant vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_constant_struct.fx vs_constant_struct vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_dot_product.fx vs_dot_product vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_length.fx vs_length vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_matrix22_vector2_multiply.fx vs_matrix22_vector2_multiply vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_matrix33_vector3_multiply.fx vs_matrix33_vector3_multiply vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_matrix44_vector4_multiply.fx vs_matrix44_vector4_multiply vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_normalize.fx vs_normalize vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_vector2_matrix22_multiply.fx vs_vector2_matrix22_multiply vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_vector3_matrix33_multiply.fx vs_vector3_matrix33_multiply vs_3_0 main || GOTO :error
CALL CompileShader.bat HLSLDecompiler vs_vector4_matrix44_multiply.fx vs_vector4_matrix44_multiply vs_3_0 main || GOTO :error

CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_VS_2 vs_2_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_2 ps_2_0 RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_VS_2a vs_2_a RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_VS_2sw vs_2_sw RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_VS_3 vs_3_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_VS_3sw vs_3_sw RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_2a ps_2_a RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_2b ps_2_b RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_2sw ps_2_sw RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_3 ps_3_0 RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_PS_3sw ps_3_sw RenderScenePS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/BasicHLSL BasicHLSL.fx BasicHLSL_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_ColorVS vs_2_0 ColorVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_ColorPS ps_2_0 ColorPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TextureVS vs_2_0 TextureVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TexturePointPS ps_2_0 TexturePointPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TexturePointCentroidPS ps_2_0 TexturePointCentroidPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TextureLinearPS ps_2_0 TextureLinearPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TextureLinearCentroidPS ps_2_0 TextureLinearCentroidPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TextureAnisotropicPS ps_2_0 TextureAnisotropicPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_TextureAnisotropicCentroidPS ps_2_0 TextureAnisotropicCentroidPS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/AntiAlias AntiAlias.fx AntiAlias_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/CompiledEffect CompiledEffect.fx CompiledEffect_VS vs_2_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/CompiledEffect CompiledEffect.fx CompiledEffect_PS ps_2_0 RenderScenePS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/CompiledEffect CompiledEffect.fx CompiledEffect_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/ConfigSystem main.fx main_VS vs_3_0 VS20 || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ConfigSystem main.fx main_PS ps_3_0 PS20 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/ConfigSystem main.fx main_FX fx_2_0 /Gec || GOTO :error

CALL CompileShader.bat SDK/Direct3D/CustomUI CustomUI.fx CustomUI_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/CustomUI CustomUI.fx CustomUI_PS ps_2_0 PixScene || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/CustomUI CustomUI.fx CustomUI_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_WorldVertexShaderVS vs_2_0 WorldVertexShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_WorldVertexShaderPS ps_2_0 WorldPixelShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_UnmodifiedPS ps_2_0 RenderUnmodified || GOTO :error
CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_BlurFactorPS ps_2_0 RenderBlurFactor || GOTO :error
CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_DepthOfFieldWithSixTexcoordsPS ps_2_0 DepthOfFieldWithSixTexcoords || GOTO :error
CALL CompileShader.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_DepthOfFieldManySamplesPS ps_3_0 DepthOfFieldManySamples || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/DepthOfField DepthOfField.fx DepthOfField_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/EffectParam EffectParam.fx EffectParam_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/EffectParam EffectParam.fx EffectParam_VS ps_2_0 PixScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/EffectParam reflect.fx reflect_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/EffectParam reflect.fx reflect_PS ps_2_0 PixScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/EffectParam specular.fx specular_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/EffectParam specular.fx specular_PS ps_2_0 PixScene || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/EffectParam EffectParam.fx EffectParam_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/EffectParam reflect.fx reflect_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/EffectParam specular.fx specular_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_SceneVS vs_2_0 HDRVertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_ScenePS ps_2_0 HDRPixScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_SceneFirstHalfPS ps_2_0 HDRPixScene_FirstHalf || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_SceneSecondHalfPS ps_2_0 HDRPixScene_SecondHalf || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_LightVS vs_2_0 HDRVertLight || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_LightPS ps_2_0 HDRPixLight || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_LightFirstHalfPS ps_2_0 HDRPixLight_FirstHalf || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_LightSecondHalfPS ps_2_0 HDRPixLight_SecondHalf || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_EnvMapVS vs_2_0 HDRVertEnvMap || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_EnvMapPS ps_2_0 HDRPixEnvMap || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_EnvMap2TexPS ps_2_0 HDRPixEnvMap2Tex || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/HDRCubeMap HDRCubeMap.fx HDRCubeMap_FX fx_2_0 || GOTO :error


CALL CompileShader.bat SDK/Direct3D/HDRFormats HDRFormats.fx HDRFormats_SceneVS vs_2_0 SceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRFormats HDRFormats.fx HDRFormats_ScenePS ps_2_0 ScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRFormats HDRFormats.fx HDRFormats_BloomPS ps_2_0 BloomPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRFormats HDRFormats.fx HDRFormats_FinalPassPS ps_2_0 FinalPass || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/HDRFormats HDRFormats.fx HDRFormats_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_TransformSceneVS vs_2_0 TransformScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_PointLightPS ps_2_0 PointLight || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_BloomPS ps_2_0 BloomPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_StarPS ps_2_0 StarPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_SampleLumInitialPS ps_2_0 SampleLumInitial || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_DownScale4x4PS ps_2_0 DownScale4x4PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_GaussBlur5x5PS ps_2_0 GaussBlur5x5PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_MergeTextures_8PS ps_2_0 MergeTextures_8PS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/HDRLighting HDRLighting.fx HDRLighting_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/HDRPipeline FinalPass.psh FinalPass_PS ps_3_0 main || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline HDRSource.psh HDRSource_PS ps_3_0 main || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline HDRSource.vsh HDRSource_VS vs_3_0 main || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline Luminance.psh Luminance_GreyScaleDownSamplePS ps_3_0 GreyScaleDownSample || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline Luminance.psh Luminance_DownSamplePS ps_3_0 DownSample || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline OcclusionMesh.vsh OcclusionMesh_VS vs_3_0 main || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline PostProcessing.psh PostProcessing_PS ps_3_0 BrightPass || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline PostProcessing.psh PostProcessing_PS ps_3_0 DownSample || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline PostProcessing.psh PostProcessing_PS ps_3_0 HorizontalBlur || GOTO :error
CALL CompileShader.bat SDK/Direct3D/HDRPipeline PostProcessing.psh PostProcessing_PS ps_3_0 VerticalBlur || GOTO :error

CALL CompileShader.bat SDK/Direct3D/Instancing Instancing.fx Instancing_HWInstancingVS vs_2_0 VS_HWInstancing || GOTO :error
CALL CompileShader.bat SDK/Direct3D/Instancing Instancing.fx Instancing_ShaderInstancingVS vs_2_0 VS_ShaderInstancing || GOTO :error
CALL CompileShader.bat SDK/Direct3D/Instancing Instancing.fx Instancing_ConstantsVS vs_2_0 VS_ConstantsInstancing || GOTO :error
CALL CompileShader.bat SDK/Direct3D/Instancing Instancing.fx Instancing_PS ps_2_0 PS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/Instancing Instancing.fx Instancing_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/IrradianceVolume LDPRT.fx LDPRT_CubicVS vs_3_0 LDPRTCubicVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume LDPRT.fx LDPRT_CubicPS ps_3_0 LDPRTCubicPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume LDPRT.fx LDPRT_VS vs_3_0 LDPRTVertexShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume LDPRT.fx LDPRT_PS ps_3_0 LDPRTPixelShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume NdotL.fx NdotL_VS vs_2_0 SimpleLightingVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume NdotL.fx NdotL_PS ps_2_0 StandardPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume PRT.fx PRT_VS vs_2_0 PRTDiffuseVS "/DNUM_CLUSTERS=10" "/DNUM_PCA=10" || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume PRT.fx PRT_PS ps_2_0 StandardPS "/DNUM_CLUSTERS=10" "/DNUM_PCA=10" || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume Scene.fx Scene_VS vs_2_0 SceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume Scene.fx Scene_PS ps_2_0 ScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume SHFuncView.fx SHFuncView_VS vs_2_0 RenderSphere || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume SHIrradianceEnvMap.fx SHIrradianceEnvMap_VS vs_2_0 SHIrradianceEnvironmentMapVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume SHIrradianceEnvMap.fx SHIrradianceEnvMap_PS ps_2_0 StandardPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume Wireframe.fx Wireframe_VS vs_2_0 WireframeVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/IrradianceVolume Wireframe.fx Wireframe_PS ps_2_0 WireframePS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume LDPRT.fx LDPRT_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume NdotL.fx NdotL_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume PRT.fx PRT_FX fx_2_0 "/DNUM_CLUSTERS=10" "/DNUM_PCA=10" || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume Scene.fx Scene_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume SHFuncView.fx SHFuncView_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume SHIrradianceEnvMap.fx SHIrradianceEnvMap_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/IrradianceVolume Wireframe.fx Wireframe_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/LocalDeformablePRT LocalDeformablePRT.fx LocalDeformablePRT_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/LocalDeformablePRT LocalDeformablePRT.fx LocalDeformablePRT_PS ps_2_0 PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/LocalDeformablePRT skybox.fx skybox_VS vs_2_0 SkyboxVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/LocalDeformablePRT skybox.fx skybox_PS ps_2_0 SkyboxPS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/LocalDeformablePRT LocalDeformablePRT.fx LocalDeformablePRT_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/LocalDeformablePRT skybox.fx skybox_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/MeshFromObj MeshFromObj.fx MeshFromObj_VS vs_2_0 Projection || GOTO :error
CALL CompileShader.bat SDK/Direct3D/MeshFromObj MeshFromObj.fx MeshFromObj_PS ps_2_0 Lighting || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/MeshFromObj MeshFromObj.fx MeshFromObj_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/MultiAnimation MultiAnimation.fx MultiAnimation_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/MultiAnimation MultiAnimation.fx MultiAnimation_SkinningVS vs_2_0 VertSkinning || GOTO :error
CALL CompileShader.bat SDK/Direct3D/MultiAnimation MultiAnimation.fx PixScene_PS ps_2_0 PixScene || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/MultiAnimation MultiAnimation.fx MultiAnimation_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/OptimizedMesh OptimizedMesh.fx OptimizedMesh_VS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/OptimizedMesh OptimizedMesh.fx OptimizedMesh_PS ps_2_0 PixScene || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/OptimizedMesh OptimizedMesh.fx OptimizedMesh_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/ParallaxOcclusionMapping ParallaxOcclusionMapping.fx ParallaxOcclusionMapping_VS vs_3_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ParallaxOcclusionMapping ParallaxOcclusionMapping.fx ParallaxOcclusionMapping_PS ps_3_0 RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ParallaxOcclusionMapping ParallaxOcclusionMapping.fx ParallaxOcclusionMapping_BumpPS ps_2_0 RenderSceneBumpMapPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ParallaxOcclusionMapping ParallaxOcclusionMapping.fx ParallaxOcclusionMapping_ParallaxPS ps_2_0 RenderSceneParallaxMappingPS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/ParallaxOcclusionMapping ParallaxOcclusionMapping.fx ParallaxOcclusionMapping_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/Pick Pick.fx Pick_VS vs_2_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/Pick Pick.fx Pick_PS ps_2_0 RenderScenePS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/Pick Pick.fx Pick_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/PixelMotionBlur PixelMotionBlur.fx PixelMotionBlur_WorldVS vs_2_0 WorldVertexShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PixelMotionBlur PixelMotionBlur.fx PixelMotionBlur_WorldPS ps_2_0 WorldPixelShader || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PixelMotionBlur PixelMotionBlur.fx PixelMotionBlur_WorldVelocityPS ps_2_0 WorldPixelShaderVelocity || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PixelMotionBlur PixelMotionBlur.fx PixelMotionBlur_MotionBlurPS ps_2_0 PostProcessMotionBlurPS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PixelMotionBlur PixelMotionBlur.fx PixelMotionBlur_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorBloomH.fx PP_ColorBloomH ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorBloomV.fx PP_ColorBloomV ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorBrightPass.fx PP_ColorBrightPass ps_2_0 BrightPassFilter || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_colorcombine.fx PP_colorcombine ps_2_0 Combine || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorCombine4.fx PP_ColorCombine4 ps_2_0 Combine || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorDownFilter4.fx PP_ColorDownFilter4 ps_2_0 DownFilter || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorEdgeDetect.fx PP_ColorEdgeDetect ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorGBlurH.fx PP_ColorGBlurH ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorGBlurV.fx PP_ColorGBlurV ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorInverse.fx PP_ColorInverse ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorMonochrome.fx PP_ColorMonochrome ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorToneMap.fx PP_ColorToneMap ps_2_0 ToneMapFilter || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_ColorUpFilter4.fx PP_ColorUpFilter4 ps_2_0 UpFilterPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_DofCombine.fx PP_DofCombine ps_2_0 DofCombine || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_NormalEdgeDetect.fx PP_NormalEdgeDetect ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_NormalMap.fx PP_NormalMap ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/PostProcess PP_PositionMap.fx PP_PositionMap ps_2_0 PostProcessPS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorBloomH.fx PP_ColorBloomH_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorBloomV.fx PP_ColorBloomV_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorBrightPass.fx PP_ColorBrightPass_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_colorcombine.fx PP_colorcombine_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorCombine4.fx PP_ColorCombine4_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorDownFilter4.fx PP_ColorDownFilter4_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorEdgeDetect.fx PP_ColorEdgeDetect_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorGBlurH.fx PP_ColorGBlurH_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorGBlurV.fx PP_ColorGBlurV_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorInverse.fx PP_ColorInverse_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorMonochrome.fx PP_ColorMonochrome_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorToneMap.fx PP_ColorToneMap_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_ColorUpFilter4.fx PP_ColorUpFilter4_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_DofCombine.fx PP_DofCombine_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_NormalEdgeDetect.fx PP_NormalEdgeDetect_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_NormalMap.fx PP_NormalMap_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/PostProcess PP_PositionMap.fx PP_PositionMap_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_SceneVS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_ScenePS ps_2_0 PixScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_LightVS vs_2_0 VertLight || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_LightPS ps_2_0 PixLight || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_ShadowVS vs_2_0 VertShadow || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_ShadowPS ps_2_0 PixShadow || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/ShadowMap ShadowMap.fx ShadowMap_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_SceneAmbientVS vs_2_0 VertSceneAmbient || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_SceneAmbientPS ps_2_0 PixSceneAmbient || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_ShadowVolumeVS vs_2_0 VertShadowVolume || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_ShadowVolumePS ps_2_0 PixShadowVolume || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_SceneVS vs_2_0 VertScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_ScenePS ps_2_0 PixScene || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_ShowDirtyStencilPS ps_2_0 ShowDirtyStencil || GOTO :error
CALL CompileShader.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_PixComplexityPS ps_2_0 PixComplexity || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/ShadowVolume ShadowVolume.fx ShadowVolume_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_VS_2 vs_2_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_2 ps_2_0 RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_VS_2a vs_2_a RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_VS_2sw vs_2_sw RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_VS_3 vs_3_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_VS_3sw vs_3_sw RenderSceneVS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_2a ps_2_a RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_2b ps_2_b RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_2sw ps_2_sw RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_3 ps_3_0 RenderScenePS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_PS_3sw ps_3_sw RenderScenePS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/SimpleSample SimpleSample.fx SimpleSample_FX fx_2_0 || GOTO :error

CALL CompileShader.bat SDK/Direct3D/SkinnedMesh SkinnedMesh.fx SkinnedMesh_VS vs_2_0 VShade || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/SkinnedMesh SkinnedMesh.fx SkinnedMesh_FX fx_2_0  || GOTO :error

CALL CompileShader.bat SDK/Direct3D/StateManager AlphaTest.fx AlphaTest_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager AlphaTest.fx AlphaTest_PS ps_2_0 PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager Reflective01.fx Reflective01_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager Reflective01.fx Reflective01_PS ps_2_0 PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager skybox01.fx skybox01_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager skybox01.fx skybox01_PS ps_2_0 PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager snow.fx snow_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager snow.fx snow_PS ps_2_0 PS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager textbump.fx textbump_VS vs_2_0 VS || GOTO :error
CALL CompileShader.bat SDK/Direct3D/StateManager textbump.fx textbump_PS ps_2_0 PS || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/StateManager AlphaTest.fx AlphaTest_VS fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/StateManager Reflective01.fx Reflective01_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/StateManager skybox01.fx skybox01_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/StateManager snow.fx snow_FX fx_2_0 || GOTO :error
CALL CompileShader2.bat SDK/Direct3D/StateManager textbump.fx textbump_FX fx_2_0 || GOTO :error

GOTO :EOF

:error
ECHO Failed with error #%errorlevel%.
EXIT /b %errorlevel%