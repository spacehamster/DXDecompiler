using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using System;
using System.Linq;

namespace DXDecompiler.DX9Shader.Decompiler
{
	class ExpressionHLSLWriter : FxlcHlslWriter
	{
		string ExpressionName { get; }

		public ExpressionHLSLWriter(ShaderModel shader, string expressionName) : base(shader)
		{
			ExpressionName = expressionName;
		}
		public static string Decompile(ShaderModel shader, string expressionName = "Expression")
		{
			var asmWriter = new ExpressionHLSLWriter(shader, expressionName);
			return asmWriter.Decompile();
		}
		protected override void Write()
		{
			var (rows, columns) = GetOutputDimensions();
			string returnType;
			if(rows == 1)
			{
				returnType = columns == 1 ? "float" : $"float{columns}";
			}
			else
			{
				returnType = $"float{rows}x{columns}";
			}

			WriteLine($"{returnType} {ExpressionName}()");
			WriteLine("{");
			Indent++;

			WriteTemporaries();
			var destinationVariables = string.Join(", ", Enumerable.Range(0, rows).Select(i => $"expr{i}"));
			WriteIndent();
			WriteLine($"float{columns} {destinationVariables};");

			WriteInstructions();

			WriteIndent();
			if(rows == 1)
			{
				WriteLine("return expr0;");
			}
			else
			{

				WriteLine($"return {returnType}({destinationVariables});");
			}

			Indent--;
			WriteLine("}");
		}
		private (int Rows, int Columns) GetOutputDimensions()
		{
			int registerNumber = 0;
			int components = 0;
			foreach(var token in Shader.Fxlc.Tokens)
			{
				var destination = token.Operands[0];
				if(destination.IsArray != 0 && destination.ArrayType == FxlcOperandType.Expr)
				{
					throw new NotImplementedException();
				}
				if(destination.OpType != FxlcOperandType.Expr)
				{
					continue;
				}
				registerNumber = Math.Max(registerNumber, (int)destination.OpIndex / 4);
				var extraComponents = Math.Max(0, (int)destination.ComponentCount - 1);
				var lastIndex = Math.Min(4, (int)destination.OpIndex % 4 + extraComponents);
				components = Math.Max(components, lastIndex);
			}
			return (registerNumber + 1, components + 1);
		}
	}
}
