using System;

namespace SlimShader.Chunks.RTS0
{
	/// <summary>
	/// Flags the CBV, SRV and UAV entries in the root signature
	/// Based on D3D12_ROOT_DESCRIPTOR_FLAGS.
	/// </summary>
	[Flags]
	public enum RootDescriptorFlags
	{
		[Description("NONE")]
		None = 0,
		[Description("DATA_VOLATILE")]
		DataVolatile = 0x2,
		[Description("DATA_STATIC_WHILE_SET_AT_EXECUTE")]
		DataStaticWhileSetAtExecute = 0x4,
		[Description("DATA_STATIC")]
		DataStatic = 0x8
	}
}