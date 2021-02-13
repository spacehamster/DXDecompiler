namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrOutputOperand : IrOperand
	{
		public uint Index;
		public uint RowIndex;
		public uint ColIndex;
		public IrOutputOperand(uint index, uint rowIndex, uint colIndex)
		{
			Index = index;
			RowIndex = rowIndex;
			ColIndex = colIndex;
		}
	}
}
