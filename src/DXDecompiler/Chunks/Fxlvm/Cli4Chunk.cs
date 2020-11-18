using DXDecompiler.Util;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.Chunks.Fxlvm
{
	/* 
	 * Format
	 * uint FourCC
	 * uint ChunkSize
	 * uint Count
	 * uint[] Numbers
	 */
	public class Cli4Chunk : BytecodeChunk
	{
		public List<Number> Numbers { get; private set; }

		public Cli4Chunk()
		{
			Numbers = new List<Number>();
		}
		public static BytecodeChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new Cli4Chunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var count = chunkReader.ReadUInt32();
			for(int i = 0; i < count; i++)
			{
				result.Numbers.Add(Number.Parse(chunkReader));
			}
			return result;
		}

		public override string ToString()
		{
			return string.Join(", ", Numbers);
		}

		public string GetLiteral(uint elementIndex, uint elementCount)
		{
			var sb = new StringBuilder();
			for(int i = 0; i < elementCount; i++)
			{
				var index = elementIndex + i;
				var number = Numbers[(int)index];
				sb.Append(number.ToString());
				if( i < elementCount - 1)
				{
					sb.Append(", ");
				}
			}			
			return sb.ToString();
		}
	}
}
