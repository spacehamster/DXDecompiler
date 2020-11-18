namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Root Signature Static Sampler Texture Address Mode
	/// Based on D3D12_TEXTURE_ADDRESS_MODE.
	/// </summary>
	public enum TextureAddressMode
	{
		[Description("TEXTURE_ADDRESS_WRAP", ChunkType.Rts0)]
		[Description("WRAP")]
		Wrap = 1,
		[Description("TEXTURE_ADDRESS_MIRROR", ChunkType.Rts0)]
		[Description("MIRROR")]
		Mirror = 2,
		[Description("TEXTURE_ADDRESS_CLAMP", ChunkType.Rts0)]
		[Description("CLAMP")]
		Clamp = 3,
		[Description("TEXTURE_ADDRESS_BORDER", ChunkType.Rts0)]
		[Description("BORDER")]
		Border = 4,
		[Description("TEXTURE_ADDRESS_MIRROR_ONCE", ChunkType.Rts0)]
		[Description("MIRROR_ONCE")]
		MirrorOnce = 5
	}
}
