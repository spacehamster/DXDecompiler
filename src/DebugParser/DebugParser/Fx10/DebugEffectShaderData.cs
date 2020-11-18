namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectShaderData
	{
		public BytecodeContainer Shader { get; private set; }
		uint ShaderOffset;
		public static DebugEffectShaderData Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugEffectShaderData();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32("ShaderOffset");
			var bytecodeReader = reader.CopyAtOffset("BytecodeReader", variableReader, (int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32("ShaderSize");
			if(shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes("Shader", (int)shaderSize));
			}
			return result;
		}
	}
}
