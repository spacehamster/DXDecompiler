using DXDecompiler.Chunks.Fxlvm;
using DXDecompiler.DebugParser.DX9;
using DXDecompiler.DX9Shader;
using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DXDecompiler.DebugParser.Chunks.Fxlvm
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
