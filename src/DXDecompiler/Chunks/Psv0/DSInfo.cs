using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class DSInfo : ValidationInfo
	{
		internal override int StructSize => 9;
		public uint InputControlPointCount { get; private set; }
		public bool OutputPositionPresent { get; private set; }
		public uint TessellatorDomain { get; private set; }
		public static DSInfo Parse(BytecodeReader reader)
		{
			return new DSInfo()
			{
				InputControlPointCount = reader.ReadUInt32(),
				OutputPositionPresent = reader.ReadByte() != 0,
				TessellatorDomain = reader.ReadUInt32(),
			};
		}
	}
}
