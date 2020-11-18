using DXDecompiler.DebugParser.DX9;
using DXDecompiler.DX9Shader;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugBinaryData
	{
		public uint Index;
		public uint Size;
		public byte[] Data;
		DebugShaderModel Shader;
		public static DebugBinaryData Parse(DebugBytecodeReader reader, DebugBytecodeReader blobReader)
		{
			var result = new DebugBinaryData();
			result.Index = blobReader.ReadUInt32("Index");
			result.Size = blobReader.ReadUInt32("Size");
			var startPosition = blobReader._reader.BaseStream.Position;
			var header = blobReader.PeakUint32();
			var shaderType = (ShaderType)(header >> 16);
			var paddedSize = result.Size + (result.Size % 4 == 0 ? 0 : 4 - result.Size % 4);
			if(shaderType == ShaderType.Pixel || shaderType == ShaderType.Vertex || shaderType == ShaderType.Expression)
			{
				var shaderReader = blobReader.CopyAtCurrentPosition("ShaderReader", blobReader);
				result.Shader = DebugShaderModel.Parse(shaderReader);
			}
			else if(result.Size > 0)
			{
				blobReader.ReadBytes("Value", (int)paddedSize);
			}
			blobReader._reader.BaseStream.Position = startPosition + paddedSize;
			return result;
		}
	}
}