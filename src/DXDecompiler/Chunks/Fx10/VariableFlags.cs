using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Based on D3D10_EFFECT_VARIABLE
	/// </summary>
	[Flags]
	public enum EffectVariableFlags
	{
		Pooled = 1,
		Annotation = 2,
		ExplicitBindPoint = 4
	}
}
