using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SlimShader.Tests.Util
{
	public class TestUtils
	{
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
		public static string StripDX9InstructionSlots(string assembly)
		{
			return Regex.Replace(assembly, @" ?\/\/ approximately \d+ instruction slots used.*", "");
		}
		public static string TrimLines(string assembly)
		{
			var trimmed = assembly
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Select(x => x.Trim())
				.Where(x => x != "")
				.ToList();
			return string.Join(Environment.NewLine, trimmed);
		}
		public static string NormalizeAssembly(string assembly)
		{
			assembly = assembly.Trim();
			assembly = Regex.Replace(assembly, @"([^\w\n]|^)([-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?)", match =>
			{
				if (match.Groups[2].Value == "-0")
				{
					return $"{match.Groups[1].Value}-0";
				}
				if(!float.TryParse(match.Groups[2].Value, out float value))
				{
					return match.Groups[0].Value;
				}
				if(value < 1E-07)
				{
					value = 0;
				}
				var result = $"{match.Groups[1].Value}{value.ToString("G1")}";
				return result;
			});
			assembly = Regex.Replace(assembly, @"{min(16f|2_8f) as def32}", match =>
			{
				return string.Format("{{min{0}}}", match.Groups[1].Value);
			});
			//TODO: Compare hex literals and floats
			/*assembly = Regex.Replace(assembly, @"0x(\d+)", match =>
			{
				var raw = Convert.ToUInt32(match.Groups[1].Value, 16);
				var f = BitConverter.ToSingle(BitConverter.GetBytes(raw), 0);
				var result = f.ToString("G6");
				return result;
			});*/
			return assembly;
		}
	}
}
