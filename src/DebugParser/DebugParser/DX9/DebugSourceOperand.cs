using DXDecompiler.DebugParser;
using DXDecompiler.DX9Shader;

namespace DebugParser.DebugParser.DX9
{
	public class DebugSourceOperand : IDebugOperand
	{
		public uint WordCount { get; set; }

		public uint RegisterNumber;
		public RegisterType RegisterType;
		public bool IsRelativeAddressMode;
		public uint MinPrecision;
		public uint SwizzleComponents;
		public SourceModifier SourceModifier;
		public DebugSourceOperand ChildOperand;

		public static DebugSourceOperand Parse(DebugBytecodeReader reader, uint index)
		{
			var token = reader.ReadUInt32($"Operand {index}");
			var result = new DebugSourceOperand();
			result.WordCount = 1;
			result.RegisterNumber = (token & 0x7FF);
			result.RegisterType = (RegisterType)(((token >> 28) & 0x7) | ((token >> 8) & 0x18));
			result.IsRelativeAddressMode = (token & (1 << 13)) != 0;
			result.MinPrecision = ((token >> 12) & 0XC);
			result.SwizzleComponents = ((token >> 16) & 0xFF);
			result.SourceModifier = (SourceModifier)((token >> 24) & 0xF);

			reader.AddNote("RegisterNumber", result.RegisterNumber);
			reader.AddNote("RegisterType", result.RegisterType);
			reader.AddNote("IsRelativeAddressMode", result.IsRelativeAddressMode);
			reader.AddNote("MinPrecision", result.MinPrecision);
			reader.AddNote("SwizzleComponents", result.SwizzleComponents);
			reader.AddNote("SourceModifier", result.SourceModifier);
			return result;
		}
		public override string ToString()
		{
			return DebugOperandUtil.OperandToString(RegisterType, RegisterNumber);
		}
	}
}
