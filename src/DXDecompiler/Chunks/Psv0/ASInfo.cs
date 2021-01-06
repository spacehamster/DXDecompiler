using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class ASInfo : ValidationInfo
	{
		public uint PayloadSizeInBytes { get; private set; }
		internal override int StructSize => 4;
		public static ASInfo Parse(BytecodeReader reader)
		{
			return new ASInfo()
			{
				PayloadSizeInBytes = reader.ReadUInt32()
			};
		}
	}
}
