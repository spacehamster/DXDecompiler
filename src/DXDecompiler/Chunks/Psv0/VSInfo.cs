using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class VSInfo : ValidationInfo
	{
		public bool OutputPositionPresent { get; private set; }
		internal override int StructSize => 1;
		public static VSInfo Parse(BytecodeReader reader)
		{
			return new VSInfo()
			{
				OutputPositionPresent = reader.ReadByte() != 0
			};
		}
	}
}
