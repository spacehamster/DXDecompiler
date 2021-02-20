namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// D3DSHADER_PARAM_REGISTER_TYPE d3d9types.h
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/d3d9types/ne-d3d9types-_d3dshader_param_register_type
	/// </summary>
	public enum RegisterType
	{
		Temp,
		Input,
		Const,
		Texture,
		Addr = Texture,
		RastOut,
		AttrOut,
		TexCoordOut, // used in vs_2_0 and below
		Output = TexCoordOut,
		ConstInt,
		ColorOut,
		DepthOut,
		Sampler,
		Const2,
		Const3,
		Const4,
		ConstBool,
		Loop,
		TempFloat16,
		MiscType,
		Label,
		Predicate
	}
}
