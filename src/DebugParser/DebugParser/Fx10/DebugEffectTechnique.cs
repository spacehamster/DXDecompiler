using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Based on D3D10_TECHNIQUE_DESC
	/// </summary>
	public class DebugEffectTechnique
	{
		public uint NameOffset;
		public string Name { get; private set; }
		public uint PassCount;
		public uint AnnotationCount;

		private DebugShaderVersion m_Version;
		public List<DebugEffectAnnotation> Annotations { get; private set; }
		public List<DebugEffectPass> Passes { get; private set; }
		public DebugEffectTechnique(DebugShaderVersion version)
		{
			Annotations = new List<DebugEffectAnnotation>();
			Passes = new List<DebugEffectPass>();
			m_Version = version;
		}
		public static DebugEffectTechnique Parse(DebugBytecodeReader reader,
				DebugBytecodeReader techniqueReader, DebugShaderVersion version)
		{
			var result = new DebugEffectTechnique(version);
			var nameOffset = result.NameOffset = techniqueReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", techniqueReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			result.PassCount = techniqueReader.ReadUInt32("PassCount");
			result.AnnotationCount = techniqueReader.ReadUInt32("AnnotationCount");

			for (int i = 0; i < result.AnnotationCount; i++)
			{
				techniqueReader.AddIndent("Annotation");
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, techniqueReader, version));
				techniqueReader.RemoveIndent();
			}
			for (int i = 0; i < result.PassCount; i++)
			{
				techniqueReader.AddIndent($"Pass {i}");
				result.Passes.Add(DebugEffectPass.Parse(reader, techniqueReader, version));
				techniqueReader.RemoveIndent();
			}
			return result;
		}
	}
}
