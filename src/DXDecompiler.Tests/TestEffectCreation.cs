using NUnit.Framework;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Tests
{
	[TestFixture]
	class TestEffectCreation
	{
		/// <summary>
		/// Effects with shared buffers and variables cause SharpDX to throw unspecified errors on effect creation
		/// SharpDX.SharpDXException : HRESULT: [0x80004005], Module: [General], ApiCode: [E_FAIL/Unspecified error], Message: Unspecified error
		/// TODO: Figure out a solution
		/// </summary>
		[Test]
		public void TestEffectPools()
		{
			var effectCode = @"// Global variables
float4 g_MaterialAmbientColor;      // Material's ambient color
float    g_fTime;                   // App's time in seconds
float4x4 g_mWorld;                  // World matrix for object
float4x4 g_mWorldViewProjection;    // World * View * Projection matrix
float4    defaultValue = float4(1, 2, 3, 4);
shared cbuffer TestShared {
	float4 sharedValue;
}
Texture2D g_MeshTexture;            // Color texture for mesh
SamplerState MeshTextureSampler
<int blabla = 27; >
{
				AddressU = Wrap;
				AddressV = Clamp;
				AddressW = MIrror;
				BorderColor = float4(2.0f, 3.0f, 4.0f, 5.0f);
				Filter = MIN_MAG_MIP_LINEAR;
				MaxAnisotropy = 5;
				MaxLOD = 6;
				MinLOD = 7;
				MipLODBias = 8;
			};
			shared Texture2D sharedTexture;            // Color texture for mesh
struct VS_OUTPUT
		{
			float4 Position   : SV_POSITION; // vertex position 
};

		VS_OUTPUT RenderSceneVS()
		{
			VS_OUTPUT Output = (VS_OUTPUT)0;
			return Output;
		}

		struct PS_OUTPUT
		{
			float4 RGBColor : SV_Target;  // Pixel color
};

		PS_OUTPUT RenderScenePS(VS_OUTPUT In,
			uniform bool bTexture)
		{
			PS_OUTPUT Output = (PS_OUTPUT)0;

			if (bTexture)
				Output.RGBColor = 5;

			return Output;
		}


		technique10 RenderSceneWithTexture1Light10_1
		{
			pass P0
			{
				SetVertexShader(CompileShader(vs_4_0, RenderSceneVS()));
				SetPixelShader(CompileShader(ps_4_0, RenderScenePS(true)));
			}
		}

		technique10 RenderSceneWithTexture1Light10_2
		{
			pass P0
			{
				SetVertexShader(CompileShader(vs_4_0, RenderSceneVS()));
				SetPixelShader(CompileShader(ps_4_0, RenderScenePS(false)));
			}
		}
";
			var result = ShaderBytecode.Compile(effectCode, "fx_4_0",
							ShaderFlags.None,
							EffectFlags.ChildEffect);
			var effectBytecode = result.Bytecode;
			var device = new Device(DriverType.Warp, DeviceCreationFlags.Debug);
			Effect effectReflection = null;
			var effectPool = new EffectPool(device, effectBytecode, EffectFlags.None);
		}
	}
}
