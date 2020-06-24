using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	public class DebugEffectHeader
	{
		/// <summary>
		/// Based on D3D10_EFFECT_DESC and maybe D3D10_STATE_BLOCK_MASK?
		/// fx_4_x has a stride of 80 bytes
		/// </summary>
		public DebugShaderVersion Version { get; private set; }
		/// <summary>
		/// Number of global variables in this effect, excluding the effect pool.
		/// </summary>
		public uint GlobalVariables;
		public uint ConstantBuffers;
		public uint ObjectCount;
		public uint SharedConstantBuffers;
		public uint SharedGlobalVariables;
		/// <summary>
		/// Resources such as textures and SamplerState
		/// </summary>
		public uint SharedObjectCount;
		public uint Techniques;
		public uint FooterOffset;
		public uint StringCount;
		public uint LocalTextureCount;
		public uint DepthStencilStateCount;
		public uint BlendStateCount;
		public uint RasterizerStateCount;
		public uint LocalSamplerCount;
		public uint RenderTargetViewCount;
		public uint DepthStencilViewCount;
		public uint ShaderCount;
		public uint InlineShaderCount;
		/// <summary>
		/// Start of fx_5_0 members
		/// </summary>
		public uint GroupCount;
		public uint UAVCount;
		public uint InterfaceVariableCount;
		public uint InterfaceVariableElementCount;
		public uint ClassInstanceElementCount;
		public static DebugEffectHeader Parse(DebugBytecodeReader chunkReader)
		{
			var result = new DebugEffectHeader();
			result.Version = DebugShaderVersion.ParseFX(chunkReader);
			result.ConstantBuffers = chunkReader.ReadUInt32("ConstantBuffers");
			result.GlobalVariables = chunkReader.ReadUInt32("GlobalVariables");
			result.ObjectCount = chunkReader.ReadUInt32("ObjectCount");
			result.SharedConstantBuffers = chunkReader.ReadUInt32("SharedConstantBuffers");
			result.SharedGlobalVariables = chunkReader.ReadUInt32("SharedGlobalVariables");
			result.SharedObjectCount = chunkReader.ReadUInt32("SharedObjectCount");
			result.Techniques = chunkReader.ReadUInt32("Techniques");
			result.FooterOffset = chunkReader.ReadUInt32("FooterOffset");
			result.StringCount = chunkReader.ReadUInt32("StringCount");
			result.LocalTextureCount = chunkReader.ReadUInt32("LocalTextureCount");
			result.DepthStencilStateCount = chunkReader.ReadUInt32("DepthStencilStateCount");
			result.BlendStateCount = chunkReader.ReadUInt32("BlendStateCount");
			result.RasterizerStateCount = chunkReader.ReadUInt32("RasterizerStateCount");
			result.LocalSamplerCount = chunkReader.ReadUInt32("LocalSamplerCount");
			result.RenderTargetViewCount = chunkReader.ReadUInt32("RenderTargetViewCount");
			result.DepthStencilViewCount = chunkReader.ReadUInt32("DepthStencilViewCount");
			result.ShaderCount = chunkReader.ReadUInt32("ShaderCount");
			result.InlineShaderCount = chunkReader.ReadUInt32("InlineShaderCount");
			if(result.Version.MajorVersion >= 5)
			{
				result.GroupCount = chunkReader.ReadUInt32("GroupCount");
				result.UAVCount = chunkReader.ReadUInt32("UAVCount");
				result.InterfaceVariableCount = chunkReader.ReadUInt32("InterfaceVariableCount");
				result.InterfaceVariableElementCount = chunkReader.ReadUInt32("InterfaceVariableElementCount");
				result.ClassInstanceElementCount = chunkReader.ReadUInt32("ClassInstanceElementCount");
			}
			return result;

		}
	}
}
