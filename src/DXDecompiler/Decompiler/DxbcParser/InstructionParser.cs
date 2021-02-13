using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Decompiler.IR;
using DXDecompiler.Decompiler.IR.Operands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler.DxbcParser
{
	public class InstructionParser
	{
		public static void ParseTokens(IrPass pass, List<OpcodeToken> opcodes)
		{
			ParseDeclarations(pass, opcodes.OfType<DeclarationToken>());
			ParseInstructions(pass, opcodes.OfType<InstructionToken>());
		}
		static void ParseDeclarations(IrPass pass, IEnumerable<DeclarationToken> declarations)
		{
			pass.Declarations = new List<DeclarationToken>(declarations);
			foreach(var token in declarations)
			{
				switch(token.Header.OpcodeType)
				{
					case OpcodeType.DclTessDomain:
						{
							var decl = token as TessellatorDomainDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("domain", decl.Domain.GetAttributeName()));
						}
						break;
					case OpcodeType.DclMaxOutputVertexCount:
						{
							var decl = token as GeometryShaderMaxOutputVertexCountDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("maxvertexcount", decl.MaxPrimitives));
						}
						break;
					case OpcodeType.DclOutputControlPointCount:
						{
							var decl = token as ControlPointCountDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("outputcontrolpoints", decl.ControlPointCount));
						}
						break;
					case OpcodeType.DclTessPartitioning:
						{
							var decl = token as TessellatorPartitioningDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("partitioning", decl.Partitioning.ToString().ToLower()));
						}
						break;
					case OpcodeType.DclHsMaxTessFactor:
						{
							var decl = token as HullShaderMaxTessFactorDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("maxtessfactor", decl.MaxTessFactor));
						}
						break;
					case OpcodeType.DclTessOutputPrimitive:
						{
							var decl = token as TessellatorOutputPrimitiveDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("outputtopology", decl.OutputPrimitive.ToString().ToLower()));
						}
						break;
					case OpcodeType.DclThreadGroup:
						{
							var decl = token as ThreadGroupDeclarationToken;
							pass.Attributes.Add(IrAttribute.Create("numthreads",
								decl.Dimensions[0],
								decl.Dimensions[1],
								decl.Dimensions[2]));
						}
						break;
				}
			}
		}
		static void ParseInstructions(IrPass pass, IEnumerable<InstructionToken> instructions)
		{
			foreach(var token in instructions)
			{
				pass.Instructions.Add(ParseInstruction(token));
			}
		}
		static IrInstruction ParseInstruction(InstructionToken instruction)
		{
			var result = new IrInstruction()
			{
				AsmDebug = instruction.ToString(),
				Operands = new List<IrOperand>(
					instruction.Operands.Select(OperandParser.Parse)),
				Opcode = (IrInstructionOpcode)instruction.Header.OpcodeType,
			};
			switch(instruction.Header.OpcodeType)
			{
				case OpcodeType.Ld:
					Swap(result.Operands, 1, 2);
					break;
			}
			return result;
		}

		static void Swap<T>(IList<T> list, int indexA, int indexB)
		{
			T tmp = list[indexA];
			list[indexA] = list[indexB];
			list[indexB] = tmp;
		}
	}
}
