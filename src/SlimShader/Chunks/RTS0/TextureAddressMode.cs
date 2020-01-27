using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.RTS0
{
	/// <summary>
	/// TextureAddressMode
	/// Based on D3D12_TEXTURE_ADDRESS_MODE.
	/// </summary>
	public enum TextureAddressMode
	{
		[Description("TEXTURE_ADDRESS_WRAP")]
		Wrap = 1,
		[Description("TEXTURE_ADDRESS_MIRROR")]
		Mirror = 2,
		[Description("TEXTURE_ADDRESS_CLAMP")]
		Clamp = 3,
		[Description("TEXTURE_ADDRESS_BORDER")]
		Border = 4,
		[Description("TEXTURE_ADDRESS_MIRROR_ONCE")]
		MirrorOnce = 5
	}
}
