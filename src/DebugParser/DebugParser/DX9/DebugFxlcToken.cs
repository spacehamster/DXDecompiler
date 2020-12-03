using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DXDecompiler.DebugParser.DX9
{
	public class DebugFxlcToken
	{
		public FxlcOpcode Type { get; private set; }
		public List<DebugFxlcOperand> Operands { get; private set; }

		public uint Token0;
		public uint OperandCount;
		public bool IsScalarInstruction;

		public DebugFxlcToken()
		{
			Operands = new List<DebugFxlcOperand>();
		}

		public static DebugFxlcToken Parse(DebugBytecodeReader reader)
		{
			var result = new DebugFxlcToken();

			var token = reader.ReadUInt32($"Token");
			var tokenComponentCount = token.DecodeValue(0, 2);
			result.Type = (FxlcOpcode)token.DecodeValue(20, 30);
			result.IsScalarInstruction = token.DecodeValue<bool>(31, 31);

			Debug.Assert(Enum.IsDefined(typeof(FxlcOpcode), result.Type),
				$"Unknown FxlcTokenType {result.Type}");

			Debug.Assert(token.DecodeValue(3, 19) == 0,
				$"Unexpected data in FxlcToken bits 3-19 {token.DecodeValue(3, 19)}");

			reader.AddNote("Token", result.Type);
			reader.AddNote("TokenComponentCount", tokenComponentCount);
			reader.AddNote("IsScalarInstruction", result.IsScalarInstruction);

			var operandCount = result.OperandCount = reader.ReadUInt32("OperandCount");
			for(int i = 0; i < operandCount; i++)
			{
				var isScalarOp = i == 0 && result.IsScalarInstruction;
				result.Operands.Add(DebugFxlcOperand.Parse(reader, tokenComponentCount, isScalarOp));
			}
			// destination operand
			result.Operands.Insert(0, DebugFxlcOperand.Parse(reader, tokenComponentCount, false));
			return result;
		}
	}
}
