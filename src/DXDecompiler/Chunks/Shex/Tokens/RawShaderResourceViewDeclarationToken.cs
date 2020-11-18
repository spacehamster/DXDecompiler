using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Shex.Tokens
{
	/// <summary>
	/// Raw Shader Resource View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_RESOURCE_RAW
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	///
	/// OpcodeToken0 is followed by 2 operands on Shader Model 5.1 and later:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	///     The indexing dimension for the register must be D3D10_SB_OPERAND_INDEX_DIMENSION_3D, 
	///     and the meaning of the index dimensions are as follows: (t<id>[<lbound>:<ubound>])
	///       1 <id>:     variable ID being declared
	///       2 <lbound>: the lower bound of the range of resources in the space
	///       3 <ubound>: the upper bound (inclusive) of this range
	///     As opposed to when the t# is used in shader instructions, where the register
	///     must be D3D10_SB_OPERAND_INDEX_DIMENSION_2D, and the meaning of the index 
	///     dimensions are as follows: (t<id>[<idx>]):
	///       1 <id>:  variable ID being used (matches dcl)
	///       2 <idx>: absolute index of resource within space (may be dynamically indexed)
	/// (2) a DWORD indicating the space index.
	/// </summary>
	public class RawShaderResourceViewDeclarationToken : DeclarationToken
	{
		public uint SpaceIndex { get; private set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static RawShaderResourceViewDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();
			var result = new RawShaderResourceViewDeclarationToken
			{
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}{2}", 
				TypeDescription,
				Operand,
				IsSM51 ? $", space={SpaceIndex}" : "");
		}
	}
}