using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.RTS0
{
	/// <summary>
	/// Flags for the main root signature
	/// Based on D3D12_ROOT_SIGNATURE_FLAGS.
	/// </summary>
	[Flags]
	public enum RootSignatureFlags
	{
		[Description("NONE")]
		None = 0,
		[Description("ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT")]
		AllowInputAssemblerInputLayout = 0x1,
		[Description("DENY_VERTEX_SHADER_ROOT_ACCESS")]
		DenyVertexShaderRootAccess = 0x2,
		[Description("DENY_HULL_SHADER_ROOT_ACCESS")]
		DenyHUllShaderRootAccess = 0x4,
		[Description("DENY_DOMAIN_SHADER_ROOT_ACCESS")]
		DenyDomainShaderRootAccess = 0x8,
		[Description("DENY_GEOMETRY_SHADER_ROOT_ACCESS")]
		DenyGeometryShaderRootAccess = 0x10,
		[Description("DENY_PIXEL_SHADER_ROOT_ACCESS")]
		DenyPixelShaderRootAccess = 0x20,
		[Description("ALLOW_STREAM_OUTPUT")]
		AllowStreamOutput = 0x40,
		[Description("LOCAL_ROOT_SIGNATURE")]
		LocalRootSignature = 0x80
	}
}
