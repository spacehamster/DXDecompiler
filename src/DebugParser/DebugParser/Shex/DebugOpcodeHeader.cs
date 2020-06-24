using SlimShader.Chunks.Shex;

namespace SlimShader.DebugParser.Shex
{
	public class DebugOpcodeHeader
	{
		public OpcodeType OpcodeType { get; set; }
		public uint Length { get; set; }
		public bool IsExtended { get; set; }
	}
}