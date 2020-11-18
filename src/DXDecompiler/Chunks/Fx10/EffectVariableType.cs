using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	/// <summary>
	/// Based on EVarType
	/// </summary>
	public enum EffectVariableType
	{
		Invalid,
		Numeric,
		Object,
		Struct,
		Interface,
	}
}
