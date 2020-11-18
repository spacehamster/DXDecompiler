using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler
{
	public partial class HLSLDecompiler
	{
		internal void LogDeclaration(IEnumerable<DeclarationToken> tokens)
		{
			foreach(var token in tokens)
			{
				LogDeclaration(token);
			}
		}
		internal void LogDeclaration(DeclarationToken token)
		{
			DebugLog(token);
		}
		internal void WriteDeclarationVariables(IEnumerable<DeclarationToken> tokens)
		{
			foreach (var token in tokens)
			{
				WriteDeclarationVariables(token);
			}
		}
		internal void WriteDeclarationVariables(DeclarationToken token)
		{
		}
		internal void WriteDeclarationParameters(IEnumerable<DeclarationToken> tokens)
		{
			foreach (var token in tokens)
			{
				WriteDeclarationVariables(token);
			}
		}
		internal void WriteDeclarationParameter(DeclarationToken token)
		{
			switch (token.Header.OpcodeType)
			{
				case OpcodeType.DclInput:
					{
						break;
					}
			}
		}
		string GetSemanticName(Operand operand)
		{
			switch (operand.OperandType)
			{
				case OperandType.InputThreadIDInGroup:
					return "SV_DispatchThreadID";
				case OperandType.InputThreadID:
					return "SV_GroupThreadID";
				case OperandType.InputThreadIDInGroupFlattened:
					return "SV_GroupID";
				case OperandType.InputThreadGroupID:
					return "SV_DispatchThreadID";
				case OperandType.InputGSInstanceID:
					return "SV_GSInstanceID";
				case OperandType.InputDomainPoint:
					return "SV_DomainLocation";
				case OperandType.InputPrimitiveID:
					return "SV_PrimitiveID";
				case OperandType.InputPatchConstant:
					return "TODO";
				default:
					throw new ArgumentException($"{operand}");
			}
		}
		internal void WriteInputDeclaration(InputRegisterDeclarationToken token)
		{
			var type = "float4";
			switch (token.Operand.OperandType)
			{
				case OperandType.InputThreadIDInGroup:
					type = "uint3";
					break;
				case OperandType.InputThreadID:
					type = "uint2";
					break;
				case OperandType.InputThreadIDInGroupFlattened:
					type = "uint3";
					break;
				case OperandType.InputThreadGroupID:
					type = "uint3";
					break;
			}
			Output.AppendFormat("{0} {1} : {2}",
				type,
				token.Operand.OperandType.GetDescription(),
				GetSemanticName(token.Operand));
		}
		internal void WriteDeclarationAnnotations(IEnumerable<DeclarationToken> tokens)
		{
			foreach (var token in tokens)
			{
				WriteDeclarationAnnotations(token);
			}
		}
		internal void WriteDeclarationAnnotations(DeclarationToken token)
		{
			var opcodeType = token.Header.OpcodeType;
			switch (opcodeType)
			{
				case OpcodeType.DclTessDomain:
					{
						var dcl = token as TessellatorDomainDeclarationToken;
						Output.AppendFormat("[domain(\"{0}\")]\n", dcl.Domain.GetAttributeName());
						break;
					}
				case OpcodeType.DclMaxOutputVertexCount:
					{
						var dcl = token as GeometryShaderMaxOutputVertexCountDeclarationToken;
						Output.AppendFormat("[maxvertexcount({0})]\n", dcl.MaxPrimitives);
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
						var dcl = token as StructuredThreadGroupSharedMemoryDeclarationToken;
						Output.AppendLine("groupshared struct {");
						for(int i = 0; i < dcl.StructByteStride; i += 4)
						{
							Output.AppendLine($"\tfloat4 m{i/4};");
						}
						Output.AppendLine($"}} {dcl.Operand.OperandType.GetDescription()}{dcl.Operand.Indices[0].Value}[{dcl.StructCount}];");
						break;
					}
				case OpcodeType.DclThreadGroupSharedMemoryRaw:
					{
						var dcl = token as RawThreadGroupSharedMemoryDeclarationToken;
						if (dcl.ElementCount != 4)
						{
							throw new ArgumentException($"Can't handle dcl_tgsm_raw with element count of {dcl.ElementCount}");
						}
						Output.AppendLine($"groupshared float {dcl.Operand.OperandType.GetDescription()}{dcl.Operand.Indices[0].Value};");
						break;
					}
				default:
					break;
			}
		}
		static string GetDeclaredInputName(BytecodeContainer container, ProgramType programtype, Operand operand)
		{
			return "TODO: GetName";
		}
	}
}
