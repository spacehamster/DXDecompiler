namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugMSInfo : DebugValidationInfo
	{
		public uint GroupSharedBytesUsed { get; private set; }
		public uint GroupSharedBytesDependentOnViewID { get; private set; }
		public uint PayloadSizeInBytes { get; private set; }
		public ushort MaxOutputVertices { get; private set; }
		public ushort MaxOutputPrimitives { get; private set; }
		internal override int StructSize => 16;
		public static DebugMSInfo Parse(DebugBytecodeReader reader)
		{
			return new DebugMSInfo()
			{
				GroupSharedBytesUsed = reader.ReadUInt32("GroupSharedBytesUsed"),
				GroupSharedBytesDependentOnViewID = reader.ReadUInt32("GroupSharedBytesDependentOnViewID"),
				PayloadSizeInBytes = reader.ReadUInt32("PayloadSizeInBytes"),
				MaxOutputVertices = reader.ReadUInt16("MaxOutputVertices"),
				MaxOutputPrimitives = reader.ReadUInt16("MaxOutputPrimitives"),
			};
		}
	}
}
