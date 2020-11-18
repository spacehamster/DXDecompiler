using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Root Signature static sampler border color.
	/// Based on D3D12_STATIC_BORDER_COLOR.
	/// </summary>
	public enum StaticBorderColor
	{
		[Description("STATIC_BORDER_COLOR_TRANSPARENT_BLACK")]
		TransparentBlack,
		[Description("STATIC_BORDER_COLOR_OPAQUE_BLACK")]
		OpaqueBlack,
		[Description("STATIC_BORDER_COLOR_OPAQUE_WHITE")]
		OpaqueWhite
	}
}
