namespace SlimShader.Chunks.Shex
{
	public enum InterpolationMode
	{
		Undefined = 0,

		[Description("constant")]
		Constant = 1,

		[Description("linear")]
		Linear = 2,

		[Description("linear centroid")]
		LinearCentroid = 3,

		[Description("linear noperspective")]
		LinearNoPerspective = 4,

		[Description("linear noperspective centroid")]
		LinearNoPerspectiveCentroid = 5,

		// Following are new in DX10.1

		[Description("linear sample")]
		LinearSample = 6,
		[Description("linear noperspective sample")]
		LinearNoPerspectiveSample = 7
	}
}