using SlimShader.DebugParser;
using SlimShader.DX9Shader;
using System.Linq;

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

			var debugEntry = reader.Members.Last();
			debugEntry.AddNote("RegisterNumber", result.RegisterNumber.ToString());
			debugEntry.AddNote("RegisterType", result.RegisterType.ToString());
			debugEntry.AddNote("IsRelativeAddressMode", result.IsRelativeAddressMode.ToString());
			debugEntry.AddNote("MinPrecision", result.MinPrecision.ToString());
			debugEntry.AddNote("SwizzleComponents", result.SwizzleComponents.ToString());
			debugEntry.AddNote("SourceModifier", result.SourceModifier.ToString());
			return result;
		}
		public override string ToString()
		{
			return DebugOperandUtil.OperandToString(RegisterType, RegisterNumber);
		}
	}
}
