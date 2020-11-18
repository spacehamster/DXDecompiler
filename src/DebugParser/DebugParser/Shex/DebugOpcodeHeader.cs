using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex
{
	public class DebugOpcodeHeader
	{
		public OpcodeType OpcodeType { get; set; }
		public uint Length { get; set; }
		public bool IsExtended { get; set; }

		public static void AddNotes(DebugBytecodeReader reader, uint opcodeToken0)
		{
			var opcodeType = opcodeToken0.DecodeValue<OpcodeType>(0, 10);
			var length = opcodeToken0.DecodeValue(24, 30);
			var isExtended = (opcodeToken0.DecodeValue(31, 31) == 1);
			reader.AddNote("OpcodeType", opcodeType);
			reader.AddNote("Length", length);
			reader.AddNote("IsExtended", isExtended);
		}
	}
}