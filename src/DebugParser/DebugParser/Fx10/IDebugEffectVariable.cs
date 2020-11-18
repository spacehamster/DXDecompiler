using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public interface IDebugEffectVariable
	{
		DebugEffectType Type { get; }
		string Name { get; }
		string Semantic { get; }
		uint Flags { get; }
		uint AnnotationCount { get; }
		uint BufferOffset { get; }
		uint ExplicitBindPoint { get; }
		IList<IDebugEffectVariable> Annotations { get; }
	}
}
