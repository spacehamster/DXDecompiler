using SlimShader.Chunks;
using SlimShader.Chunks.RTS0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SlimShader.Decompiler
{
	internal class RootSignature
	{
		internal static void WriteRootSignature(BytecodeContainer container, StringBuilder output, bool wrap = true)
		{
			var signature = container.Chunks
					.OfType<RootSignatureChunk>()
					.FirstOrDefault();
			if (signature == null) return;
			var result = new RootSignature().RootSignatureToString(signature);
			result = Regex.Replace(result, @"^(\s*)([^\n\r]*)", @"$1""$2"" \", RegexOptions.Multiline);
			result = result.Substring(0, result.Length - 2);
			output.AppendLine(@"#define RS1 \");
			output.AppendLine(result);
		}
		string FormatFlags<T>(T value) where T : Enum
		{
			List<string> result = new List<string>();
			foreach (Enum v in Enum.GetValues(typeof(T)))
			{
				if((int)(ValueType)v == 0)
				{
					continue;
				}
				if (value.HasFlag(v))
				{
					result.Add(v.GetDescription());
				}
			}
			return string.Join(" | ", result);
		}
		string RootSignatureToString(RootSignatureChunk signature, int indent = 0)
		{
			var items = new List<string>();
			if (signature.Flags != RootSignatureFlags.None)
			{
				items.Add($"RootFlags({FormatFlags(signature.Flags)})");
			}
			foreach (var param in signature.RootParameters)
			{
				items.Add(RootParameterToString(param));
			}
			foreach (var sampler in signature.StaticSamplers)
			{
				items.Add(StaticSamplerToString(sampler));
			}
			return string.Join(",\n", items);
		}

		private string RootParameterToString(RootParameter param)
		{
			switch (param.ParameterType)
			{
				case RootParameterType.Cbv:
				case RootParameterType.Srv:
				case RootParameterType.Uav:
					return RootDescriptorToString(param as RootDescriptor);
				case RootParameterType.DescriptorTable:
					return DescriptorTableToString(param as RootDescriptorTable);
				case RootParameterType._32BitConstants:
					return RootConstantsToString(param as RootConstants);
				default:
					throw new InvalidOperationException($"Unexpected type {param.ParameterType}");
			}
		}

		private string RootDescriptorToString(RootDescriptor param)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}({1}{2}",
				param.ParameterType.GetDescription(),
				param.ParameterType.GetRegisterName(),
				param.ShaderRegister);
			if (param.RegisterSpace != 0)
			{
				sb.AppendFormat(", space={0}", param.RegisterSpace);
			}
			if (param.Flags != RootDescriptorFlags.None)
			{
				sb.AppendFormat(", flags={0}", FormatFlags(param.Flags));
			}
			if (param.ShaderVisibility != ShaderVisibility.All)
			{
				sb.AppendFormat(", visibility={0}", param.ShaderVisibility.GetDescription());
			}
			sb.Append(")");
			return sb.ToString();
		}
		private string DescriptorTableToString(RootDescriptorTable param)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}(\n",
				param.ParameterType.GetDescription());
			for (int i = 0; i < param.DescriptorRanges.Count; i++)
			{
				var range = param.DescriptorRanges[i];
				sb.AppendFormat("\t{0}({1}{2}",
					range.RangeType.GetDescription(),
					range.RangeType.GetRegisterName(),
					range.BaseShaderRegister); ;
				if (range.NumDescriptors > 1 && range.NumDescriptors < uint.MaxValue)
				{
					sb.AppendFormat(", numDescriptors={0}", range.NumDescriptors);
				}
				else if (range.NumDescriptors == uint.MaxValue)
				{
					sb.Append(", numDescriptors=unbounded");
				}
				if (range.RegisterSpace > 0)
				{
					sb.AppendFormat(", space={0}", range.RegisterSpace);
				}
				if (range.Flags != DescriptorRangeFlags.None)
				{
					sb.AppendFormat(", flags={0}", FormatFlags(range.Flags));
				}
				if (range.OffsetInDescriptorsFromTableStart != uint.MaxValue)
				{
					sb.AppendFormat(", offset={0}", range.OffsetInDescriptorsFromTableStart);
				}
				if (i < param.DescriptorRanges.Count - 1)
				{
					sb.AppendLine("), ");
				}
			}
			sb.Append("))");
			return sb.ToString();
		}
		private string RootConstantsToString(RootConstants param)
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}({1}{2}",
				param.ParameterType.GetDescription(),
				param.ParameterType.GetRegisterName(),
				param.ShaderRegister);
			if(param.Num32BitValues > 0)
			{
				sb.AppendFormat(", num32BitConstants={0}", param.Num32BitValues);
			}
			if (param.RegisterSpace > 0)
			{
				sb.AppendFormat(", space={0}", param.RegisterSpace);
			}
			if (param.ShaderVisibility != ShaderVisibility.All)
			{
				sb.AppendFormat(", visibility={0}", param.ShaderVisibility.GetDescription());
			}
			sb.Append(")");
			return sb.ToString();
		}
		private string StaticSamplerToString(StaticSampler sampler)
		{ 
			var sb = new StringBuilder();
			sb.AppendFormat("StaticSampler(s{0}",
				sampler.ShaderRegister);
			if (sampler.Filter != Filter.Anisotropic)
			{
				sb.AppendFormat(", filter={0}", sampler.Filter.GetDescription());
			}
			if (sampler.AddressU != TextureAddressMode.Wrap)
			{
				sb.AppendFormat(", addressU={0}", sampler.AddressU.GetDescription());
			}
			if (sampler.AddressV != TextureAddressMode.Wrap)
			{
				sb.AppendFormat(", addressV={0}", sampler.AddressV.GetDescription());
			}
			if (sampler.AddressW != TextureAddressMode.Wrap)
			{
				sb.AppendFormat(", addressW={0}", sampler.AddressW.GetDescription());
			}
			if (sampler.MipLODBias != 0)
			{
				sb.AppendFormat(", mipLODBias={0}", sampler.MipLODBias);
			}
			if (sampler.MaxAnisotropy != 16)
			{
				sb.AppendFormat(", maxAnisotropy={0}", sampler.MaxAnisotropy);
			}
			if (sampler.ComparisonFunc != ComparisonFunc.LessEqual)
			{
				sb.AppendFormat(", comparisonFunc={0}", sampler.ComparisonFunc.GetDescription());
			}
			if (sampler.BorderColor != StaticBorderColor.OpaqueWhite)
			{
				sb.AppendFormat(", borderColor={0}", sampler.BorderColor.GetDescription());
			}
			if (sampler.MinLOD != 0)
			{
				sb.AppendFormat(", minLOD={0}", sampler.MinLOD);
			}
			if (sampler.MaxLOD != float.MaxValue)
			{
				sb.AppendFormat(", maxLOD={0}", sampler.MaxLOD);
			}
			if (sampler.RegisterSpace > 0)
			{
				sb.AppendFormat(", space={0}", sampler.RegisterSpace);
			}
			if (sampler.ShaderVisibility != ShaderVisibility.All)
			{
				sb.AppendFormat(", visibility={0}", sampler.ShaderVisibility.GetDescription());
			}
			sb.Append(")");
			return sb.ToString();
		}
	}
}
