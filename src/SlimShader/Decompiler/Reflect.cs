using SlimShader.Chunks.Rdef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Decompiler
{
	internal class Reflect
	{
		internal static ConstantBuffer GetConstantBufferFromBindingPoint(ConstantBufferType constantBufferType, uint bindPoint, BytecodeContainer container) 
 		{
			var constantBuffers = container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType == constantBufferType)
				.ToList();
			return constantBuffers[(int)bindPoint];
		}
	}
}
