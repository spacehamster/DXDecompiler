namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugDSInfo : DebugValidationInfo
	{
		internal override int StructSize => 9;
		public uint InputControlPointCount { get; private set; }
		public byte OutputPositionPresent { get; private set; }
		public uint TessellatorDomain { get; private set; }
		public static DebugDSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugDSInfo()
			{
				InputControlPointCount = reader.ReadUInt32("InputControlPointCount"),
				OutputPositionPresent = reader.ReadByte("OutputPositionPresent"),
				TessellatorDomain = reader.ReadUInt32("TessellatorDomain"),
			};
		}
	}
}
