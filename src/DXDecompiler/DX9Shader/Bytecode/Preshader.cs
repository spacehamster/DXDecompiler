using DXDecompiler.Util;

namespace DXDecompiler.DX9Shader.Bytecode
{
	public class Preshader
	{
		public ShaderModel Shader { get; private set; }

		public static Preshader Parse(BytecodeReader reader)
		{
			var result = new Preshader();
			result.Shader = ShaderModel.Parse(reader);
			return result;
		}
	}
}
