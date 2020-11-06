using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	class SourceOperand : Operand
	{
		public uint RegisterNumber;
		public RegisterType RegisterType;
		public bool IsRelativeAddressMode;
		public uint MinPrecision;
		public uint SwizzleComponents;
		public SourceModifier SourceModifier;
		public SourceOperand ChildOperand;
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
			MinPrecision = ((value >> 12) & 0XC);
			SwizzleComponents = ((value >> 16) & 0xFF);
			SourceModifier = (SourceModifier)((value >> 24) & 0xF);
		}
		public override string ToString()
		{
			return GetParamRegisterName(RegisterType, (int)RegisterNumber);
		}
	}
}
