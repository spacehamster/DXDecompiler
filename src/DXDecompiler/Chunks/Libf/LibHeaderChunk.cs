using DXDecompiler.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Libf
{
	public class LibHeaderChunk : BytecodeChunk
	{
		public string CreatorString { get; private set; }
		public List<LibraryDesc> FunctionDescs { get; private set; }
		public LibHeaderChunk()
		{
			FunctionDescs = new List<LibraryDesc>();
		}
		public static LibHeaderChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var result = new LibHeaderChunk();

			//multiple creator strings?
			var unknown1 = chunkReader.ReadUInt32();
			Debug.Assert(unknown1 == 1, $"LibraryHeader.unknown1 is {unknown1}");
			var creatorStringOffset = chunkReader.ReadUInt32();
			var creatorStringReader = chunkReader.CopyAtOffset((int)creatorStringOffset);
			result.CreatorString = creatorStringReader.ReadString();
			//guessing flags, library chunk never seems to flags set.
			var unknown0 = chunkReader.ReadUInt32();
			Debug.Assert(unknown0 == 0, "Unexpected value for LibHeaderChunk.Unknown0");
			var functionCount = chunkReader.ReadUInt32();
			//Contains function strings and function flags
			var functionInfoOffset = chunkReader.ReadUInt32();
			var functionInfoReader = reader.CopyAtOffset((int)functionInfoOffset);
			for(int i = 0; i < functionCount; i++)
			{
				// is 0 for lib_4_0, lib_4_1, lib_5_0
				// is 1 for lib_4_0_level_9_1_vs_only, lib_4_0_level_9_3_vs_only
				// is 2 for lib_4_0_level_9_1_ps_only, lib_4_0_level_9_3_ps_only
				// is 3 for lib_4_0_level_9_1, lib_4_0_level_9_3
				var mode = (ProfileMode)functionInfoReader.ReadUInt32();
				var functionNameOffset = functionInfoReader.ReadUInt32();
				var functionNameReader = reader.CopyAtOffset((int)functionNameOffset);
				var name = functionNameReader.ReadString();
				result.FunctionDescs.Add(new LibraryDesc(name, mode));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("// Library:  flags 0, {0} functions: ", FunctionDescs.Count()));
			for(int i = 0; i < FunctionDescs.Count; i++)
			{
				sb.AppendLine(string.Format("//   {0}  {1}",
					i, FunctionDescs[i].Name));
			}
			sb.AppendLine("//");
			sb.AppendLine($"// Created by:  {CreatorString}");
			return sb.ToString();
		}
	}
}
