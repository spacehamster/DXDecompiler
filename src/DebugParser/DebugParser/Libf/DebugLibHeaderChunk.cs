using SlimShader.Chunks;
using SlimShader.Chunks.Libf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Libf
{
	public class DebugLibHeaderChunk : DebugBytecodeChunk
	{
		public string CreatorString { get; private set; }
		public List<LibraryDesc> FunctionDescs { get; private set; }
		public uint Flags { get; private set; }
		public DebugLibHeaderChunk()
		{
			FunctionDescs = new List<LibraryDesc>();
		}
		public static DebugLibHeaderChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var chunkReader = reader.CopyAtCurrentPosition("chunkReader", reader);
			var result = new DebugLibHeaderChunk();
			var unknown1 = chunkReader.ReadUInt32("LibHeader.Unknown1");
			var creatorStringOffset = chunkReader.ReadUInt32("CreatorStringOffset");
			result.Flags = chunkReader.ReadUInt32("Flags");
			var functionCount = chunkReader.ReadUInt32("functionCount");
			var functionInfoOffset = chunkReader.ReadUInt32("functionInfoOffset");
			var creatorStringReader = chunkReader.CopyAtOffset("creatorStringReader", chunkReader, (int)creatorStringOffset);
			result.CreatorString = creatorStringReader.ReadString("CreatorString");
			var functionInfoReader = reader.CopyAtOffset("functionInfoReader", chunkReader, (int)functionInfoOffset);
			for (int i = 0; i < functionCount; i++)
			{
				// is 0 for lib_4_0, lib_4_1, lib_5_0
				// is 1 for lib_4_0_level_9_1_vs_only, lib_4_0_level_9_3_vs_only
				// is 2 for lib_4_0_level_9_1_ps_only, lib_4_0_level_9_3_ps_only
				// is 3 for lib_4_0_level_9_1, lib_4_0_level_9_3
				var mode = (ProfileMode)functionInfoReader.ReadUInt32("mode");
				var functionNameOffset = functionInfoReader.ReadUInt32("functionNameOffset");
				var functionNameReader = reader.CopyAtOffset("functionNameReader", reader, (int)functionNameOffset);
				var name = functionNameReader.ReadString("name");
				result.FunctionDescs.Add(new LibraryDesc(name, mode));
			}
			return result;
		}
	}
}
