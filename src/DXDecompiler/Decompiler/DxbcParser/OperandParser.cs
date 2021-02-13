using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Decompiler.IR.Operands;

namespace DXDecompiler.Decompiler.DxbcParser
{
	public class OperandParser
	{
		public static IrOperand Parse(Operand operand)
		{
			IrOperand result;
			switch(operand.OperandType)
			{
				case Chunks.Shex.OperandType.ConstantBuffer:
					result = new IrConstantBufferOperand(0, 0, 0);
					break;
				case Chunks.Shex.OperandType.Sampler:
					result = new IrResourceOperand(IR.IrResourceType.Sampler, 0);
					break;
				case Chunks.Shex.OperandType.Resource:
					result = new IrResourceOperand(IR.IrResourceType.Texture, 0);
					break;
				case Chunks.Shex.OperandType.UnorderedAccessView:
					result = new IrResourceOperand(IR.IrResourceType.UnorderedAccessView, 0);
					break;
				default:
					result = new IrDebugOperand(operand.ToString());
					break;
			}
			result.DebugText = operand.ToString();
			return result;
		}
	}
}
