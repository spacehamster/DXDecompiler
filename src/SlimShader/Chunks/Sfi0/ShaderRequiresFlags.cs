using System;

namespace SlimShader.Chunks.Sfi0
{
	[Flags]
	public enum ShaderRequiresFlags
	{
		None = 0,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support double data type.
		/// </summary>
		Doubles = 0x1,

		/// <summary>
		/// Shader requires an early depth stencil.
		/// </summary>
		EarlyDepthStencil = 0x2,

		/// <summary>
		/// Shader requires unordered access views (UAVs) at every pipeline stage.
		/// </summary>
		UavsAtEveryStage = 0x4,

		/// <summary>
		/// Shader requires 64 UAVs.
		/// </summary>
		Requires64Uavs = 0x8,

		/// <summary>
		/// Shader requires the graphics driver and hardware to support minimum precision.
		/// </summary>
		MinimumPrecision = 0x10,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support extended doubles instructions.
		/// </summary>
		DoubleExtensionsFor11Point1 = 0x20,


		/// <summary>
		/// Shader requires that the graphics driver and hardware support the msad4 intrinsic function in shaders.
		/// </summary>
		ShaderExtensionsFor11Point1 = 0x40,

		/// <summary>
		/// Shader requires that the graphics driver and hardware support Direct3D 9 shadow support.
		/// </summary>
		Level9ComparisonFiltering = 0x80,

		/// <summary>
		/// Unknown
		/// </summary>
		TiledResources = 0x100,

		/// <summary>
		/// Unknown
		/// </summary>
		StencilRef = 0x200,

		/// <summary>
		/// Unknown
		/// </summary>
		InnerCoverage = 0x400,

		/// <summary>
		/// Unknown
		/// </summary>
		TypedUAVLoadAdditionalFormats = 0x800,

		/// <summary>
		/// Unknown
		/// </summary>
		Rovs = 0x1000,

		/// <summary>
		/// Unknown
		/// </summary>
		SVArrayIndexFromFeedingRasterizer = 0x2000
	}
}
