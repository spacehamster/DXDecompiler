using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Util;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectInterfaceVariable : IDebugEffectVariable
	{
		public string Name { get; private set; }
		public DebugEffectType Type { get; private set; }
		public uint Flags { get; private set; }
		public string InstanceName { get; private set; }
		public uint ArrayIndex { get; private set; }
		public List<DebugEffectAnnotation> Annotations { get; private set; }


		uint IDebugEffectVariable.AnnotationCount => AnnotationCount;
		public uint ExplicitBindPoint => 0;
		IList<IDebugEffectVariable> IDebugEffectVariable.Annotations => 
			Annotations.Cast<IDebugEffectVariable>().ToList();
		public string Semantic => "";
		public uint BufferOffset => 0;

		uint TypeOffset;
		uint NameOffset;
		uint AnnotationCount;
		uint InstanceNameOffset;
		public uint DefaultValueOffset;
		public DebugEffectInterfaceVariable()
		{
			Annotations = new List<DebugEffectAnnotation>();
		}
		internal static DebugEffectInterfaceVariable Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader, DebugShaderVersion version)
		{
			var result = new DebugEffectInterfaceVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", variableReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			var typeOffset = result.TypeOffset = variableReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeReader", variableReader, (int)typeOffset);
			result.Type = DebugEffectType.Parse(reader, typeReader, version);
			//Pointer to InterfaceInitializer
			result.DefaultValueOffset = variableReader.ReadUInt32("DefaultValueOffset");
			var initializerReader = reader.CopyAtOffset("InitializerReader", variableReader, (int)result.DefaultValueOffset);
			var instanceNameOffset = result.InstanceNameOffset = initializerReader.ReadUInt32("InstanceNameOffset");
			var instanceNameReader = reader.CopyAtOffset("InstanceNameReader", variableReader, (int)instanceNameOffset);
			result.InstanceName = instanceNameReader.ReadString("InstanceName");
			result.Flags = variableReader.ReadUInt32("Flags");
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				variableReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, variableReader, version));
				variableReader.RemoveIndent();
			}
	
			return result;
		}
	}
}
