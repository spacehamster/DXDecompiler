namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrConstantOperand : IrOperand
	{
		public Number4 Value;
		public IrConstantOperand(double value)
		{
			Value = new Number4(value, 0);
		}
		public IrConstantOperand(long value)
		{
			Value = new Number4(value, 0);
		}
	}
}
