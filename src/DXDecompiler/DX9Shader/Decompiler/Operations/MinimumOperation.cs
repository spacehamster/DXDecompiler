namespace DXDecompiler.DX9Shader
{
	public class MinimumOperation : Operation
	{
		public MinimumOperation(HlslTreeNode value1, HlslTreeNode value2)
		{
			AddInput(value1);
			AddInput(value2);
		}

		public HlslTreeNode Value1 => Inputs[0];
		public HlslTreeNode Value2 => Inputs[1];

		public override string Mnemonic => "min";
	}
}
