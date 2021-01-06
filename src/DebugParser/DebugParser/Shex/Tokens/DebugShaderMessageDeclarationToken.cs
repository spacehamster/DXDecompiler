using DXDecompiler.Chunks.Shex;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugShaderMessageDeclarationToken : DebugImmediateDeclarationToken
	{
		public uint InfoQueueMessageID;
		public ShaderMessageFormat MessageFormat;
		public uint NumCharacters;
		public uint NumOperands;
		public uint OperandsLength;
		public List<DebugOperand> Operands;
		public string Format;

		public DebugShaderMessageDeclarationToken()
		{
			Operands = new List<DebugOperand>();
		}

		public static DebugShaderMessageDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var member = reader.LocalMembers.Last();
			var length = reader.ReadUInt32("length") - 2;

			var result = new DebugShaderMessageDeclarationToken
			{
				DeclarationLength = length,
				InfoQueueMessageID = reader.ReadUInt32("InfoQueueMessageID"),
				MessageFormat = reader.ReadEnum32<ShaderMessageFormat>("MessageFormat"),
				NumCharacters = reader.ReadUInt32("NumCharacters"),
				NumOperands = reader.ReadUInt32("NumOperands"),
				OperandsLength = reader.ReadUInt32("OperandsLength")
			};

			for(int i = 0; i < result.NumOperands; i++)
				result.Operands.Add(DebugOperand.Parse(reader, OpcodeType.CustomData));

			result.Format = reader.ReadString("Format");

			// String is padded to a multiple of DWORDs.
			uint remainingBytes = (4 - ((result.NumCharacters + 1) % 4)) % 4;
			reader.ReadBytes("StringPadding", (int)remainingBytes);

			return result;
		}
	}
}
