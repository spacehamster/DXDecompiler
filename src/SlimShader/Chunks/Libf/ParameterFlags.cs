using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Libf
{
	/// <summary>
	/// ParameterFlags
	/// Based on D3D_PARAMETER_FLAGS
	/// </summary>
	[Flags]
	public enum ParameterFlags
	{
		None = 0,
		In = 0x1,
		Out = 0x2,
		ForceDWord = 0x7fffffff
	}
}
