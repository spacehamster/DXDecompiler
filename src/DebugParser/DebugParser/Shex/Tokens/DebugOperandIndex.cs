using SlimShader.Chunks.Shex;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugOperandIndex
	{
		public OperandIndexRepresentation Representation;
		public ulong Value;
		public DebugOperand Register;
	}
}