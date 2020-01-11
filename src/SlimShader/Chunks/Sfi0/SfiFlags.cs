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
		DoublePrecisionFloatingPoint = 0x1,
		EarlyDepthStencil = 0x2,
		UAVSlots64 = 0x8,
		MinimumPrecisionDataTypes = 0x10,
		DoublePrecisionExtensions = 0x20,
		ShaderExtensionsFor11_1 = 0x40,
		ComparisonFilteringForFeatureLevel9 = 0x80,
		TypedUAVLoadAdditionalFormats = 0x800,
		SVArrayIndexFromFeedingRasterizer = 0x2000
	}
}
