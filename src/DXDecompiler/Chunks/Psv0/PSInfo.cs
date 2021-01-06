using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class PSInfo : ValidationInfo
	{
		public byte DepthOutput { get; private set; }
		public byte SampleFrequency { get; private set; }
		internal override int StructSize => 2;
		public static PSInfo Parse(BytecodeReader reader)
		{
			return new PSInfo()
			{
				DepthOutput = reader.ReadByte(),
				SampleFrequency = reader.ReadByte()
			};
		}
	}
}
