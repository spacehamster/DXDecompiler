using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader
{
	public class CommentToken : Token
	{
		public CommentToken(Opcode opcode, int length, ShaderModel shaderModel) : base(opcode, length, shaderModel)
		{

		}
	}
}
