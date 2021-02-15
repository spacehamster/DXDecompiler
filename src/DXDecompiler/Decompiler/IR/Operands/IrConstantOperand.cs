using DXDecompiler.Decompiler.IR.ResourceDefinitions;

namespace DXDecompiler.Decompiler.IR.Operands
{
	public class IrConstantOperand : IrOperand
	{
		public IrShaderType Type = new IrShaderType()
		{
			VariableClass = Chunks.Rdef.ShaderVariableClass.Vector,
			VariableType = IrShaderVariableType.F32,
		};
		public Number4 Value;
		public IrConstantOperand(double value)
		{
			Value = new Number4(
				Number.FromFloat((float)value),
				Number.FromFloat(0),
				Number.FromFloat(0),
				Number.FromFloat(0));
		}
		public IrConstantOperand(long value)
		{
			Value = new Number4(
				Number.FromInt((int)value),
				Number.FromInt(0),
				Number.FromInt(0),
				Number.FromInt(0));
			Type.VariableType = IrShaderVariableType.I32;
		}
	}
}
