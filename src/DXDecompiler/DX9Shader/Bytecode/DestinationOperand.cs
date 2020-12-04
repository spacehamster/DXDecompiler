namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Destination Parameter Token
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/destination-parameter-token
	/// 
	/// Bits
	/// [10:00] Bits 0 through 10 indicate the register number (offset in register file).
	/// 
	/// [12:11] Bits 11 and 12 are the fourth and fifth bits [3,4] for indicating the register type.
	/// 
	/// [13] For vertex shader (VS) version 3_0 and later, bit 13 indicates whether relative addressing mode is used. If set to 1, relative addressing applies.
	/// 
	/// For all pixel shader (PS) versions and vertex shader versions earlier than 3_0, bit 13 is reserved and set to 0x0.
	/// 
	/// [15:14] Reserved. This value is set to 0x0.
	/// 
	/// [19:16] Write mask. 
	/// 
	/// [23:20] Bits 20 through 23 indicate the result modifier. Multiple result modifiers can be used. 
	/// 
	/// [27:24] For PS versions earlier than 2_0, bits 24 through 27 specify the result shift scale (signed shift). For PS version 2_0 and later and VS, these bits are reserved and set to 0x0. [30:28] Bits 28 through 30 are the first three bits [0,1,2] for indicating the register type.
	/// 
	/// [31] Bit 31 is 0x1.
	/// </summary>
	public class DestinationOperand : Operand
	{
		public uint RegisterNumber { get; private set; }
		public RegisterType RegisterType { get; private set; }
		public uint MinPrecision { get; private set; }
		public ComponentFlags DestinationWriteMask { get; private set; }
		public ResultModifier ResultModifier { get; private set; }
		public DestinationOperand Child { get; private set; }
		public DestinationOperand(uint value)
		{
			Parse(value);
		}
		public DestinationOperand(uint value, uint child)
		{
			Parse(value);
			Child = new DestinationOperand(child);
		}
		private void Parse(uint value)
		{
			RegisterNumber = value & 0x7FF;
			RegisterType = (RegisterType)(((value >> 28) & 0x7) | ((value >> 8) & 0x18));
			MinPrecision = (value >> 12) & 0XC;
			DestinationWriteMask = (ComponentFlags)((value >> 16) & 0xF);
			ResultModifier = (ResultModifier)((value >> 20) & 0xF);
		}
		public override string ToString()
		{
			return GetParamRegisterName(RegisterType, RegisterNumber);
		}

		public string GetDestinationName()
		{
			var resultModifier = ResultModifier;

			string registerName = GetParamRegisterName(RegisterType, RegisterNumber);
			const int registerLength = 4;
			string writeMaskName = GetDestinationWriteMaskName(registerLength, false);
			string destinationName = $"{registerName}{writeMaskName}";
			if(resultModifier != ResultModifier.None)
			{
				//destinationName += "TODO:Modifier!!!";
			}
			return destinationName;
		}

		// Length of ".yw" = 2
		public int GetDestinationMaskLength()
		{
			ComponentFlags writeMask = DestinationWriteMask;
			int length = 0;
			for(int i = 0; i < 4; i++)
			{
				var mask = (ComponentFlags)(1 << i);
				if((writeMask & mask) != ComponentFlags.None)
				{
					length++;
				}
			}
			return length;
		}

		public string GetDestinationWriteMaskName(uint destinationLength, bool hlsl)
		{
			ComponentFlags writeMask = DestinationWriteMask;
			var writeMaskLength = GetDestinationMaskLength();

			if(!hlsl)
			{
				destinationLength = 4; // explicit mask in assembly
			}
			// Check if mask is the same length and of the form .xyzw
			if(writeMaskLength == destinationLength && writeMask == (ComponentFlags)((1 << writeMaskLength) - 1))
			{
				return "";
			}

			string writeMaskName =
				string.Format(".{0}{1}{2}{3}",
				((writeMask & ComponentFlags.X) != 0) ? "x" : "",
				((writeMask & ComponentFlags.Y) != 0) ? "y" : "",
				((writeMask & ComponentFlags.Z) != 0) ? "z" : "",
				((writeMask & ComponentFlags.W) != 0) ? "w" : "");
			return writeMaskName;
		}
	}
}
