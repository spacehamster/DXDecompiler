using SlimShader.DX9Shader;
using System.Collections.Generic;

namespace DebugParser.DebugParser.DX9
{
	public interface IDebugToken
	{
		Opcode Opcode { get; set; }
		List<IDebugOperand> Operands { get; set; }
	}
}
