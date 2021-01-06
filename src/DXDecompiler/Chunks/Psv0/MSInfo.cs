using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class MSInfo : ValidationInfo
	{
		public uint GroupSharedBytesUsed { get; private set; }
		public uint GroupSharedBytesDependentOnViewID { get; private set; }
		public uint PayloadSizeInBytes { get; private set; }
		public ushort MaxOutputVertices { get; private set; }
		public ushort MaxOutputPrimitives { get; private set; }
		internal override int StructSize => 16;
		public static MSInfo Parse(BytecodeReader reader)
		{
			return new MSInfo()
			{
				GroupSharedBytesUsed = reader.ReadUInt32(),
				GroupSharedBytesDependentOnViewID = reader.ReadUInt32(),
				PayloadSizeInBytes = reader.ReadUInt32(),
				MaxOutputVertices = reader.ReadUInt16(),
				MaxOutputPrimitives = reader.ReadUInt16(),
			};
		}
	}
}
