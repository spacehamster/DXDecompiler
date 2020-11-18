using System.Linq;

namespace DXDecompiler.DX9Shader
{
	public abstract class Operation : HlslTreeNode
	{
		public abstract string Mnemonic { get; }

		public override string ToString()
		{
			string parameters = string.Join(", ", Inputs.Select(c => c.ToString()));
			return $"{Mnemonic}({parameters})";
		}
	}
}
