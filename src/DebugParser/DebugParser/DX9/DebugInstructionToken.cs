using DXDecompiler.DebugParser;
using DXDecompiler.DX9Shader;
using System.Collections.Generic;

namespace DebugParser.DebugParser.DX9
{
	public class DebugInstructionToken : IDebugToken
	{
		uint token;
		public Opcode Opcode { get; set; }
		public List<IDebugOperand> Operands { get; set; } = new List<IDebugOperand>();
		public int Modifier { get; set; }
		public bool Predicated { get; set; }
		public bool CoIssue { get; set; }
		public static DebugInstructionToken Parse(DebugBytecodeReader reader, uint token, Opcode opcode, uint size)
		{
			var result = new DebugInstructionToken();
			result.token = token;
			result.Opcode = opcode;
			uint read = 0;
			uint destIndex = opcode == Opcode.Def ? (uint)1 : 0;
			while(read < size)
			{
				var index = (uint)result.Operands.Count;
				IDebugOperand operand;
				if(index == destIndex)
				{
					operand = DebugDestinationOperand.Parse(reader, index);
				}
				else
				{
					operand = DebugSourceOperand.Parse(reader, index);
				}
				result.Operands.Add(operand);
				read += operand.WordCount;
			}
			return result;
		}
	}
}
