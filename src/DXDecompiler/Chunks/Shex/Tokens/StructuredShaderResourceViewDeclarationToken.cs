using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Shex.Tokens
{
	/// <summary>
	/// Structured Shader Resource View Declaration
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_RESOURCE_STRUCTURED
	/// [23:11] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token.
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	///
	/// OpcodeToken0 is followed by 2 operands:
	/// (1) an operand, starting with OperandToken0, defining which
	///     g# register (D3D10_SB_OPERAND_TYPE_RESOURCE) is 
	///     being declared.
	/// (2) a DWORD indicating UINT32 struct byte stride
	/// </summary>
	public class StructuredShaderResourceViewDeclarationToken : DeclarationToken
	{
		public uint StructByteStride { get; private set; }

		public uint SpaceIndex { get; private set; }

		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static StructuredShaderResourceViewDeclarationToken Parse(BytecodeReader reader, ShaderVersion version)
		{
			var token0 = reader.ReadUInt32();
			var result = new StructuredShaderResourceViewDeclarationToken
			{
				Operand = Operand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10)),
				StructByteStride = reader.ReadUInt32()
			};
			if(version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32();
			}
			return result;
		}

		public override string ToString()
		{
			return string.Format("{0} {1}, {2}{3}",
				TypeDescription, Operand, StructByteStride,
				IsSM51 ? $", space={SpaceIndex}" : "");
		}
	}
}