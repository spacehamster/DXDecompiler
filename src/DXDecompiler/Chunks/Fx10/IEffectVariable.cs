using System.Collections.Generic;

namespace DXDecompiler.Chunks.Fx10
{
	public interface IEffectVariable
	{
		EffectType Type { get; }
		string Name { get; }
		string Semantic { get; }
		uint Flags { get; }
		uint AnnotationCount { get; }
		uint BufferOffset { get; }
		uint ExplicitBindPoint { get; }
		IList<IEffectVariable> Annotations { get; }
	}
}
