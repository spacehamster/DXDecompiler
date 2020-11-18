using DebugParser.DebugParser.DX9;
using DXDecompiler.DX9Shader;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.DX9
{
	public class DebugToken : IDebugToken
	{
		public uint Token;
		public byte[] Data;
		public Opcode Opcode { get; set; }
		public List<IDebugOperand> Operands { get; set; } = new List<IDebugOperand>();
	}
}
