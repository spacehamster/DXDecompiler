using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Decompiler
{
	internal static class Extentsions
	{
		public static int GetNumSwizzleElements(this Operand operand)
		{
			return operand.GetNumSwizzleElementsWithMask(ComponentMask.All);
		}
		public static int GetNumSwizzleElementsWithMask(this Operand operand, ComponentMask mask)
		{
			switch (operand.OperandType)
			{
				case OperandType.InputThreadIDInGroupFlattened:
					return 1;
				case OperandType.InputThreadIDInGroup:
				case OperandType.InputThreadID:
				case OperandType.InputThreadGroupID:
					return operand.NumComponents;
				case OperandType.Immediate32:
				case OperandType.Immediate64:
				case OperandType.OutputDepthGreaterEqual:
				case OperandType.OutputDepthLessEqual:
				case OperandType.OutputDepth:
					// Translate numComponents into bitmask
					// 1 -> 1, 2 -> 3, 3 -> 7 and 4 -> 15
					ComponentMask compMask = (ComponentMask)(1 << operand.NumComponents) - 1;
					compMask &= mask;
					return compMask.GetNumberOfFlagsSet();
			}
			int count = 0;
			if (operand.NumComponents != 1)
			{
				if (operand.SelectionMode == Operand4ComponentSelectionMode.Mask)
				{
					var compMask = operand.ComponentMask & mask;
					if (compMask == ComponentMask.All) return 4;
					if (compMask.HasFlag(ComponentMask.X))
					{
						count++;
					}
					if (compMask.HasFlag(ComponentMask.Y))
					{
						count++;
					}
					if (compMask.HasFlag(ComponentMask.Z))
					{
						count++;
					}
					if (compMask.HasFlag(ComponentMask.W))
					{
						count++;
					}
				}
				else if (operand.SelectionMode == Operand4ComponentSelectionMode.Swizzle)
				{
					//TODO: rest of function
				}
				else if (operand.SelectionMode == Operand4ComponentSelectionMode.Select1)
				{
					if (operand.Swizzles[0] == Operand4ComponentName.X && mask.HasFlag(ComponentMask.X))
					{
						count++;
					}
					if (operand.Swizzles[0] == Operand4ComponentName.Y && mask.HasFlag(ComponentMask.Y))
					{
						count++;
					}
					if (operand.Swizzles[0] == Operand4ComponentName.Z && mask.HasFlag(ComponentMask.Y))
					{
						count++;
					}
					if (operand.Swizzles[0] == Operand4ComponentName.W && mask.HasFlag(ComponentMask.W))
					{
						count++;
					}
				}
			}
			if(count == 0)
			{
				var compMask = (ComponentMask)((1 << operand.NumComponents) - 1);
				compMask &= mask;
				return compMask.GetNumberOfFlagsSet();
			}
			return count;
		}
		public static int GetNumberOfFlagsSet(this ComponentMask mask)
		{
			// Calculate number of bits in a
			// Taken from https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSet64
			// Works only up to 14 bits (we're only using up to 4)
			ulong c = (((ulong)mask) * 0x200040008001 & 0x111111111111111) % 0xf;
			return (int)c;
		}
	}
}
