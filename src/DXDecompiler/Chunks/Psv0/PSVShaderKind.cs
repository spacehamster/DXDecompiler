namespace DXDecompiler.Chunks.Psv0
{
	public enum PSVShaderKind : byte    // DXIL::ShaderKind
	{
		Pixel = 0,
		Vertex,
		Geometry,
		Hull,
		Domain,
		Compute,
		Library,
		RayGeneration,
		Intersection,
		AnyHit,
		ClosestHit,
		Miss,
		Callable,
		Mesh,
		Amplification,
		Invalid,
	};
}
