using SlimShader.Chunks.Aon9;

namespace SlimShader.DebugParser.Aon9
{
	public class DebugRuntimeConstantMapping
	{
		public ushort TargetReg;
		public RuntimeConstantDescription ConstantDescription;
		public static DebugRuntimeConstantMapping Parse(DebugBytecodeReader reader)
		{
			var result = new DebugRuntimeConstantMapping();
			result.ConstantDescription = reader.ReadEnum16<RuntimeConstantDescription>("ConstantDescription");
			result.TargetReg = reader.ReadUInt16("TargetReg");
			return result;
		}
	}
}