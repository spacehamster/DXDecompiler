namespace SlimShader.DebugParser.Aon9
{
	public class DebugLoopRegisterMapping
	{
		public ushort TargetReg { get; private set; }
		public ushort Buffer { get; private set; }
		public ushort SourceReg { get; private set; }
		public ushort Component { get; private set; }
		public static DebugLoopRegisterMapping Parse(DebugBytecodeReader reader)
		{
			var result = new DebugLoopRegisterMapping();
			result.Buffer = reader.ReadUInt16("Buffer");
			result.SourceReg = reader.ReadUInt16("SourceReg");
			result.Component = reader.ReadUInt16("Component");
			result.TargetReg = reader.ReadUInt16("TargetReg");
			return result;
		}
	}
}