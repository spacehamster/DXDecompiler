using SlimShader.DX9Shader.Bytecode;
using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.DX9Shader.Bytecode.Fxlvm;
using System.Linq;

namespace SlimShader.DX9Shader.Decompiler
{
	class ExpressionHLSLWriter : DecompileWriter
	{
		ShaderModel Shader;
		ConstantTable Ctab;
		CliToken Cli;
		string ExpressionName;
		public ExpressionHLSLWriter(ShaderModel shader, string expressionName)
		{
			Shader = shader;
			ExpressionName = expressionName;
			Ctab = shader.ConstantTable;
			Cli = shader.Cli;
		}
		public static string Decompile(ShaderModel shader, string expressionName = "Expression")
		{
			var asmWriter = new ExpressionHLSLWriter(shader, expressionName);
			return asmWriter.Decompile();
		}
		protected override void Write()
		{
			WriteLine($"float4 {ExpressionName}()");
			WriteLine("{");
			Indent++;
			WriteIndent();
			WriteLine("float4 expr;");
			foreach(var token in Shader.Fxlc.Tokens)
			{
				Write(token);
			}
			WriteIndent();
			WriteLine("return expr;");
			Indent--;
			WriteLine("}");

			if(Shader.Preshader != null)
			{
				Write("Have Pres");
			}
		}

		void Write(FxlcToken token)
		{
			WriteIndent();
			WriteLine($"// {token.ToString(Shader.ConstantTable, Shader.Cli)}");
			switch (token.Opcode)
			{
				case Bytecode.Fxlvm.FxlcOpcode.Mov:
					WriteIndent();
					WriteLine("{0} = {1};",
						token.Operands[0].ToString(Ctab, Cli),
						token.Operands[1].ToString(Ctab, Cli));
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Neg:
					WriteIndent();
					WriteLine("{0} = -{1};",
						token.Operands[0].ToString(Ctab, Cli),
						token.Operands[1].ToString(Ctab, Cli));
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Frc:
					WriteFunction("frac", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Exp:
					WriteFunction("exp", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Log:
					WriteFunction("log", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Rsq:
					WriteFunction("rsq", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Sin:
					WriteFunction("sin", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Cos:
					WriteFunction("cos", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Asin:
					WriteFunction("asin", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Acos:
					WriteFunction("acos", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Atan:
					WriteFunction("atam", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Atan2:
					WriteFunction("atan2", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Sqrt:
					WriteFunction("sqrt", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Ineg:
					WriteFunction("~int", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Imax:
					WriteFunction("(int)max(", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Not:
					WriteFunction("!", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Utof:
					WriteFunction("utof", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Ftoi:
					WriteFunction("ftoi", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Ftou:
					WriteFunction("ftou", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Btoi:
					WriteFunction("btoi", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Round:
					WriteFunction("round", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Floor:
					WriteFunction("floor", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Ceil:
					WriteFunction("ceil", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Min:
					WriteFunction("min", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Max:
					WriteFunction("max", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Add:
					WriteInfix("+", token);
					break;
				case Bytecode.Fxlvm.FxlcOpcode.Mul:
					WriteInfix("*", token);
					break;


				case Bytecode.Fxlvm.FxlcOpcode.Lt:
					WriteInfix("<", token);
					break;
			}
		}
		void WriteInfix(string op, FxlcToken token)
		{
			WriteIndent();
			WriteLine("{0} = {1} {2} {3};",
				token.Operands[0].ToString(Ctab, Cli),
				token.Operands[1].ToString(Ctab, Cli),
				op,
				token.Operands[2].ToString(Ctab, Cli));
		}
		void WriteFunction(string func, FxlcToken token)
		{
			WriteIndent();
			var operands = token.Operands
				.Skip(1)
				.Select(o => o.ToString(Ctab, Cli));
			WriteLine ("{0} = {1}({2});",
				token.Operands[0].ToString(Ctab, Cli),
				func,
				string.Join(", ", operands));
		}
	}
}
