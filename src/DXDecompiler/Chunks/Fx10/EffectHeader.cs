using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectHeader
	{
		/// <summary>
		/// Based on D3D10_EFFECT_DESC and maybe D3D10_STATE_BLOCK_MASK?
		/// fx_4_x has a stride of 80 bytes
		/// </summary>
		public ShaderVersion Version { get; private set; }
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
		public static EffectHeader Parse(BytecodeReader chunkReader)
		{
			var result = new EffectHeader();
			result.Version = ShaderVersion.ParseFX(chunkReader);
			result.ConstantBuffers = chunkReader.ReadUInt32();
			result.GlobalVariables = chunkReader.ReadUInt32();
			result.ObjectCount = chunkReader.ReadUInt32();
			result.SharedConstantBuffers = chunkReader.ReadUInt32();
			result.SharedGlobalVariables = chunkReader.ReadUInt32();
			result.SharedObjectCount = chunkReader.ReadUInt32();
			result.Techniques = chunkReader.ReadUInt32();
			result.FooterOffset = chunkReader.ReadUInt32();
			result.StringCount = chunkReader.ReadUInt32();
			result.LocalTextureCount = chunkReader.ReadUInt32();
			result.DepthStencilStateCount = chunkReader.ReadUInt32();
			result.BlendStateCount = chunkReader.ReadUInt32();
			result.RasterizerStateCount = chunkReader.ReadUInt32();
			result.LocalSamplerCount = chunkReader.ReadUInt32();
			result.RenderTargetViewCount = chunkReader.ReadUInt32();
			result.DepthStencilViewCount = chunkReader.ReadUInt32();
			result.ShaderCount = chunkReader.ReadUInt32();
			result.InlineShaderCount = chunkReader.ReadUInt32();
			if(result.Version.MajorVersion >= 5)
			{
				result.GroupCount = chunkReader.ReadUInt32();
				result.UAVCount = chunkReader.ReadUInt32();
				result.InterfaceVariableCount = chunkReader.ReadUInt32();
				result.InterfaceVariableElementCount = chunkReader.ReadUInt32();
				result.ClassInstanceElementCount = chunkReader.ReadUInt32();
			}
			return result;
		}
	}
}
