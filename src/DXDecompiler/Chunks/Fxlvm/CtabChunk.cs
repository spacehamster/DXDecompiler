using SlimShader.DX9Shader;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fxlvm
{
	/* 
	* Format
	* uint FourCC
	* uint ChunkSize
	* uint Count
	* uint[] Numbers
	*/
	public class CtabChunk : BytecodeChunk
	{
		public ConstantTable ConstantTable { get; private set; }

		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new CtabChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			result.ConstantTable = ConstantTable.Parse(chunkReader);
			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetType().Name);
			foreach(var decl in ConstantTable.ConstantDeclarations)
			{
				sb.AppendLine($"ConstantDeclarations: {decl}");
			}
			return sb.ToString();
		}
	}
}
