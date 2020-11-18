using DXDecompiler.DX9Shader.Bytecode.Declaration;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader.Bytecode.Fxlvm
{
	public class FxlcToken
	{
		public FxlcOpcode Opcode { get; private set; }
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
			var singleFirstComponent = token.DecodeValue(31, 31);

			Debug.Assert(Enum.IsDefined(typeof(FxlcOpcode), result.Opcode),
				$"Unknown FxlcOpcode {result.Opcode}");

			Debug.Assert(token.DecodeValue(3, 19) == 0,
				$"Unexpected data in FxlcToken bits 3-19 {token.DecodeValue(3, 19)}");

			var operandCount = reader.ReadUInt32();
			for(int i = 0; i < operandCount; i++)
			{
				var componentCount = i == 0 && singleFirstComponent == 1 ?
					1 : tokenComponentCount;
				result.Operands.Add(FxlcOperand.Parse(reader, componentCount));
			}
			// destination operand
			result.Operands.Insert(0, FxlcOperand.Parse(reader, tokenComponentCount));
			return result;
		}

		public string ToString(ConstantTable ctab, CliToken cli)
		{
			var operands = string.Join(", ", Operands.Select(o => o.ToString(ctab, cli)));
			return string.Format("{0} {1}",
					Opcode.ToString().ToLowerInvariant(),
					operands);
		}
		public string ToString(ConstantTable ctab, Chunks.Fxlvm.Cli4Chunk cli)
		{
			var operands = string.Join(", ", Operands.Select(o => o.ToString(ctab, cli)));
			return string.Format("{0} {1}",
					Opcode.ToString().ToLowerInvariant(),
					operands);
		}
	}
}
