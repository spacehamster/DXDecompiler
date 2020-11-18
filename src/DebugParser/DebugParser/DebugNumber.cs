using System.Runtime.InteropServices;

namespace DXDecompiler.DebugParser
{
	[StructLayout(LayoutKind.Explicit, Size = SizeInBytes)]
	public struct DebugNumber
	{
		public const int SizeInBytes = sizeof(byte) * 4;

		[FieldOffset(0)]
		public byte Byte0;
		[FieldOffset(1)]
		public byte Byte1;
		[FieldOffset(2)]
		public byte Byte2;
		[FieldOffset(3)]
		public byte Byte3;
		[FieldOffset(0)]
		public int Int;
		[FieldOffset(0)]
		public uint UInt;
		[FieldOffset(0)]
		public float Float;

		public DebugNumber(byte[] rawBytes)
			: this()
		{
			Byte0 = rawBytes[0];
			Byte1 = rawBytes[1];
			Byte2 = rawBytes[2];
			Byte3 = rawBytes[3];
		}

		public static DebugNumber Parse(DebugBytecodeReader reader)
		{
			const int byteCount = 4;
			var bytes = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
				bytes[i] = reader.ReadByte($"NumberByte{i}");
			return new DebugNumber(bytes);
		}
	}
}
