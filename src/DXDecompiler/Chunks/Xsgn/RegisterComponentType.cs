namespace DXDecompiler.Chunks.Xsgn
{
	public enum RegisterComponentType
	{
		Unknown = 0,

		[Description("uint")]
		UInt32 = 1,

		[Description("int")]
		SInt32 = 2,

		[Description("float")]
		Float32 = 3,

		[Description("uint16")]
		UInt16 = 4,

		[Description("int16")]
		SInt16 = 5,

		[Description("fp16")]
		Float16 = 6,

		[Description("uint64")]
		UInt64 = 7,

		[Description("int64")]
		SInt64 = 8,

		[Description("double")]
		Float64 = 9,
	}
}