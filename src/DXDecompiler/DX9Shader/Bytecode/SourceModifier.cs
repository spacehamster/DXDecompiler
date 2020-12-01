namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Source Modifier Type
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/source-parameter-token
	/// </summary>
	public enum SourceModifier
	{
		None,
		Negate,
		Bias,
		BiasAndNegate,
		Sign,
		SignAndNegate,
		Complement,
		X2,
		X2AndNegate,
		DivideByZ,
		DivideByW,
		Abs,
		AbsAndNegate,
		Not
	}
}
