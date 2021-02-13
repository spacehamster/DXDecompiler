namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public enum IrShaderVariableType
	{
		Void = 0,
		I1, I16, U16, I32, U32, I64, U64,
		F16, F32, F64,
		SNormF16, UNormF16, SNormF32, UNormF32, SNormF64, UNormF64,
		PackedS8x32, PackedU8x32,
		InterfacePointer
	}
}
