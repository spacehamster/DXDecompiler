using SlimShader.Util;
using System;
using System.Diagnostics;
using System.Text;

namespace SlimShader.Chunks.Aon9
{
	public class RuntimeConstantMapping
	{
		public ushort TargetReg;
		public RuntimeConstantDescription ConstantDescription;
		public static RuntimeConstantMapping Parse(BytecodeReader reader)
		{
			var result = new RuntimeConstantMapping();
			result.ConstantDescription = (RuntimeConstantDescription)reader.ReadUInt16();
			result.TargetReg = reader.ReadUInt16();
			Debug.Assert(
				Enum.IsDefined(typeof(RuntimeConstantDescription), result.ConstantDescription),
				$"Unknown RuntimeConstantDescription {result.ConstantDescription}");
			return result;
		}
		public override string ToString()
		{
			return string.Format("// c{0, -9} {1, 50}",
				TargetReg, ConstantDescription.GetDescription());
		}
	}
}
