using System;

namespace SlimShader.DebugParser.Icfe
{
	public class DebugClassType
	{
		public string Name { get; set; }
		public uint ID { get; set; }
		public uint ConstantBufferStride { get; set; }
		public uint Texture { get; set; }
		public uint Sampler { get; set; }

		public static DebugClassType Parse(DebugBytecodeReader reader, DebugBytecodeReader classTypeReader)
		{
			var nameOffset = classTypeReader.ReadUInt32("nameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", classTypeReader, (int)nameOffset);

			return new DebugClassType
			{
				Name = nameReader.ReadString("Name"),
				ID = classTypeReader.ReadUInt16("ID"),
				ConstantBufferStride = classTypeReader.ReadUInt16("ConstantBufferStride"),
				Texture = classTypeReader.ReadUInt16("Texture"),
				Sampler = classTypeReader.ReadUInt16("Sampler")
			};
		}
	}
}