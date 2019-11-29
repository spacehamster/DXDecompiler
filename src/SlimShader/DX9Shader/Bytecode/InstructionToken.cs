using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class InstructionToken : Token
	{
		public List<Parameter> Params;
		public InstructionToken(Opcode opcode, int length, ShaderModel shaderModel) : base(opcode, length, shaderModel) 
		{
			Params = new List<Parameter>();
		}
	}
}
