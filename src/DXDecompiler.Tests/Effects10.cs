using NUnit.Framework;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D10;
using Fx10 = DXDecompiler.Chunks.Fx10;
using System.Collections.Generic;
using System.Linq;
using DXDecompiler.Tests.DX10Util;

namespace DXDecompiler.Tests
{
	class Effects10
	{
		public static void CompareShaderResourceView(ShaderResourceView reflectionResourceView)
		{

		}
		public static void CompareBuffer(Buffer reflectionBuffer)
		{
			if (reflectionBuffer == null) return;
			var desc = reflectionBuffer.Description;
			Assert.AreEqual(desc.SizeInBytes, desc.SizeInBytes);
			Assert.AreEqual(desc.Usage, desc.Usage);
			Assert.AreEqual(desc.BindFlags, desc.BindFlags);
			Assert.AreEqual(desc.CpuAccessFlags, desc.CpuAccessFlags);
			Assert.AreEqual(desc.OptionFlags, desc.OptionFlags);
		}
		public static void CompareConstantBuffer(EffectConstantBuffer reflectionEffectBuffer, Fx10.EffectBuffer buffer)
		{
			var desc = reflectionEffectBuffer.Description;
			Assert.AreEqual(desc.Name, buffer.Name);
			Assert.AreEqual(desc.Semantic ?? "", "");
			Assert.AreEqual((uint)desc.Flags, 0);
			Assert.AreEqual(desc.AnnotationCount, 0);
			Assert.AreEqual(desc.BufferOffset, 0);
			Assert.AreEqual(desc.ExplicitBindPoint, buffer.RegisterNumber == uint.MaxValue ? 0 : buffer.RegisterNumber);
			Assert.AreEqual(reflectionEffectBuffer.TypeInfo.Description.Type, (ShaderVariableType)buffer.Type);
			if(reflectionEffectBuffer.TypeInfo.Description.Type == ShaderVariableType.ConstantBuffer)
			{
				var cbufferDesc = reflectionEffectBuffer.GetConstantBuffer().Description;
				//TODO: Compare cbufferDesc
			} else
			{
				var tbufferDesc = reflectionEffectBuffer.GetTextureBuffer().Description;
				//TODO: Compare tbufferDesc
			}
		}
		public static void CompareVariable(EffectVariable reflectionVariable, Fx10.IEffectVariable variable)
		{
			EffectTypeDescription typeDesc = reflectionVariable.TypeInfo.Description;
			var type = variable.Type;
			Assert.AreEqual(typeDesc.TypeName, type.TypeName);
			Assert.AreEqual(typeDesc.Class, (ShaderVariableClass)type.VariableClass);
			Assert.AreEqual(typeDesc.Type, (ShaderVariableType)type.VariableType);
			Assert.AreEqual(typeDesc.Elements, type.ElementCount);
			Assert.AreEqual(typeDesc.Members, type.MemberCount);
			Assert.AreEqual(typeDesc.Rows, type.Rows);
			Assert.AreEqual(typeDesc.Columns, type.Columns);
			Assert.AreEqual(typeDesc.PackedSize, type.PackedSize);
			Assert.AreEqual(typeDesc.UnpackedSize, type.UnpackedSize);
			Assert.AreEqual(typeDesc.Stride, type.Stride);
			EffectVariableDescription variableDesc = reflectionVariable.Description;
			Assert.AreEqual(variableDesc.Name, variable.Name);
			Assert.AreEqual(variableDesc.Semantic ?? "", variable.Semantic);
			Assert.AreEqual(variableDesc.Flags, (EffectVariableFlags)variable.Flags);
			Assert.AreEqual(variableDesc.AnnotationCount, variable.AnnotationCount);
			//TODO: SharpDX seems defines BufferOffset relative to buffer, but variable is just relative to struct
			//Assert.AreEqual(variableDesc.BufferOffset, variable.BufferOffset == uint.MaxValue ? 0 : variable.BufferOffset);
			Assert.AreEqual(variableDesc.ExplicitBindPoint, variable.ExplicitBindPoint);
			var annotations = GetAnnotations(reflectionVariable);
			if (typeDesc.Class == ShaderVariableClass.Struct)
			{
				//TODO: SharpDx has GetMemberValues on the EffectType.  
				for (int i = 0; i < typeDesc.Members; i++)
				{
					var reflectionMember = reflectionVariable.GetMemberByIndex(i);
					var member = variable.Type.Members[i];
					CompareVariable(reflectionMember, member);
				}
			}
			for (int i = 0; i < annotations.Count; i++)
			{
				var reflectionAnnotation = annotations[i];
				var annotation = variable.Annotations[i];
				CompareVariable(reflectionAnnotation, annotation);
			}
			if (typeDesc.Type == ShaderVariableType.Blend)
			{
				var specDesc = reflectionVariable
								.AsBlend()
								.GetBlendState()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.ConstantBuffer)
			{
				var specDesc = reflectionVariable
								.AsConstantBuffer()
								.GetConstantBuffer()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.DepthStencil)
			{
				var specDesc = reflectionVariable
								.AsDepthStencil()
								.GetDepthStencilState()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.DepthStencilView)
			{
				var specDesc = reflectionVariable
								.AsDepthStencilView()
								.GetDepthStencil()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.Rasterizer)
			{
				var specDesc = reflectionVariable
								.AsRasterizer()
								.GetRasterizerState()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.Rasterizer)
			{
				var specDesc = reflectionVariable
								.AsRasterizer()
								.GetRasterizerState()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.RenderTargetView)
			{
				var specDesc = reflectionVariable
								.AsRenderTargetView()
								.GetRenderTarget()
								.Description;
			}
			if (typeDesc.Type == ShaderVariableType.Sampler)
			{
				var specDesc = reflectionVariable
								.AsSampler()
								.GetSampler()
								.Description;
				var stateAnnotations = (variable as Fx10.EffectObjectVariable).Assignments;
			}
			if (typeDesc.Type == ShaderVariableType.PixelShader ||
				typeDesc.Type == ShaderVariableType.VertexShader ||
				typeDesc.Type == ShaderVariableType.GeometryShader)
			{
				var shader = reflectionVariable
								.AsShader();
			}
			if (false)
			{

				var shader = reflectionVariable
								.AsShaderResource();
			}
		}
		public static List<EffectVariable> GetAnnotations(EffectVariable reflectionVariable)
		{
			var result = new List<EffectVariable>();
			for (int i = 0; i < reflectionVariable.Description.AnnotationCount; i++)
			{
				var annotation = reflectionVariable.GetAnnotationByIndex(i);
				result.Add(annotation);
			}
			return result;
		}
		public static List<EffectVariable> GetEffectVariables(Effect effect)
		{
			var desc = effect.Description;
			var result = new List<EffectVariable>();
			for (int i = 0; i < desc.GlobalVariableCount + desc.SharedGlobalVariableCount; i++)
			{
				var variable = effect.GetVariableByIndex(i);
				result.Add(variable);
			}
			return result;
		}
		public static List<EffectTechnique> Gettechniques(Effect effect)
		{
			var desc = effect.Description;
			var result = new List<EffectTechnique>();
			for (int i = 0; i < desc.TechniqueCount; i++)
			{
				var variable = effect.GetTechniqueByIndex(i);
				result.Add(variable);
			}
			return result;
		}
		public static void CompareEffect(BytecodeContainer container, byte[] effectBytecode, string testName)
		{

			var chunk = container.Chunks.OfType<Fx10.EffectChunk>().First();
			if (chunk.Header.Techniques == 0)
			{
				return;
			}
			if (chunk.Header.Version.MinorVersion == 1)
			{
				Assert.Warn("Version fx_4_1 is not supported by SharpDX");
				return;
			}
			if (chunk.IsChildEffect)
			{
				Assert.Warn("Child Effects are not supported by SharpDX");
				return;
			}
			var device = new Device(DriverType.Warp, DeviceCreationFlags.Debug);
			var effectReflection = new Effect(device, effectBytecode, EffectFlags.None);
			EffectDescription desc = effectReflection.Description;
			var header = chunk.Header;
			Assert.AreEqual((bool)desc.IsChildEffect, header.SharedConstantBuffers > 0);
			Assert.AreEqual(desc.ConstantBufferCount, header.ConstantBuffers);
			Assert.AreEqual(desc.SharedConstantBufferCount, header.SharedConstantBuffers);
			Assert.AreEqual(desc.GlobalVariableCount, header.GlobalVariables + header.ObjectCount);
			Assert.AreEqual(desc.SharedGlobalVariableCount, header.SharedGlobalVariables);
			Assert.AreEqual(desc.TechniqueCount, header.Techniques);
			var variables = chunk.AllVariables.ToList();
			var reflectionVariables = GetEffectVariables(effectReflection);
			var reflectionNames = reflectionVariables
				.Select(v => $"{v.Description.Name}, {v.TypeInfo.Description.Type}, {v.TypeInfo.Description.Class}")
				.ToList();
			for (int i = 0; i < desc.GlobalVariableCount + desc.SharedGlobalVariableCount; i++)
			{
				CompareVariable(reflectionVariables[i], variables[i]);
			}
			var buffers = chunk.AllBuffers.ToList();
			var reflectionBuffers = effectReflection.GetConstantBuffers();
			for (int i = 0; i < desc.ConstantBufferCount + desc.SharedConstantBufferCount; i++)
			{
				CompareConstantBuffer(reflectionBuffers[i], buffers[i]);
			}
			var techniques = effectReflection.GetTechniques();
			for (int i = 0; i < desc.TechniqueCount; i++)
			{
				CompareTechniques(techniques[i], chunk.Techniques[i]);
			}
		}

		private static void CompareTechniques(EffectTechnique reflectionTechnique, Fx10.EffectTechnique technique)
		{
			EffectTechniqueDescription desc = reflectionTechnique.Description;
			Assert.AreEqual(desc.Name, technique.Name);
			Assert.AreEqual(desc.AnnotationCount, technique.Annotations.Count);
			Assert.AreEqual(desc.PassCount, technique.Passes.Count);
			var passes = reflectionTechnique.GetPasses();
			for(int i = 0; i < desc.PassCount; i++)
			{
				ComparePass(passes[i], technique.Passes[i]);
			}
			var annotations = reflectionTechnique.GetAnnotations();
			for (int i = 0; i < desc.AnnotationCount; i++)
			{
				CompareVariable(annotations[i], technique.Annotations[i]);
			}
		}

		private static void ComparePass(EffectPass reflectionPass, Fx10.EffectPass pass)
		{
			EffectPassDescription desc = reflectionPass.Description;
			Assert.AreEqual(desc.Name, pass.Name);
			var annotations = reflectionPass.GetAnnotations();
			for(int i = 0; i < desc.AnnotationCount; i++)
			{
				CompareVariable(annotations[i], pass.Annotations[i]);
			}
			/*var pixelShader = pass.Shaders.FirstOrDefault(
				s => s.ShaderType == Fx10.EffectShaderType.PixelShader);
			if(pixelShader != null)
			{
				EffectPassShaderDescription shaderDesc = reflectionPass.PixelShaderDescription;
				CompareShader(shaderDesc, pixelShader);
			}
			var vertexShader = pass.Shaders.FirstOrDefault(
				s => s.ShaderType == Fx10.EffectShaderType.VertexShader);
			if (vertexShader != null)
			{
				EffectPassShaderDescription shaderDesc = reflectionPass.VertexShaderDescription;
				CompareShader(shaderDesc, vertexShader);
			}
			var geometryShader = pass.Shaders.FirstOrDefault(
				s => s.ShaderType == Fx10.EffectShaderType.GeometryShader);
			if (geometryShader != null)
			{
				EffectPassShaderDescription shaderDesc = reflectionPass.GeometryShaderDescription;
				CompareShader(shaderDesc, geometryShader);
			}*/
		}
	}
}
