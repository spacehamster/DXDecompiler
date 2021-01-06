namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugPSInfo : DebugValidationInfo
	{
		public byte DepthOutput { get; private set; }
		public byte SampleFrequency { get; private set; }
		internal override int StructSize => 2;
		public static DebugPSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugPSInfo()
			{
				DepthOutput = reader.ReadByte("DepthOutput"),
				SampleFrequency = reader.ReadByte("SampleFrequency")
			};
		}
	}
}
