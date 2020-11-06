using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	public class FxlcChunk : BytecodeChunk
	{
		FxlcBlock Fxlc;
		public List<FxlcToken> Tokens => Fxlc.Tokens;

		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize, BytecodeContainer container)
		{
			var result = new FxlcChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			result.Fxlc = FxlcBlock.Parse(chunkReader);
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("eval(");
			var ctab = Container.GetChunk<CtabChunk>();
			var cli = Container.GetChunk<Cli4Chunk>();
			var code = string.Join($"{Environment.NewLine}                    ",
				Tokens.Select(t => t.ToString(ctab.ConstantTable, cli)));
			sb.Append(code);
			sb.Append(")");
			return sb.ToString();
		}
	}
}
