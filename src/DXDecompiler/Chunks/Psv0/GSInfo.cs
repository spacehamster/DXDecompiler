using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class GSInfo : ValidationInfo
	{
		internal override int StructSize => 13;
		public Primitive InputPrimitive { get; private set; }
		public PrimitiveTopology OutputTopology { get; private set; }
		public uint OutputStreamMask { get; private set; }
		public bool OutputPositionPresent { get; private set; }
		public static GSInfo Parse(BytecodeReader reader)
		{
			return new GSInfo()
			{
				InputPrimitive = (Primitive)reader.ReadUInt32(),
				OutputTopology = (PrimitiveTopology)reader.ReadUInt32(),
				OutputStreamMask = reader.ReadUInt32(),
				OutputPositionPresent = reader.ReadByte() != 0
			};
		}
	}
}
