using SlimShader.Chunks.Shex;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public class DebugRawThreadGroupSharedMemoryDeclarationToken : DebugDeclarationToken
	{
		public uint ElementCount;

		public static DebugRawThreadGroupSharedMemoryDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("OpcodeType", token0.DecodeValue<OpcodeType>(0, 10));

			var result = new DebugRawThreadGroupSharedMemoryDeclarationToken
			{
				Operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				ElementCount = reader.ReadUInt32("ElementCount")
			};
			return result;
		}
	}
}
