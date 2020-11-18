using DXDecompiler.Chunks.Shex;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugOperandIndex
	{
		public OperandIndexRepresentation Representation;
		public ulong Value;
		public DebugOperand Register;
	}
}