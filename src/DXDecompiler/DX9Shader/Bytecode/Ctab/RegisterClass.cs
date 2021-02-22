namespace DXDecompiler.DX9Shader.Bytecode.Ctab
{
	/// <summary>
	/// Refer D3DXPARAMETER_CLASS d3dx9shader.h
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxparameter-class
	/// </summary>
	public enum ParameterClass
	{
		Scalar,
		Vector,
		MatrixRows,
		MatrixColumns,
		Object,
		Struct
	}
}
