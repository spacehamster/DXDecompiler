using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.RTS0
{
	/// <summary>
	/// The comparison function used by a root descriptor sampler.
	/// Based on D3D12_COMPARISON_FUNC.
	/// </summary>
	public enum ComparisonFunc
	{
		[Description("COMPARISON_NEVER")]
		Never = 1,
		[Description("COMPARISON_LESS")]
		Less = 2,
		[Description("COMPARISON_EQUAL")]
		Equal = 3,
		[Description("COMPARISON_LESS_EQUAL")]
		LessEqual = 4,
		[Description("COMPARISON_GREATER")]
		Greater = 5,
		[Description("COMPARISON_NOT_EQUAL")]
		NotEqual = 6,
		[Description("COMPARISON_GREATER_EQUAL")]
		GreaterEqual = 7,
		[Description("COMPARISON_ALWAYS")]
		Always = 8
	}
}
