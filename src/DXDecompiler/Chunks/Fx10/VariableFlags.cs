using System;

namespace DXDecompiler.Chunks.Fx10
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
