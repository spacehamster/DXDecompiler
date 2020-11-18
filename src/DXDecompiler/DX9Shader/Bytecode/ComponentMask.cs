using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader
{
	[Flags]
	public enum ComponentFlags
	{
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		W = 8,
	}
}
