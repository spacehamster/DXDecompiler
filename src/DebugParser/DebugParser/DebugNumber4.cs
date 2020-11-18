using System;
using System.Runtime.InteropServices;

namespace DXDecompiler.DebugParser
{
	[StructLayout(LayoutKind.Explicit, Size = DebugNumber.SizeInBytes * 4)]
	public struct DebugNumber4
	{
		[FieldOffset(0)]
		public DebugNumber Number0;
		[FieldOffset(DebugNumber.SizeInBytes * 1)]
		public DebugNumber Number1;
		[FieldOffset(DebugNumber.SizeInBytes * 2)]
		public DebugNumber Number2;
		[FieldOffset(DebugNumber.SizeInBytes * 3)]
		public DebugNumber Number3;

		[FieldOffset(0)]
		public double Double0;
		[FieldOffset(sizeof(double))]
		public double Double1;

		public void SetNumber(int i, DebugNumber value)
		{
			switch(i)
			{
				case 0:
					Number0 = value;
					break;
				case 1:
					Number1 = value;
					break;
				case 2:
					Number2 = value;
					break;
				case 3:
					Number3 = value;
					break;
				default:
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}
		public void SetDouble(int i, double value)
		{
			switch(i)
			{
				case 0:
					Double0 = value;
					break;
				case 1:
					Double1 = value;
					break;
				default:
					throw new ArgumentOutOfRangeException("i", string.Format("Index '{0}' is out of range.", i));
			}
		}
	}
}
