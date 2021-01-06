using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class HSInfo : ValidationInfo
	{
		internal override int StructSize => 16;
		public uint InputControlPointCount { get; private set; }
		public uint OutputControlPointCount { get; private set; }
		public TessellatorDomain TessellatorDomain { get; private set; }
		public TessellatorOutputPrimitive TessellatorOutputPrimitive { get; private set; }
		public static HSInfo Parse(BytecodeReader reader)
		{
			return new HSInfo()
			{
				InputControlPointCount = reader.ReadUInt32(),
				OutputControlPointCount = reader.ReadUInt32(),
				TessellatorDomain = (TessellatorDomain)reader.ReadUInt32(),
				TessellatorOutputPrimitive = (TessellatorOutputPrimitive)reader.ReadUInt32(),
			};
		}
	}
}
