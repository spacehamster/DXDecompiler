using DXDecompiler.Chunks.Common;
using DXDecompiler.Decompiler.IR.ResourceDefinitions;
using System;
using static DXDecompiler.Decompiler.IR.IrPass;

namespace DXDecompiler.Decompiler.IR
{
	public static class IrExtensions
	{
		public static PassType GetPassType(this ProgramType programType)
		{
			switch(programType)
			{
				case ProgramType.VertexShader:
					return PassType.VertexShader;
				case ProgramType.PixelShader:
					return PassType.PixelShader;
				case ProgramType.GeometryShader:
					return PassType.GeometryShader;
				case ProgramType.DomainShader:
					return PassType.DomainShader;
				case ProgramType.HullShader:
					return PassType.HullShaderControlPointPhase;
				case ProgramType.ComputeShader:
					return PassType.ComputeShader;
				default:
					throw new InvalidOperationException($"Could not get PassType for ProgramType {programType}");
			}
		}
		public static string GetVariableTypeName(this IrShaderVariableType variableType)
		{
			switch(variableType)
			{
				case IrShaderVariableType.Void:
					return "void";
				case IrShaderVariableType.I1:
					return "bool";
				case IrShaderVariableType.I16:
					return "int16_t";
				case IrShaderVariableType.U16:
					return "uint16_t";
				case IrShaderVariableType.I32:
					return "int";
				case IrShaderVariableType.U32:
					return "uint";
				case IrShaderVariableType.I64:
					return "int64_t";
				case IrShaderVariableType.U64:
					return "uint64_t";
				case IrShaderVariableType.F16:
					return "half";
				case IrShaderVariableType.F32:
					return "float";
				case IrShaderVariableType.F64:
					return "double";
				case IrShaderVariableType.SNormF16:
					return "snorm half";
				case IrShaderVariableType.UNormF16:
					return "unorm half";
				case IrShaderVariableType.SNormF32:
					return "snorm float";
				case IrShaderVariableType.UNormF32:
					return "unorm float";
				case IrShaderVariableType.SNormF64:
					return "snorm double";
				case IrShaderVariableType.UNormF64:
					return "unorm double";
				case IrShaderVariableType.PackedS8x32:
					return "int8_t_packed";
				case IrShaderVariableType.PackedU8x32:
					return "uint8_t_packed";
				default:
					throw new InvalidOperationException($"Could not get name for IrShaderVariableType {variableType}");
			}
		}
	}
}
