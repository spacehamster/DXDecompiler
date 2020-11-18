using System;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader
{
	public class InstructionToken : Token
	{
		public List<Operand> Operands;
		public InstructionToken(Opcode opcode, int length, ShaderModel shaderModel) : base(opcode, length, shaderModel) 
		{
			Operands = new List<Operand>();
		}
		public override string ToString()
		{
			string result = Opcode.ToString();
			for(int i = 0; i < Operands.Count; i++)
			{
				var operand = Operands[i];
				result += " ";
				if(operand is DestinationOperand)
				{
					result += GetDestinationName();
				} else
				{
					result += GetSourceName(i);
				}
			}
			return result;
		}

		string GetDestinationName()
		{
			Token instruction = this;
			var resultModifier = instruction.GetDestinationResultModifier();

			int destIndex = instruction.GetDestinationParamIndex();

			string registerName = instruction.GetParamRegisterName(destIndex);
			const int registerLength = 4;
			string writeMaskName = instruction.GetDestinationWriteMaskName(registerLength, false);
			string destinationName = $"{registerName}{writeMaskName}";
			if (resultModifier != ResultModifier.None)
			{
				//destinationName += "TODO:Modifier!!!";
			}
			return destinationName;
		}

		static string ApplyModifier(SourceModifier modifier, string value)
		{
			switch (modifier)
			{
				case SourceModifier.None:
					return value;
				case SourceModifier.Negate:
					return $"-{value}";
				case SourceModifier.Bias:
					return $"{value}_bias";
				case SourceModifier.BiasAndNegate:
					return $"-{value}_bias";
				case SourceModifier.Sign:
					return $"{value}_bx2";
				case SourceModifier.SignAndNegate:
					return $"-{value}_bx2";
				case SourceModifier.Complement:
					throw new NotImplementedException();
				case SourceModifier.X2:
					return $"{value}_x2";
				case SourceModifier.X2AndNegate:
					return $"-{value}_x2";
				case SourceModifier.DivideByZ:
					return $"{value}_dz";
				case SourceModifier.DivideByW:
					return $"{value}_dw";
				case SourceModifier.Abs:
					return $"{value}_abs";
				case SourceModifier.AbsAndNegate:
					return $"-{value}_abs";
				case SourceModifier.Not:
					throw new NotImplementedException();
				default:
					throw new NotImplementedException();
			}
		}
		string GetSourceName(int srcIndex)
		{
			Token instruction = this;
			string sourceRegisterName = instruction.GetParamRegisterName(srcIndex);
			sourceRegisterName = ApplyModifier(instruction.GetSourceModifier(srcIndex), sourceRegisterName);
			sourceRegisterName += instruction.GetSourceSwizzleName(srcIndex);
			if (instruction.IsRelativeAddressMode(srcIndex))
			{
				sourceRegisterName += $"[{GetSourceName(srcIndex + 1)}]";
			}
			return sourceRegisterName;
		}
	}
}
