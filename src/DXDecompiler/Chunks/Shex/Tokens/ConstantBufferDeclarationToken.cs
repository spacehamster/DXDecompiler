using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Shex.Tokens
{
	/// <summary>
	/// Constant Buffer Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_CONSTANT_BUFFER
	/// [11]    D3D10_SB_CONSTANT_BUFFER_ACCESS_PATTERN
	/// [23:12] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 1 operand on Shader Model 4.0 through 5.0:
	/// (1) Operand, starting with OperandToken0, defining which CB slot (cb#[size])
	///     is being declared. (operand type: D3D10_SB_OPERAND_TYPE_CONSTANT_BUFFER)
	///     The indexing dimension for the register must be 
	///     D3D10_SB_OPERAND_INDEX_DIMENSION_2D, where the first index specifies
	///     which cb#[] is being declared, and the second (array) index specifies the size 
	///     of the buffer, as a count of 32-bit*4 elements.  (As opposed to when the 
	///     cb#[] is used in shader instructions, and the array index represents which 
	///     location in the constant buffer is being referenced.)
	///     If the size is specified as 0, the CB size is not known (any size CB
	///     can be bound to the slot).
	///
	/// The order of constant buffer declarations in a shader indicates their
	/// relative priority from highest to lowest (hint to driver).
	/// 
	/// OpcodeToken0 is followed by 3 operands on Shader Model 5.1 and later:
	/// (1) Operand, starting with OperandToken0, defining which CB range (ID and bounds)
	///     is being declared. (operand type: D3D10_SB_OPERAND_TYPE_CONSTANT_BUFFER)
	///     The indexing dimension for the register must be D3D10_SB_OPERAND_INDEX_DIMENSION_3D, 
	///     and the meaning of the index dimensions are as follows: (cb<id>[<lbound>:<ubound>])
	///       1 <id>:     variable ID being declared
	///       2 <lbound>: the lower bound of the range of constant buffers in the space
	///       3 <ubound>: the upper bound (inclusive) of this range
	///     As opposed to when the cb#[] is used in shader instructions: (cb<id>[<idx>][<loc>])
	///       1 <id>:  variable ID being used (matches dcl)
	///       2 <idx>: absolute index of constant buffer within space (may be dynamically indexed)
	///       3 <loc>: location of vector within constant buffer being referenced,
	///          which may also be dynamically indexed, with no access pattern flag required.
	/// (2) a DWORD indicating the size of the constant buffer as a count of 16-byte vectors.
	///     Each vector is 32-bit*4 elements == 128-bits == 16 bytes.
	///     If the size is specified as 0, the CB size is not known (any size CB
	///     can be bound to the slot).
	/// (3) a DWORD indicating the space index.
	///
	/// </summary>
	public class ConstantBufferDeclarationToken : DeclarationToken
	{
		public ConstantBufferAccessPattern AccessPattern { get; internal set; }
		public uint ConstantBufferID => (uint)Operand.Indices[0].Value;
		public uint ConstantBufferSize => IsSM51 ?
			m_ConstantBufferSize :
			(uint)Operand.Indices[1].Value;
		public uint SpaceIndex { get; internal set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;
		private uint m_ConstantBufferSize;

		public static ConstantBufferDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();
			var result = new ConstantBufferDeclarationToken
			{
				AccessPattern = token0.DecodeValue<ConstantBufferAccessPattern>(11, 11),
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10))
			};
			if (version.IsSM51)
			{
				result.m_ConstantBufferSize = reader.ReadUInt32();
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			if (!IsSM51)
			{
				return string.Format("{0} {1}, {2}", TypeDescription,
					Operand.ToString().ToUpper(), AccessPattern.GetDescription());
			}
			else
			{
				return string.Format("{0} {1}[{2}], {3}, space={4}", TypeDescription,
					Operand.ToString().ToUpper(), m_ConstantBufferSize, 
					AccessPattern.GetDescription(), SpaceIndex);
			}
		}
	}
}