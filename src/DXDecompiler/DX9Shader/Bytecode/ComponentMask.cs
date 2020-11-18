using System;

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
