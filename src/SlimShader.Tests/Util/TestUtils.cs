using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SlimShader.Tests.Util
{
	public class TestUtils
	{
		public static string NormalizeFloats(string text)
		{
			var pattern = @"([^\w\n]|^)([-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?)";
			return Regex.Replace(text, pattern, new MatchEvaluator(Replacer));
		}
		public static string Replacer(Match match)
		{
			if(match.Groups[2].Value == "-0")
			{
				return $"{match.Groups[1].Value}-0";
			}
			var value = float.Parse(match.Groups[2].Value);
			var result = $"{match.Groups[1].Value}{value.ToString("G7")}";
			return result;
		}
		public static string GetShaderEntryPoint(ShaderProfile version)
		{
			switch (version.Version)
			{
				case ShaderVersion.VertexShader:
					return "MainVS";
				case ShaderVersion.PixelShader:
					return "MainPS";
				case ShaderVersion.HullShader:
					return "MainHS";
				case ShaderVersion.GeometryShader:
					return "MainGS";
				case ShaderVersion.DomainShader:
					return "MainDS";
				case ShaderVersion.ComputeShader:
					return "MainCS";
				default:
					return "Main";
			}
		}
		public static string GetShaderProfile(ShaderProfile version)
		{
			return version.ToString();
  		}
	}
}
