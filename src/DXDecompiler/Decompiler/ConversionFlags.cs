namespace DXDecompiler.Decompiler
{
	internal enum ConversionFlags
	{
		None = 0x0,
		Integer = 0x1,
		NameOnly = 0x2,
		DeclrationName = 0x4,
		Destination = 0x8,
		UnsignedInteger=0x10,
		Double=0x20,
		BitCastToFloat=0x40,
		BitCastToInt=0x80,
		BitCastToUint=0x100,
		ExpandToVec2=0x200,
		ExpandToVec3 = 0x400,
		ExpandToVec4 = 0x800,
	}
}
