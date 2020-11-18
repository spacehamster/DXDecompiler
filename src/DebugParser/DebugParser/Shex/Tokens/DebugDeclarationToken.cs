using DXDecompiler.Chunks.Shex;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public abstract class DebugDeclarationToken : DebugOpcodeToken
	{
		public DebugOperand Operand { get; internal set; }

		public static DebugDeclarationToken Parse(DebugBytecodeReader reader, OpcodeType opcodeType, DebugShaderVersion version)
		{
			switch (opcodeType)
			{
				case OpcodeType.DclGlobalFlags:
					return DebugGlobalFlagsDeclarationToken.Parse(reader);
				case OpcodeType.DclResource:
					return DebugResourceDeclarationToken.Parse(reader, version);
				case OpcodeType.DclSampler:
					return DebugSamplerDeclarationToken.Parse(reader, version);
				case OpcodeType.DclInput:
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputSiv:
				case OpcodeType.DclInputPs:
				case OpcodeType.DclInputPsSgv:
				case OpcodeType.DclInputPsSiv:
					return DebugInputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclOutput:
				case OpcodeType.DclOutputSgv:
				case OpcodeType.DclOutputSiv:
					return DebugOutputRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexRange:
					return DebugIndexingRangeDeclarationToken.Parse(reader);
				case OpcodeType.DclTemps:
					return DebugTempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclIndexableTemp:
					return DebugIndexableTempRegisterDeclarationToken.Parse(reader);
				case OpcodeType.DclConstantBuffer:
					return DebugConstantBufferDeclarationToken.Parse(reader, version);
				case OpcodeType.DclGsInputPrimitive:
					return DebugGeometryShaderInputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclGsOutputPrimitiveTopology:
					return DebugGeometryShaderOutputPrimitiveTopologyDeclarationToken.Parse(reader);
				case OpcodeType.DclMaxOutputVertexCount:
					return DebugGeometryShaderMaxOutputVertexCountDeclarationToken.Parse(reader);
				case OpcodeType.DclGsInstanceCount:
					return DebugGeometryShaderInstanceCountDeclarationToken.Parse(reader);
				case OpcodeType.DclInputControlPointCount:
				case OpcodeType.DclOutputControlPointCount:
					return DebugControlPointCountDeclarationToken.Parse(reader);
				case OpcodeType.DclTessDomain:
					return DebugTessellatorDomainDeclarationToken.Parse(reader);
				case OpcodeType.DclTessPartitioning:
					return DebugTessellatorPartitioningDeclarationToken.Parse(reader);
				case OpcodeType.DclTessOutputPrimitive:
					return DebugTessellatorOutputPrimitiveDeclarationToken.Parse(reader);
				case OpcodeType.DclHsMaxTessFactor:
					return DebugHullShaderMaxTessFactorDeclarationToken.Parse(reader);
				case OpcodeType.DclHsForkPhaseInstanceCount:
					return DebugHullShaderForkPhaseInstanceCountDeclarationToken.Parse(reader);
				case OpcodeType.DclFunctionBody:
					return DebugFunctionBodyDeclarationToken.Parse(reader);
				case OpcodeType.DclFunctionTable:
					return DebugFunctionTableDeclarationToken.Parse(reader);
				case OpcodeType.DclInterface:
					return DebugInterfaceDeclarationToken.Parse(reader);
				case OpcodeType.DclThreadGroup:
					return DebugThreadGroupDeclarationToken.Parse(reader);
				case OpcodeType.DclUnorderedAccessViewTyped:
					return DebugTypedUnorderedAccessViewDeclarationToken.Parse(reader, version);
				case OpcodeType.DclUnorderedAccessViewRaw:
					return DebugRawUnorderedAccessViewDeclarationToken.Parse(reader, version);
				case OpcodeType.DclUnorderedAccessViewStructured:
					return DebugStructuredUnorderedAccessViewDeclarationToken.Parse(reader, version);
				case OpcodeType.DclThreadGroupSharedMemoryRaw:
					return DebugRawThreadGroupSharedMemoryDeclarationToken.Parse(reader);
				case OpcodeType.DclThreadGroupSharedMemoryStructured:
					return DebugStructuredThreadGroupSharedMemoryDeclarationToken.Parse(reader);
				case OpcodeType.DclResourceRaw:
					return DebugRawShaderResourceViewDeclarationToken.Parse(reader, version);
				case OpcodeType.DclResourceStructured:
					return DebugStructuredShaderResourceViewDeclarationToken.Parse(reader, version);
				case OpcodeType.DclStream:
					return DebugStreamDeclarationToken.Parse(reader);
				default:
					throw new ParseException("OpcodeType '" + opcodeType + "' is not supported.");
			}
		}
	}
}
