using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC
	/// </summary>
	public class DebugEffectMember : IDebugEffectVariable
	{

		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint BufferOffset { get; private set; }
		public DebugEffectType Type { get; private set; }

		public uint Flags => 0;
		public uint AnnotationCount => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IDebugEffectVariable> Annotations => new IDebugEffectVariable[0];

		public uint TypeOffset;
		public uint NameOffset;
		public uint SemanticNameOffset;
		public static DebugEffectMember Parse(DebugBytecodeReader reader, DebugBytecodeReader memberReader, DebugShaderVersion version)
		{
			var result = new DebugEffectMember();
			var nameOffset = result.NameOffset = memberReader.ReadUInt32("NameOffset");

			var nameReader = reader.CopyAtOffset("NameReader", memberReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");

			result.SemanticNameOffset = memberReader.ReadUInt32("SemanticNameOffset");
			if (result.SemanticNameOffset != 0)
			{
				var semanticNameReader = reader.CopyAtOffset("SemanticNameReader", memberReader, (int)result.SemanticNameOffset);
				result.Semantic = semanticNameReader.ReadString("Semantic");
			} else
			{
				result.Semantic = "";
			}
			result.BufferOffset = memberReader.ReadUInt32("BufferOffset");
			result.TypeOffset = memberReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeReader", memberReader, (int)result.TypeOffset);
			result.Type = DebugEffectType.Parse(reader, typeReader, version);
			return result;
		}
	}
}
