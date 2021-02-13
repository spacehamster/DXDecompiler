namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrTempOperand : IrOperand
	{
		public uint Index;
		public IrTempOperand(uint index)
		{
			Index = index;
		}
	}
}
