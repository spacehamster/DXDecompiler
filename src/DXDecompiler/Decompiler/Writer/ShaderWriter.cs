using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class ShaderWriter : BaseWriter
	{
		public ShaderWriter(DecompileContext context) : base(context)
		{

		}

		public void WriteShader(IrShader shader)
		{
			foreach(var comment in shader.PreComments)
			{
				WriteIndent();
				WriteLine(comment);
			}
			if(shader.ResourceDefinition != null)
			{
				Context.ResourceDefinitionWriter.WriteResourceDefinition(shader.ResourceDefinition);
			}
			if(shader.InterfaceManger != null)
			{
				Context.InterfaceWriter.WriteInterface(shader.InterfaceManger);
			}
			foreach(var signature in shader.Signatures)
			{
				Context.SignatureWriter.WriteSignature(signature);
			}
			foreach(var pass in shader.Passes)
			{
				Context.PassWriter.WritePass(pass);
			}
			if(shader.Effect != null)
			{
				Context.EffectWriter.WriteEffect(shader.Effect);
			}
			foreach(var comment in shader.PostComments)
			{
				WriteIndent();
				WriteLine(comment);
			}
		}
	}
}
