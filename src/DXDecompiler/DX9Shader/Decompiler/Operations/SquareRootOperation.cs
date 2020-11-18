namespace DXDecompiler.DX9Shader
{
	public class SquareRootOperation : UnaryOperation
	{
		public SquareRootOperation(HlslTreeNode value)
		{
			AddInput(value);
		}

		public override string Mnemonic => "sqrt";
	}
}
