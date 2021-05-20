using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.Util;
using System;
using System.Diagnostics;
using System.Linq;

namespace DXDecompiler.DX9Shader.Bytecode.Fxlvm
{
	public class FxlcOperand
	{
		public uint IsArray { get; private set; }
		public FxlcOperandType OpType { get; private set; }
		public uint OpIndex { get; private set; }
		public FxlcOperandType ArrayType { get; private set; }
		public uint ArrayIndex { get; private set; }
		public uint ComponentCount { get; private set; }

		public static FxlcOperand Parse(BytecodeReader reader, uint componentCount, bool isScalarOp)
		{
			var result = new FxlcOperand()
			{
				IsArray = reader.ReadUInt32(),
				OpType = (FxlcOperandType)reader.ReadUInt32(),
				OpIndex = reader.ReadUInt32(),
			};
			result.ComponentCount = isScalarOp && result.OpType != FxlcOperandType.Literal ? 1 : componentCount;
			Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.OpType),
				$"Unexpected FxlcOperandType OpType {result.OpType}");
			if(result.IsArray == 1)
			{
				result.ArrayType = (FxlcOperandType)reader.ReadUInt32();
				result.ArrayIndex = reader.ReadUInt32();

				Debug.Assert(Enum.IsDefined(typeof(FxlcOperandType), result.ArrayType),
					$"Unexpected FxlcOperandType ArrayType {result.ArrayType}");
			}

			return result;
		}

		private string FormatComponent(uint componentIndex, uint componentCount)
		{
			switch(componentCount)
			{
				case 0:
					return "";
				case 1:
					switch(componentIndex)
					{
						case 0:
							return ".x";
						case 1:
							return ".y";
						case 2:
							return ".z";
						case 3:
							return ".w";
						default:
							return $".UnknownIndex{componentIndex}";
					}
				case 2:
					switch(componentIndex)
					{
						case 0:
							return ".xy";
						case 1:
							return ".yz";
						case 2:
							return ".zw";
						case 3:
							return ".wx";
						default:
							return $".UnknownIndex{componentIndex}";
					}
				case 3:
					switch(componentIndex)
					{
						case 0:
							return ".xyz";
						case 1:
							return ".yzw";
						case 2:
							return ".zwx";
						case 3:
							return ".wxy";
						default:
							return $".UnknownIndex{componentIndex}";
					}
				case 4:
					switch(componentIndex)
					{
						case 0:
							return "";
						case 1:
							return ".yzwx";
						case 2:
							return ".zwxy";
						case 3:
							return ".wxyz";
						default:
							return $".UnknownIndex{componentIndex}";
					}
				default:
					return $".UnknownCount{componentCount}";
			}
		}
		private string FormatOperand(ConstantTable ctab, CliToken cli, FxlcOperandType type, uint index, out string component)
		{
			var elementIndex = index / 4;
			var componentIndex = index % 4;
			component = FormatComponent(componentIndex, ComponentCount);
			switch(type)
			{
				case FxlcOperandType.Literal:
					var literal = cli.GetLiteral(index);
					if(literal == "-0")
					{
						literal = string.Format("{0}, 0, 0, 0", literal);
					}
					else
					{
						literal = string.Join(", ", Enumerable.Repeat(cli.GetLiteral(index), (int)ComponentCount));
					}
					component = string.Empty;
					return $"({literal})";
				case FxlcOperandType.Temp:
					return $"r{elementIndex}";
				case FxlcOperandType.Variable:
					return $"c{elementIndex}";
				case FxlcOperandType.Expr:
					return $"c{elementIndex}";
				default:
					return $"unknown{elementIndex}"; ;
			}
		}
		private string FormatOperand(ConstantTable ctab, Chunks.Fxlvm.Cli4Chunk cli, FxlcOperandType type, uint index)
		{
			var elementIndex = index / 4;
			var componentIndex = index % 4;
			var component = FormatComponent(componentIndex, ComponentCount);
			switch(type)
			{
				case FxlcOperandType.Literal:
					return string.Format("({0})", cli.GetLiteral(index, ComponentCount));
				case FxlcOperandType.Temp:
					return string.Format("r{0}{1}", elementIndex, component);
				case FxlcOperandType.Variable:
					return string.Format("{0}{1}",
						ctab.GetVariable(elementIndex), component);
				case FxlcOperandType.Expr:
					if(ComponentCount == 1)
					{
						if(componentIndex == 0)
						{
							return string.Format("expr{0}", component);
						}
						else
						{
							return string.Format("expr0{0}", component);
						}
					}
					return string.Format("expr{0}", component);
				default:
					return string.Format("unknown{0}{1}", elementIndex, component);
			}
		}
		/// <summary>
		/// Format operand for FX9 preshaders
		/// </summary>
		/// <param name="ctab"></param>
		/// <param name="cli"></param>
		/// <returns></returns>
		public string FormatOperand(ConstantTable ctab, CliToken cli)
		{
			if(IsArray == 0)
			{
				return FormatOperand(ctab, cli, OpType, OpIndex, out var component) + component;
			}
			else
			{
				var arrayOperand = FormatOperand(ctab, cli, ArrayType, ArrayIndex, out var elementComponent);
				var indexOperand = FormatOperand(ctab, cli, OpType, OpIndex, out _);
				return string.Format("{0}[{1}.x]{2}", arrayOperand, indexOperand, elementComponent);
			}
		}
		/// <summary>
		/// Format operand for FX10 expressions
		/// </summary>
		/// <param name="ctab"></param>
		/// <param name="cli"></param>
		/// <returns></returns>
		public string FormatOperand(ConstantTable ctab, Chunks.Fxlvm.Cli4Chunk cli)
		{
			if(IsArray == 0)
			{
				return FormatOperand(ctab, cli, OpType, OpIndex);
			}
			else
			{
				return string.Format("{0}[{1}]",
					FormatOperand(ctab, cli, ArrayType, ArrayIndex),
					FormatOperand(ctab, cli, OpType, OpIndex));
			}
		}

		private string FormatOperand(FxlcOperandType type, uint index)
		{
			var elementIndex = index / 4;
			var componentIndex = index % 4;
			var component = FormatComponent(componentIndex, ComponentCount);
			switch(type)
			{
				case FxlcOperandType.Literal:
					var literal = string.Join(", ",
						Enumerable.Repeat(index, (int)ComponentCount));
					return string.Format("l({0})", literal);
				case FxlcOperandType.Temp:
					return string.Format("r{0}{1}", elementIndex, component);
				case FxlcOperandType.Variable:
					return string.Format("c{0}{1}", elementIndex, component);
				case FxlcOperandType.Expr:
					return string.Format("expr{0}{1}", elementIndex, component);
				default:
					return string.Format("unknown{0}{1}", elementIndex, component);
			}
		}
		/// <summary>
		/// Display a debug representation of the operand. Displays indexes instead of values for literals
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if(IsArray == 0)
			{
				return FormatOperand(OpType, OpIndex);
			}
			else
			{
				return string.Format("{0}[{1}]",
					FormatOperand(ArrayType, ArrayIndex),
					FormatOperand(OpType, OpIndex));
			}
		}
	}
}
