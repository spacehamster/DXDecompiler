using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugFunctionTableDeclarationToken : DebugDeclarationToken
	{
		public uint Identifier;
		public List<uint> FunctionBodyIndices;

		public DebugFunctionTableDeclarationToken()
		{
			FunctionBodyIndices = new List<uint>();
		}

		public static DebugFunctionTableDeclarationToken Parse(DebugBytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var result = new DebugFunctionTableDeclarationToken
			{
				Identifier = reader.ReadUInt32("Identifier")
			};

			uint tableLength = reader.ReadUInt32("tableLength");
			for (int i = 0; i < tableLength; i++)
				result.FunctionBodyIndices.Add(reader.ReadUInt32($"FunctionBodyIndices[{i}]"));

			return result;
		}
	}
}
