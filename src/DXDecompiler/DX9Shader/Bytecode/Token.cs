using System;

namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Base Token Type
	/// Refer https://docs.microsoft.com/en-us/windows-hardware/drivers/display/shader-code-tokens
	/// TODO: This should be made abstract
	/// </summary>
	public class Token
	{
		public Opcode Opcode { get; private set; }
		public uint[] Data { get; private set; }
		public int Modifier { get; set; }
		public bool Predicated { get; set; }
		public bool CoIssue { get; set; }
		public bool HasDestination => Opcode.HasDestination();
		public bool IsTextureOperation => Opcode.IsTextureOperation();

		protected ShaderModel _shaderModel;

		public Token(Opcode opcode, int numParams, ShaderModel shaderModel)
		{
			Opcode = opcode;
			Data = new uint[numParams];
			_shaderModel = shaderModel;
		}

		public override string ToString()
		{
			return Opcode.ToString();
		}
	}
}
