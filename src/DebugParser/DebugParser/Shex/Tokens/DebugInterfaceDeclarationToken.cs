using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugInterfaceDeclarationToken : DebugDeclarationToken
	{
		public bool DynamicallyIndexed;
		public uint Identifier;
		public uint ExpectedFunctionTableLength;
		public ushort TableLength;
		public ushort ArrayLength;
		public List<uint> FunctionTableIdentifiers;

		public DebugInterfaceDeclarationToken()
		{
			FunctionTableIdentifiers = new List<uint>();
		}

		public static DebugInterfaceDeclarationToken Parse(DebugBytecodeReader reader)
		{
			uint token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("DynamicallyIndex", token0.DecodeValue(11, 11) == 1);
			var result = new DebugInterfaceDeclarationToken
			{
				DynamicallyIndexed = (token0.DecodeValue(11, 11) == 1),
				Identifier = reader.ReadUInt32("Identifier"),
				ExpectedFunctionTableLength = reader.ReadUInt32("ExpectedFunctionTableLength")
			};

			uint token3 = reader.ReadUInt32("token3");
			result.TableLength = token3.DecodeValue<ushort>(00, 15);
			result.ArrayLength = token3.DecodeValue<ushort>(16, 31);
			reader.AddNote("TableLength", result.TableLength);
			reader.AddNote("ArrayLength", result.ArrayLength);

			for (int i = 0; i < result.TableLength; i++)
				result.FunctionTableIdentifiers.Add(reader.ReadUInt32($"FunctionTableIdentifiers[{i}]"));

			return result;
		}
	}
}
