using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	// Sampler Declaration
	//
	// OpcodeToken0:
	//
	// [10:00] D3D10_SB_OPCODE_DCL_SAMPLER
	// [14:11] D3D10_SB_SAMPLER_MODE
	// [23:15] Ignored, 0
	// [30:24] Instruction length in DWORDs including the opcode token.
	// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	//         contains extended operand description.  This dcl is currently not
	//         extended.
	//
	// OpcodeToken0 is followed by 1 operand on Shader Models 4.0 through 5.0:
	// (1) Operand starting with OperandToken0, defining which sampler
	//     (D3D10_SB_OPERAND_TYPE_SAMPLER) register # is being declared.
	//
	// OpcodeToken0 is followed by 2 operands on Shader Model 5.1 and later:
	// (1) an operand, starting with OperandToken0, defining which
	//     s# register (D3D10_SB_OPERAND_TYPE_SAMPLER) is being declared.
	//     The indexing dimension for the register must be D3D10_SB_OPERAND_INDEX_DIMENSION_3D, 
	//     and the meaning of the index dimensions are as follows: (s<id>[<lbound>:<ubound>])
	//       1 <id>:     variable ID being declared
	//       2 <lbound>: the lower bound of the range of samplers in the space
	//       3 <ubound>: the upper bound (inclusive) of this range
	//     As opposed to when the s# is used in shader instructions, where the register
	//     must be D3D10_SB_OPERAND_INDEX_DIMENSION_2D, and the meaning of the index 
	//     dimensions are as follows: (s<id>[<idx>]):
	//       1 <id>:  variable ID being used (matches dcl)
	//       2 <idx>: absolute index of sampler within space (may be dynamically indexed)
	// (2) a DWORD indicating the space index.
	/// </summary>
	public class SamplerDeclarationToken : DeclarationToken
	{
		public SamplerMode SamplerMode { get; internal set; }
		public uint SpaceIndex { get; internal set; }
		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;
		public static SamplerDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();
			var operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var result = new SamplerDeclarationToken
			{
				SamplerMode = token0.DecodeValue<SamplerMode>(11, 14),
				Operand = operand
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}", TypeDescription, Operand,
				SamplerMode.GetDescription());
		}
	}
}