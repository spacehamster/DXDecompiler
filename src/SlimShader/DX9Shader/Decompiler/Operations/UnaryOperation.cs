namespace SlimShader.DX9Shader
{
    public abstract class UnaryOperation : Operation
    {
        public HlslTreeNode Value => Inputs[0];
    }
}
