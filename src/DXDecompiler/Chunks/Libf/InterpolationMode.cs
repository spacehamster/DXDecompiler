using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Libf
{
	public enum InterpolationMode
	{
		Undefined = 0,
		Constant = 1,
		Linear = 2,
		LinearCentroid = 3,
		LinearNoPerspective = 4,
		LinearNoPerspectiveCentroid = 5,
		LinearSample = 6,
		LinearNoPerspetiveSample = 7
	}
}
