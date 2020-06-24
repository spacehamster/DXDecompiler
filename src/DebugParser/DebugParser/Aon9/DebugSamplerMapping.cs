namespace SlimShader.DebugParser.Aon9
{
	public class DebugSamplerMapping
	{
		public byte TargetSampler { get; private set; }
		public byte SourceSampler { get; private set; }
		public byte SourceResource { get; private set; }
		public static DebugSamplerMapping Parse(DebugBytecodeReader reader)
		{
			var result = new DebugSamplerMapping();
			result.TargetSampler = reader.ReadByte("TargetSampler");
			result.SourceSampler = reader.ReadByte("SourceSampler");
			result.SourceResource = reader.ReadByte("SourceResource");
			var padding = reader.ReadByte("padding");
			return result;
		}
	}
}