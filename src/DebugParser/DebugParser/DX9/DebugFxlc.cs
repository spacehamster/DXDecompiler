using SlimShader.DebugParser.Chunks.Fxlvm;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SlimShader.DebugParser.DX9
{
	public class DebugFxlc
	{
		List<DebugFxlcToken> Tokens = new List<DebugFxlcToken>();
		public static DebugFxlc Parse(DebugBytecodeReader reader, uint size)
		{
			var result = new DebugFxlc();
			var basePosition = reader._reader.BaseStream.Position;
			var tokenCount = reader.ReadUInt32("TokenCount");
			for (int i = 0; i < tokenCount; i++)
			{
				var token = reader.PeakUint32();
				var type = (FxlcOpcode)token.DecodeValue(20, 30);
				reader.AddIndent($"Token{i}({type})");
				result.Tokens.Add(DebugFxlcToken.Parse(reader));
				reader.RemoveIndent();
			}
			var padding = reader.ReadBytes($"Padding", 8);
			var paddingUint64 = BitConverter.ToInt64(padding, 0);
			var expected = 0x0F0F0f0FF0F0F0F0;
			Debug.Assert(paddingUint64 == expected);
			return result;
		}
	}
}
