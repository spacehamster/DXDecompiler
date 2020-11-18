using DXDecompiler.Chunks.Fx10;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectAnnotation : IDebugEffectVariable
	{
		public string Name { get; private set; }
		public DebugEffectType Type { get; private set; }
		public List<DebugNumber> DefaultNumericValue { get; private set; }
		public List<string> DefaultStringValue { get; private set; }

		public string Semantic => "";
		public uint Flags => 2; //Annotation Flag. TODO: Fix Type
		public uint AnnotationCount => 0;
		public uint BufferOffset => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IDebugEffectVariable> Annotations => new IDebugEffectVariable[0];

		public uint NameOffset;
		public uint TypeOffset;
		public uint ValueOffset;
		private uint ElementCount => Type.ElementCount == 0 ? 1 : Type.ElementCount;
		public DebugEffectAnnotation()
		{
			DefaultNumericValue = new List<DebugNumber>();
			DefaultStringValue = new List<string>();
		}
		public static DebugEffectAnnotation Parse(DebugBytecodeReader reader, DebugBytecodeReader annotationReader, DebugShaderVersion version)
		{
			var result = new DebugEffectAnnotation();
			var nameOffset = result.NameOffset = annotationReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", reader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			var typeOffset = result.TypeOffset = annotationReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeReader", annotationReader, (int)typeOffset);
			result.Type = DebugEffectType.Parse(reader, typeReader, version);
			//Note: Points to 27 and "foo" in Texture2D tex<int bla=27;string blu="foo";>;
			/// Value format and stride depends on Type
			var valueOffset = result.ValueOffset = annotationReader.ReadUInt32("ValueOffset");
			var defaultValueReader = reader.CopyAtOffset("DefaultValueReader", annotationReader, (int)valueOffset);
			if(result.Type.EffectVariableType == EffectVariableType.Numeric)
			{
				for(int i = 0; i < result.Type.PackedSize / 4; i++)
				{
					result.DefaultNumericValue.Add(DebugNumber.Parse(defaultValueReader));
				}
			}
			else
			{
				for(int i = 0; i < result.ElementCount; i++)
				{
					result.DefaultStringValue.Add(defaultValueReader.ReadString($"DefaultValueString{i}"));
				}
			}
			return result;
		}
	}
}
