namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrConstantBufferOperand : IrOperand
	{
		public uint? BufferId;
		public uint BuffereBindPoint;
		public uint BufferOffset;

		public IrConstantBufferOperand(uint bufferId, uint bufferBindPoint, uint bufferOffset)
		{
			BufferId = bufferId;
			BuffereBindPoint = bufferBindPoint;
			BufferOffset = bufferOffset;
		}
		public IrConstantBufferOperand(uint bufferBindPoint, uint bufferOffset)
		{
			BuffereBindPoint = bufferBindPoint;
			BufferOffset = bufferOffset;
		}
	}
}
