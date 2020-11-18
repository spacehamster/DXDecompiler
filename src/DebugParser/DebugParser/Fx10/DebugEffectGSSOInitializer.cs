using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectGSSOInitializer
	{
		public BytecodeContainer Shader { get; private set; }
		public string SODecl { get; private set; }

		public uint ShaderOffset;
		public uint SODeclOffset;
		public static DebugEffectGSSOInitializer Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugEffectGSSOInitializer();
			var shaderOffset = result.ShaderOffset = variableReader.ReadUInt32("ShaderOffset");
			var SODeclOffset = result.SODeclOffset = variableReader.ReadUInt32("SODeclOffset");

			var bytecodeReader = reader.CopyAtOffset("BytecodeReader", variableReader, (int)shaderOffset);
			var shaderSize = bytecodeReader.ReadUInt32("ShaderSize");
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(bytecodeReader.ReadBytes("Shader", (int)shaderSize));
			}

			var declReader = reader.CopyAtOffset("DeclReader", variableReader, (int)SODeclOffset);
			result.SODecl = declReader.ReadString("SODecl");
			return result;
		}
	}
}
