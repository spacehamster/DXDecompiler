using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrResourceOperand : IrOperand
	{
		public IrResourceType ResourceType;
		public int RangeId;
		public int IndexId;
		public IrResourceOperand(IrResourceType resourceType, int rangeId, int indexId)
		{
			ResourceType = resourceType;
			RangeId = rangeId;
			IndexId = indexId;
		}
		public IrResourceOperand(IrResourceType resourceType, int regIndex)
		{
			ResourceType = resourceType;
			IndexId = regIndex;
		}
	}
}
