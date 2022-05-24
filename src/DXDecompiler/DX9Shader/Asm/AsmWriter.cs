﻿using DXDecompiler.DX9Shader.Asm;
using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.DX9Shader.Decompiler;
using System;
using System.IO;
using System.Linq;

namespace DXDecompiler.DX9Shader
{
	public class AsmWriter : DecompileWriter
	{
		ShaderModel shader;
		public AsmWriter(ShaderModel shader)
		{
			this.shader = shader;
		}

		public static string Disassemble(byte[] bytecode)
		{
			var shaderModel = ShaderReader.ReadShader(bytecode);
			return Disassemble(shaderModel);
		}

		public static string Disassemble(ShaderModel shaderModel)
		{
			if(shaderModel.Type == ShaderType.Effect)
			{
				var effectWriter = new EffectAsmWriter(shaderModel.EffectChunk);
				return effectWriter.Decompile();
			}
			var asmWriter = new AsmWriter(shaderModel);
			return asmWriter.Decompile();
		}

		public string Disassemble(InstructionToken instruction)
		{
			using var writer = new StringWriter();
			var previous = Writer;
			try
			{
				Writer = writer;
				WriteInstruction(instruction);
				return writer.ToString();
			}
			finally
			{
				Writer = previous;
			}
		}

		static string ApplyModifier(SourceModifier modifier, string value)
		{
			switch(modifier)
			{
				case SourceModifier.None:
					return value;
				case SourceModifier.Negate:
					return $"-{value}";
				case SourceModifier.Bias:
					return $"{value}_bias";
				case SourceModifier.BiasAndNegate:
					return $"-{value}_bias";
				case SourceModifier.Sign:
					return $"{value}_bx2";
				case SourceModifier.SignAndNegate:
					return $"-{value}_bx2";
				case SourceModifier.Complement:
					throw new NotImplementedException();
				case SourceModifier.X2:
					return $"{value}_x2";
				case SourceModifier.X2AndNegate:
					return $"-{value}_x2";
				case SourceModifier.DivideByZ:
					return $"{value}_dz";
				case SourceModifier.DivideByW:
					return $"{value}_dw";
				case SourceModifier.Abs:
					return $"{value}_abs";
				case SourceModifier.AbsAndNegate:
					return $"-{value}_abs";
				case SourceModifier.Not:
					throw new NotImplementedException();
				default:
					throw new NotImplementedException();
			}
		}

		string GetDestinationName(InstructionToken instruction)
		{
			var resultModifier = instruction.GetDestinationResultModifier();

			int destIndex = instruction.GetDestinationParamIndex();

			string registerName = instruction.GetParamRegisterName(destIndex);
			const int registerLength = 4;
			string writeMaskName = instruction.GetDestinationWriteMaskName(registerLength, false);
			string destinationName = $"{registerName}{writeMaskName}";
			if(resultModifier != ResultModifier.None)
			{
				//destinationName += "TODO:Modifier!!!";
			}
			return destinationName;
		}

		string GetSourceName(InstructionToken instruction, int srcIndex, bool isLogicalIndex = true)
		{
			int dataIndex;
			if(isLogicalIndex)
			{
				// compute the actual data index, which might be different from logical index 
				// because of relative addressing mode.

				// TODO: Handle relative addressing mode in a better way,
				// by using `InstructionToken.Operands`:
				// https://github.com/spacehamster/DXDecompiler/pull/6#issuecomment-782958769

				// if instruction has destination, then source starts at the index 1
				// here we assume destination won't have relative addressing,
				// so we assume destination will only occupy 1 slot,
				// that is, the start index for sources will be 1 if instruction.HasDestination is true.
				var begin = instruction.HasDestination ? 1 : 0;
				dataIndex = begin;
				while(srcIndex > begin)
				{
					if(instruction.IsRelativeAddressMode(dataIndex))
					{
						++dataIndex;
					}
					++dataIndex;
					--srcIndex;
				}
			}
			else
			{
				dataIndex = srcIndex;
			}
			string sourceRegisterName = instruction.GetParamRegisterName(dataIndex);
			sourceRegisterName = ApplyModifier(instruction.GetSourceModifier(dataIndex), sourceRegisterName);
			if(instruction.IsRelativeAddressMode(dataIndex))
			{
				sourceRegisterName += $"[{GetSourceName(instruction, dataIndex + 1, isLogicalIndex: false)}]";
			}
			sourceRegisterName += instruction.GetSourceSwizzleName(dataIndex);
			return sourceRegisterName;
		}
		string Version()
		{
			string minor;
			switch(shader.MinorVersion)
			{
				case 0:
					minor = "0";
					break;
				case 255:
					minor = "sw";
					break;
				default:
					minor = "x";
					break;
			}
			if(shader.Type == ShaderType.Vertex)
			{
				return $"vs_{shader.MajorVersion}_{minor}";
			}
			else if(shader.Type == ShaderType.Pixel)
			{
				return $"ps_{shader.MajorVersion}_{minor}";
			}
			else if(shader.Type == ShaderType.Effect)
			{
				return $"fx_{shader.MajorVersion}_{minor}";
			}
			else if(shader.Type == ShaderType.Lib4Vertex)
			{
				return $"lib_4_0_vs_{shader.MajorVersion}_{minor}";
			}
			else if(shader.Type == ShaderType.Lib4Pixel)
			{
				return $"lib_4_0_ps_{shader.MajorVersion}_{minor}";
			}
			else
			{
				return $"{shader.Type}_{shader.MajorVersion}_{minor}";
			}
		}
		protected override void Write()
		{
			WritePreshader();
			WriteConstantTable(shader.ConstantTable);
			Indent++;
			WriteIndent();
			WriteLine("{0}", Version());
			foreach(Token token in shader.Tokens)
			{
				if(token is InstructionToken instruction)
				{
					WriteInstruction(instruction);
				}
			}
			Indent--;
			WriteLine();
			WriteStatistics();
		}
		public void WriteConstantTable(ConstantTable constantTable)
		{
			if(constantTable == null) return;
			WriteLine("//");
			WriteLine("// Generated by {0}", constantTable.Creator);
			if(constantTable.ConstantDeclarations.Count == 0) return;
			WriteLine("//");
			WriteLine("// Parameters:");
			WriteLine("//");
			foreach(var declaration in constantTable.ConstantDeclarations)
			{
				var decompiled = ConstantTypeWriter.Decompile(declaration.Type, declaration.Name, false, Indent);
				WriteLine(string.Join("\n", decompiled.Split('\n').Select(l => $"//   {l}")));
			}
			WriteLine("//");
			WriteLine("//");
			WriteLine("// Registers:");
			WriteLine("//");
			var maxNameLength = constantTable.ConstantDeclarations.Max(cd => cd.Name.Length);
			if(maxNameLength < 12) maxNameLength = 12;
			WriteLine("//   Name{0} Reg   Size", new string(' ', maxNameLength - 4));
			WriteLine("//   {0} ----- ----", new string('-', maxNameLength));
			foreach(var declaration in constantTable.ConstantDeclarations
					.OrderBy(cd => (int)cd.RegisterSet * 1000 + cd.RegisterIndex))
			{
				var size = declaration.Rows * declaration.Columns / 4;
				if(size == 0) size = 1;
				size = declaration.RegisterCount;
				WriteLine(string.Format("//   {0} {1,-5} {2,4}",
					declaration.Name.PadRight(maxNameLength, ' '),
					declaration.GetRegisterName(),
					size));
			}
			WriteLine("//");
			WriteLine("");
		}
		public void WritePreshader()
		{
			if(shader.Preshader == null) return;
			WriteConstantTable(shader.Preshader.Shader.ConstantTable);
			var preshader = PreshaderAsmWriter.Disassemble(shader.Preshader.Shader);
			Write(preshader);
		}
		public void WriteStatistics()
		{
			var instructions = shader.Tokens
				.ToArray();
			int instructionCount = 0;
			int arithmeticCount = 0;
			int textureCount = 0;
			foreach(var token in instructions.OfType<InstructionToken>())
			{
				var size = token.GetInstructionSlotCount();
				instructionCount += size;
				var type = token.GetSlotType();
				if(type.HasFlag(Statistics.SlotType.Arithmetic))
				{
					arithmeticCount += size;
				}
				if(type.HasFlag(Statistics.SlotType.Texture))
				{
					textureCount += size;
				}
			}
			if(arithmeticCount != instructionCount)
			{
				Write("// approximately {0} instruction {1} used ({2} texture, {3} arithmetic)",
					instructionCount,
					instructionCount > 1 ? "slots" : "slot",
					textureCount,
					arithmeticCount);
			}
			else
			{
				Write("// approximately {0} instruction {1} used",
					instructionCount,
					instructionCount > 1 ? "slots" : "slot");
			}
		}
		bool ShouldDeclareSemantics(InstructionToken instruction, RegisterType registerType)
		{
			if(registerType == RegisterType.MiscType) return false;
			if(registerType == RegisterType.Addr) return false;
			if(shader.Type == ShaderType.Vertex)
			{
				return true;
			}
			if(registerType == RegisterType.Input)
			{
				switch(instruction.GetDeclUsage())
				{
					case DeclUsage.TexCoord:
					case DeclUsage.Color:
						return true;
					default:
						return false;
				}
			}
			return true;
		}
		private string GetInstructionModifier(InstructionToken instruction)
		{
			string result = "";
			var modifer = instruction.GetDestinationResultModifier();
			if(modifer.HasFlag(ResultModifier.Saturate))
			{
				result += "_sat";
			}
			if(modifer.HasFlag(ResultModifier.PartialPrecision))
			{
				result += "_pp";
			}
			if(modifer.HasFlag(ResultModifier.Centroid))
			{
				result += "_centroid";
			}
			return result;
		}
		public string SingleToString(byte[] rawBytes)
		{
			if(rawBytes[0] == 0 && rawBytes[1] == 0 && rawBytes[2] == 0 && rawBytes[3] == 128)
				return "-0"; // "Negative" zero
			var floatValue = BitConverter.ToSingle(rawBytes, 0);
			var result = (floatValue).ToString("G7");
			return result;
		}
		bool AddIndentInstruction(Token instruction)
		{
			switch(instruction.Opcode)
			{
				case Opcode.If:
				case Opcode.IfC:
				case Opcode.Rep:
				case Opcode.Loop:
				case Opcode.Else:
					return true;
			}
			return false;
		}
		bool RemoveIndentInstruction(Token instruction)
		{
			switch(instruction.Opcode)
			{
				case Opcode.Else:
				case Opcode.Endif:
				case Opcode.EndLoop:
				case Opcode.EndRep:
					return true;
			}
			return false;
		}
		private void WriteInstruction(InstructionToken instruction)
		{
			if(RemoveIndentInstruction(instruction))
			{
				Indent--;
			}
			if(instruction.Opcode != Opcode.Comment && instruction.Opcode != Opcode.End)
			{
				WriteIndent();
			}
			if(AddIndentInstruction(instruction))
			{
				Indent++;
			}
			switch(instruction.Opcode)
			{
				case Opcode.Abs:
					WriteLine("abs{0} {1}, {2}",
						GetInstructionModifier(instruction),
						GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Add:
					WriteLine("add{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Cmp:
					WriteLine("cmp{0} {1}, {2}, {3}, {4}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Dcl:
					string dclInstruction = "dcl";
					var registerType = instruction.GetParamRegisterType(1);
					if(ShouldDeclareSemantics(instruction, registerType))
					{
						dclInstruction += "_" + instruction.GetDeclSemantic().ToLower();
					}
					WriteLine("{0}{1} {2}", dclInstruction, GetInstructionModifier(instruction), GetDestinationName(instruction));
					break;
				case Opcode.Def:
					{
						string modifier = GetInstructionModifier(instruction);
						string constRegisterName = instruction.GetParamRegisterName(0);
						string constValue0 = SingleToString(instruction.GetParamBytes(1));
						string constValue1 = SingleToString(instruction.GetParamBytes(2));
						string constValue2 = SingleToString(instruction.GetParamBytes(3));
						string constValue3 = SingleToString(instruction.GetParamBytes(4));
						WriteLine("def{0} {1}, {2}, {3}, {4}, {5}", modifier, constRegisterName, constValue0, constValue1, constValue2, constValue3);
					}
					break;
				case Opcode.DefI:
					{
						string constRegisterName = instruction.GetParamRegisterName(0);
						WriteLine("defi{0} {1}, {2}, {3}, {4}, {5}",
							GetInstructionModifier(instruction), constRegisterName,
							instruction.Data[1], instruction.Data[2], instruction.Data[3], instruction.Data[4]);
					}
					break;
				case Opcode.DP2Add:
					WriteLine("dp2add{0} {1}, {2}, {3}, {4}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Dp3:
					WriteLine("dp3{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Dp4:
					WriteLine("dp4{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Else:
					WriteLine("else");
					break;
				case Opcode.Endif:
					WriteLine("endif");
					break;
				case Opcode.Exp:
					WriteLine("exp{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.Frc:
					WriteLine("frc{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.If:
					WriteLine("if {0}", GetSourceName(instruction, 0));
					break;
				case Opcode.IfC:
					WriteLine("if_{0} {1}, {2}",
						((IfComparison)instruction.Modifier).ToString().ToLower(),
						GetSourceName(instruction, 0), GetSourceName(instruction, 1));
					break;
				case Opcode.Log:
					WriteLine("log{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.Lrp:
					WriteLine("lrp{0} {1}, {2}, {3}, {4}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Mad:
					WriteLine("mad{0} {1}, {2}, {3}, {4}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Max:
					WriteLine("max{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Min:
					WriteLine("min{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Mov:
					WriteLine("mov{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.MovA:
					WriteLine("mova{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Mul:
					WriteLine("mul{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Nop:
					WriteLine("nop");
					break;
				case Opcode.Nrm:
					WriteLine("nrm{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Pow:
					WriteLine("pow{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Rcp:
					WriteLine("rcp{0} {1}, {2}",
						GetInstructionModifier(instruction),
						GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Rsq:
					WriteLine("rsq{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sge:
					WriteLine("sge{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Slt:
					WriteLine("slt{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.SinCos:
					if(shader.MajorVersion >= 3)
					{
						WriteLine("sincos {0}, {1}", GetDestinationName(instruction),
							GetSourceName(instruction, 1));
					}
					else
					{
						WriteLine("sincos {0}, {1}, {2}, {3}", GetDestinationName(instruction),
							GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					}
					break;
				case Opcode.Sub:
					WriteLine("sub{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Tex:
					if((shader.MajorVersion == 1 && shader.MinorVersion >= 4) || (shader.MajorVersion > 1))
					{
						WriteLine("texld {0}, {1}, {2}", GetDestinationName(instruction),
							GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					}
					else
					{
						WriteLine("tex {0}", GetDestinationName(instruction));
					}
					break;
				case Opcode.TexLDL:
					WriteLine("texldl {0}, {1}, {2}", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.TexKill:
					WriteLine("texkill {0}", GetDestinationName(instruction));
					break;
				case Opcode.Rep:
					WriteLine("rep {0}",
						GetDestinationName(instruction));
					break;
				case Opcode.EndRep:
					WriteLine("endrep");
					break;
				case Opcode.DSX:
					WriteLine("dsx{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.DSY:
					WriteLine("dsy{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.TexLDD:
					WriteLine("texldd {0}, {1}, {2}, {3}, {4}", GetDestinationName(instruction), GetSourceName(instruction, 1),
						GetSourceName(instruction, 2), GetSourceName(instruction, 3),
						GetSourceName(instruction, 4));
					break;
				case Opcode.BreakC:
					WriteLine("break_{0} {1}, {2}",
						((IfComparison)instruction.Modifier).ToString().ToLower(),
						GetSourceName(instruction, 0),
						GetSourceName(instruction, 1));
					break;
				//TODO: Add tests for Loop, and Lit
				case Opcode.Loop:
					WriteLine("loop {0}, {1}",
						GetSourceName(instruction, 0),
						GetSourceName(instruction, 1));
					break;
				case Opcode.EndLoop:
					WriteLine("endloop");
					break;
				case Opcode.Lit:
					WriteLine("lit{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.ExpP:
					WriteLine("expp{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.LogP:
					WriteLine("logp{0} {1}, {2}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.M4x4:
					WriteLine("m4x4{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.M4x3:
					WriteLine("m4x3{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.M3x3:
					WriteLine("m3x3{0} {1}, {2}, {3}",
						GetInstructionModifier(instruction), GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Call:
					WriteLine("call {0}",
						GetSourceName(instruction, 0));
					break;
				case Opcode.Ret:
					WriteLine("ret");
					break;
				case Opcode.Label:
					WriteLine("label {0}", GetSourceName(instruction, 0));
					break;
				case Opcode.Comment:
				case Opcode.End:
					break;
				default:
					WriteLine(instruction.Opcode.ToString());
					//WriteLine("// Warning - Not Implemented");
					throw new NotImplementedException($"Instruction not implemented {instruction.Opcode}");
			}
		}
	}
}
