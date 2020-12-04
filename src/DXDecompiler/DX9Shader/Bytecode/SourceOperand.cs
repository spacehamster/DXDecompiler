namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Source Paramter Token
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/source-parameter-token
	/// 
	/// Bits
	/// [10:00] Bits 0 through 10 indicate the register number (offset in register file).
	/// 
	/// [12:11] Bits 11 and 12 are the fourth and fifth bits [3,4] for indicating the register type.
	/// 
	/// [13] For a pixel shader (PS) versions earlier than 3_0, bit 13 is reserved and set to 0x0.
	/// 
	/// For pixel shader (PS) version 3_0 and later and all versions of vertex shader (VS), bit 13 indicates whether relative addressing mode is used. If set to 1, relative addressing applies.
	/// 
	/// [15:14] Reserved for all versions of PS and VS. This value is set to 0x0.
	/// 
	/// [23:16] Bits 16 through 23 indicate channel swizzle. All arithmetic operations are performed in four (X,Y,Z,W) parallel channels. Swizzle specifies which source component participates in a channel of operation. For more information about swizzle, see the latest DirectX SDK documentation.
	/// 
	/// [27:24] Bits 24 through 27 indicate the source modifier.
	/// 
	/// [30:28] Bits 28 through 30 are the first three bits [0,1,2] for indicating the register type.
	/// 
	/// [31] Bit 31 is 0x1.
	/// </summary>
	class SourceOperand : Operand
	{
		public uint RegisterNumber { get; private set; }
		public RegisterType RegisterType { get; private set; }
		public bool IsRelativeAddressMode { get; private set; }
		public uint MinPrecision { get; private set; }
		public uint SwizzleComponents { get; private set; }
		public SourceModifier SourceModifier { get; private set; }
		public SourceOperand ChildOperand { get; private set; }
		public SourceOperand(uint value)
		{
			Parse(value);
		}
		public SourceOperand(uint value, uint child)
		{
			ChildOperand = new SourceOperand(child);
			Parse(value);
		}
		private void Parse(uint value)
		{
			RegisterNumber = (value & 0x7FF);
			RegisterType = (RegisterType)(((value >> 28) & 0x7) | ((value >> 8) & 0x18));
			IsRelativeAddressMode = (value & (1 << 13)) != 0;
			MinPrecision = (value >> 12) & 0XC;
			SwizzleComponents = (value >> 16) & 0xFF;
			SourceModifier = (SourceModifier)((value >> 24) & 0xF);
		}
		public override string ToString()
		{
			return GetParamRegisterName(RegisterType, RegisterNumber);
		}
	}
}