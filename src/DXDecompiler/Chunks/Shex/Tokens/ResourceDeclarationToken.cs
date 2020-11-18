using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Shex.Tokens
{
	/// <summary>
	/// Resource Declaration (non multisampled)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_RESOURCE
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION
	/// [23:16] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands on Shader Models 4.0 through 5.0:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	///
	/// OpcodeToken0 is followed by 3 operands on Shader Model 5.1 and later:
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
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	/// (3) a DWORD indicating the space index.
	///
	/// -------
	/// 
	/// Resource Declaration (multisampled)
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D10_SB_OPCODE_DCL_RESOURCE (same opcode as non-multisampled case)
	/// [15:11] D3D10_SB_RESOURCE_DIMENSION (must be TEXTURE2DMS or TEXTURE2DMSARRAY)
	/// [22:16] Sample count 1...127.  0 is currently disallowed, though
	///         in future versions 0 could mean "configurable" sample count
	/// [23:23] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands on Shader Models 4.0 through 5.0:
	/// (1) an operand, starting with OperandToken0, defining which
	///     t# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is being declared.
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	///
	/// OpcodeToken0 is followed by 3 operands on Shader Model 5.1 and later:
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
	/// (2) a Resource Return Type token (ResourceReturnTypeToken)
	/// (3) a DWORD indicating the space index.
	/// </summary>
	public class ResourceDeclarationToken : DeclarationToken
	{
		public ResourceDimension ResourceDimension { get; internal set; }
		public byte SampleCount { get; internal set; }
		public ResourceReturnTypeToken ReturnType { get; internal set; }
		public uint SpaceIndex { get; internal set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public bool IsMultiSampled
		{
			get
			{
				switch (ResourceDimension)
				{
					case ResourceDimension.Texture2DMultiSampled:
					case ResourceDimension.Texture2DMultiSampledArray:
						return true;
					default:
						return false;
				}
			}
		}

		public static ResourceDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();

			var resourceDimension = token0.DecodeValue<ResourceDimension>(11, 15);

			byte sampleCount;
			switch (resourceDimension)
			{
				case ResourceDimension.Texture2DMultiSampled:
				case ResourceDimension.Texture2DMultiSampledArray:
					sampleCount = token0.DecodeValue<byte>(16, 22);
					break;
				default:
					sampleCount = 0;
					break;
			}

			var operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var returnType = ResourceReturnTypeToken.Parse(reader);
			var result = new ResourceDeclarationToken
			{
				ResourceDimension = resourceDimension,
				SampleCount = sampleCount,
				Operand = operand,
				ReturnType = returnType
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0}_{1}{2} ({3}) {4}{5}", 
				TypeDescription, 
				ResourceDimension.GetDescription(),
				(IsMultiSampled) ? "(" + SampleCount + ")" : string.Empty, 
				ReturnType,
				Operand,
				IsSM51 ? $", space={SpaceIndex}" : "");
		}
	}
}