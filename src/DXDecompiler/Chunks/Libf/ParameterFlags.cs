using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Libf
{
	/// <summary>
	/// Indicates wether a library parameter is an input or output
	/// Based on D3D_PARAMETER_FLAGS
	/// </summary>
	[Flags]
	public enum ParameterFlags
	{
		None = 0,
		In = 0x1,
		Out = 0x2
	}
}
