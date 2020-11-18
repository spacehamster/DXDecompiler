namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectExpressionAssignment : DebugEffectAssignment
	{
		public DebugBytecodeContainer Shader { get; private set; }
		public new static DebugEffectExpressionAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader assignmentReader)
		{
			var result = new DebugEffectExpressionAssignment();
			var shaderSize = assignmentReader.ReadUInt32("ShaderSize");

			if(shaderSize != 0)
			{
				result.Shader = DebugBytecodeContainer.Parse(assignmentReader.CopyAtCurrentPosition("ExpressionReader", assignmentReader));
			}
			return result;
		}
	}
}
