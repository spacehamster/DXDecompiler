using DXDecompiler.DX9Shader.Bytecode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader.Decompiler
{
	class PreshaderWriter : FxlcHlslWriter
	{
		HashSet<uint> CtabOverride { get; } = new();
		PrsiToken Prsi { get; }
		

		public PreshaderWriter(Preshader shader) : base(shader.Shader)
		{
			Prsi = shader.Shader.Prsi;
		}

		public static string Decompile(Preshader shader, int indent, out HashSet<uint> ctabOverride)
		{
			var writer = new PreshaderWriter(shader)
			{
				Indent = indent
			};
			ctabOverride = writer.CtabOverride;
			return writer.Decompile();
		}

		protected override void Write()
		{
			WriteIndent();
			WriteLine("/*");
			WriteLine(string.Join("\n", Prsi.Dump().Split('\n').Select(x => new string(' ', Indent * 4) + x)));
			WriteIndent();
			WriteLine("*/");

			for(var i = 0; i < Prsi.OutputRegisterCount; ++i)
			{
				WriteIndent();
				WriteLine($"float4 expr{i + Prsi.OutputRegisterOffset};");
			}

			WriteIndent();
			WriteLine("{");
			Indent++;

			WriteTemporaries();

			WriteIndent();
			WriteLine($"// {Shader.Type}_{Shader.MajorVersion}_{Shader.MinorVersion}");

			WriteInstructions(CtabOverride);

			Indent--;
			WriteIndent();
			WriteLine("}");
		}
	}
}
