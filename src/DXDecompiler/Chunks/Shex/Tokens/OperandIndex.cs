namespace DXDecompiler.Chunks.Shex.Tokens
{
	public class OperandIndex
	{
		public OperandIndexRepresentation Representation { get; set; }
		public ulong Value { get; set; }
		public Operand Register { get; set; }

		public override string ToString()
		{
			string result = string.Empty;
			if(Register != null)
				result += Register + " + ";
			if(Value == uint.MaxValue)
			{
				result += "*";
			}
			else
			{
				result += Value;
			}
			return result;
		}
	}
}