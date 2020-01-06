@ECHO OFF

CLS

ECHO Compiling shaders...
ECHO.
rem CompileShader.bat
rem "C:\Program Files (x86)\Windows Kits\8.1\bin\x86\fxc" %1/%2 /Fo %1/%3.o /Fc %1/%3.asm /T %4 /E %5 /nologo %6 %7
rem args are: folder, sourceFile, bytecode output, assembly output, profile, entrypoint, extra args
rem args are: folder, input, output, profile, entrypoint, extra args



CALL CompileShader.bat Internal Textures.hlsl Textures_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat Internal Textures.hlsl Textures_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat Internal HLSLSample.hlsl HLSLSample_VS vs_5_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat Internal HLSLSample.hlsl HLSLSample_PS ps_5_0 RenderScenePS || GOTO :error
CALL CompileShader.bat Internal BasicHLSL.hlsl BasicHLSL_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Internal BasicHLSL.hlsl BasicHLSL_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Internal Interfaces.hlsl Interfaces ps_5_0 main || GOTO :error
CALL CompileShader.bat Internal DynamicIndexing.hlsl DynamicIndexing ps_5_0 main || GOTO :error
CALL CompileShader.bat Internal ResourceDefinitions.hlsl ResourceDefinitions ps_5_0 main || GOTO :error

Set UNITY_INCLUDES="%cd%\Unity\CGIncludes"
CALL CompileShader.bat Unity fog_test.hlsl fog_test_Exp2_VS_25 vs_5_0 vert "/I%UNITY_INCLUDES%" /Gec "/DFOG_EXP2=1" "/DSHADER_TARGET=25" "/DUNITY_REVERSED_Z=1" || GOTO :error
CALL CompileShader.bat Unity fog_test.hlsl fog_test_Exp2_PS_25 ps_5_0 frag "/I%UNITY_INCLUDES%" /Gec "/DFOG_EXP2=1" "/DSHADER_TARGET=25" "/DUNITY_REVERSED_Z=1" || GOTO :error
CALL CompileShader.bat Unity fog_test.hlsl fog_test_Exp2_VS_50 vs_5_0 vert "/I%UNITY_INCLUDES%" /Gec "/DFOG_EXP2=1" "/DSHADER_TARGET=50" "/DUNITY_REVERSED_Z=1" || GOTO :error
CALL CompileShader.bat Unity fog_test.hlsl fog_test_Exp2_PS_50 ps_5_0 frag "/I%UNITY_INCLUDES%" /Gec "/DFOG_EXP2=1" "/DSHADER_TARGET=50" "/DUNITY_REVERSED_Z=1" || GOTO :error

rem CALL CompileShader.bat Unity fog_test.hlsl fog_test_Exp2_PS ps_5_0 frag "/DFOG_EXP2=1" "/I CGIncludes" || GOTO :error

CALL CompileShader.bat FxDis test.hlsl test_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat FxDis test.hlsl test_PS ps_5_0 PS || GOTO :error

CALL CompileShader.bat HlslCrossCompiler/ds5 basic.hlsl basic ds_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/hs5 basic.hlsl basic hs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps4 fxaa.hlsl fxaa ps_4_0 main "/DFXAA_HLSL_4=1" || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps4 primID.hlsl primID ps_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 conservative_depth.hlsl conservative_depth_ge ps_5_0 DepthGreaterThan || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 conservative_depth.hlsl conservative_depth_le ps_5_0 DepthLessThan || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 interface_arrays.hlsl interface_arrays ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 interfaces.hlsl interfaces ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/ps5 sample.hlsl sample ps_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 mov.hlsl mov vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 multiple_const_buffers.hlsl multiple_const_buffers vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs4 switch.hlsl switch vs_4_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 any.hlsl any vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 const_temp.hlsl const_temp vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 mad_imm.hlsl mad_imm vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 mov.hlsl mov vs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/vs5 sincos.hlsl sincos vs_5_0 main || GOTO :error

CALL CompileShader.bat HlslCrossCompiler/apps Extrude.fx Extrude_GS gs_5_0 GS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps Extrude.fx Extrude_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps Extrude.fx Extrude_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps Integer.fx Integer_VS vs_5_0 main|| GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps LambertLit.fx LambertLit_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps LambertLit.fx LambertLit_PSSolid ps_5_0 PSSolid || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps LambertLit.fx LambertLit_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps Subroutines.hlsl Subroutines_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps Subroutines.hlsl Subroutines_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps tessellation.hlsl tesselation_DS ds_5_0 DS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps tessellation.hlsl tesselation_HS hs_5_0 HS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps tessellation.hlsl tesselation_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps tessellation.hlsl tesselation_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic Clipping.hlsl Clipping_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic compute.hlsl compute cs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic id.hlsl id_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic id.hlsl id_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic template.hlsl template_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic template.hlsl template_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic template.hlsl template_VS_PostFX vs_5_0 VS_PostFX || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic template.hlsl template_PS_PostFx ps_5_0 PS_PostFX || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic wavy.hlsl wavy_VS vs_5_0 VS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic wavy.hlsl wavy_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic/postProcessing invert.hlsl invert_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic/postProcessing monochrome.hlsl monochrome_PS ps_5_0 PS || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/apps/generic/postProcessing sobel.hlsl sobel_PS ps_5_0 PS || GOTO :error

CALL CompileShader.bat HlslCrossCompiler/cs5 Issue11.hlsl Issue11 cs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/cs5 Issue11Struct.hlsl Issue11Struct cs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/cs5 Issue34.hlsl Issue34 cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/cs5 ThreadGroupSharedMem.hlsl ThreadGroupSharedMem cs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/gs4 PipesGS.fx PipesGS gs_4_0 GSCrawlPipesMain || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/gs5 instance.fx instance gs_5_0 main || GOTO :error
CALL CompileShader.bat HlslCrossCompiler/gs5 stream.fx stream gs_5_0 main || GOTO :error

CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx CubeMapGS_VS vs_4_0 VS_CubeMap || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx CubeMapGS_GS gs_4_0 GS_CubeMap || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/CubeMapGS CubeMapGS.fx CubeMapGS_PS ps_4_0 PS_EnvMappedScene || GOTO :error

CALL CompileShader.bat Sdk/Direct3D10/TransparencyAA10.1 Sprite.fx Sprite_VS vs_4_0 VsSprite || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/TransparencyAA10.1 Sprite.fx Sprite_PS ps_4_0 PsSprite || GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/TransparencyAA10.1 TransparencyAA10_1.fx TransparencyAA10_1_VS vs_4_1 VsRenderScene || GOTO :error|| GOTO :error
CALL CompileShader.bat Sdk/Direct3D10/TransparencyAA10.1 TransparencyAA10_1.fx TransparencyAA10_1_PS ps_4_1 PsTransparencyAA "/DDX10_1_ENABLED=1" "/DMSAA_SAMPLES=4" || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_EdgeFactorCS.hlsl TessellatorCS40_EdgeFactorCS cs_4_0 CSEdgeFactor || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_NumVerticesIndicesCS.hlsl TessellatorCS40_NumVerticesIndicesCS cs_4_0 CSNumVerticesIndices || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_ScatterIDCS.hlsl TessellatorCS40_ScatterIDCS_Vertex cs_4_0 CSScatterVertexTriIDIndexID || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_ScatterIDCS.hlsl TessellatorCS40_ScatterIDCS_Index cs_4_0 CSScatterIndexTriIDIndexID || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_TessellateIndicesCS.hlsl TessellatorCS40_TessellateIndicesCS cs_4_0 CSTessellationIndices || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/AdaptiveTessellationCS40 TessellatorCS40_TessellateVerticesCS.hlsl TessellatorCS40_TessellateVerticesCS cs_4_0 CSTessellationVertices || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Raw cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Raw_Double cs_5_0 CSMain "/DTEST_DOUBLE=1" || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Structured cs_5_0 CSMain "/DUSE_STRUCTURED_BUFFERS=1" || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicCompute11 BasicCompute11.hlsl BasicCompute11_Structured_Double cs_5_0 CSMain "/DUSE_STRUCTURED_BUFFERS=1" "/DTEST_DOUBLE=1" || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BasicHLSL11 BasicHLSL11_PS.hlsl BasicHLSL11_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BasicHLSL11 BasicHLSL11_VS.hlsl BasicHLSL11_VS vs_5_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SimpleSample11 SimpleSample.hlsl SimpleSample_PS ps_5_0 RenderScenePS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleSample11 SimpleSample.hlsl SimpleSample_VS vs_5_0 RenderSceneVS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HDecode.hlsl BC6HDecode cs_4_0 main || GOTO :error
REM CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HEncode.hlsl BC6HEncode_G10 cs_4_0 TryModeG10CS || GOTO :error
REM CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC6HEncode.hlsl BC6HEncode_LE10 cs_4_0 TryModeLE10CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Decode.hlsl BC7Decode cs_4_0 main || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_456 cs_4_0 TryMode456CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_137 cs_4_0 TryMode137CS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/BC6HBC7EncoderDecoder11 BC7Encode.hlsl BC7Encode_02 cs_4_0 TryMode02CS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/CascadedShadowMaps11 RenderCascadeScene.hlsl RenderCascadeScene_PS ps_4_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/CascadedShadowMaps11 RenderCascadeScene.hlsl RenderCascadeScene_VS vs_4_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/ComputeShaderSort11 ComputeShaderSort11.hlsl ComputeShaderSort11_BitonicSort cs_5_0 BitonicSort || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ComputeShaderSort11 ComputeShaderSort11.hlsl ComputeShaderSort11_MatrixTranspose cs_5_0 MatrixTranspose || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_PS ps_5_0 PS_RenderScene || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_VSSM vs_4_0 VS_RenderSceneSM || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/ContactHardeningShadows11 ContactHardeningShadows11.hlsl ContactHardeningShadows11_VS vs_4_0 VS_RenderScene || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DDSWithoutD3DX11 DDSWithoutD3DX.hlsl DDSWithoutD3DX_VS vs_4_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DDSWithoutD3DX11 DDSWithoutD3DX.hlsl DDSWithoutD3DX_PS ps_4_0 RenderScenePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_VS_NoTessellation vs_4_0 VS_NoTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_VS vs_4_0 VS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_HS hs_5_0 HS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_DS ds_5_0 DS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DecalTessellation11 DecalTessellation11.hlsl DecalTessellation11_PS ps_5_0 PS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_VS_NoTessellation vs_4_0 VS_NoTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_VS vs_4_0 VS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_HS hs_5_0 HS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_DS ds_5_0 DS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 DetailTessellation11.hlsl DetailTessellation11_PS ps_4_0 BumpMapPS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_VS vs_5_0 VSPassThrough || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_GS gs_5_0 GSPointSprite || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 Particle.hlsl Particle_PS ps_5_0 PSConstantColor || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 POM.hlsl POM_VS vs_5_0 RenderSceneVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DetailTessellation11 POM.hlsl POM_PS ps_4_0 RenderScenePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_PS.hlsl DynamicShaderLinkage11_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/DynamicShaderLinkage11 DynamicShaderLinkage11_VS.hlsl DynamicShaderLinkage11_VS vs_5_0 VSMain || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_BuildGridCS cs_5_0 BuildGridCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ClearGridIndicesCS cs_5_0 ClearGridIndicesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_BuildGridIndicesCS cs_5_0 BuildGridIndicesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_RearrangeParticlesCS cs_5_0 RearrangeParticlesCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Simple cs_5_0 DensityCS_Simple || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Shared cs_5_0 DensityCS_Shared || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_DensityCS_Grid cs_5_0 DensityCS_Grid || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Simple cs_5_0 ForceCS_Simple || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Shared cs_5_0 ForceCS_Shared || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_ForceCS_Grid cs_5_0 ForceCS_Grid || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidCS11.hlsl FluidCS11_IntegrateCS cs_5_0 IntegrateCS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_VS vs_5_0 ParticleVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_GS gs_5_0 ParticleGS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/FluidCS11 FluidRender.hlsl FluidRender_PS ps_5_0 ParticlePS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 BrightPassAndHorizFilterCS.hlsl BrightPassAndHorizFilterCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 DumpToTexture.hlsl DumpToTexture ps_5_0 PSDump || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FilterCS.hlsl FilterCS_Vertical cs_5_0 CSVerticalFilter || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FilterCS.hlsl FilterCS_Horizontal cs_5_0 CSHorizFilter || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_VS vs_5_0 QuadVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_PS ps_5_0 PSFinalPass || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 FinalPass.hlsl FinalPass_PS_CPUReduction ps_5_0 PSFinalPassForCPUReduction || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 PSApproach.hlsl PSApproach_PS ps_5_0 DownScale3x3_BrightPass || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 ReduceTo1DCS.hlsl ReduceTo1DCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 ReduceToSingleCS.hlsl ReduceToSingleCS cs_5_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 skybox11.hlsl Skybox11_VS vs_5_0 SkyboxVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/HDRToneMappingCS11 skybox11.hlsl Skybox11_PS ps_5_0 SkyboxPS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 NBodyGravityCS11.hlsl NBodyGravityCS11 cs_4_0 CSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_GS gs_4_0 GSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_PS ps_4_0 PSParticleDraw || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/NBodyGravityCS11 ParticleDraw.hlsl ParticleDraw_VS vs_4_0 VSParticleDraw || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_VS vs_5_0 VS_RenderScene || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_VS_WithTessellation vs_5_0 VS_RenderSceneWithTessellation || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_HS hs_5_0 HS_PNTriangles || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_DS ds_5_0 DS_PNTriangles || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/PNTriangles11 PNTriangles11.hlsl PNTriangles11_PS ps_4_0 PS_RenderScene || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_DS ds_5_0 BezierDS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_HS hs_5_0 BezierHS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_PS ps_4_0 BezierPS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SimpleBezier11 SimpleBezier11.hlsl SimpleBezier11_VS vs_4_0 BezierVS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_VS_PatchSkinning vs_5_0 PatchSkinningVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_VS_MeshSkinning vs_5_0 MeshSkinningVS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_HS hs_5_0 SubDToBezierHS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_HS_4444 hs_5_0 SubDToBezierHS4444 || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_DS ds_5_0 BezierEvalDS || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/SubD11 SubD11.hlsl SubD11_PS ps_5_0 SmoothPS || GOTO :error

CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_PS_BlurX ps_5_0 PSBlurX || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 2DQuadShaders.hlsl 2DQuadShaders_PS_BlurY ps_5_0 PSBlurY || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceScene.hlsl RenderVarianceScene_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceScene.hlsl RenderVarianceScene_PS ps_5_0 PSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceShadow.hlsl RenderVarianceShadow_VS vs_5_0 VSMain || GOTO :error
CALL CompileShader.bat Sdk/Direct3D11/VarianceShadows11 RenderVarianceShadow.hlsl RenderVarianceShadow_PS ps_5_0 PSMain || GOTO :error

CALL CompileShader.bat 3Dmigoto compute.hlsl compute cs_5_0 main "/Od" || GOTO :error
CALL CompileShader.bat 3Dmigoto debug.hlsl debug cs_5_0 main "/Od"  || GOTO :error
CALL CompileShader.bat 3Dmigoto double_precision.hlsl double_precision ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto geometry.hlsl geometry gs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto hull.hlsl hull hs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto min_precision.hlsl min_precision ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto resource_types.hlsl resource_types_5 ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto resource_types.hlsl resource_types_4 ps_4_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto resource_types5.hlsl resource_types5 ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto samplepos.hlsl samplepos ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_cs.hlsl signatures_cs cs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_ds.hlsl signatures_ds ds_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_gs.hlsl signatures_gs gs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_gs5.hlsl signatures_gs5 gs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_hs.hlsl signatures_hs hs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_ps.hlsl signatures_ps_0 ps_5_0 main "/DTEST_DEPTH=0" || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_ps.hlsl signatures_ps_1 ps_5_0 main "/DTEST_DEPTH=1"|| GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_ps.hlsl signatures_ps_2 ps_5_0 main "/DTEST_DEPTH=2" || GOTO :error
CALL CompileShader.bat 3Dmigoto signatures_vs.hlsl signatures_vs vs_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto structured_buffers.hlsl structured_buffers_5 ps_5_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto structured_buffers.hlsl structured_buffers_4 ps_4_0 main || GOTO :error
CALL CompileShader.bat 3Dmigoto structured_buffers.hlsl structured_buffers_extra_5 ps_5_0 main "/DUSE_INNER_STRUCT=1" "/DUSE_DUP_NAME=1" "/DUSE_PRIMITIVE_TYPES=1" "/DUSE_DOUBLES=1" "/DUSE_RW_STRUCTURED_BUFFER=1" "/DUSE_DYNAMICALLY_INDEXED_ARRAYS=1" || GOTO :error
CALL CompileShader.bat 3Dmigoto structured_buffers.hlsl structured_buffers_extra_4 ps_4_0 main "/DUSE_INNER_STRUCT=1" "/DUSE_DUP_NAME=1" "/DUSE_PRIMITIVE_TYPES=1" "/DUSE_DOUBLES=1" "/DUSE_RW_STRUCTURED_BUFFER=1" "/DUSE_DYNAMICALLY_INDEXED_ARRAYS=1" || GOTO :error

CALL CompileShader.bat 3Dmigoto sv_gsinstanceid.hlsl sv_gsinstanceid gs_5_0 main || GOTO :error

GOTO :EOF

:error
ECHO Failed with error #%errorlevel%.
EXIT /b %errorlevel%