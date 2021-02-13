using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR
{
	public class IrClass
	{
		public List<IrPass> Passes = new List<IrPass>();
		public string Name;
	}
}
