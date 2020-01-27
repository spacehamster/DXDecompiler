using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.RTS0
{
	public class RootSignatureChunk : BytecodeChunk
	{
		public static RootSignatureChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var size = chunkSize / sizeof(uint);

			var chunk = new RootSignatureChunk();

			return chunk;
		}
	}
}
