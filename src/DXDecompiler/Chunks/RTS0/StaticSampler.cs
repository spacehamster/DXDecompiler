using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Root Signature Parameter
	/// Based on D3D12_STATIC_SAMPLER_DESC.
	/// </summary>
	public class StaticSampler
	{
		public Filter Filter { get; private set; }
		public TextureAddressMode AddressU { get; private set; }
		public TextureAddressMode AddressV { get; private set; }
		public TextureAddressMode AddressW { get; private set; }
		public float MipLODBias { get; private set; }
		public uint MaxAnisotropy { get; private set; }
		public ComparisonFunc ComparisonFunc { get; private set; }
		public StaticBorderColor BorderColor { get; private set; }
		public float MinLOD { get; private set; }
		public float MaxLOD { get; private set; }
		public uint ShaderRegister { get; private set; }
		public uint RegisterSpace { get; private set; }
		public ShaderVisibility ShaderVisibility { get; private set; }
		public static StaticSampler Parse(BytecodeReader reader)
		{
			return new StaticSampler()
			{
				Filter = (Filter)reader.ReadUInt32(),
				AddressU = (TextureAddressMode)reader.ReadUInt32(),
				AddressV = (TextureAddressMode)reader.ReadUInt32(),
				AddressW = (TextureAddressMode)reader.ReadUInt32(),
				MipLODBias = reader.ReadSingle(),
				MaxAnisotropy = reader.ReadUInt32(),
				ComparisonFunc = (ComparisonFunc)reader.ReadUInt32(),
				BorderColor = (StaticBorderColor)reader.ReadUInt32(),
				MinLOD = reader.ReadSingle(),
				MaxLOD = reader.ReadSingle(),
				ShaderRegister = reader.ReadUInt32(),
				RegisterSpace = reader.ReadUInt32(),
				ShaderVisibility = (ShaderVisibility)reader.ReadUInt32(),
			};
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"\tFilter: {Filter}");
			sb.AppendLine($"\tAddressU: {AddressU}");
			sb.AppendLine($"\tAddressV: {AddressV}");
			sb.AppendLine($"\tAddressW: {AddressW}");
			sb.AppendLine($"\tMipLODBias: {MipLODBias}");
			sb.AppendLine($"\tMaxAnisotropy: {MaxAnisotropy}");
			sb.AppendLine($"\tComparisonFunc: {ComparisonFunc}");
			sb.AppendLine($"\tBorderColor: {BorderColor}");
			sb.AppendLine($"\tMinLOD: {MinLOD}");
			sb.AppendLine($"\tMaxLOD: {MaxLOD}");
			sb.AppendLine($"\tShaderRegister: {ShaderRegister}");
			sb.AppendLine($"\tRegisterSpace: {RegisterSpace}");
			sb.AppendLine($"\tShaderVisibility: {ShaderVisibility}");
			return sb.ToString();
		}
	}
}
