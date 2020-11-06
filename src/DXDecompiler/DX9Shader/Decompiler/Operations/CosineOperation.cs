namespace SlimShader.DX9Shader
{
	public class CosineOperation : UnaryOperation
	{
		public CosineOperation(HlslTreeNode value)
		{
			AddInput(value);
		}

		public override string Mnemonic => "cos";
	}
}
