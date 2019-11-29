using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Sfi0
{
	[Flags]
	public enum SfiFlags
	{
		None = 0,
		RequiresDoublePrecisionFloatingPoint = 0x1,
		RequiresEarlyDepthStencil = 0x2,
		RequiresUAVSlots64 = 0x8,
		RequiresMinimumPrecisionDataTypes = 0x10,
		RequiresDoublePrecisionExtensions = 0x20,
		RequiresShaderExtensionsFor11_1 = 0x40,
	}
}
