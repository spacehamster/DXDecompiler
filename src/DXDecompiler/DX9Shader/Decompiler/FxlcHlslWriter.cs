using DXDecompiler.DX9Shader.Bytecode;
using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.Decompiler
{
	class FxlcHlslWriter : DecompileWriter
	{
		protected ShaderModel Shader { get; }
		protected ConstantTable Ctab { get; }
		protected CliToken Cli { get; }

		public FxlcHlslWriter(ShaderModel shader)
		{
			Shader = shader;
			Ctab = shader.ConstantTable;
			Cli = shader.Cli;
		}

		protected void WriteTemporaries()
		{
			var temporaryRegisters = new SortedSet<uint>();
			foreach(var operands in Shader.Fxlc.Tokens.SelectMany(t => t.Operands))
			{
				if(operands.IsArray != 0)
				{
					if(operands.ArrayType == FxlcOperandType.Temp)
					{
						// will this ever happen?
						throw new NotImplementedException();
					}
				}
				if(operands.OpType == FxlcOperandType.Temp)
				{
					temporaryRegisters.Add(operands.OpIndex);
				}
			}
			foreach(var tempIndex in temporaryRegisters)
			{
				WriteIndent();
				WriteLine($"float4 temp{tempIndex};");
			}
		}

		protected void WriteInstructions(HashSet<uint> ctabOverride = null)
		{
			foreach(var token in Shader.Fxlc.Tokens)
			{
				Write(token, ctabOverride);
			}
		}

		void Write(FxlcToken token, HashSet<uint> ctabOverride = null)
		{
			WriteIndent();
			WriteLine($"// {token.ToString(Shader.Cli)}");
			switch(token.Opcode)
			{
				default:
					throw new NotImplementedException(token.Opcode.ToString());
				case FxlcOpcode.Rcp:
					WriteFunction("1.0f / ", token, ctabOverride);
					break;
				case FxlcOpcode.Mov:
					WriteAssignment(token, ctabOverride, "{0}", token.Operands[1].FormatOperand(Cli, Ctab, ctabOverride));
					break;
				case FxlcOpcode.Neg:
					WriteAssignment(token, ctabOverride, "-{0}", token.Operands[1].FormatOperand(Cli, Ctab, ctabOverride));
					break;
				case FxlcOpcode.Frc:
					WriteFunction("frac", token, ctabOverride);
					break;
				case FxlcOpcode.Exp:
					WriteFunction("exp", token, ctabOverride);
					break;
				case FxlcOpcode.Log:
					WriteFunction("log", token, ctabOverride);
					break;
				case FxlcOpcode.Rsq:
					WriteFunction("1.0f / sqrt", token, ctabOverride);
					break;
				case FxlcOpcode.Sin:
					WriteFunction("sin", token, ctabOverride);
					break;
				case FxlcOpcode.Cos:
					WriteFunction("cos", token, ctabOverride);
					break;
				case FxlcOpcode.Asin:
					WriteFunction("asin", token, ctabOverride);
					break;
				case FxlcOpcode.Acos:
					WriteFunction("acos", token, ctabOverride);
					break;
				case FxlcOpcode.Atan:
					WriteFunction("atan", token, ctabOverride);
					break;
				case FxlcOpcode.Atan2:
					WriteFunction("atan2", token, ctabOverride);
					break;
				case FxlcOpcode.Sqrt:
					WriteFunction("sqrt", token, ctabOverride);
					break;
				case FxlcOpcode.Ineg:
					WriteFunction("~int", token, ctabOverride);
					break;
				case FxlcOpcode.Imax:
					WriteFunction("(int)max", token, ctabOverride);
					break;
				case FxlcOpcode.Not:
					WriteFunction("!", token, ctabOverride);
					break;
				case FxlcOpcode.Utof:
					WriteFunction("utof", token, ctabOverride);
					break;
				case FxlcOpcode.Ftoi:
					WriteFunction("ftoi", token, ctabOverride);
					break;
				case FxlcOpcode.Ftou:
					WriteFunction("ftou", token, ctabOverride);
					break;
				case FxlcOpcode.Btoi:
					WriteFunction("btoi", token, ctabOverride);
					break;
				case FxlcOpcode.Round:
					WriteFunction("round", token, ctabOverride);
					break;
				case FxlcOpcode.Floor:
					WriteFunction("floor", token, ctabOverride);
					break;
				case FxlcOpcode.Ceil:
					WriteFunction("ceil", token, ctabOverride);
					break;
				case FxlcOpcode.Min:
					WriteFunction("min", token, ctabOverride);
					break;
				case FxlcOpcode.Max:
					WriteFunction("max", token, ctabOverride);
					break;
				case FxlcOpcode.Dot:
					WriteFunction("dot", token, ctabOverride);
					break;
				case FxlcOpcode.Add:
					WriteInfix("+", token, ctabOverride);
					break;
				case FxlcOpcode.Mul:
					WriteInfix("*", token, ctabOverride);
					break;
				case FxlcOpcode.Lt:
					WriteInfix("<", token, ctabOverride);
					break;
				case FxlcOpcode.Ge:
					WriteInfix(">=", token, ctabOverride);
					break;
				case FxlcOpcode.Cmp:
					WriteAssignment(token, ctabOverride, "({0} >= 0 ? {1} : {2})",
						token.Operands.Skip(1).Select(o => o.FormatOperand(Cli, Ctab, ctabOverride)).ToArray());
					break;
			}
		}
		void WriteAssignment(FxlcToken token, HashSet<uint> ctabOverride, string format, params string[] args)
		{
			var destination = token.Operands[0];
			if(destination.ArrayIndex % 4 != 0)
			{
				// I'm not sure if write masks apply to preshaders as well...
				throw new NotImplementedException();
			}
			WriteIndent();
			WriteLine("{0} = {1};", destination.FormatOperand(Cli, Ctab, ctabOverride), string.Format(format, args));
			if(destination.IsArray != 0)
			{
				throw new NotImplementedException();
			}
			if(destination.OpType == FxlcOperandType.Expr)
			{
				ctabOverride?.Add(destination.OpIndex / 4);
			}
		}
		void WriteInfix(string op, FxlcToken token, HashSet<uint> ctabOverride)
		{
			WriteAssignment(token, ctabOverride, "{0} {1} {2}",
				token.Operands[1].FormatOperand(Cli, Ctab, ctabOverride),
				op,
				token.Operands[2].FormatOperand(Cli, Ctab, ctabOverride));
		}
		void WriteFunction(string func, FxlcToken token, HashSet<uint> ctabOverride)
		{
			var operands = token.Operands
				.Skip(1)
				.Select(o => o.FormatOperand(Cli, Ctab, ctabOverride));
			WriteAssignment(token, ctabOverride, "{0}({1})", func, string.Join(", ", operands));
		}
	}
}
