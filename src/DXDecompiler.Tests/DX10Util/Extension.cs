using SharpDX.Direct3D10;
using System.Collections.Generic;

namespace DXDecompiler.Tests.DX10Util
{
	public static class Extension
	{
		public static List<T> GetAsList<T>(int count, System.Func<int, T> Getter)
		{
			var result = new List<T>();
			for (int i = 0; i < count; i++)
			{
				var variable = Getter(i);
				result.Add(variable);
			}
			return result;
		}
		public static List<EffectTechnique> GetTechniques(this Effect effect)
		{
			return GetAsList(
				effect.Description.TechniqueCount, 
				(i) => effect.GetTechniqueByIndex(i));
		}
		public static List<EffectConstantBuffer> GetConstantBuffers(this Effect effect)
		{
			return GetAsList(
				effect.Description.ConstantBufferCount + effect.Description.SharedConstantBufferCount,
				(i) => effect.GetConstantBufferByIndex(i));
		}
		public static List<EffectVariable> GetVariables(this Effect effect)
		{
			return GetAsList(
				effect.Description.GlobalVariableCount + effect.Description.SharedGlobalVariableCount,
				(i) => effect.GetVariableByIndex(i));
		}
		public static List<EffectPass> GetPasses(this EffectTechnique technique)
		{
			return GetAsList(
				technique.Description.PassCount,
				(i) => technique.GetPassByIndex(i));
		}
		public static List<EffectVariable> GetAnnotations(this EffectTechnique technique)
		{
			return GetAsList(
				technique.Description.AnnotationCount,
				(i) => technique.GetAnnotationByIndex(i));
		}
		public static List<EffectVariable> GetAnnotations(this EffectVariable variable)
		{
			return GetAsList(
				variable.Description.AnnotationCount,
				(i) => variable.GetAnnotationByIndex(i));
		}
		public static List<EffectVariable> GetAnnotations(this EffectPass pass)
		{
			return GetAsList(
				pass.Description.AnnotationCount,
				(i) => pass.GetAnnotationByIndex(i));
		}
	}
}
