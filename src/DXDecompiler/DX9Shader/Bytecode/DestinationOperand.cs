namespace DXDecompiler.DX9Shader
{
	public class DestinationOperand : Operand
	{
		public uint RegisterNumber;
		public RegisterType RegisterType;
		public uint MinPrecision;
		public ComponentFlags DestinationWriteMask;
		public ResultModifier ResultModifier;
		public DestinationOperand(uint value)
		{
			RegisterNumber = value & 0x7FF;
			RegisterType = (RegisterType)(((value >> 28) & 0x7) | ((value >> 8) & 0x18));
			MinPrecision = (value >> 12) & 0XC;
			DestinationWriteMask = (ComponentFlags)((value >> 16) & 0xF);
			ResultModifier = (ResultModifier)((value >> 20) & 0xF);
		}
		public override string ToString()
		{
			return GetParamRegisterName(RegisterType, (int)RegisterNumber);
		}

	}
}
