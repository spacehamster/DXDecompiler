using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// Based on D3D10_PASS_DESC
	/// </summary>
	public class DebugEffectPass
	{
		public uint NameOffset;
		public string Name { get; private set; }
		public uint ShaderCount;
		public uint AnnotationCount;
		public List<DebugEffectAssignment> Assignments { get; private set; }
		public List<DebugEffectAnnotation> Annotations { get; private set; }
		public DebugEffectPass()
		{
			Assignments = new List<DebugEffectAssignment>();
			Annotations = new List<DebugEffectAnnotation>();
		}
		public static DebugEffectPass Parse(DebugBytecodeReader reader, DebugBytecodeReader passReader, DebugShaderVersion version)
		{
			var result = new DebugEffectPass();
			var nameOffset = result.NameOffset = passReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", passReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			result.ShaderCount = passReader.ReadUInt32("ShaderCount");
			result.AnnotationCount = passReader.ReadUInt32("AnnotationCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				passReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, passReader, version));
				passReader.RemoveIndent();
			}
			for (int i = 0; i < result.ShaderCount; i++)
			{
				passReader.AddIndent($"Shader {i}");
				result.Assignments.Add(DebugEffectAssignment.Parse(reader, passReader));
				passReader.RemoveIndent();
			}
			return result;
		}
	}
}
