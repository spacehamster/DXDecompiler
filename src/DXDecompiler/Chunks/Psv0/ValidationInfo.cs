namespace DXDecompiler.Chunks.Psv0
{
	public abstract class ValidationInfo
	{
		internal const int UnionSize = 16;
		internal virtual int StructSize { get; }
	}
}
