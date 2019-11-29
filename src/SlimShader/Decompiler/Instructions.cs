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
		internal void TranslateInstruction(InstructionToken token)
		{
			DebugLog(token);
			switch (token.Header.OpcodeType)
			{
				case OpcodeType.FtoI:
				case OpcodeType.FtoU:
					{
						break;
					}
				case OpcodeType.Mov:
					{
						WriteMoveBinaryOp(token.Operands[0], token.Operands[1]);
						break;
					}
				case OpcodeType.ItoF:
				case OpcodeType.Utof:
					{
						var dest = token.Operands[0];
						var src = token.Operands[1];
						var destCount = dest.GetNumSwizzleElements();
						var srcCount = src.GetNumSwizzleElements();
						AddIndent();
						AddAssignToDest(token.Operands[0]);
						Output.Append(GetConstructorForType(ShaderVariableType.Float,
							 srcCount == destCount ? destCount : 4));
						Output.Append("(");
						WriteOperand(src);
						Output.AppendLine(");");
						break;
					}
				case OpcodeType.Mad:
					{
						CallTernaryOp("*", "+", token, 0, 1, 2, 3);
						break;
					}
				case OpcodeType.IMad:
					{
						CallTernaryOp("*", "+", token, 0, 1, 2, 3);
						break;
					}
				case OpcodeType.DAdd:
					{
						CallBinaryOp("+", token, 0, 1, 2);
						break;
					}
				case OpcodeType.IAdd:
					{
						CallBinaryOp("+", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Add:
					{
						CallBinaryOp("+", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Or:
					{
						CallBinaryOp("|", token, 0, 1, 2);
						break;
					}
				case OpcodeType.And:
					{
						CallBinaryOp("&", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Ge:
					{
						AddComparision(token, ComparisonType.Ge);
						break;
					}
				case OpcodeType.Mul:
					{
						CallBinaryOp("*", token, 0, 1, 2);
						break;
					}
				case OpcodeType.IMul:
					{
						CallBinaryOp("*", token, 0, 1, 2);
						break;
					}
				case OpcodeType.UDiv:
					{
						CallBinaryOp("/", token, 0, 2, 3);
						CallBinaryOp("%", token, 1, 2, 3);
						break;
					}
				case OpcodeType.Div:
					{
						CallBinaryOp("/", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Sincos:
					{
						if(token.Operands[0].OperandType == token.Operands[2].OperandType &&
							token.Operands[0].Indices[0].Value == token.Operands[2].Indices[0].Value)
						{
							if (token.Operands[1].OperandType != OperandType.Null)
							{
								CallHelper1("cos", token, 1, 2);
							}
							if (token.Operands[0].OperandType != OperandType.Null)
							{
								CallHelper1("cos", token, 0, 2);
							}
						} else
						{
							if (token.Operands[0].OperandType != OperandType.Null)
							{
								CallHelper1("sin", token, 0, 2);
							}
							if (token.Operands[1].OperandType != OperandType.Null)
							{
								CallHelper1("cos", token, 1, 2);
							}
						}
						break;
					}
				case OpcodeType.Dp2:
					{
						CallHelper2("dot", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Dp3:
					{
						CallHelper2("dot", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Dp4:
					{
						CallHelper2("dot", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Ne:
					{
						AddComparision(token, ComparisonType.Ne);
						break;
					}
				case OpcodeType.IGe:
					{
						AddComparision(token, ComparisonType.Ge);
						break;
					}
				case OpcodeType.ILt:
					{
						AddComparision(token, ComparisonType.Lt);
						break;
					}
				case OpcodeType.Lt:
					{
						AddComparision(token, ComparisonType.Lt);
						break;
					}
				case OpcodeType.IEq:
					{
						AddComparision(token, ComparisonType.Eq);
						break;
					}
				case OpcodeType.ULt:
					{
						AddComparision(token, ComparisonType.Lt);
						break;
					}
				case OpcodeType.UGe:
					{
						AddComparision(token, ComparisonType.Ge);
						break;
					}
				case OpcodeType.MovC:
					{
						AddMOVCBinaryOp(token.Operands[0], token.Operands[1],
							token.Operands[2], token.Operands[3]);
						break;
					}
				case OpcodeType.SwapC:
					{
						break;
					}
				case OpcodeType.Log:
					{
						CallHelper1("log", token, 0, 1);
						break;
					}
				case OpcodeType.Rsq:
					{
						CallHelper1("normalize", token, 0, 1);
						break;
					}
				case OpcodeType.Exp:
					{
						CallHelper1("exp2", token, 0, 1);
						break;
					}
				case OpcodeType.Sqrt:
					{
						CallHelper1("sqrt", token, 0, 1);
						break;
					}
				case OpcodeType.RoundPi:
					{
						CallHelper1("ceil", token, 0, 1);
						break;
					}
				case OpcodeType.RoundNi:
					{
						CallHelper1("floor", token, 0, 1);
						break;
					}
				case OpcodeType.RoundZ:
					{
						CallHelper1("trunc", token, 0, 1);
						break;
					}
				case OpcodeType.RoundNe:
					{
						CallHelper1("roundEven", token, 0, 1);
						break;
					}
				case OpcodeType.Frc:
					{
						CallHelper1("frac", token, 0, 1);
						break;
					}
				case OpcodeType.IMax:
					{
						CallHelper2("max", token, 0, 1, 2);
						break;
					}
				case OpcodeType.UMax:
					{
						CallHelper2("max", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Max:
					{
						CallHelper2("max", token, 0, 1, 2);
						break;
					}
				case OpcodeType.IMin:
					{
						CallHelper2("min", token, 0, 1, 2);
						break;
					}
				case OpcodeType.UMin:
					{
						CallHelper2("min", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Min:
					{
						CallHelper2("min", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Gather4:
					{
						break;
					}
				case OpcodeType.Gather4PoC:
					{
						break;
					}
				case OpcodeType.Gather4Po:
					{
						break;
					}
				case OpcodeType.Gather4C:
					{
						break;
					}
				case OpcodeType.Sample:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.SampleL:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.SampleC:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.SampleCLz:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.SampleD:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.SampleB:
					{
						TranslateTextureSample(token);
						break;
					}
				case OpcodeType.Ret:
					{
						AddIndent();
						Output.AppendLine($"return output;");
						break;
					}
				case OpcodeType.InterfaceCall:
					{
						break;
					}
				case OpcodeType.Label:
					{
						break;
					}
				case OpcodeType.FirstBitHi:
					{
						break;
					}
				case OpcodeType.FirstBitSHi:
					{
						break;
					}
				case OpcodeType.BfRev:
					{
						break;
					}
				case OpcodeType.Bfi:
					{
						break;
					}
				case OpcodeType.Cut:
					{
						break;
					}
				case OpcodeType.CutStream:
					{
						break;
					}
				case OpcodeType.EmitStream:
					{
						break;
					}
				case OpcodeType.EmitThenCutStream:
					{
						break;
					}
				case OpcodeType.Loop:
					{
						AddIndent();
						Output.AppendLine("while(true){");
						indent++;
						break;
					}
				case OpcodeType.EndLoop:
					{
						indent--;
						AddIndent();
						Output.AppendLine("}");
						break;
					}
				case OpcodeType.Break:
					{
						AddIndent();
						Output.AppendLine("break;");
						indent--;
						break;
					}
				case OpcodeType.BreakC:
					{
						WriteConditional(token);
						break;
					}
				case OpcodeType.ContinueC:
					{
						WriteConditional(token);
						break;
					}
				case OpcodeType.If:
					{
						WriteConditional(token);
						break;
					}
				case OpcodeType.RetC:
					{
						WriteConditional(token);
						break;
					}
				case OpcodeType.Else:
					{
						indent--;
						AddIndent();
						Output.AppendLine("} else {");
						indent++;
						break;
					}
				case OpcodeType.EndSwitch:
				case OpcodeType.EndIf:
					{
						indent--;
						AddIndent();
						Output.AppendLine("}");
						break;
					}
				case OpcodeType.Continue:
					{
						Output.AppendLine("continue;");
						break;
					}
				case OpcodeType.Default:
					{
						AddIndent();
						Output.AppendLine("default:");
						break;
					}
				case OpcodeType.Nop:
					{
						break;
					}
				case OpcodeType.Sync:
					{
						break;
					}
				case OpcodeType.Switch:
					{
						AddIndent();
						indent++;
						Output.Append("switch(int(");
						WriteOperand(token.Operands[0]);
						Output.AppendLine(")){");
						break;
					}
				case OpcodeType.Case:
					{
						AddIndent();
						indent++;
						Output.Append("case ");
						WriteOperand(token.Operands[0]);
						Output.AppendLine(":");
						break;
					}
				case OpcodeType.Eq:
					{
						AddComparision(token, ComparisonType.Eq);
						break;
					}
				case OpcodeType.UShr:
					{
						CallBinaryOp(">>", token, 0, 1, 2);
						break;
					}
				case OpcodeType.IShl:
					{
						CallBinaryOp("<<", token, 0, 1, 2);
						break;
					}
				case OpcodeType.IShr:
					{
						CallBinaryOp(">>", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Ld:
				case OpcodeType.LdMs:
					{
						break;
					}
				case OpcodeType.Discard:
					{
						break;
					}
				case OpcodeType.Lod:
					{
						break;
					}
				case OpcodeType.EvalCentroid:
					{
						break;
					}
				case OpcodeType.EvalSampleIndex:
					{
						break;
					}
				case OpcodeType.EvalSnapped:
					{
						break;
					}
				case OpcodeType.LdStructured:
					{
						break;
					}
				case OpcodeType.LdUavTyped:
					{
						break;
					}
				case OpcodeType.StoreRaw:
					{
						break;
					}
				case OpcodeType.StoreStructured:
					{
						break;
					}
				case OpcodeType.StoreUavTyped:
					{
						break;
					}
				case OpcodeType.LdRaw:
					{
						break;
					}
				case OpcodeType.AtomicAnd:
				case OpcodeType.AtomicOr:
				case OpcodeType.AtomicXor:
				case OpcodeType.AtomicCmpStore:
				case OpcodeType.AtomicIAdd:
				case OpcodeType.AtomicIMax:
				case OpcodeType.AtomicIMin:
				case OpcodeType.AtomicUMax:
				case OpcodeType.AtomicUMin:
				case OpcodeType.ImmAtomicIAdd:
				case OpcodeType.ImmAtomicAnd:
				case OpcodeType.ImmAtomicOr:
				case OpcodeType.ImmAtomicXor:
				case OpcodeType.ImmAtomicExch:
				case OpcodeType.ImmAtomicCmpExch:
				case OpcodeType.ImmAtomicIMax:
				case OpcodeType.ImmAtomicIMin:
				case OpcodeType.ImmAtomicUMax:
				case OpcodeType.ImmAtomicUMin:
					{
						break;
					}
				case OpcodeType.UBfe:
				case OpcodeType.IBfe:
					{
						break;
					}
				case OpcodeType.Rcp:
					{
						break;
					}
				case OpcodeType.F32ToF16:
					{
						break;
					}
				case OpcodeType.F16ToF32:
					{
						break;
					}
				case OpcodeType.INeg:
					{
						break;
					}
				case OpcodeType.DerivRtx:
				case OpcodeType.RtxCoarse:
				case OpcodeType.RtxFine:
					{
						break;
					}
				case OpcodeType.DerivRty:
				case OpcodeType.RtyCoarse:
				case OpcodeType.RtyFine:
					{
						break;
					}
				case OpcodeType.ImmAtomicAlloc:
					{
						break;
					}
				case OpcodeType.ImmAtomicConsume:
					{
						break;
					}
				case OpcodeType.Not:
					{
						break;
					}
				case OpcodeType.Xor:
					{
						CallBinaryOp("^", token, 0, 1, 2);
						break;
					}
				case OpcodeType.Resinfo:
					{
						break;
					}
				case OpcodeType.SamplePos:
					{
						AddCallInterface1("GetSamplePosition", token, 0, 1, 2);
						break;
					}
				case OpcodeType.CountBits:
					{
						AddIndent();
						AddAssignToDest(token.Operands[0]);
						Output.Append(" = bitCount(");
						WriteOperand(token.Operands[1]);
						Output.AppendLine(");");
						break;
					}
				case OpcodeType.Emit:
					{
						break;
					}
				case OpcodeType.Abort:
					{
						AddIndent();
						Output.AppendLine("abort();");
						break;
					}
				case OpcodeType.DMax:
				case OpcodeType.DMin:
				case OpcodeType.DMul:
				case OpcodeType.DEq:
				case OpcodeType.DGe:
				case OpcodeType.DLt:
				case OpcodeType.DNe:
				case OpcodeType.DMov:
				case OpcodeType.DMovC:
				case OpcodeType.DToD:
				case OpcodeType.FToD:
				case OpcodeType.Ddiv:
				case OpcodeType.Dfma:
				case OpcodeType.Drcp:
				case OpcodeType.Msad:
				case OpcodeType.Dtoi:
				case OpcodeType.Dtou:
				case OpcodeType.Itod:
				case OpcodeType.Utod:
					break;
				case OpcodeType.Bufinfo:
					break;
				case OpcodeType.FirstBitLo:
					break;
				case OpcodeType.SampleInfo:
					break;
				case OpcodeType.INe:
					break;
				default:
					throw new Exception($"Unexpected token {token}");
			}
			void CallHelper1(string name, InstructionToken instruction, int dest, int src0)
			{
				AddIndent();
				WriteOperand(instruction.Operands[dest]);
				Output.AppendFormat(" = {0}(", name);
				WriteOperand(instruction.Operands[src0]);
				Output.AppendLine(");");
			}
			void CallHelper2(string name, InstructionToken instruction, int dest, int src0, int src1)
			{
				AddIndent();
				WriteOperand(instruction.Operands[dest]);
				Output.AppendFormat(" = {0}(", name);
				WriteOperand(instruction.Operands[src0]);
				Output.Append(", ");
				WriteOperand(instruction.Operands[src1]);
				Output.AppendLine(");");
			}
			void CallBinaryOp(string symbol, InstructionToken instruction, int dest, int src0, int src1)
			{
				AddIndent();
				WriteOperand(instruction.Operands[dest]);
				Output.Append(" = ");
				WriteOperand(instruction.Operands[src0]);
				Output.AppendFormat(" {0} ", symbol);
				WriteOperand(instruction.Operands[src1]);
				Output.AppendLine(";");
			}
			void WriteConditional(InstructionToken instruction)
			{
				AddIndent();
				Output.Append("if((");
				WriteOperand(instruction.Operands[0]);
				string statement = "";
				if (instruction.Header.OpcodeType == OpcodeType.BreakC)
				{
					statement = "break";
				}
				else if (instruction.Header.OpcodeType == OpcodeType.ContinueC)
				{
					statement = "continue";
				}
				else if (instruction.Header.OpcodeType == OpcodeType.RetC)
				{
					statement = "return";
				}
				if (instruction.TestBoolean == InstructionTestBoolean.Zero)
				{
					if (instruction.Header.OpcodeType == OpcodeType.If)
					{
						Output.AppendLine(") == 0u){");
						indent++;
					}
					else
					{
						Output.AppendFormat(") == 0u){{ {0}; }}\n", statement);
					}
				}
				else
				{
					if (instruction.Header.OpcodeType == OpcodeType.If)
					{
						Output.AppendLine(") != 0u){");
						indent++;
					}
					else
					{
						Output.AppendFormat(") != 0u){{ {0}; }}\n", statement);
					}
				}
			}
			void WriteMoveBinaryOp(Operand dest, Operand source)
			{
				AddIndent();
				AddAssignToDest(dest);
				WriteOperand(source);
				Output.AppendLine(";");
			}
			void AddMOVCBinaryOp(Operand dest, Operand src0, Operand src1, Operand src2)
			{
				AddIndent();
				AddAssignToDest(dest);
				Output.Append("(");
				WriteOperand(src0);
				Output.Append(" >= 0) ? ");
				WriteOperand(src1);
				Output.Append(" : ");
				WriteOperand(src2);
				Output.AppendLine(";");
			}
			void AddAssignToDest(Operand dest)
			{
				WriteOperand(dest);
				Output.Append(" = ");
			}
			void AddComparision(InstructionToken inst, ComparisonType cmpType)
			{
				string[] ops = new string[] {
					"==",
					"<",
					">=",
					"!=",
				};
				AddIndent();
				AddAssignToDest(inst.Operands[0]);
				Output.Append("( ");
				WriteOperand(inst.Operands[1]);
				Output.AppendFormat(" {0} ", ops[(int)cmpType]);
				WriteOperand(inst.Operands[1]);
				Output.AppendLine(" ) ? 1.0 : 0.0;");
			}
			void CallTernaryOp(string op1, string op2, InstructionToken instruction, int dest, int src0, int src1, int src2)
			{
				AddIndent();
				AddAssignToDest(instruction.Operands[dest]);
				WriteOperand(instruction.Operands[src0]);
				Output.AppendFormat(" {0} ", op1);
				WriteOperand(instruction.Operands[src1]);
				Output.AppendFormat(" {0} ", op2);
				WriteOperand(instruction.Operands[src2]);
				Output.AppendLine(";");
			}
			string GetConstructorForType(ShaderVariableType type, int components)
			{
				string[] uintTypes = new string[] { " ", "uint", "uint2", "uint3", "uint4" };
				string[] intTypes = new string[] { " ", "int", "int2", "int3", "int4" };
				string[] floatTypes = new string[] { " ", "float", "float2", "float3", "float4" };
				if (components < 1 || components > 4)
					return "ERROR TOO MANY COMPONENTS IN VECTOR";
				switch (type)
				{
					case ShaderVariableType.UInt:
						return uintTypes[components];
					case ShaderVariableType.Int:
						return intTypes[components];
					case ShaderVariableType.Float:
						return floatTypes[components];
					default:
						return $"ERROR UNSUPPORTED TYPE {type}";
				}
			}
			void AddCallInterface1(string method, InstructionToken inst, int dest, int src0, int src1)
			{
				AddIndent();
				AddAssignToDest(inst.Operands[dest]);
				WriteOperand(inst.Operands[src0]);
				Output.AppendFormat(".{0}(", method);
				WriteOperand(inst.Operands[src1]);
				Output.AppendLine(");");
			}
			void TranslateTextureSample(InstructionToken inst)
			{
				AddIndent();
				AddAssignToDest(inst.Operands[0]);
				WriteOperand(inst.Operands[2]);
				Output.Append(".Sample(");
				WriteOperand(inst.Operands[3]);
				Output.Append(", ");
				WriteOperand(inst.Operands[1]);
				Output.AppendLine(");");
			}
		}
	}
}
