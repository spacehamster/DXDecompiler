namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugGSInfo : DebugValidationInfo
	{
		internal override int StructSize => 13;
		public uint InputPrimitive { get; private set; }
		public uint OutputTopology { get; private set; }
		public uint OutputStreamMask { get; private set; }
		public byte OutputPositionPresent { get; private set; }
		public static DebugGSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugGSInfo()
			{
				InputPrimitive = reader.ReadUInt32("InputPrimitive"),
				OutputTopology = reader.ReadUInt32("OutputTopology"),
				OutputStreamMask = reader.ReadUInt32("OutputStreamMask"),
				OutputPositionPresent = reader.ReadByte("OutputPositionPresent")
			};
		}
	}
}
