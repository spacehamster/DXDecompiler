namespace SlimShader.Chunks.RTS0
{
	/// <summary>
	/// Root Signature parameter visibility to shader types.
	/// Based on D3D12_SHADER_VISIBILITY.
	/// </summary>
	public enum ShaderVisibility
	{
		[Description("SHADER_VISIBILITY_ALL")]
		All = 0,
		[Description("SHADER_VISIBILITY_VERTEX")]
		Vertex = 1,
		[Description("SHADER_VISIBILITY_HULL")]
		Hull = 2,
		[Description("SHADER_VISIBILITY_DOMAIN")]
		Domain = 3,
		[Description("SHADER_VISIBILITY_GEOMETRY")]
		Geometry = 4,
		[Description("SHADER_VISIBILITY_PIXEL")]
		Pixel = 5
	}
}