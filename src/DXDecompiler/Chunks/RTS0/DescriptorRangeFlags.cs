using System;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Flags for descriptor ranges
	/// Based on D3D12_DESCRIPTOR_RANGE_FLAGS.
	/// </summary>
	[Flags]
	public enum DescriptorRangeFlags
	{
		[Description("NONE")]
		None = 0,
		[Description("DESCRIPTORS_VOLATILE")]
		DescriptorsVolatile = 0x1,
		[Description("DATA_VOLATILE")]
		DataVolatile = 0x2,
		[Description("DATA_STATIC_WHILE_SET_AT_EXECUTE")]
		DataStaticWhileSetAtExecute = 0x4,
		[Description("DATA_STATIC")]
		DataStatic = 0x8,
		[Description("DESCRIPTORS_STATIC_KEEPING_BUFFER_BOUNDS_CHECKS")]
		DescriptorsStaticKeepingBufferBoundsChecks = 0x10000
	}
}