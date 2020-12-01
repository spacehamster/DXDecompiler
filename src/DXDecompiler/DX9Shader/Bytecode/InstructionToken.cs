using System;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Instruction Token 
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/instruction-token
	/// 
	/// Bits
	/// 
	/// [15:00] Bits 0 through 15 indicate an operation code. D3DSIO_* is an example of an operation code, where* represents the instruction. For example, the following code snippet shows an ADD instruction:
	/// 
	/// [23:16] Bits 16 through 23 indicate specific controls related to the operation code.
	///
	/// [27:24] For pixel and vertex shader versions earlier than 2_0, bits 24 through 27 are reserved and set to 0x0.
	///
	/// For pixel and vertex shader versions 2_0 and later, bits 24 through 27 specify the size in DWORDs of the instruction excluding the instruction token itself (that is, the number of tokens that comprise the instruction excluding the instruction token).
	/// 
	/// [28] For pixel and vertex shader versions earlier than 2_0, bit 28 is reserved and set to 0x0.
	///
	/// For pixel and vertex shader versions 2_0 and later, bit 28 indicates whether the instruction is predicated(that is, contains an extra predicate source token at the end of the shader code.If this bit is set to 0x1, the instruction is predicated.
	///
	/// [29] Reserved. This value is set to 0x0.
	///
	/// [30] For pixel shader versions earlier than 2_0, bit 30 is the co-issue bit. If set to 1, execute this instruction with previous instructions; otherwise, execute separately.
	/// For pixel shader version 2_0 and later and all vertex shader versions, bit 30 is reserved and set to 0x0.
	/// 
	/// [31] Bit 31 is zero (0x0).
	/// </summary>
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
				}
				else
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
			if(resultModifier != ResultModifier.None)
			{
				//destinationName += "TODO:Modifier!!!";
			}
			return destinationName;
		}

		static string ApplyModifier(SourceModifier modifier, string value)
		{
			switch(modifier)
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
			if(instruction.IsRelativeAddressMode(srcIndex))
			{
				sourceRegisterName += $"[{GetSourceName(srcIndex + 1)}]";
			}
			return sourceRegisterName;
		}
	}
}
