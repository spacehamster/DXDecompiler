using DXDecompiler.DX9Shader.Bytecode.Ctab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.Bytecode.Fxlvm
{
	/// <summary>
	/// Mainly used to execute index shaders and select the default-displayed shader in effect disassembly
	/// </summary>
	class Fx9FxlVm
	{
		private abstract class VmOperand
		{
			public int Start { get; }
			public int Count { get; }

			public VmOperand(FxlcOperand operand)
			{
				Start = (int)operand.OpIndex;
				Count = (int)operand.ComponentCount;
			}
			public abstract void Assign(IEnumerable<float> values);
			public abstract IEnumerable<float> Retrieve();
		}

		private readonly Dictionary<int, float> _expressions = new();
		private readonly Dictionary<int, float> _temporaries = new();
		private readonly List<FxlcToken> _tokens;
		private readonly ConstantTable _ctab;
		private readonly CliToken _cli;

		public static List<float> Execute(ShaderModel shader)
		{
			return new Fx9FxlVm(shader.Fxlc, shader.ConstantTable, shader.Cli).Execute();
		}

		private Fx9FxlVm(FxlcBlock block, ConstantTable ctab, CliToken cli)
		{
			_tokens = block.Tokens;
			_ctab = ctab;
			_cli = cli;
		}

		private VmOperand MakeOperand(FxlcOperand operand)
		{
			return operand.OpType switch
			{
				FxlcOperandType.Expr => new TempOperand(operand, _expressions),
				FxlcOperandType.Temp => new TempOperand(operand, _temporaries),
				FxlcOperandType.Variable => new CtabOperand(operand, _ctab),
				FxlcOperandType.Literal => new LiteralOperand(operand, _cli),
				_ => throw new NotImplementedException()
			};
		}

		private List<float> Execute()
		{
			foreach(var token in _tokens)
			{
				var destination = MakeOperand(token.Operands[0]);

				switch(token.Opcode)
				{
					case FxlcOpcode.Mov:
						destination.Assign(MakeOperand(token.Operands[1]).Retrieve());
						break;
					case FxlcOpcode.Add:
						var a = MakeOperand(token.Operands[1]).Retrieve();
						var b = MakeOperand(token.Operands[2]).Retrieve();
						destination.Assign(a.Zip(b, (a, b) => a + b));
						break;
					default:
						throw new NotImplementedException(token.Opcode.ToString());
				}

			}
			var result = new List<float>();
			for(var i = 0; i < 4; ++i)
			{
				_expressions.TryGetValue(i, out var value);
				result.Add(value);
			}
			return result;
		}

		private class TempOperand : VmOperand
		{
			private readonly Dictionary<int, float> _source;

			public TempOperand(FxlcOperand operand, Dictionary<int, float> source) : base(operand)
			{
				_source = source;
			}

			public override void Assign(IEnumerable<float> values)
			{
				var list = values.ToList();
				var end = Start + Count;
				for(var i = Start; i < end; ++i)
				{
					_source[i] = list[i % list.Count];
				}
			}

			public override IEnumerable<float> Retrieve()
			{
				var end = Start + Count;
				for(var i = Start; i < end; ++i)
				{
					_source.TryGetValue(i, out var value);
					yield return value;
				}
			}
		}

		private class CtabOperand : VmOperand
		{
			private readonly ConstantTable _source;
			private readonly RegisterSet _registerSet;

			public CtabOperand(FxlcOperand operand, ConstantTable table) : base(operand)
			{
				_source = table;
				// TODO
				_registerSet = RegisterSet.Float4;
				if(operand.IsArray != 0)
				{
					throw new NotImplementedException();
				}
			}

			public override void Assign(IEnumerable<float> values) => throw new NotSupportedException();

			public override IEnumerable<float> Retrieve()
			{
				List<float> current = null;
				int offset = 0;
				var end = Start + Count;
				for(var i = Start; i < end; ++i)
				{
					if(current == null || (offset + i) < current.Count)
					{
						foreach(var x in _source.ConstantDeclarations)
						{
							if(x.RegisterSet != _registerSet)
							{
								continue;
							}
							var constantDecarationStart = x.RegisterIndex * 4;
							var constantValueCount = x.RegisterCount * 4;
							if(i < constantDecarationStart || i >= (constantDecarationStart + constantValueCount))
							{
								continue;
							}
							if(x.DefaultValueWithPadding.Count != constantValueCount)
							{
								throw new InvalidOperationException();
							}
							offset = -constantDecarationStart;
							current = x.DefaultValueWithPadding;
						}
					}

					yield return current[offset + i];
				}
			}
		}

		private class LiteralOperand : VmOperand
		{
			public IEnumerable<float> Literals { get; }

			public LiteralOperand(FxlcOperand operand, CliToken cli) : base(operand)
			{
				Literals = Enumerable.Repeat(cli.GetLiteralAsFloat(operand.OpIndex), (int)operand.ComponentCount);	
			}

			public override void Assign(IEnumerable<float> values) => throw new NotSupportedException();

			public override IEnumerable<float> Retrieve() => Literals;
		}
	}
}
