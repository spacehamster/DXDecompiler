using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Util;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Base on D3D10_EFFECT_VARIABLE_DESC
	/// </summary>
	public class DebugEffectNumericVariable : IDebugEffectVariable
	{
		public string Name { get; private set; }
		public DebugEffectType Type { get; private set; }
		public uint SemanticOffset { get; private set; }
		public string Semantic { get; private set; }
		public uint BufferOffset { get; private set; }
		public uint ExplicitBindPoint { get; private set; }
		public List<Number> DefaultValue { get; private set; }
		public List<DebugEffectAnnotation> Annotations { get; private set; }

		//TODO
		public uint Flags => 0;
		uint IDebugEffectVariable.AnnotationCount => AnnotationCount;
		IList<IDebugEffectVariable> IDebugEffectVariable.Annotations => 
			Annotations.Cast<IDebugEffectVariable>().ToList();

		uint AnnotationCount;
		uint NameOffset;
		uint TypeOffset;
		public uint DefaultValueOffset;
		public DebugEffectNumericVariable()
		{
			Annotations = new List<DebugEffectAnnotation>();
		}
		internal static DebugEffectNumericVariable Parse(DebugBytecodeReader reader, 
			DebugBytecodeReader variableReader, DebugShaderVersion version, bool isShared)
		{
			var result = new DebugEffectNumericVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", variableReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			var typeOffset = result.TypeOffset = variableReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeReader", variableReader, (int)typeOffset);
			result.Type = DebugEffectType.Parse(reader, typeReader, version);
			var semanticOffset = result.SemanticOffset = variableReader.ReadUInt32("SemeanticOffset");
			if (semanticOffset != 0)
			{
				var semanticReader = reader.CopyAtOffset("SemanticReader", variableReader, (int)semanticOffset);
				result.Semantic = semanticReader.ReadString("Semantic");
			} else
			{
				result.Semantic = "";
			}
			result.BufferOffset = variableReader.ReadUInt32("BufferOffset");
			var defaultValueOffset = result.DefaultValueOffset = variableReader.ReadUInt32("DefaultValueOffset");

			List<Number> defaultValue = null;
			var size = result.Type.PackedSize;
			if (defaultValueOffset != 0)
			{
				defaultValue = new List<Number>();
				var defaultValueReader = reader.CopyAtOffset("DefaultValueReader", variableReader, (int)defaultValueOffset);
				if (size % 4 != 0)
					throw new ParseException("Can only deal with 4-byte default values at the moment.");
				for (int i = 0; i < size; i += 4)
					defaultValue.Add(new Number(defaultValueReader.ReadBytes("Number", 4)));
			}
			result.DefaultValue = defaultValue;

			if (!isShared)
			{
				result.ExplicitBindPoint = variableReader.ReadUInt32("ExplicitBindPoint");
				//TODO: Unknown1
				//Debug.Assert(result.Unknown1 == 0, $"EffectBufferVariable.Unknown1 {result.Unknown1}");
			}
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for(int i = 0; i < result.AnnotationCount; i++)
			{
				variableReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, variableReader, version));
				variableReader.RemoveIndent();
			}
			return result;
		}
	}
}
