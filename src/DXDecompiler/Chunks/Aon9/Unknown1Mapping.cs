using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Aon9
{
	public class Unknown1Mapping
	{
		public static Unknown1Mapping Parse(BytecodeReader reader)
		{ 
			var result = new Unknown1Mapping();
			return result;
		}

		public override string ToString()
		{
			return string.Format("// Unknown1Mapping mappings:");
		}
	}
}
