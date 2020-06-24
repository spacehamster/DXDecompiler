using SlimShader.Chunks.Fxlvm;
using SlimShader.DebugParser.DX9;
using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using SlimShader.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugFxlcChunk : DebugBytecodeChunk
	{
		DebugFxlc Fxlc;
		public List<DebugFxlcToken> Tokens = new List<DebugFxlcToken>();

		public static DebugBytecodeChunk Parse(DebugBytecodeReader reader, uint chunkSize, DebugBytecodeContainer container)
		{
			var result = new DebugFxlcChunk();
			var chunkReader = reader.CopyAtCurrentPosition("FxlcChunkReader", reader);
			result.Fxlc = DebugFxlc.Parse(chunkReader, chunkSize);
			var padding = chunkReader.ReadUInt32("Padding");
			Debug.Assert(padding == 0xFFFF);
			return result;
		}
	}
}
