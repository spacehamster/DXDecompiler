using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader
{
	public static class EnumExtensions
	{
		public static string GetDescription(this ParameterType value)
		{
			switch (value)
			{
				case ParameterType.Sampler1D:
					return "sampler1D";
				case ParameterType.Sampler2D:
					return "sampler2D";
				case ParameterType.Sampler3D:
					return "sampler3D";
				case ParameterType.SamplerCube:
					return "samplerCUBE";
				case ParameterType.Texture1D:
					return "texture1D";
				case ParameterType.Texture2D:
					return "texture2D";
				case ParameterType.Texture3D:
					return "texture3D";
				default:
					return value.ToString().ToLower();
			}
		}
		public static string GetDescription(this RegisterSet value)
		{
			switch (value)
			{
				case RegisterSet.Bool:
					return "b";
				case RegisterSet.Float4:
					return "c";
				case RegisterSet.Int4:
					return "i";
				case RegisterSet.Sampler:
					return "s";
				default:
					throw new InvalidOperationException();
			}
		}
		public static string GetDescription(this ShaderType value)
		{
			switch (value)
			{
				case ShaderType.Effect:
					return "fx";
				case ShaderType.Pixel:
					return "ps";
				case ShaderType.Vertex:
					return "vs";
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
