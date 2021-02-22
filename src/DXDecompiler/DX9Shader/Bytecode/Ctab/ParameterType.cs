namespace DXDecompiler.DX9Shader.Bytecode.Ctab
{
	/// <summary>
	/// Refer D3DXPARAMETER_TYPE d3dx9shader.h
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxparameter-type
	/// </summary>
	public enum ParameterType
	{
		Void,
		Bool,
		Int,
		Float,
		String,
		Texture,
		Texture1D,
		Texture2D,
		Texture3D,
		TextureCube,
		Sampler,
		Sampler1D,
		Sampler2D,
		Sampler3D,
		SamplerCube,
		PixelShader,
		VertexShader,
		PixelFragment,
		VertexFragment,
		Unsupported
	}
}