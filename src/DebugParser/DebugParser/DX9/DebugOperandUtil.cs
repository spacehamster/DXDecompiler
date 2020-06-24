using SlimShader.DX9Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugParser.DebugParser.DX9
{
	class DebugOperandUtil
	{
		public static string OperandToString(RegisterType registerType, uint registerNumber)
		{
			switch (registerType)
			{
				case RegisterType.Const:
					return $"c{registerNumber}";
				case RegisterType.Const2:
					return $"c2_{registerNumber}";
				case RegisterType.Const3:
					return $"c3_{registerNumber}";
				case RegisterType.Const4:
					return $"c4_{registerNumber}";
				case RegisterType.Temp:
					return $"r{registerNumber}";
				case RegisterType.Sampler:
					return $"s{registerNumber}";
				case RegisterType.Texture:
					return $"t{registerNumber}";
				case RegisterType.Input:
					return $"v{registerNumber}";
				case RegisterType.Output:
					return $"o{registerNumber}";
			}
			return $"{registerType}{registerNumber}";
		}
	}
}
