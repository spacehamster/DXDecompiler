using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using System.Collections.Generic;
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
			WriteLine($"float4 {ExpressionName}()");
			WriteLine("{");
			Indent++;

			WriteTemporaries();

			WriteIndent();
			WriteLine("float expr0;");

			WriteInstructions();

			WriteIndent();
			WriteLine("return expr0;");

			Indent--;
			WriteLine("}");
		}
	}
}
