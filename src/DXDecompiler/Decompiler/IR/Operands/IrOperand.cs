namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrOperand
	{
		public string DebugText;
		public IrOperand()
		{
		}

		public override string ToString()
		{
			return DebugText ?? "";
		}
	}
}
