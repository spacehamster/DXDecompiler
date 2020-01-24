using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Shex.Tokens
{
	/// <summary>
	/// Structured Unordered Access View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_UNORDERED_ACCESS_VIEW_STRUCTURED
	/// [15:11] Ignored, 0
	/// [16:16] D3D11_SB_GLOBALLY_COHERENT_ACCESS or 0 (LOCALLY_COHERENT)
	/// [17:17] D3D11_SB_RASTERIZER_ORDERED_ACCESS or 0
	/// [22:18] Ignored, 0
	/// [23:23] D3D11_SB_UAV_HAS_ORDER_PRESERVING_COUNTER or 0
	///
	///            The presence of this flag means that if a UAV is bound to the
	///            corresponding slot, it must have been created with 
	///            D3D11_BUFFER_UAV_FLAG_COUNTER at the API.  Also, the shader
	///            can contain either imm_atomic_alloc or _consume instructions
	///            operating on the given UAV.
	/// 
	///            If this flag is not present, the shader can still contain
	///            either imm_atomic_alloc or imm_atomic_consume instructions for
	///            this UAV.  But if such instructions are present in this case,
	///            and a UAV is bound corresponding slot, it must have been created 
	///            with the D3D11_BUFFER_UAV_FLAG_APPEND flag at the API.
	///            Append buffers have a counter as well, but values returned 
	///            to the shader are only valid for the lifetime of the shader 
	///            invocation.
	///
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is 
	///     being declared.
	/// (2) a DWORD indicating UINT32 byte stride
	///
	/// OpcodeToken0 is followed by 3 operands on Shader Model 5.1 and later:
	/// (1) an operand, starting with OperandToken0, defining which
	///     u# register (D3D11_SB_OPERAND_TYPE_UNORDERED_ACCESS_VIEW) is being declared.
	///     The indexing dimension for the register must be D3D10_SB_OPERAND_INDEX_DIMENSION_3D, 
	///     and the meaning of the index dimensions are as follows: (u<id>[<lbound>:<ubound>])
	///       1 <id>:     variable ID being declared
	///       2 <lbound>: the lower bound of the range of UAV's in the space
	///       3 <ubound>: the upper bound (inclusive) of this range
	///     As opposed to when the u# is used in shader instructions, where the register
	///     must be D3D10_SB_OPERAND_INDEX_DIMENSION_2D, and the meaning of the index 
	///     dimensions are as follows: (u<id>[<idx>]):
	///       1 <id>:  variable ID being used (matches dcl)
	///       2 <idx>: absolute index of uav within space (may be dynamically indexed)
	/// (2) a DWORD indicating UINT32 byte stride
	/// (3) a DWORD indicating the space index.
	/// </summary>
	public class StructuredUnorderedAccessViewDeclarationToken : UnorderedAccessViewDeclarationTokenBase
	{
		/// <summary>
		/// The presence of this flag means that if a UAV is bound to the
		/// corresponding slot, it must have been created with 
		/// D3D11_BUFFER_UAV_FLAG_COUNTER at the API.  Also, the shader
		/// can contain either imm_atomic_alloc or _consume instructions
		/// operating on the given UAV.
		/// 
		/// If this flag is not present, the shader can still contain
		/// either imm_atomic_alloc or imm_atomic_consume instructions for
		/// this UAV.  But if such instructions are present in this case,
		/// and a UAV is bound corresponding slot, it must have been created 
		/// with the D3D11_BUFFER_UAV_FLAG_APPEND flag at the API.
		/// Append buffers have a counter as well, but values returned 
		/// to the shader are only valid for the lifetime of the shader 
		/// invocation.
		/// </summary>
		public bool HasOrderPreservingCounter { get; private set; }

		public uint ByteStride { get; private set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static StructuredUnorderedAccessViewDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();
			var result = new StructuredUnorderedAccessViewDeclarationToken
			{
				Coherency = token0.DecodeValue<UnorderedAccessViewCoherency>(16, 16),
				HasOrderPreservingCounter = (token0.DecodeValue(23, 23) == 0),
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				ByteStride = reader.ReadUInt32()
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0}{1} {2}, {3}", 
				TypeDescription,
				Coherency.GetDescription(),
				Operand, 
				ByteStride);
		}
	}
}