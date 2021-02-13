namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrInputOperand : IrOperand
	{
		public uint Index;
		public uint RowIndex;
		public uint ColIndex;
		public IrInputOperand(uint index, uint rowIndex, uint colIndex)
		{
			Index = index;
			RowIndex = rowIndex;
			ColIndex = colIndex;
		}
	}
}
