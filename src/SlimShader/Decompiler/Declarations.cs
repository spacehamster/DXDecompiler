using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Decompiler
{
	public partial class DXDecompiler
	{
		internal void TranslateDeclaration(DeclarationToken token)
		{
			DebugLog(token);
			var opcodeType = token.Header.OpcodeType;
			switch (opcodeType)
			{
				case OpcodeType.DclInputSgv:
				case OpcodeType.DclInputPsSgv:
					{
						break;
					}
				case OpcodeType.DclOutputSiv:
					{
						break;
					}
				case OpcodeType.DclInput:
					{
						break;
					}
				case OpcodeType.DclInputPsSiv:
					{
						break;
					}
				case OpcodeType.DclInputSiv:
					{
						break;
					}
				case OpcodeType.DclInputPs:
					{
						break;
					}
				case OpcodeType.DclTemps:
					{
						break;
					}
				case OpcodeType.DclConstantBuffer:
					{
						break;
					}
				case OpcodeType.DclResource:
					{
						break;
					}
				case OpcodeType.DclOutput:
					{
						break;
					}
				case OpcodeType.DclGlobalFlags:
					{
						break;
					}
				case OpcodeType.DclTessOutputPrimitive:
					{
						break;
					}
				case OpcodeType.DclTessDomain:
					{
						break;
					}
				case OpcodeType.DclTessPartitioning:
					{
						break;
					}
				case OpcodeType.DclGsOutputPrimitiveTopology:
					{
						break;
					}
				case OpcodeType.DclMaxOutputVertexCount:
					{
						var dcl = token as GeometryShaderMaxOutputVertexCountDeclarationToken;
						Output.AppendFormat("[maxvertexcount({0})]\n", dcl.MaxPrimitives);
						break;
					}
				case OpcodeType.DclGsInputPrimitive:
					{
						break;
					}
				case OpcodeType.DclInterface:
					{
						break;
					}
				case OpcodeType.DclFunctionBody:
					{
						break;
					}
				case OpcodeType.DclFunctionTable:
					{
						break;
					}
				case OpcodeType.CustomData:
					{
						break;
					}
				case OpcodeType.DclHsForkPhaseInstanceCount:
					{
						break;
					}
				case OpcodeType.DclIndexableTemp:
					{
						break;
					}
				case OpcodeType.DclIndexRange:
					{
						break;
					}
				case OpcodeType.HsDecls:
					{
						break;
					}
				case OpcodeType.DclInputControlPointCount:
					{
						break;
					}
				case OpcodeType.DclOutputControlPointCount:
					{
						break;
					}
				case OpcodeType.HsForkPhase:
					{
						break;
					}
				case OpcodeType.HsJoinPhase:
					{
						break;
					}
				case OpcodeType.DclSampler:
					{
						break;
					}
				case OpcodeType.DclUnorderedAccessViewTyped:
					{
						break;
					}
				case OpcodeType.DclUnorderedAccessViewStructured:
					{
						break;
					}
				case OpcodeType.DclUnorderedAccessViewRaw:
					{
						break;
					}
				case OpcodeType.DclResourceStructured:
					{
						break;
					}
				case OpcodeType.DclResourceRaw:
					{
						break;
					}
				case OpcodeType.DclThreadGroup:
					{
						var dcl = token as ThreadGroupDeclarationToken;
						Output.AppendFormat("[numthreads({0}, {1}, {2})]\n",
							dcl.Dimensions[0],
							dcl.Dimensions[1],
							dcl.Dimensions[2]);
						break;
					}
				case OpcodeType.DclThreadGroupSharedMemoryStructured:
					{
						break;
					}
				case OpcodeType.DclThreadGroupSharedMemoryRaw:
					{
						break;
					}
				case OpcodeType.DclStream:
					{
						break;
					}
				case OpcodeType.DclGsInstanceCount:
					{
						break;
					}

				default:
					throw new Exception($"Unexcepted opcode {opcodeType}");

			}
		}
		static string GetDeclaredInputName(BytecodeContainer container, ProgramType programtype, Operand operand)
		{
			return "TODO: GetName";
		}
	}
}
