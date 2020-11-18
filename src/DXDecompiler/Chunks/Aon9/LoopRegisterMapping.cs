using DXDecompiler.Util;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Aon9
{
	public class LoopRegisterMapping
	{
		public ushort TargetReg { get; private set; }
		public ushort Buffer { get; private set; }
		public ushort SourceReg { get; private set; }
		public ushort Component { get; private set; }

		public static LoopRegisterMapping Parse(BytecodeReader reader)
		{
			var result = new LoopRegisterMapping();
			result.Buffer = reader.ReadUInt16();
			result.SourceReg = reader.ReadUInt16();
			result.Component = reader.ReadUInt16();
			result.TargetReg = reader.ReadUInt16();
			return result;
		}
		public override string ToString()
		{
			return string.Format("// i{0, -9} cb{1, -5} {2, 10} {3, 9}",
				TargetReg, Buffer, SourceReg, Component);
		}
	}
}
