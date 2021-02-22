using DXDecompiler.DX9Shader;
using DXDecompiler.DX9Shader.Bytecode.Ctab;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDecompiler.Tests
{
	[TestFixture]
	class TestDX9Effects
	{
		// TODO: double is not supported yet.
		[Test]
		public void TestEffect9Constants()
		{
			var bytecode = ShaderBytecode.Compile(@"
				struct T {
					float f;
					int i;
					int2 j;
					float4 v;
				};
				T t : register(vs, c5) : register(ps, c5) = {
					1.0f,
					2,
					int2(2, 3),
					float4(4, 5, 6, 7)
				};

				struct M {
					column_major float2x3 f;
					float g;
				} m : register(vs, c80) : register(ps, c80);

				struct U {
					row_major float2x3 f;
					T t[2];
					float g;
				};
				U u[3] : register(vs, c10) : register(ps, c10);

				float4 VS(float4 p : POSITION) : POSITION {
					// some random code which uses those constants
					// so they won't be optimized away.
					return p * 2 * t.i * u[2].g * m.g * u[0].t[1].v;
				}

				technique Tq {
					pass p0 {
						VertexShader = compile vs_2_0 VS();
					}
				}
			", "fx_2_0").Bytecode;
			var shaderModel = ShaderReader.ReadShader(bytecode);
			var constants = from blob in shaderModel.EffectChunk.StateBlobLookup.Values
							where blob.BlobType == DX9Shader.FX9.StateBlobType.Shader
							from constdecl in blob.Shader.ConstantTable.ConstantDeclarations
							select constdecl;
			// constant "t"
			var t = constants.First(c => c.Name == "t");
			Assert.AreEqual(5, t.RegisterIndex);

			var t_j = t.GetRegisterTypeByOffset(2);
			Assert.AreEqual(7, t_j.RegisterIndex);
			Assert.AreEqual(ParameterClass.Vector, t_j.Type.ParameterClass);
			Assert.AreEqual(ParameterType.Int, t_j.Type.ParameterType);
			var int2Name = t.GetMemberNameByOffset(2);
			Assert.AreEqual("t.j", int2Name);

			// constant "m"
			var m = constants.First(c => c.Name == "m");
			Assert.AreEqual(80, m.RegisterIndex);

			var m_f = m.GetRegisterTypeByOffset(2);
			Assert.AreEqual(80, m_f.RegisterIndex);
			Assert.AreEqual(ParameterClass.MatrixColumns, m_f.Type.ParameterClass);
			Assert.AreEqual("m.f", m.GetMemberNameByOffset(2));

			var m_g = m.GetRegisterTypeByOffset(3);
			Assert.AreEqual(83, m_g.RegisterIndex);
			Assert.AreEqual("m.g", m.GetMemberNameByOffset(3));

			// constant "u"
			var u = constants.First(c => c.Name == "u");
			Assert.AreEqual(10, u.RegisterIndex);

			var u_2_g = u.GetRegisterTypeByOffset(32);
			Assert.AreEqual(42, u_2_g.RegisterIndex);
			Assert.AreEqual(ParameterClass.Scalar, u_2_g.Type.ParameterClass);
			Assert.AreEqual("u[2].g", u.GetMemberNameByOffset(32));

			var u_2_t_1_v = u.GetRegisterTypeByOffset(31);
			Assert.AreEqual(41, u_2_t_1_v.RegisterIndex);
			Assert.AreEqual(ParameterClass.Vector, u_2_t_1_v.Type.ParameterClass);
			Assert.AreEqual(ParameterType.Float, u_2_t_1_v.Type.ParameterType);
			Assert.AreEqual(1, u_2_t_1_v.Type.Rows);
			Assert.AreEqual(4, u_2_t_1_v.Type.Columns);
			Assert.AreEqual("u[2].t[1].v", u.GetMemberNameByOffset(31));
		}

		// TODO: double is not supported yet.
		[Test]
		public void TestSimpleVertexShaderWithConstants()
		{
			var bytecode = ShaderBytecode.Compile(@"
				struct T {
					float f;
					float4 v;
				};

				struct U {
					row_major float2x3 f;
					T t[2];
					float g;
				};
				U u[3];

				float4 HakureiReimuAliceMargatroid;
				row_major float2x2 m;
				column_major float2x2 n;

				sampler Gensokyo[2];

				float4 VS(float4 p : POSITION) : POSITION {
					return p * HakureiReimuAliceMargatroid.w  * u[2].t[1].v * m[0].y * n[0].y;
				}

				float4 PS(float4 t : TEXCOORD) : COLOR {
					return tex2D(Gensokyo[0], t.xy) + tex2D(Gensokyo[1], t.zw);
				}

				technique Tq {
					pass p0 {
						VertexShader = compile vs_3_0 VS();
						PixelShader = compile ps_3_0 PS();
					}
				}
			", "fx_2_0").Bytecode;
			var shaderModel = ShaderReader.ReadShader(bytecode);
			var shaders = from blob in shaderModel.EffectChunk.StateBlobLookup.Values
								where blob.BlobType == DX9Shader.FX9.StateBlobType.Shader
								select blob.Shader;
			string AstDecompile(ShaderModel shader) => new HlslWriter(shader, doAstAnalysis: true).Decompile();

			var testVertexShader = shaders.First(s => s.Type == ShaderType.Vertex);
			var decompiledVertexShader = AstDecompile(testVertexShader);
			// here we use the `.w` component, to make sure `HakureiReimuAliceMargatroid.w` 
			// is inside the actual decompiled shader too, not just inside constant declaration 
			// (that is, `float4 HakureiReimuAliceMargatroid`).
			StringAssert.Contains("HakureiReimuAliceMargatroid.w", decompiledVertexShader);
			// assert that `u[2].t[1].v` appears inside the decompiled source code
			StringAssert.Contains("u[2].t[1].v", decompiledVertexShader);

			var testPixelShader = shaders.First(s => s.Type == ShaderType.Pixel);
			var decompiledPixelShader = AstDecompile(testPixelShader);
			// assert that sampler array elements `Gensokyo[0]` and `[1]` 
			// appears inside the decompile source code
			StringAssert.Contains("Gensokyo[0]", decompiledPixelShader);
			StringAssert.Contains("Gensokyo[1]", decompiledPixelShader);

			var fromNonAst = HlslWriter.Decompile(bytecode);
			StringAssert.Contains("HakureiReimuAliceMargatroid.w", fromNonAst);
			StringAssert.Contains("u[2].t[1].v", fromNonAst);
			StringAssert.Contains("Gensokyo[0]", fromNonAst);
			StringAssert.Contains("Gensokyo[1]", fromNonAst);
		}
	}
}
