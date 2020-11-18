using DXDecompiler.Util;
using System.Diagnostics;

namespace DXDecompiler.Chunks.Aon9
{
	public class SamplerMapping
	{
		public byte TargetSampler { get; private set; }
		public byte SourceSampler { get; private set; }
		public byte SourceResource { get; private set; }

		public static SamplerMapping Parse(BytecodeReader reader)
		{
			var result = new SamplerMapping();
			result.SourceResource = reader.ReadByte();
			result.SourceSampler = reader.ReadByte();
			result.TargetSampler = reader.ReadByte();
			var padding = reader.ReadByte();
			Debug.Assert(padding == 0, "Padding is 0");
			return result;
		}

		public override string ToString()
		{
			return string.Format("// s{0, -13} s{1, -14} t{2, -16}",
					TargetSampler, SourceSampler, SourceResource);
		}
	}
}
