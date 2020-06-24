using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.FX9
{
	public class VariableBlob
	{
		public uint Index { get; private set; }
		public string Value { get; private set; }
		public ShaderModel Shader { get; private set; }
		public bool IsShader => Shader != null;

		public static VariableBlob Parse(BytecodeReader reader, BytecodeReader dataReader)
		{
			var result = new VariableBlob();
			result.Index = dataReader.ReadUInt32();
			var blobSize = dataReader.ReadUInt32();
			var paddedSize = blobSize + (blobSize % 4 == 0 ? 0 : 4 - blobSize % 4);
			var shaderReader = dataReader.CopyAtCurrentPosition();
			var data = dataReader.ReadBytes((int)paddedSize);
			if (!_IsShader(data))
			{
				if (blobSize == 0)
				{
					result.Value = "";
				}
				else
				{
					result.Value = Encoding.UTF8.GetString(data, 0, (int)(blobSize - 1));
				}
			} else
			{
				result.Shader = ShaderModel.Parse(shaderReader);
			}
			return result;
		}

		private static bool _IsShader(byte[] data)
		{
			if (data.Length < 4) return false;
			var type = (ShaderType)BitConverter.ToUInt16(data, 2);
			switch (type)
			{
				case ShaderType.Effect:
				case ShaderType.Pixel:
				case ShaderType.Expression:
				case ShaderType.Vertex:
					return true;
				default:
					return false;
			}
		}
	}
}
