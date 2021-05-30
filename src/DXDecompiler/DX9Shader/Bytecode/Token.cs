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

		// TODO: Replace all references to uint[] Data with parsed C# classes.
		// Instructions parse data into Operands
		// DefI and Def will need to parse Data into constant values
		// Comment tokens will need to have a uint[] Data field added 
		// (note: I have not found any examples of comment tokens that cannot be parsed into shader model chunks instead)
		// Instruction operations such as GetSourceName and GetDestination will need to be moved to their respective operand class
		// This may not be easy if those methods rely on data from instructions (need to look into this)
		// This will allow from better handling of relative addressing and allow for easier debugging
		// See also:
		// https://github.com/spacehamster/DXDecompiler/pull/6#issuecomment-782958769
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

		public void AddData()
		{
			int len = Data.Length;
			len++;
			uint[] d = new uint[len];
			System.Array.Copy(Data, d, Data.Length);
			Data = d;
		}

		public override string ToString()
		{
			return Opcode.ToString();
		}
	}
}
