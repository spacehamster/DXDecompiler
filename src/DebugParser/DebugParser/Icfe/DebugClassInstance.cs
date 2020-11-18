using System;
using System.Diagnostics;

namespace DXDecompiler.DebugParser.Icfe
{
	public class DebugClassInstance
	{
		public string Name { get; private set; }
		public ushort Type { get; private set; }
		public ushort ConstantBuffer { get; private set; }
		public ushort ConstantBufferOffset { get; private set; }
		public ushort Texture { get; private set; }
		public ushort Sampler { get; private set; }

		public static DebugClassInstance Parse(DebugBytecodeReader reader, DebugBytecodeReader classInstanceReader)
		{
			var nameOffset = classInstanceReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", classInstanceReader, (int)nameOffset);
			var name = nameReader.ReadString("nameReader");

			var type = classInstanceReader.ReadUInt16("type");
			var unknown = classInstanceReader.ReadUInt16("ClassInstanceUnknown");

			return new DebugClassInstance
			{
				Name = name,
				Type = type,
				ConstantBuffer = classInstanceReader.ReadUInt16("ConstantBuffer"),
				ConstantBufferOffset = classInstanceReader.ReadUInt16("ConstantBufferOffset"),
				Texture = classInstanceReader.ReadUInt16("Texture"),
				Sampler = classInstanceReader.ReadUInt16("Sampler")
			};
		}
	}
}