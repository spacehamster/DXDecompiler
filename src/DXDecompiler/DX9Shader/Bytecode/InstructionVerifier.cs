using System.Diagnostics;

namespace DXDecompiler.DX9Shader
{
	public class InstructionVerifier
	{
		[System.Diagnostics.Conditional("DEBUG")]
		public static void Verify(Token instruction)
		{
			//Assert(currentInstruction.Modifier == 0);

			//TODO: Why?
			Debug.Assert(!instruction.Predicated, "Instruction must not be predicated");

			switch(instruction.Opcode)
			{
				case Opcode.Dcl:
					// https://msdn.microsoft.com/en-us/library/windows/hardware/ff549176(v=vs.85).aspx
					Debug.Assert(instruction.Data.Length == 2, "Dcl expected instruction length to be 2");
					uint param0 = instruction.Data[0];
					switch(instruction.GetParamRegisterType(1))
					{
						case RegisterType.Sampler:
							Debug.Assert((param0 & 0x07FFFFFF) == 0, "Sampler param unexpected bits set");
							break;
						case RegisterType.Input:
						case RegisterType.Output:
						case RegisterType.Texture:
							Debug.Assert((param0 & 0x0000FFF0) == 0, "Register param unexpected bits set");
							Debug.Assert((param0 & 0x7FF00000) == 0, "Register param unexpected bits set");
							break;
					}
					Debug.Assert((param0 & 0x80000000) != 0, "Register param unexpected bits set");
					break;
				case Opcode.Def:
					{
						Debug.Assert(instruction.Data.Length == 5, "Def expected instruction length to be 5");
						var registerType = instruction.GetParamRegisterType(0);
						Debug.Assert(
							registerType == RegisterType.Const ||
							registerType == RegisterType.Const2 ||
							registerType == RegisterType.Const3 ||
							registerType == RegisterType.Const4,
							"Def unexpected register type found");
					}
					break;
				case Opcode.DefI:
					{
						Debug.Assert(instruction.Data.Length == 5, "DefI expected instruction length to be 5");
						var registerType = instruction.GetParamRegisterType(0);
						Debug.Assert(registerType == RegisterType.ConstInt, "Def unexpected register type found");
					}
					break;
				case Opcode.IfC:
					IfComparison comp = (IfComparison)instruction.Modifier;
					Debug.Assert(
						comp == IfComparison.GT ||
						comp == IfComparison.EQ ||
						comp == IfComparison.GE ||
						comp == IfComparison.LT ||
						comp == IfComparison.NE ||
						comp == IfComparison.LE,
						"IfC unknown comparison type found");
					break;
				default:
					//throw new NotImplementedException();
					break;
			}
		}
	}
}
