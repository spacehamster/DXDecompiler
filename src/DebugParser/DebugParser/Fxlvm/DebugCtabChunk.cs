using SlimShader.DebugParser.DX9;
using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Declaration;
using System.Collections.Generic;
using System.Linq;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugCtabChunk : DebugBytecodeChunk
	{
		public DebugConstantTable ConstantTable { get; private set; }
		public static DebugBytecodeChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			//TODO: Merge Ctab parsing with DX9 Ctab
			var result = new DebugCtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader, (int)chunkSize);
			result.ConstantTable = DebugConstantTable.Parse(chunkReader);
			return result;
		}
	}
}
