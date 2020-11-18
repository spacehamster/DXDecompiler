using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Shex
{
	public enum ConstantBufferAccessPattern
	{
		[Description("immediateIndexed")]
		ImmediateIndexed = 0,

		[Description("dynamicIndexed")]
		DynamicIndexed = 1
	}
}