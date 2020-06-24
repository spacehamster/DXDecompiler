using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Fx10
{
	public class EffectInterfaceVariable : IEffectVariable
	{
		public string Name { get; private set; }
		public EffectType Type { get; private set; }
		public uint Flags { get; private set; }
		public string InstanceName { get; private set; }
		public uint ArrayIndex { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }


		uint IEffectVariable.AnnotationCount => (uint)Annotations.Count;
		public uint ExplicitBindPoint => 0;
		IList<IEffectVariable> IEffectVariable.Annotations => Annotations.Cast<IEffectVariable>().ToList();
		public string Semantic => "";
		public uint BufferOffset => 0;

		public EffectInterfaceVariable()
		{
			Annotations = new List<EffectAnnotation>();
		}
		internal static EffectInterfaceVariable Parse(BytecodeReader reader, BytecodeReader variableReader, ShaderVersion version)
		{
			var result = new EffectInterfaceVariable();
			var nameOffset = variableReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var typeOffset = variableReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = EffectType.Parse(reader, typeReader, version);
			//Pointer to InterfaceInitializer
			var defaultValueOffset = variableReader.ReadUInt32();
			var initializerReader = reader.CopyAtOffset((int)defaultValueOffset);
			var instanceNameOffset = initializerReader.ReadUInt32();
			var instanceNameReader = reader.CopyAtOffset((int)instanceNameOffset);
			result.InstanceName = instanceNameReader.ReadString();
			result.Flags = variableReader.ReadUInt32();
			var annotationCount = variableReader.ReadUInt32();
			for (int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, variableReader, version));
			}
	
			return result;
		}
		public override string ToString()
		{
			string arrayFormat = "";
			if (Type.ElementCount > 1)
			{
				arrayFormat = string.Format("[{0}]", Type.ElementCount);
			}
			return string.Format("{0} {1}{2};", Type.TypeName, Name, arrayFormat);
		}
	}
}
