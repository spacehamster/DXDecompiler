using System;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrIndexOperand : IrOperand
	{
		public IrOperand Base;
		public IrOperand Index;
	}
}
