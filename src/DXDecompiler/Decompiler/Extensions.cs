using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Chunks.RTS0;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Chunks.Xsgn;
using System;
using System.Linq;

namespace DXDecompiler.Decompiler
{
	internal static class Extensions
	{
		public static int GetNumSwizzleElements(this Operand operand)
		{
			return operand.GetNumSwizzleElementsWithMask(ComponentMask.All);
		}
		public static int GetNumSwizzleElementsWithMask(this Operand operand, ComponentMask mask)
		{
			switch(operand.OperandType)
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
			if(operand.NumComponents != 1)
			{
				if(operand.SelectionMode == Operand4ComponentSelectionMode.Mask)
				{
					var compMask = operand.ComponentMask & mask;
					if(compMask == ComponentMask.All) return 4;
					if(compMask.HasFlag(ComponentMask.X))
					{
						count++;
					}
					if(compMask.HasFlag(ComponentMask.Y))
					{
						count++;
					}
					if(compMask.HasFlag(ComponentMask.Z))
					{
						count++;
					}
					if(compMask.HasFlag(ComponentMask.W))
					{
						count++;
					}
				}
				else if(operand.SelectionMode == Operand4ComponentSelectionMode.Swizzle)
				{
					for(int i = 0; i < 4; i++)
					{
						if(((int)mask & (1 << i)) == 0)
							continue;
						if(operand.Swizzles[0] == Operand4ComponentName.X)
						{
							count++;
						}
						if(operand.Swizzles[0] == Operand4ComponentName.Y)
						{
							count++;
						}
						if(operand.Swizzles[0] == Operand4ComponentName.Z)
						{
							count++;
						}
						if(operand.Swizzles[0] == Operand4ComponentName.W)
						{
							count++;
						}
					}
				}
				else if(operand.SelectionMode == Operand4ComponentSelectionMode.Select1)
				{
					if(operand.Swizzles[0] == Operand4ComponentName.X && mask.HasFlag(ComponentMask.X))
					{
						count++;
					}
					if(operand.Swizzles[0] == Operand4ComponentName.Y && mask.HasFlag(ComponentMask.Y))
					{
						count++;
					}
					if(operand.Swizzles[0] == Operand4ComponentName.Z && mask.HasFlag(ComponentMask.Z))
					{
						count++;
					}
					if(operand.Swizzles[0] == Operand4ComponentName.W && mask.HasFlag(ComponentMask.W))
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
		public static ComponentMask GetUsedComponents(this Operand operand)
		{
			if(operand.ParentType == OpcodeType.DclConstantBuffer)
			{
				return ComponentMask.None;
			}
			switch(operand.SelectionMode)
			{
				case Operand4ComponentSelectionMode.Mask:
					return operand.ComponentMask;
				case Operand4ComponentSelectionMode.Swizzle:
					var mask = ComponentMask.None;
					mask |= operand.Swizzles[0].ToComponentMask();
					mask |= operand.Swizzles[1].ToComponentMask();
					mask |= operand.Swizzles[2].ToComponentMask();
					mask |= operand.Swizzles[3].ToComponentMask();
					return mask;
				case Operand4ComponentSelectionMode.Select1:
					return operand.Swizzles[0].ToComponentMask();
				default:
					throw new InvalidOperationException("Unrecognised selection mode: " + operand.SelectionMode);
			}
		}
		public static int GetNumberOfFlagsSet(this ComponentMask mask)
		{
			// Calculate number of bits in a
			// Taken from https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSet64
			// Works only up to 14 bits (we're only using up to 4)
			ulong c = (((ulong)mask) * 0x200040008001 & 0x111111111111111) % 0xf;
			return (int)c;
		}
		public static ComponentMask ToComponentMask(this Operand4ComponentName name)
		{
			switch(name)
			{
				case Operand4ComponentName.X:
					return ComponentMask.X;
				case Operand4ComponentName.Y:
					return ComponentMask.Y;
				case Operand4ComponentName.Z:
					return ComponentMask.Z;
				case Operand4ComponentName.W:
					return ComponentMask.W;
				default:
					throw new ArgumentException();
			}
		}
		public static string GetStreamName(this PrimitiveTopology topology)
		{
			switch(topology)
			{
				case PrimitiveTopology.PointList:
					return "PointStream";
				case PrimitiveTopology.TriangleStrip:
					return "TriangleStream";
				default:
					return topology.ToString();
			}
		}
		public static string GetAttributeName(this TessellatorDomain topology)
		{
			switch(topology)
			{
				case TessellatorDomain.Isoline:
					return "isoline";
				case TessellatorDomain.Triangle:
					return "triangle";
				case TessellatorDomain.Quadrilateral:
					return "quad";
				default:
					return topology.ToString();
			}
		}
		public static bool IsStructured(this ShaderInputType shaderInputType)
		{
			switch(shaderInputType)
			{
				case ShaderInputType.Structured:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
					return true;
				default:
					return false;
			}
		}
		public static bool IsUav(this ShaderInputType shaderInputType)
		{
			switch(shaderInputType)
			{
				case ShaderInputType.UavRwTyped:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavRwByteAddress:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
					return true;
				default:
					return false;
			}
		}
		public static string GetTypeName(this MinPrecision minPrecision)
		{
			switch(minPrecision)
			{
				case MinPrecision.Min16f:
					return "min16float";
				case MinPrecision.Min10f:
					return "min10float";
				case MinPrecision.Min12i:
					return "min12int";
				case MinPrecision.Min16i:
					return "min16int";
				case MinPrecision.Min16u:
					return "min16uint";
				default:
					throw new ArgumentException($"Invalid MinPrecision {minPrecision}");
			}
		}
		public static uint GetCBVarSize(this ShaderTypeMember member, bool wholeArraySize = false)
		{
			uint size;
			if(member.Type.VariableClass == ShaderVariableClass.Struct)
			{
				var last = member.Type.Members.Last();
				size = last.Offset + GetCBVarSize(last, true);
			}
			else if(member.Type.VariableClass == ShaderVariableClass.MatrixRows)
			{
				var columns = (uint)member.Type.Columns;
				var rows = (uint)member.Type.Rows;
				size = (rows - 1) * 16 + columns * 4;
			}
			else if(member.Type.VariableClass == ShaderVariableClass.MatrixColumns)
			{
				var columns = (uint)member.Type.Columns;
				var rows = (uint)member.Type.Rows;
				size = (columns - 1) * 16 + rows * 4;
			}
			else
			{
				size = (uint)member.Type.Columns * (uint)member.Type.Rows * 4;
			}
			if(wholeArraySize && member.Type.ElementCount > 1)
			{
				uint paddedSize = ((size + 15) / 16) * 16; // Arrays are padded to float4 size
				size = ((uint)member.Type.ElementCount - 1) * paddedSize + size; // Except the last element
			}
			return size;
		}
		public static string GetName(this SignatureParameterDescription param)
		{
			return $"{param.SemanticName.ToLower()}{param.SemanticIndex}";
		}
		public static ConstantBufferType ToCBType(this ShaderInputType type)
		{
			switch(type)
			{
				case ShaderInputType.CBuffer:
					return ConstantBufferType.ConstantBuffer;
				case ShaderInputType.TBuffer:
					return ConstantBufferType.TextureBuffer;
				default:
					return ConstantBufferType.ResourceBindInformation;
			}
		}
		public static string GetRegisterName(this RootParameterType type)
		{
			switch(type)
			{
				case RootParameterType._32BitConstants:
				case RootParameterType.Cbv:
					return "b";
				case RootParameterType.Srv:
					return "t";
				case RootParameterType.Uav:
					return "u";
				default:
					throw new InvalidOperationException($"Register name not supported for Root Parameter Type {type}");
			}
		}
		public static string GetRegisterName(this DescriptorRangeType type)
		{
			switch(type)
			{
				case DescriptorRangeType.Sampler:
					return "s";
				case DescriptorRangeType.Cbv:
					return "b";
				case DescriptorRangeType.Srv:
					return "t";
				case DescriptorRangeType.Uav:
					return "u";
				default:
					throw new InvalidOperationException($"Register name not supported for Descriptor Range Type {type}");
			}
		}
	}

}
