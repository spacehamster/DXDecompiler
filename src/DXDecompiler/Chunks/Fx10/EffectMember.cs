using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC
	/// </summary>
	public class EffectMember : IEffectVariable
	{

		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint BufferOffset { get; private set; }
		public EffectType Type { get; private set; }

		public uint Flags => 0;
		public uint AnnotationCount => 0;
		public uint ExplicitBindPoint => 0;
		public IList<IEffectVariable> Annotations => new IEffectVariable[0];

		public static EffectMember Parse(BytecodeReader reader, BytecodeReader memberReader, ShaderVersion version)
		{
			var result = new EffectMember();
			var nameOffset = memberReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			var semanticNameOffset = memberReader.ReadUInt32();
			if(semanticNameOffset != 0)
			{
				var semanticNameReader = reader.CopyAtOffset((int)semanticNameOffset);
				result.Semantic = semanticNameReader.ReadString();
			}
			else
			{
				result.Semantic = "";
			}
			result.BufferOffset = memberReader.ReadUInt32();
			var typeOffset = memberReader.ReadUInt32();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = EffectType.Parse(reader, typeReader, version);
			return result;
		}
	}
}
