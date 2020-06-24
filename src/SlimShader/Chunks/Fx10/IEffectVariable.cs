using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
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
