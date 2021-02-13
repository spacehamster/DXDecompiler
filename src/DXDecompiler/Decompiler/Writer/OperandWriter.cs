using DXDecompiler.Decompiler.IR.Operands;
using System;

namespace DXDecompiler.Decompiler.Writer
{
	public class OperandWriter : BaseWriter
	{
		public OperandWriter(DecompileContext context) : base(context)
		{

		}

		public void WriteOperand(IrOperand operand)
		{
			if(operand is IrTempOperand temp)
			{
				WriteOperand(temp);
			}
			else if(operand is IrInputOperand input)
			{
				WriteOperand(input);
			}
			else if(operand is IrOutputOperand output)
			{
				WriteOperand(output);
			}
			else if(operand is IrResourceOperand resource)
			{
				WriteOperand(resource);
			}
			else if(operand is IrConstantOperand constant)
			{
				WriteOperand(constant);
			}
			else if(operand is IrConstantBufferOperand constantBuffer)
			{
				WriteOperand(constantBuffer);
			}
			else if(operand is IrDebugOperand unknownOperand)
			{
				Write($"DBG_{unknownOperand.DebugText}");
			}
			else
			{
				throw new InvalidOperationException($"Unexpected operand {operand}");
			}
		}
		public void WriteOperand(IrTempOperand operand)
		{
			Write($"temp{operand.Index}");
		}
		public void WriteOperand(IrConstantBufferOperand operand)
		{
			if(operand.BufferId != null)
			{
				Write($"CB{operand.BufferId}[{operand.BuffereBindPoint}][{operand.BufferOffset}]");
			}
			else
			{
				Write($"CB{operand.BuffereBindPoint}[{operand.BufferOffset}]");
			}
		}
		public void WriteOperand(IrInputOperand operand)
		{
			Write($"input{operand.Index}");
		}
		public void WriteOperand(IrOutputOperand operand)
		{
			Write($"output{operand.Index}");
		}
		public void WriteOperand(IrResourceOperand operand)
		{
			Write($"{operand.ResourceType}{operand.IndexId}[{operand.RangeId}]");
		}
		public void WriteOperand(IrConstantOperand operand)
		{
			Write($"l({operand.Value})");
		}
	}
}
