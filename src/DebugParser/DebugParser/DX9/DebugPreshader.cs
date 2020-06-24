
namespace SlimShader.DebugParser.DX9
{
	public class DebugPreshader
	{
		DebugShaderModel Shader;
		public static DebugPreshader Parse(DebugBytecodeReader reader)
		{
			var result = new DebugPreshader();
			result.Shader = DebugShaderModel.Parse(reader);
			return result;
		}
	}
}
