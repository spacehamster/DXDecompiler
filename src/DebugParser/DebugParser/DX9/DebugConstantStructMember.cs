using DXDecompiler.DebugParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugParser.DebugParser.DX9
{
	public class DebugConstantStructMember
	{
		public string Name;
		public DebugConstantType Type;
		public DebugConstantStructMember Parse(DebugBytecodeReader reader, DebugBytecodeReader memberReader) {
			var result = new DebugConstantStructMember();
			var nameOffset = memberReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", memberReader, (int)nameOffset);
			result.Name = Name;
			var typeOffset = memberReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeREader", memberReader, (int)typeOffset);
			result.Type = DebugConstantType.Parse(reader, typeReader);
			return result;
		}
	}
}
