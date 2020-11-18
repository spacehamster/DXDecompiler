using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Root Signature chunk
	/// Based on D3D12_VERSIONED_ROOT_SIGNATURE_DESC.
	/// </summary>
	public class RootSignatureChunk : BytecodeChunk
	{
		public RootSignatureVersion Version { get; private set; }
		public List<RootParameter> RootParameters { get; private set; }
		public List<StaticSampler> StaticSamplers { get; private set; }
		public RootSignatureFlags Flags { get; private set; }
		public RootSignatureChunk()
		{
			RootParameters = new List<RootParameter>();
			StaticSamplers = new List<StaticSampler>();
		}
		public static RootSignatureChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var chunkReader = reader.CopyAtCurrentPosition();
			var result = new RootSignatureChunk();
			result.Version = (RootSignatureVersion)chunkReader.ReadUInt32();
			var numParameters = chunkReader.ReadUInt32();
			var parameterOffset = chunkReader.ReadUInt32();
			var numberStaticSamplers = chunkReader.ReadUInt32();
			var staticSamplerOffset = chunkReader.ReadUInt32();
			result.Flags = (RootSignatureFlags)chunkReader.ReadUInt32();
			var paramReader = reader.CopyAtOffset((int)parameterOffset);
			for (int i = 0; i < numParameters; i++)
			{
				result.RootParameters.Add(RootParameter.Parse(chunkReader, paramReader, result.Version));
			}
			var staticSamplerReader = reader.CopyAtOffset((int)staticSamplerOffset);
			for (int i = 0; i < numberStaticSamplers; i++)
			{
				result.StaticSamplers.Add(StaticSampler.Parse(staticSamplerReader));
			}
			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"Version: {Version}");
			sb.AppendLine($"RootParameter: {RootParameters.Count}");
			for(int i = 0; i < RootParameters.Count; i++)
			{
				sb.AppendLine(RootParameters[i].ToString());
			}
			sb.AppendLine($"StaticSamplers: {StaticSamplers.Count}");
			for (int i = 0; i < StaticSamplers.Count; i++)
			{
				sb.AppendLine(StaticSamplers[i].ToString());
			}
			sb.AppendLine($"Flags: {Flags}");
			return sb.ToString();
		}
	}
}
