namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugHSInfo : DebugValidationInfo
	{
		internal override int StructSize => 16;
		public uint InputControlPointCount { get; private set; }
		public uint OutputControlPointCount { get; private set; }
		public uint TessellatorDomain { get; private set; }
		public uint TessellatorOutputPrimitive { get; private set; }
		public static DebugHSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugHSInfo()
			{
				InputControlPointCount = reader.ReadUInt32("InputControlPointCount"),
				OutputControlPointCount = reader.ReadUInt32("OutputControlPointCount"),
				TessellatorDomain = reader.ReadUInt32("TessellatorDomain"),
				TessellatorOutputPrimitive = reader.ReadUInt32("TessellatorOutputPrimitive"),
			};
		}
	}
}
