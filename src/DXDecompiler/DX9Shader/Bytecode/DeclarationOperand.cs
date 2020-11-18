namespace DXDecompiler.DX9Shader
{
	public class DeclarationOperand : Operand
	{
		public DeclUsage DeclUsage;
		public uint DeclIndex;
		public SamplerTextureType DeclSamplerTextureType;
		public DeclarationOperand(uint value)
		{
			DeclUsage = (DeclUsage)(value & 0x1F);
			DeclIndex = (value >> 16) & 0x0F;
			DeclSamplerTextureType = (SamplerTextureType)((value >> 27) & 0xF);
		}
		public override string ToString()
		{
			return $"{DeclUsage}{DeclIndex}_{DeclSamplerTextureType}";
		}
	}
}
