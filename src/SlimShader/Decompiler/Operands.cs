using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks;
using SlimShader.Util;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using System.Linq;
using System;

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
				case OperandType.Input:
					{
						return $"input.field{operand.Indices[0].Value}";
					}
				case OperandType.Output:
					{
						return $"output.field{operand.Indices[0].Value}";
					}
				case OperandType.ConstantBuffer:
					{
						int bufferIndex = (int)operand.Indices[0].Value;
						int fieldIndex = (int)operand.Indices[1].Value;
						var cb = GetConstantBuffer(ConstantBufferType.ConstantBuffer, bufferIndex);
						var index = fieldIndex * 16;
						var variable = cb.Variables.First(
								v => index >= v.StartOffset &&
								index < (v.StartOffset + v.Size));
						string name = variable.Name;
						if (variable.ShaderType.VariableClass == ShaderVariableClass.MatrixColumns ||
							variable.ShaderType.VariableClass == ShaderVariableClass.MatrixRows)
						{
							name += $"[{(index - variable.StartOffset) / 16}]";
						}
						return name;
					}
				case OperandType.Resource:
					{
						var resourceIndex = operand.Indices[0].Value;
						var binding = Container.ResourceDefinition.ResourceBindings
							.First(rb =>
								rb.Type == ShaderInputType.Texture &&
								rb.BindPoint == resourceIndex
							);
						return binding.Name;
					}
				case OperandType.Sampler:
					{
						var resourceIndex = operand.Indices[0].Value;
						var binding = Container.ResourceDefinition.ResourceBindings
							.First(rb =>
								rb.Type == ShaderInputType.Sampler &&
								rb.BindPoint == resourceIndex
							);
						return binding.Name;
					}
			}
			return $"{operand.OperandType.GetDescription()} {GetOperandIndex(operand)}";
		}
		void WriteOperand(Operand operand)
		{
			var name = GetOperandName(operand);
			Output.Append(name);
			WriteSwizzle(operand);
		}
		void WriteSwizzle(Operand operand)
		{
			string components = string.Empty;
			var parentType = operand.ParentType;
			var numComponents = operand.NumComponents;
			var immediateValues = operand.ImmediateValues;
			if (operand.ParentType != OpcodeType.DclConstantBuffer)
			{
				switch (operand.SelectionMode)
				{
					case Operand4ComponentSelectionMode.Mask:
						components = operand.ComponentMask.GetDescription();
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
				Output.Append(components);
			}
		}
	}
}
