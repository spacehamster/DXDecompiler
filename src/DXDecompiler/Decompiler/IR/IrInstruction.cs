using DXDecompiler.Decompiler.IR.Operands;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class IrInstruction
	{
		internal Chunks.Shex.Tokens.InstructionToken Token;
		public IrInstructionOpcode Opcode;
		public List<IrOperand> Operands;
		public string AsmDebug;
		public IrInstruction()
		{
			Operands = new List<IrOperand>();
		}
		public IrInstruction(Chunks.Shex.Tokens.InstructionToken token)
		{
			Token = token;
			AsmDebug = Token.ToString();
			Operands = new List<IrOperand>(
				token.Operands.Select(o => new IrDebugOperand(o.ToString())));
			Opcode = (IrInstructionOpcode)Token.Header.OpcodeType;
		}
	}
}
