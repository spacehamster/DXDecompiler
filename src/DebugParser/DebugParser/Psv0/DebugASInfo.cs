namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugASInfo : DebugValidationInfo
	{
		public uint PayloadSizeInBytes { get; private set; }
		internal override int StructSize => 4;
		public static DebugASInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugASInfo()
			{
				PayloadSizeInBytes = reader.ReadUInt32("PayloadSizeInBytes")
			};
		}
	}
}
