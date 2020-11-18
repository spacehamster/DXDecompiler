using DXDecompiler.Util;

namespace DXDecompiler.DX9Shader.FX9
{
	/*
	 * 
	 */
	public class StateBlob
	{
		public uint TechniqueIndex { get; private set; }
		public uint PassIndex { get; private set; }
		public uint SamplerStateIndex { get; private set; }
		public uint AssignmentIndex { get; private set; }
		public StateBlobType BlobType { get; private set; }
		public string VariableName { get; private set; }
		public ShaderModel Shader { get; private set; }
		public static StateBlob Parse(BytecodeReader reader, BytecodeReader shaderReader)
		{
			var result = new StateBlob();
			result.TechniqueIndex = shaderReader.ReadUInt32();
			result.PassIndex = shaderReader.ReadUInt32();
			result.SamplerStateIndex = shaderReader.ReadUInt32();
			result.AssignmentIndex = shaderReader.ReadUInt32();
			result.BlobType = (StateBlobType)shaderReader.ReadUInt32();
			var dataReader = shaderReader.CopyAtCurrentPosition();
			var blobSize = shaderReader.ReadUInt32();
			var paddedSize = blobSize + (blobSize % 4 == 0 ? 0 : 4 - blobSize % 4);
			//Seak ahead
			var data = shaderReader.ReadBytes((int)paddedSize); 
			if(result.BlobType == StateBlobType.Shader)
			{
				result.Shader = ShaderReader.ReadShader(data);
			}
			else if (result.BlobType == StateBlobType.Variable)
			{
				result.VariableName = dataReader.TryReadString();
			} 
			else if(result.BlobType == StateBlobType.IndexShader)
			{
				var _blobSize = dataReader.ReadUInt32();
				var variableSize = dataReader.ReadUInt32();
				result.VariableName = dataReader.ReadString();
				if (variableSize > (result.VariableName.Length + 1)) {
					var paddingCount = variableSize - (result.VariableName.Length + 1);
					var padding = dataReader.ReadBytes((int)paddingCount);
				}
				result.Shader = result.Shader = ShaderModel.Parse(dataReader);
			}
			return result;
		}
	}
}
