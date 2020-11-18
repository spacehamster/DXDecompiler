using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// The comparison function used by a root descriptor sampler.
	/// Based on D3D12_COMPARISON_FUNC.
	/// </summary>
	public enum ComparisonFunc
	{
		[Description("COMPARISON_NEVER", ChunkType.Rts0)]
		[Description("NEVER")]
		Never = 1,
		[Description("COMPARISON_LESS", ChunkType.Rts0)]
		[Description("LESS")]
		Less = 2,
		[Description("COMPARISON_EQUAL", ChunkType.Rts0)]
		[Description("EQUAL")]
		Equal = 3,
		[Description("COMPARISON_LESS_EQUAL", ChunkType.Rts0)]
		[Description("LESS_EQUAL")]
		LessEqual = 4,
		[Description("COMPARISON_GREATER", ChunkType.Rts0)]
		[Description("GREATER")]
		Greater = 5,
		[Description("COMPARISON_NOT_EQUAL", ChunkType.Rts0)]
		[Description("NOT_EQUAL")]
		NotEqual = 6,
		[Description("COMPARISON_GREATER_EQUAL", ChunkType.Rts0)]
		[Description("GREATER_EQUAL")]
		GreaterEqual = 7,
		[Description("COMPARISON_ALWAYS", ChunkType.Rts0)]
		[Description("ALWAYS")]
		Always = 8
	}
}
