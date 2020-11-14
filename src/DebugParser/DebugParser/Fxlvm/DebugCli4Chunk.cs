using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fxlvm
{
	public class DebugCli4Chunk : DebugBytecodeChunk
	{
		public List<DebugNumber> Numbers { get; private set; }

		public DebugCli4Chunk()
		{
			Numbers = new List<DebugNumber>();
		}
		public static DebugCli4Chunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugCli4Chunk();
			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader);
			var count = chunkReader.ReadUInt32("Count");
			for(int i = 0; i < count; i++)
			{
				chunkReader.AddIndent($"Number {i}");
				var info = chunkReader.Members.Last();
				var number = DebugNumber.Parse(chunkReader);
				result.Numbers.Add(number);
				info.AddNote("Int", number.Int);
				info.AddNote("Uint", number.UInt);
				info.AddNote("Float", number.Float);
				chunkReader.RemoveIndent();
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
