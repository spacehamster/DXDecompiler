using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class InstructionWriter : BaseWriter
	{
		public InstructionWriter(DecompileContext context) : base(context)
		{
		}

		public void WriteInstruction(IrInstruction instruction)
		{
			switch(instruction.Opcode.GetInstructionType())
			{
				case IrInstructionType.IntrinsicCall:
					WriteCallInstruction(instruction);
					break;
				case IrInstructionType.IntrinsicCallNoDest:
					WriteCallInstructionNoDest(instruction);
					break;
				case IrInstructionType.BinaryOp:
					WriteBinaryOp(instruction);
					break;
				case IrInstructionType.SampleCall:
					WriteSampleCall(instruction);
					break;
				case IrInstructionType.ObjectCall:
					WriteObjectCall(instruction);
					break;
				case IrInstructionType.ObjectCallNoDest:
					WriteObjectCallNoDest(instruction);
					break;
				case IrInstructionType.ControlFlow:
					WriteControlFlow(instruction);
					break;
				default:
					switch(instruction.Opcode)
					{
						case IrInstructionOpcode.Comment:
							WriteIndent();
							WriteLineFormat("// Comment: {0}", instruction.AsmDebug);
							break;
						case IrInstructionOpcode.NotImplemented:
							WriteIndent();
							WriteLineFormat("// Not Implemented: {0}", instruction.AsmDebug);
							break;
						case IrInstructionOpcode.Dfma:
						case IrInstructionOpcode.IMad:
						case IrInstructionOpcode.Mad:
							WriteMad(instruction);
							break;
						case IrInstructionOpcode.Mov:
							WriteMov(instruction);
							break;
						default:
							WriteIndent();
							WriteLineFormat("// Not Implemented [{0}]: {1}", instruction.Opcode, instruction.AsmDebug);
							break;
					}
					break;
			}
		}
		void WriteCallInstruction(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			WriteFormat(" = {0}(", name);
			for(int i = 1; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteCallInstructionNoDest(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			WriteFormat("{0}(", name);
			for(int i = 0; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteBinaryOp(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(" {0} ", name);
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteObjectCall(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(".{0}(", name);
			for(int i = 2; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteSampleCall(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			WriteFormat(".{0}(", name);
			for(int i = 3; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				Write(", ");
			}
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteObjectCallNoDest(IrInstruction instruction)
		{
			var name = instruction.Opcode.GetDescription();
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			WriteFormat(".{0}(", name);
			for(int i = 1; i < instruction.Operands.Count; i++)
			{
				Context.OperandWriter.WriteOperand(instruction.Operands[i]);
				if(i < instruction.Operands.Count - 1)
				{
					Write(", ");
				}
			}
			Write(");");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteControlFlow(IrInstruction instruction)
		{
			switch(instruction.Opcode)
			{
				case IrInstructionOpcode.EndIf:
				case IrInstructionOpcode.EndLoop:
				case IrInstructionOpcode.EndSwitch:
					DecreaseIndent();
					break;
			}
			WriteIndent();
			switch(instruction.Opcode)
			{
				case IrInstructionOpcode.MovC:
				case IrInstructionOpcode.BreakC:
					Write("if(");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write(") ");
					break;
			}
			switch(instruction.Opcode)
			{
				case IrInstructionOpcode.MovC:
					Context.OperandWriter.WriteOperand(instruction.Operands[2]);
					Write(" = ");
					Context.OperandWriter.WriteOperand(instruction.Operands[3]);
					Write(";");
					break;
				case IrInstructionOpcode.Loop:
					Write("while(true){");
					IncreaseIndent();
					break;
				case IrInstructionOpcode.If:
					Write("if(?){");
					IncreaseIndent();
					break;
				case IrInstructionOpcode.Switch:
					Write("switch(");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write("){");
					IncreaseIndent();
					break;
				case IrInstructionOpcode.Case:
					Write("case ");
					Context.OperandWriter.WriteOperand(instruction.Operands[0]);
					Write(":");
					break;
				case IrInstructionOpcode.Default:
					Write("default:");
					break;
				case IrInstructionOpcode.BreakC:
				case IrInstructionOpcode.Break:
					Write("break;");
					break;
				case IrInstructionOpcode.EndIf:
				case IrInstructionOpcode.EndLoop:
				case IrInstructionOpcode.EndSwitch:
					Write("}");
					break;
				case IrInstructionOpcode.Ret:
					Write("return;");
					break;
				default:
					WriteLineFormat("// Not Implemented {0}", instruction.AsmDebug);
					return;
			}
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteMov(IrInstruction instruction)
		{
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
		void WriteMad(IrInstruction instruction)
		{
			WriteIndent();
			Context.OperandWriter.WriteOperand(instruction.Operands[0]);
			Write(" = ");
			Context.OperandWriter.WriteOperand(instruction.Operands[1]);
			WriteFormat(" * ");
			Context.OperandWriter.WriteOperand(instruction.Operands[2]);
			WriteFormat(" + ");
			Context.OperandWriter.WriteOperand(instruction.Operands[3]);
			Write(";");
			WriteLineFormat(" // {0}", instruction.AsmDebug);
		}
	}
}
