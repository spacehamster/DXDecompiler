using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Aon9
{
	public enum DataConversionType
	{
		[Description("FLT")]
		Float = 0,
		[Description("BOOL")]
		Bool = 1,
		[Description("INT")]
		Int = 2,
		[Description("UINT")]
		Uint = 3
	}
}
