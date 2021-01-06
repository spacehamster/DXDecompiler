using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System.Diagnostics;

namespace DXDecompiler.Chunks.Dxil
{
	public class DxilBaseChunk : BytecodeChunk
	{
		public ShaderVersion Version { get; private set; }
		public uint DxilVersion { get; private set; }
		public byte[] Bitcode { get; private set; }
		protected void ParseInstance(BytecodeReader reader, uint chunkSize)
		{
			Version = ShaderVersion.ParseShex(reader);
			var sizeInUint32 = reader.ReadUInt32();
			var dxilMagic = reader.ReadUInt32();
			DxilVersion = reader.ReadUInt32();
			var bitcodeOffset = reader.ReadUInt32();
			Debug.Assert(bitcodeOffset == 16, "Unexpected bitcode offset found");
			var bitcodeLength = reader.ReadInt32();
			var bitcodeReader = reader.CopyAtCurrentPosition();
			Bitcode = bitcodeReader.ReadBytes(bitcodeLength);
		}
	}
}
