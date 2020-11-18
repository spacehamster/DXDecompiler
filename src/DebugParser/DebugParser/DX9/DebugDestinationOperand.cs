using DXDecompiler.DebugParser;
using DXDecompiler.DX9Shader;
using System.Linq;

namespace DebugParser.DebugParser.DX9
{
	public class DebugDestinationOperand : IDebugOperand
	{
		public uint WordCount { get; set; }

		public uint RegisterNumber;
		public RegisterType RegisterType;
		public uint MinPrecision;
		public ComponentFlags DestinationWriteMask;
		public ResultModifier ResultModifier;

		public static DebugDestinationOperand Parse(DebugBytecodeReader reader, uint index)
		{
			var token = reader.ReadUInt32($"Operand {index}");
			var result = new DebugDestinationOperand();
			result.WordCount = 1;

			result.RegisterNumber = token & 0x7FF;
			result.RegisterType = (RegisterType)(((token >> 28) & 0x7) | ((token >> 8) & 0x18));
			result.MinPrecision = (token >> 12) & 0XC;
			result.DestinationWriteMask = (ComponentFlags)((token >> 16) & 0xF);
			result.ResultModifier = (ResultModifier)((token >> 20) & 0xF);

			reader.AddNote("RegisterNumber", result.RegisterNumber);
			reader.AddNote("RegisterType", result.RegisterType);
			reader.AddNote("MinPrecision", result.MinPrecision);
			reader.AddNote("DestinationWriteMask", result.DestinationWriteMask);
			reader.AddNote("ResultModifier", result.ResultModifier);
			return result;
		}
		public override string ToString()
		{
			return DebugOperandUtil.OperandToString(RegisterType, RegisterNumber);
		}
	}
}
