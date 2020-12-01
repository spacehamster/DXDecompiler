using System;

namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Result Modifer Type https://msdn.microsoft.com/en-us/library/windows/hardware/ff552738%28v=vs.85%29.aspx
	/// </summary>
	[Flags]
	public enum ResultModifier
	{
		None = 0,
		Saturate = 1,
		PartialPrecision = 2,
		Centroid = 4
	}
}
