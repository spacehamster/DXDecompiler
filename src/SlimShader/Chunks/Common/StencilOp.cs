using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Common
{
	/// <summary>
	/// Based on D3D12_STENCIL_OP
	/// </summary>
	public enum StencilOp
	{
		[Description("KEEP")]
		Keep = 1,
		[Description("ZERO")]
		Zero = 2,
		[Description("REPLACE")]
		Replace = 3,
		[Description("INCR_STAT")]
		IncrStat = 4,
		[Description("DECR_STAT")]
		DecrStat = 5,
		[Description("INVERT")]
		Invert = 6,
		[Description("INCR")]
		Incr = 7,
		[Description("DECR")]
		Decr = 8
	}
}
