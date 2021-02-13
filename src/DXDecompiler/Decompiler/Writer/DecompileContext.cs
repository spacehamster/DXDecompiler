using DXDecompiler.Decompiler.IR;

using System.IO;

namespace DXDecompiler.Decompiler.Writer
{
	public class DecompileContext
	{
		public StringWriter Writer;
		public IrShader Shader;
		public int Indent = 0;
		public ShaderWriter ShaderWriter;
		public ResourceDefinitionWriter ResourceDefinitionWriter;
		public PassWriter PassWriter;
		public EffectWriter EffectWriter;
		public InstructionWriter InstructionWriter;
		public OperandWriter OperandWriter;
		public InterfaceWriter InterfaceWriter;
		public SignatureWriter SignatureWriter;
		public DecompileContext(StringWriter writer, IrShader shader)
		{
			Writer = writer;
			Shader = shader;
			ShaderWriter = new ShaderWriter(this);
			PassWriter = new PassWriter(this);
			EffectWriter = new EffectWriter(this);
			InstructionWriter = new InstructionWriter(this);
			OperandWriter = new OperandWriter(this);
			ResourceDefinitionWriter = new ResourceDefinitionWriter(this);
			InterfaceWriter = new InterfaceWriter(this);
			SignatureWriter = new SignatureWriter(this);
		}
		public void Write()
		{
			ShaderWriter.WriteShader(Shader);
		}
	}
}
