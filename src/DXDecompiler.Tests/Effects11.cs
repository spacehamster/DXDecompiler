using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;
using DXDecompiler.Chunks.Fx10;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace DXDecompiler.Tests
{
	public class Effects11
	{
		public static void CompareVariable(EffectVariable reflectionVariable, object variable)
		{
			EffectTypeDescription typeDesc = reflectionVariable.TypeInfo.Description;
			Assert.AreEqual(typeDesc.TypeName, typeDesc.TypeName);
			Assert.AreEqual(typeDesc.Class, typeDesc.Class);
			Assert.AreEqual(typeDesc.Type, typeDesc.Type);
			Assert.AreEqual(typeDesc.Elements, typeDesc.Elements);
			Assert.AreEqual(typeDesc.Members, typeDesc.Members);
			Assert.AreEqual(typeDesc.Rows, typeDesc.Rows);
			Assert.AreEqual(typeDesc.Columns, typeDesc.Columns);
			Assert.AreEqual(typeDesc.PackedSize, typeDesc.PackedSize);
			Assert.AreEqual(typeDesc.UnpackedSize, typeDesc.UnpackedSize);
			Assert.AreEqual(typeDesc.Stride, typeDesc.Stride);
			EffectVariableDescription variableDesc = reflectionVariable.Description;
			Assert.AreEqual(variableDesc.Name, variableDesc.Name);
			Assert.AreEqual(variableDesc.Semantic, variableDesc.Semantic);
			Assert.AreEqual(variableDesc.Flags, variableDesc.Flags);
			Assert.AreEqual(variableDesc.AnnotationCount, variableDesc.AnnotationCount);
			Assert.AreEqual(variableDesc.BufferOffset, variableDesc.BufferOffset);
			Assert.AreEqual(variableDesc.ExplicitBindPoint, variableDesc.ExplicitBindPoint);

			if (variable is EffectNumericVariable v1)
			{
				Assert.AreEqual(variableDesc.Name, v1.Name);
				Assert.AreEqual(0, v1.ExplicitBindPoint);
			}
			if (variable is EffectObjectVariable v2)
			{
				Assert.AreEqual(variableDesc.Name, v2.Name);
			}

			for (int i = 0; i < variableDesc.AnnotationCount; i++)
			{
				var annotation = reflectionVariable.GetAnnotationByIndex(i);
			}
			if (typeDesc.Type == ShaderVariableType.Sampler)
			{
				EffectSamplerVariable sampVariable = reflectionVariable.AsSampler();
				SamplerState samplerState = sampVariable.GetSampler();
				var sampDesc = samplerState.Description;
			}
		}
		public static int GetBufferCount(Effect effect)
		{
			for (int i = 0; i < 1000; i++)
			{
				var cb = effect.GetConstantBufferByIndex(i);
				if (!cb.IsValid) return i;
			}
			return 1000;
		}
		public static int GetVariableCount(Effect effect)
		{
			for (int i = 0; i < 1000; i++)
			{
				var cb = effect.GetVariableByIndex(i);
				if (!cb.IsValid) return i;
			}
			return 1000;
		}
		public static List<EffectVariable> GetEffectVariables(Effect effect)
		{
			var desc = effect.Description;
			var result = new List<EffectVariable>();
			for (int i = 0; i < desc.GlobalVariableCount; i++)
			{
				var variable = effect.GetVariableByIndex(i);
				result.Add(variable);
			}
			return result;
		}
		public static void CompareEffect(BytecodeContainer container, byte[] effectBytecode, string testName)
		{
			var device = new Device(
				DriverType.Warp,
				DeviceCreationFlags.Debug,
				FeatureLevel.Level_10_1);
			Effect effectReflection = null;
			if (container == null)
			{
				effectReflection = new Effect(device, effectBytecode, EffectFlags.None);
				Warn.If(true, "Container is null");
				return;
			}
			var chunk = container.Chunks.OfType<EffectChunk>().First();

			if (chunk.IsChildEffect)
			{
				effectReflection = new Effect(device, effectBytecode, EffectFlags.None);
				//var effectPool = new EffectPool(device, effectBytecode, EffectFlags.None);
				//effectReflection = effectPool.AsEffect();
			}
			else
			{
				effectReflection = new Effect(device, effectBytecode, EffectFlags.None);
			}
			EffectDescription desc = effectReflection.Description;
			var header = chunk.Header;
			//Assert.AreEqual((bool)desc.IsChildEffect, header.SharedConstantBuffers > 0);
			Assert.AreEqual(desc.ConstantBufferCount, header.ConstantBuffers);
			//Assert.AreEqual(desc.SharedConstantBufferCount, header.SharedConstantBuffers);
			Assert.AreEqual(desc.GlobalVariableCount, header.GlobalVariables + header.ObjectCount);
			//Assert.AreEqual(desc.SharedGlobalVariableCount, header.SharedGlobalVariables);
			Assert.AreEqual(desc.TechniqueCount, header.Techniques);
			var reflectionConstantBufferCount = GetBufferCount(effectReflection);
			Assert.AreEqual(reflectionConstantBufferCount, header.ConstantBuffers + header.SharedConstantBuffers);
			var reflectionVariableCount = GetVariableCount(effectReflection);

			Assert.AreEqual(reflectionVariableCount, header.ObjectCount + header.SharedObjectCount + header.GlobalVariables + header.SharedGlobalVariables);
			var variables = chunk.AllVariables.ToList();
			var reflectionVariables = GetEffectVariables(effectReflection);
			var reflectionNames = reflectionVariables
				.Select(v => $"{v.Description.Name}, {v.TypeInfo.Description.Type}, {v.TypeInfo.Description.Class}")
				.ToList();
			for (int i = 0; i < desc.GlobalVariableCount; i++)
			{
				CompareVariable(reflectionVariables[i], variables[i]);
			}
			var buffers = chunk.AllBuffers.ToList();
			for (int i = 0; i < desc.ConstantBufferCount; i++)
			{
				var cb = effectReflection.GetConstantBufferByIndex(i);
				//CompareConstantBuffer(cb, buffers[i]);
			}
		}
	}
}
