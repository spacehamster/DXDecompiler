namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrDebugOperand : IrOperand
	{
		public IrDebugOperand(string debug)
		{
			DebugText = debug;
		}
	}
}
