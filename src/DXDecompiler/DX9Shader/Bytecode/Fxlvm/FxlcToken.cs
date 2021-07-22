using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DXDecompiler.DX9Shader.Bytecode.Fxlvm
{
	public class FxlcToken
	{
		public FxlcOpcode Opcode { get; private set; }
		public bool IsScalarInstruction { get; private set; }
		public List<FxlcOperand> Operands { get; private set; }

		public FxlcToken()
		{
			Operands = new List<FxlcOperand>();
		}

		public static FxlcToken Parse(BytecodeReader reader)
		{
			var result = new FxlcToken();
			var token = reader.ReadUInt32();
			var tokenComponentCount = token.DecodeValue(0, 2);
			result.Opcode = (FxlcOpcode)token.DecodeValue(20, 30);
			result.IsScalarInstruction = token.DecodeValue<bool>(31, 31);

			Debug.Assert(Enum.IsDefined(typeof(FxlcOpcode), result.Opcode),
				$"Unknown FxlcOpcode {result.Opcode}");

			Debug.Assert(token.DecodeValue(3, 19) == 0,
				$"Unexpected data in FxlcToken bits 3-19 {token.DecodeValue(3, 19)}");

			var operandCount = reader.ReadUInt32();
			for(int i = 0; i < operandCount; i++)
			{
				var isScalarOp = i == 0 && result.IsScalarInstruction;
				result.Operands.Add(FxlcOperand.Parse(reader, tokenComponentCount, isScalarOp && i == 0));
			}
			// destination operand
			result.Operands.Insert(0, FxlcOperand.Parse(reader, tokenComponentCount, false));
			return result;
		}

		/// <summary>
		/// ToString method for debugging purposes. We are not not able to properly represent
		/// the instruction's assembly without access to the ctab and cli chunks.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var operands = string.Join(", ", Operands.Select(o => o.ToString()));
			return string.Format("{0} {1}",
					Opcode.ToString().ToLowerInvariant(),
					operands);
		}

		public string ToString(CliToken cli)
		{
			var operands = string.Join(", ", Operands.Select(o => o.FormatOperand(cli, null)));
			return string.Format("{0} {1}",
					Opcode.ToString().ToLowerInvariant(),
					operands);
		}
		public string ToString(ConstantTable ctab, Chunks.Fxlvm.Cli4Chunk cli)
		{
			var operands = string.Join(", ", Operands.Select(o => o.FormatOperand(ctab, cli)));
			return string.Format("{0} {1}",
					Opcode.ToString().ToLowerInvariant(),
					operands);
		}
	}
}
