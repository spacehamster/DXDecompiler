namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Describes the type of Root Parameter
	/// Based on D3D12_ROOT_PARAMETER_TYPE.
	/// </summary>
	public enum RootParameterType
	{
		[Description("DescriptorTable")]
		DescriptorTable,
		[Description("RootConstants")]
		_32BitConstants,
		[Description("CBV")]
		Cbv,
		[Description("SRV")]
		Srv,
		[Description("UAV")]
		Uav
	}
}
