namespace DXDecompiler.DebugParser.Dxil
{
	public class DebugDxilBaseChunk : DebugBytecodeChunk
	{
		public DebugShaderVersion Version { get; private set; }
		public uint DxilVersion { get; private set; }
		public byte[] Bitcode { get; private set; }
		protected void ParseInstance(DebugBytecodeReader reader, uint chunkSize)
		{
			Version = DebugShaderVersion.ParseShex(reader);
			var sizeInUint32 = reader.ReadUInt32("SizeInUint32");
			var dxilMagic = reader.ReadUInt32("DxilMagic");
			DxilVersion = reader.ReadUInt32("DxilVersion");
			var bitcodeOffset = reader.ReadUInt32("BitcodeOffset");
			var bitcodeLength = reader.ReadInt32("BitcodeLength");
			var bitcodeReader = reader.CopyAtCurrentPosition("BitcodeReader", reader);
			Bitcode = bitcodeReader.ReadBytes("Bitcode", bitcodeLength);
		}
	}
}
