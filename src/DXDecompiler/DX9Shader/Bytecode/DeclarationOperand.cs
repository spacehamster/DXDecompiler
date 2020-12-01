namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// 
	/// </summary>
	public class DeclarationOperand : Operand
	{
		public DeclUsage DeclUsage { get; private set; }
		public uint DeclIndex { get; private set; }
		public SamplerTextureType DeclSamplerTextureType { get; private set; }

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
