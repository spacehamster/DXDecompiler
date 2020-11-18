using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using System.Linq;

namespace DXDecompiler.DX9Shader.Asm
{
	public class PreshaderAsmWriter : DecompileWriter
	{
		public ShaderModel Preshader;
		public PreshaderAsmWriter(ShaderModel preshader)
		{
			Preshader = preshader;
		}
		public static string Disassemble(ShaderModel preshader)
		{
			var asmWriter = new PreshaderAsmWriter(preshader);
			return asmWriter.Decompile();
		}
		protected override void Write()
		{
			WriteLine("preshader");
			foreach (var instruction in Preshader.Fxlc.Tokens)
			{
				WriteInstruction(instruction);
			}
		}
		private void WriteInstruction(FxlcToken instruction)
		{
			var operands = instruction.Operands
				.Select(o => FormatOperand(o));
			WriteLine("{0} {1}",
				instruction.Opcode.ToString().ToLower(),
				string.Join(", ", operands));
		}
		string FormatOperand(FxlcOperand operand)
		{
			return operand.ToString(Preshader.ConstantTable, Preshader.Cli);
		}
	}
}
