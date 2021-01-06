namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugVSInfo : DebugValidationInfo
	{
		public byte OutputPositionPresent { get; private set; }
		internal override int StructSize => 1;
		public static DebugVSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugVSInfo()
			{
				OutputPositionPresent = reader.ReadByte("OutputPositionPresent"),
			};
		}
	}
}
