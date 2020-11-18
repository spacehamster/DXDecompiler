using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Ifce
{
	public class ClassInstance
	{
		public string Name { get; private set; }
		public ushort Type { get; private set; }
		public ushort ConstantBuffer { get; private set; }
		public ushort ConstantBufferOffset { get; private set; }
		public ushort Texture { get; private set; }
		public ushort Sampler { get; private set; }
		public ushort ElementCount { get; private set; }

		public static ClassInstance Parse(BytecodeReader reader, BytecodeReader classInstanceReader)
		{
			var nameOffset = classInstanceReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			var name = nameReader.ReadString();
			return new ClassInstance
			{
				Name = name,
				Type = classInstanceReader.ReadUInt16(),
				ElementCount = classInstanceReader.ReadUInt16(),
				ConstantBuffer = classInstanceReader.ReadUInt16(),
				ConstantBufferOffset = classInstanceReader.ReadUInt16(),
				Texture = classInstanceReader.ReadUInt16(),
				Sampler = classInstanceReader.ReadUInt16(),
			};
		}

		public override string ToString()
		{
			// For example:
			// Name                        Type CB CB Offset Texture Sampler
			// --------------------------- ---- -- --------- ------- -------
			// g_ambientLight                12  0         0       -       -
			var arrayFormat = "";
			if(ElementCount > 1)
			{
				arrayFormat = string.Format("[{0}]", ElementCount);
			}
			string name = string.Format("{0}{1}", Name, arrayFormat);
			return string.Format("{0,-27} {1,4} {2,2} {3,9} {4,7} {5,7}",
				name, Type, ConstantBuffer, ConstantBufferOffset,
				(Texture == 0xFFFF) ? "-" : Texture.ToString(),
				(Sampler == 0xFFFF) ? "-" : Sampler.ToString());
		}
	}
}