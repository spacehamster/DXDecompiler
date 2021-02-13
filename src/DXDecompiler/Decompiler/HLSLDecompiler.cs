using System.IO;
using System.Text;
using DXDecompiler.Decompiler.IR;
using DXDecompiler.Decompiler.Writer;

namespace DXDecompiler.Decompiler
{
	public class HLSLDecompiler
	{
		private HLSLDecompiler(BytecodeContainer container)
		{
			Container = container;
			ShaderIR = DxbcParser.Parser.Parse(container);
		}

		public static string Decompile(byte[] data)
		{
			BytecodeContainer container = new BytecodeContainer(data);
			HLSLDecompiler decompiler = new HLSLDecompiler(container);
			return decompiler.Decompile();
		}

		public string Decompile()
		{
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			DecompileContext context = new DecompileContext(sw, this.ShaderIR);
			context.Write();
			return sb.ToString();
		}

		private BytecodeContainer Container;

		private IrShader ShaderIR;
	}
}
