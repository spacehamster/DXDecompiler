namespace DXDecompiler.DebugParser.Chunks.Fx10.Assignemnt
{
	public class DebugEffectVariableAssignment : DebugEffectAssignment
	{
		public string Value { get; private set; }
		public new static DebugEffectVariableAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectVariableAssignment();
			result.Value = assignmentReader.ReadString("Value");
			return result;
		}
	}
}
