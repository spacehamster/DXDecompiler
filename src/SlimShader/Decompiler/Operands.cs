using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks;
using SlimShader.Util;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using System.Linq;
using System;
using System.Text;

namespace SlimShader.Decompiler
{
	public partial class DXDecompiler
	{
		string GetOperandIndex(Operand operand)
		{
			string index = string.Empty;
			switch (operand.IndexDimension)
			{
				case OperandIndexDimension._0D:
					break;
				case OperandIndexDimension._1D:
					index = (operand.Indices[0].Representation == OperandIndexRepresentation.Relative
						|| operand.Indices[0].Representation == OperandIndexRepresentation.Immediate32PlusRelative
						|| !operand.OperandType.RequiresRegisterNumberFor1DIndex())
						? string.Format("1d[{0}]", operand.Indices[0])
						: operand.Indices[0].ToString();
					break;
				case OperandIndexDimension._2D:
					index = (operand.Indices[0].Representation == OperandIndexRepresentation.Relative
						|| operand.Indices[0].Representation == OperandIndexRepresentation.Immediate32PlusRelative
						|| !operand.OperandType.RequiresRegisterNumberFor2DIndex())
						? string.Format("2d1[{0}][{1}]", operand.Indices[0], operand.Indices[1])
						: string.Format("2d2{0}[{1}]", operand.Indices[0], operand.Indices[1]);
					break;
				case OperandIndexDimension._3D:
					break;
			}
			return index;
		}
		string GetOperandName(Operand operand)
		{
			try
			{
				switch (operand.OperandType)
				{
					case OperandType.Immediate32:
					case OperandType.Immediate64:
						{
							var parentType = operand.ParentType;
							var numComponents = operand.NumComponents;
							var immediateValues = operand.ImmediateValues;
							if (numComponents == 1)
							{
								return operand.OperandType == OperandType.Immediate64
									? immediateValues.GetDouble(0).ToString()
									: immediateValues.GetNumber(0).ToString(parentType.GetNumberType());
							}
							string result = (operand.OperandType == OperandType.Immediate64) ?
								$"double{numComponents}(" :
								$"float{numComponents}(";
							for (int i = 0; i < numComponents; i++)
							{
								result += operand.OperandType == OperandType.Immediate64
									? immediateValues.GetDouble(i).ToString()
									: immediateValues.GetNumber(i).ToString(parentType.GetNumberType());
								if (i < numComponents - 1)
								{
									result += ", ";
								}
							}
							result += ")";
							return result;
						}
					case OperandType.Temp:
						return $"r{operand.Indices[0].Value}";
					case OperandType.InputCoverageMask:
						{
							var reg = RegisterState.GetRegister(operand);
							return $"input.{reg.Name}";
						}
					case OperandType.Input:
						{
							var input = Container.InputSignature.Parameters
								.Single(p => p.Register == operand.Indices[0].Value &&
								p.ReadWriteMask.HasFlag(operand.GetUsedComponents()));
							return $"input.{input.GetName()}";
						}
					case OperandType.InputControlPoint:
						{
							var input = Container.InputSignature.Parameters
								.Single(p => p.Register == operand.Indices[1].Value);
							return $"{operand.OperandType.GetDescription()}[{operand.Indices[1].Value}].field{operand.Indices[1].Value}{input.ReadWriteMask.GetDescription().Trim()}";
						}
					case OperandType.Output:
						{
							var elements = Container.OutputSignature.Parameters
								.Where(p => p.Register == operand.Indices[0].Value &&
								p.ReadWriteMask.HasFlag(operand.GetUsedComponents()))
								.ToArray();
							var output = elements.Length == 1 ? elements[0] : null;
							return $"output.{output.GetName()}";
						}
					case OperandType.ConstantBuffer:
						{
							//return RegisterState.GetRegister(operand).Name;
							uint bindPoint = (uint)operand.Indices[0].Value;
							uint fieldIndex = (uint)operand.Indices[1].Value;
							var cb = GetConstantBuffer(OperandType.ConstantBuffer, bindPoint);
							var offset = fieldIndex * 16;
							GetShaderVariableByOffset(cb, offset, out string fullname);
							if (fullname == null)
							{
								GetShaderVariableByOffset(cb, offset, out string foo);
								return $"cb{bindPoint}[{fieldIndex}]";
							}
							return fullname;
						}
					case OperandType.Interface:
						{
							var register = RegisterState.GetRegister(operand);
							return register?.Name ?? operand.ToString();
						}
					case OperandType.Resource:
						{
							var binding = GetResourceBinding(operand.OperandType, (uint)operand.Indices[0].Value);
							return binding.Name;
						}
					case OperandType.Sampler:
						{
							var binding = GetResourceBinding(operand.OperandType, (uint)operand.Indices[0].Value);
							return binding.Name;
						}
					case OperandType.ImmediateConstantBuffer:
						{
							var reg = GetOperandDescriptionWithMask(operand.Indices[0].Register, ComponentMask.All);
							return $"{operand.OperandType.GetDescription()}[{reg} + {operand.Indices[0].Value}]";
						}
					case OperandType.UnorderedAccessView:
						{
							var binding = GetResourceBinding(operand.OperandType, (uint)operand.Indices[0].Value);
							return binding.Name;
						}
				}
			} catch (Exception ex)
			{
				return $"error_{operand.ToString()}";
			}
			return $"{operand.OperandType.GetDescription()}{GetOperandIndex(operand)}";
		}
		void WriteOperand(Operand operand)
		{
			WriteOperandWithMask(operand, ComponentMask.All);
		}
		void WriteOperandWithMask(Operand operand, ComponentMask mask)
		{
			Output.Append(GetOperandDescriptionWithMask(operand, mask));
		}
		string GetOperandDescriptionWithMask(Operand operand, ComponentMask mask)
		{
			var name = GetOperandName(operand);
			var swizzle = GetSwizzle(operand, mask);
			return $"{name}{swizzle}";
		}
		string GetSwizzle(Operand operand, ComponentMask mask)
		{
			string components = string.Empty;
			var parentType = operand.ParentType;
			var numComponents = operand.NumComponents;
			var immediateValues = operand.ImmediateValues;
			var sb = new StringBuilder();
			if(mask == ComponentMask.None)
			{
				return "";
			}
			if (operand.ParentType != OpcodeType.DclConstantBuffer)
			{
				switch (operand.SelectionMode)
				{
					case Operand4ComponentSelectionMode.Mask:
						var newMask = operand.ComponentMask & mask;
						components = newMask.GetDescription();
						break;
					case Operand4ComponentSelectionMode.Swizzle:
						components = operand.Swizzles[0].GetDescription()
							+ operand.Swizzles[1].GetDescription()
							+ operand.Swizzles[2].GetDescription()
							+ operand.Swizzles[3].GetDescription();
						break;
					case Operand4ComponentSelectionMode.Select1:
						components = operand.Swizzles[0].GetDescription();
						break;
					default:
						throw new InvalidOperationException("Unrecognised selection mode: " + operand.SelectionMode);
				}
				if (!string.IsNullOrEmpty(components))
					components = "." + components;
				sb.Append(components);
			}
			return sb.ToString();
		}
		internal ShaderTypeMember GetShaderVariableByOffset(ConstantBuffer cb, uint offset, out string fullname)
		{
			foreach (var variable in cb.Variables)
			{
				bool isArray = false;
				var result = IsOffsetInType(variable.Member, 0, offset, out fullname, out isArray);
				if (result != null)
				{
					return result;
				}
			}
			fullname = null;
			return null;
		}
		internal ShaderTypeMember IsOffsetInType(ShaderTypeMember member, uint parentOffset, uint offsetToFind, out string fullName, out bool isArray)
		{
			uint thisOffset = parentOffset + member.Offset;
			uint thisSize = member.GetCBVarSize();
			uint paddedSize = ((thisSize + 15) / 16) * 16;
			uint arraySize = thisSize;
			fullName = null;
			if (member.Type.ElementCount > 1)
			{
				arraySize = (paddedSize * ((uint)member.Type.ElementCount - 1)) + thisSize;
			}
			isArray = false;
			if (offsetToFind < thisOffset || offsetToFind >= (thisOffset + arraySize))
			{
				return null;
			}
			string arrayIndex = "";
			if (member.Type.VariableClass == ShaderVariableClass.Struct)
			{
				uint offsetInStruct = (offsetToFind - thisOffset) % paddedSize;
				foreach (var child in member.Type.Members)
				{
					var foundType = IsOffsetInType(child, thisOffset, thisOffset + offsetInStruct, out string childName, out isArray);
					if (foundType != null)
					{
						if (child.Type.ElementCount > 1)
						{
							arrayIndex = $"[{(offsetToFind - thisOffset) / thisSize}]";
						}
						fullName = $"{member.Name}{arrayIndex}.{childName}";
						return foundType;
					}
				}
			}
			else if ((member.Type.VariableClass == ShaderVariableClass.MatrixRows ||
			  member.Type.VariableClass == ShaderVariableClass.MatrixColumns) ||
			  ((member.Type.VariableClass == ShaderVariableClass.Scalar ||
			  member.Type.VariableClass == ShaderVariableClass.Vector) &&
			  member.Type.ElementCount > 1))
			{
				isArray = true;
				arrayIndex = $"[{(offsetToFind - thisOffset) / 16}]";
			}
			else if (member.Type.VariableClass == ShaderVariableClass.Vector)
			{
				//Check for vector starting at a non-vec4 offset.
			}
			fullName = $"{member.Name}{arrayIndex}";
			return member;
		}
	}
}
