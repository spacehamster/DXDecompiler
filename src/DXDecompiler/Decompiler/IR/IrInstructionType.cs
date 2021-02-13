namespace DXDecompiler.Decompiler.IR
{
	public enum IrInstructionType
	{
		IntrinsicCall,
		IntrinsicCallNoDest,
		MemberCall,
		BinaryOp,
		ObjectCall,
		ObjectCallNoDest,
		SampleCall,
		ControlFlow,
		Misc
	}
}
