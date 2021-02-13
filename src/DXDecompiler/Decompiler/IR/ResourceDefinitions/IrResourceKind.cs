namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public enum IrResourceKind
	{
		Invalid = 0,
		Texture1D,
		Texture2D,
		Texture2DMS,
		Texture3D,
		TextureCube,
		Texture1DArray,
		Texture2DArray,
		Texture2DMSArray,
		TextureCubeArray,
		TypedBuffer,
		RawBuffer,
		StructuredBuffer,
		CBuffer,
		Sampler,
		TBuffer,
		RTAccelerationStructure,
		FeedbackTexture2D,
		FeedbackTexture2DArray,
		NumEntries,
	}
}
