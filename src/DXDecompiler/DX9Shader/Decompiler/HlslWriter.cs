using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.DX9Shader.Decompiler;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader
{
	public class HlslWriter : DecompileWriter
	{
		private class SourceOperand
		{
			public string Body { get; set; }
			public string Swizzle { get; set; }
			public string Modifier { get; set; }
			public ParameterType? SamplerType { get; set; }

			public override string ToString()
			{
				var body = string.Format(Modifier, Body);
				if(char.IsDigit(body.Last()))
				{
					body = $"({body})";
				}
				return body + Swizzle;
			}
		}

		private readonly ShaderModel _shader;
		private readonly bool _doAstAnalysis;
		private int _iterationDepth = 0;

		private EffectHLSLWriter _effectWriter;
		private RegisterState _registers;
		string _entryPoint;

		public HlslWriter(ShaderModel shader, bool doAstAnalysis = false, string entryPoint = null)
		{
			_shader = shader;
			_doAstAnalysis = doAstAnalysis;
			if(string.IsNullOrEmpty(entryPoint))
			{
				_entryPoint = $"{_shader.Type}Main";
			}
			else
			{
				_entryPoint = entryPoint;
			}

		}

		public static string Decompile(byte[] bytecode, string entryPoint = null)
		{
			var shaderModel = ShaderReader.ReadShader(bytecode);
			return Decompile(shaderModel);
		}
		public static string Decompile(ShaderModel shaderModel, string entryPoint = null, EffectHLSLWriter effect = null)
		{
			if(shaderModel.Type == ShaderType.Effect)
			{
				return EffectHLSLWriter.Decompile(shaderModel.EffectChunk);
			}
			var hlslWriter = new HlslWriter(shaderModel, false, entryPoint)
			{
				_effectWriter = effect
			};
			return hlslWriter.Decompile();
		}

		private string GetDestinationName(InstructionToken instruction, out string writeMaskName)
		{
			return _registers.GetDestinationName(instruction, out writeMaskName);
		}

		private string GetDestinationNameWithWriteMask(InstructionToken instruction)
		{
			var destinationName = GetDestinationName(instruction, out var writeMask);
			return destinationName + writeMask;
		}

		private SourceOperand GetSourceName(InstructionToken instruction, int srcIndex, bool isLogicalIndex = true)
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

			ParameterType? samplerType = null;
			var registerNumber = instruction.GetParamRegisterNumber(dataIndex);
			var registerType = instruction.GetParamRegisterType(dataIndex);
			if(registerType == RegisterType.Sampler)
			{
				var decl = _registers.FindConstant(RegisterSet.Sampler, registerNumber);
				var type = decl.GetRegisterTypeByOffset(registerNumber - decl.RegisterIndex);
				samplerType = type.Type.ParameterType;
			}


			var body = _registers.GetSourceName(instruction, dataIndex, out var swizzle, out var modifier);
			return new SourceOperand
			{
				Body = body,
				Swizzle = swizzle,
				Modifier = modifier,
				SamplerType = samplerType
			};
		}

		private void WriteInstruction(InstructionToken instruction)
		{
			WriteIndent();
			WriteLine($"// {instruction}");
			switch(instruction.Opcode)
			{
				case Opcode.Def:
				case Opcode.DefI:
				case Opcode.Dcl:
				case Opcode.End:
					return;
				// these opcodes doesn't need indents:
				case Opcode.Else:
					Indent--;
					WriteIndent();
					WriteLine("} else {");
					Indent++;
					return;
				case Opcode.Endif:
					Indent--;
					WriteIndent();
					WriteLine("}");
					return;
				case Opcode.EndRep:
					Indent--;
					_iterationDepth--;
					WriteIndent();
					WriteLine("}");
					return;
			}
			WriteIndent();

			void WriteAssignment(string sourceFormat, params SourceOperand[] args)
			{
				var destination = GetDestinationName(instruction, out var writeMask);
				var sourceResult = string.Format(sourceFormat, args);

				var swizzleSizes = args.Select(x => x.Swizzle.StartsWith(".") ? x.Swizzle.Trim('.').Length : -1);
				var returnsScalar = instruction.Opcode.ReturnsScalar() || swizzleSizes.All(x => x == 1);

				if(writeMask.Length > 0)
				{
					destination += writeMask;
					if(returnsScalar)
					{
						// do nothing, don't need to append write mask as swizzle
					}
					// if the instruction is parallel then we are safe to "edit" source swizzles
					else if(instruction.Opcode.IsParallel(_shader))
					{
						foreach(var arg in args)
						{
							const string xyzw = ".xyzw";
							var trimmedSwizzle = ".";
							if(string.IsNullOrEmpty(arg.Swizzle))
							{
								arg.Swizzle = xyzw;
							}
							while(arg.Swizzle.Length <= 4)
							{
								arg.Swizzle += arg.Swizzle.Last();
							}
							for(var i = 1; i <= 4; ++i)
							{
								if(writeMask.Contains(xyzw[i]))
								{
									trimmedSwizzle += arg.Swizzle[i];
								}
							}
							arg.Swizzle = trimmedSwizzle;
						}
						sourceResult = string.Format(sourceFormat, args);
					}
					// if we cannot "edit" the swizzles, we need to apply write masks on the source result
					else if(sourceResult.Last() != ')')
					{
						sourceResult = $"({sourceResult}){writeMask}";
					}
				}
				WriteLine("{0} = {1};", destination, sourceResult);
			}

			void WriteTextureAssignment(string postFix, SourceOperand sampler, params SourceOperand[] others)
			{
				var operation = sampler.SamplerType switch
				{
					ParameterType.Sampler1D => "tex1D",
					ParameterType.Sampler2D => "tex2D",
					ParameterType.Sampler3D => "tex3D",
					ParameterType.SamplerCube => "texCUBE",
					ParameterType.Sampler => "texUnknown",
					_ => throw new InvalidOperationException(sampler.SamplerType.ToString())
				};
				var args = new SourceOperand[others.Length + 1];
				args[0] = sampler;
				others.CopyTo(args, 1);
				var format = string.Join(", ", args.Select((_, i) => $"{{{i}}}"));
				WriteAssignment($"{operation}{postFix}({format})", args);
			}

			switch(instruction.Opcode)
			{
				case Opcode.Abs:
					WriteAssignment("abs({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.Add:
					WriteAssignment("{0} + {1}", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Cmp:
					// TODO: should be per-component
					WriteAssignment("({0} >= 0) ? {1} : {2}",
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.DP2Add:
					WriteAssignment("dot({0}, {1}) + {2}",
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Dp3:
					WriteAssignment("dot({0}, {1})", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Dp4:
					WriteAssignment("dot({0}, {1})", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Exp:
					WriteAssignment("exp2({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.Frc:
					WriteAssignment("frac({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.If:
					WriteLine("if ({0}) {{", GetSourceName(instruction, 0));
					Indent++;
					break;
				case Opcode.IfC:
					if((IfComparison)instruction.Modifier == IfComparison.GE &&
						instruction.GetSourceModifier(0) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(1) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0) ==
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1))
					{
						WriteLine("if ({0} == 0) {{", instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0));
					}
					else if((IfComparison)instruction.Modifier == IfComparison.LT &&
						instruction.GetSourceModifier(0) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(1) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0) ==
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1))
					{
						WriteLine("if ({0} != 0) {{", instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0));
					}
					else
					{
						string ifComparison;
						switch((IfComparison)instruction.Modifier)
						{
							case IfComparison.GT:
								ifComparison = ">";
								break;
							case IfComparison.EQ:
								ifComparison = "==";
								break;
							case IfComparison.GE:
								ifComparison = ">=";
								break;
							case IfComparison.LE:
								ifComparison = "<=";
								break;
							case IfComparison.NE:
								ifComparison = "!=";
								break;
							case IfComparison.LT:
								ifComparison = "<";
								break;
							default:
								throw new InvalidOperationException();
						}
						WriteLine("if ({0} {2} {1}) {{", GetSourceName(instruction, 0), GetSourceName(instruction, 1), ifComparison);
					}
					Indent++;
					break;
				case Opcode.Log:
					WriteAssignment("log2({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.Lrp:
					WriteAssignment("lerp({1}, {2}, {0})",
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Mad:
					WriteAssignment("{0} * {1} + {2}",
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Max:
					WriteAssignment("max({0}, {1})", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Min:
					WriteAssignment("min({0}, {1})", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Mov:
					WriteAssignment("{0}", GetSourceName(instruction, 1));
					break;
				case Opcode.MovA:
					WriteAssignment("{0}", GetSourceName(instruction, 1));
					break;
				case Opcode.Mul:
					WriteAssignment("{0} * {1}", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Nrm:
					WriteAssignment("normalize({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.Pow:
					WriteAssignment("pow({0}, {1})", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Rcp:
					WriteAssignment("1.0f / {0}", GetSourceName(instruction, 1));
					break;
				case Opcode.Rsq:
					WriteAssignment("1 / sqrt({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.Sge:
					if(instruction.GetSourceModifier(1) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(2) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1) ==
						instruction.GetParamRegisterName(2) + instruction.GetSourceSwizzleName(2))
					{
						WriteAssignment("({0} == 0) ? 1 : 0", new SourceOperand
						{
							Body = instruction.GetParamRegisterName(1),
							Swizzle = instruction.GetSourceSwizzleName(1),
							Modifier = "{0}"
						});
					}
					else
					{
						WriteAssignment("({0} >= {1}) ? 1 : 0", GetSourceName(instruction, 1),
							GetSourceName(instruction, 2));
					}
					break;
				case Opcode.Slt:
					WriteAssignment("({0} < {1}) ? 1 : 0", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.SinCos:
					WriteLine("sincos({1}, {0}, {0});", GetDestinationNameWithWriteMask(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sub:
					WriteAssignment("{0} - {1}", GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Tex:
					if((_shader.MajorVersion == 1 && _shader.MinorVersion >= 4) || (_shader.MajorVersion > 1))
					{
						WriteTextureAssignment(string.Empty, GetSourceName(instruction, 2), GetSourceName(instruction, 1));
					}
					else
					{
						WriteAssignment("tex2D()");
					}
					break;
				case Opcode.TexLDL:
					WriteTextureAssignment("lod", GetSourceName(instruction, 2), GetSourceName(instruction, 1));
					break;
				case Opcode.Comment:
					{
						byte[] bytes = new byte[instruction.Data.Length * sizeof(uint)];
						Buffer.BlockCopy(instruction.Data, 0, bytes, 0, bytes.Length);
						var ascii = FormatUtil.BytesToAscii(bytes);
						WriteLine($"// Comment: {ascii}");
						break;
					}
				case Opcode.Rep:
					WriteLine("for (int it{0} = 0; it{0} < {1}; ++it{0}) {{", _iterationDepth, GetSourceName(instruction, 0));
					_iterationDepth++;
					Indent++;
					break;
				case Opcode.TexKill:
					WriteLine("clip({0});", GetDestinationNameWithWriteMask(instruction));
					break;
				case Opcode.DSX:
					WriteAssignment("ddx({0})", GetSourceName(instruction, 1));
					break;
				case Opcode.DSY:
					WriteAssignment("ddy({0})", GetSourceName(instruction, 1));
					break;
				default:
					throw new NotImplementedException(instruction.Opcode.ToString());
			}
		}
		void WriteTemps()
		{
			Dictionary<RegisterKey, int> tempRegisters = new Dictionary<RegisterKey, int>();
			foreach(var inst in _shader.Instructions)
			{
				foreach(var operand in inst.Operands)
				{
					if(operand is DestinationOperand dest)
					{
						if(dest.RegisterType == RegisterType.Temp
							|| (_shader.Type == ShaderType.Vertex && dest.RegisterType == RegisterType.Addr))
						{
							var registerKey = new RegisterKey(dest.RegisterType, dest.RegisterNumber);
							if(!tempRegisters.ContainsKey(registerKey))
							{
								var reg = new RegisterDeclaration(registerKey);
								_registers.RegisterDeclarations[registerKey] = reg;
								tempRegisters[registerKey] = (int)inst.GetDestinationWriteMask();
							}
							else
							{
								tempRegisters[registerKey] |= (int)inst.GetDestinationWriteMask();
							}
						}
					}
				}
			}
			if(tempRegisters.Count == 0) return;
			foreach(IGrouping<int, RegisterKey> group in tempRegisters.GroupBy(
				kv => kv.Value,
				kv => kv.Key))
			{
				int writeMask = group.Key;
				string writeMaskName = writeMask switch
				{
					0x1 => "float",
					0x3 => "float2",
					0x7 => "float3",
					0xF => "float4",
					_ => "float4",// TODO
				};
				WriteIndent();
				WriteLine("{0} {1};", writeMaskName, string.Join(", ", group));
			}
		}
		protected override void Write()
		{
			if(_shader.Type == ShaderType.Expression)
			{
				throw new InvalidOperationException($"Expression should be written using {nameof(ExpressionHLSLWriter)} in {nameof(EffectHLSLWriter)}");
			}
			_registers = new RegisterState(_shader);

			foreach(var declaration in _registers.ConstantDeclarations)
			{
				if(_effectWriter?.CommonConstantDeclarations.ContainsKey(declaration.Name) is true)
				{
					// skip common constant declarations
					continue;
				}
				// write constant declaration
				var decompiled = ConstantTypeWriter.Decompile(declaration, _shader);
				var assignment = string.IsNullOrEmpty(decompiled.DefaultValue)
					? string.Empty
					: $" = {decompiled.DefaultValue}";
				WriteLine($"{decompiled.Code}{decompiled.RegisterAssignmentString}{assignment};");
			}


			ProcessMethodInputType(out var methodParameters);
			ProcessMethodOutputType(out var methodReturnType, out var methodSemantic);
			WriteLine("{0} {1}({2}){3}",
				methodReturnType,
				_entryPoint,
				methodParameters,
				methodSemantic);
			WriteLine("{");
			Indent++;

			if(_shader.Preshader != null)
			{
				var preshader = PreshaderWriter.Decompile(_shader.Preshader, Indent, out var ctabOverride);
				_registers.CtabOverride = ctabOverride;
				WriteLine(preshader);
			}

			if(_registers.MethodOutputRegisters.Count > 1)
			{
				WriteIndent();
				WriteLine($"{methodReturnType} o;");
			}
			else
			{
				var output = _registers.MethodOutputRegisters.First().Value;
				WriteIndent();
				WriteLine("{0} {1};", methodReturnType, _registers.GetRegisterName(output.RegisterKey));
			}
			WriteTemps();
			HlslAst ast = null;
			if(_doAstAnalysis)
			{
				var parser = new BytecodeParser();
				ast = parser.Parse(_shader);
				ast.ReduceTree();

				WriteAst(ast);
			}
			else
			{
				WriteInstructionList();

				if(_registers.MethodOutputRegisters.Count > 1)
				{
					WriteIndent();
					WriteLine($"return o;");
				}
				else
				{
					var output = _registers.MethodOutputRegisters.First().Value;
					WriteIndent();
					WriteLine($"return {_registers.GetRegisterName(output.RegisterKey)};");
				}

			}
			Indent--;
			WriteLine("}");
		}

		private void WriteStructureDeclaration(IEnumerable<RegisterDeclaration> registers, string postFix, out string typeName)
		{
			var declarations = registers
				.Select(x =>
				{
					var registerName = Operand.GetParamRegisterName(x.RegisterKey.Type, x.RegisterKey.Number);
					var comment = $"// {x.RegisterKey} {registerName}";
					var code = $"{x.TypeName} {x.Name} : {x.Semantic};";
					return (Comment: comment, Code: code);
				});
			var codes = declarations.Select(t => t.Code).ToArray();
			typeName = $"{_entryPoint}_{postFix}";
			if(_effectWriter is not null)
			{
				// check if a compatible type has alreaady been defined inside this effect
				foreach(var (name, existing) in _effectWriter.InputOutputStructures)
				{
					if(codes.All(x => existing.Contains(x)))
					{
						typeName = name;
						// we don't need to write declaration again in this case
						return;
					}
				}
				_effectWriter.InputOutputStructures.Add((typeName, codes));
			}

			WriteLine($"struct {typeName}");
			WriteLine("{");
			Indent++;
			foreach(var (comment, code) in declarations)
			{
				WriteIndent();
				WriteLine(comment);
				WriteIndent();
				WriteLine(code);
			}
			Indent--;
			WriteLine("};");
			WriteLine();
		}

		private void ProcessMethodInputType(out string methodParameters)
		{
			var registers = _registers.MethodInputRegisters.Values;
			switch(registers.Count)
			{
				case 0:
					methodParameters = string.Empty;
					break;
				case 1:
					var input = registers.First();
					methodParameters = $"{input.TypeName} {input.Name} : {input.Semantic}";
					break;
				default:
					WriteStructureDeclaration(registers, "Input", out var inputTypeName);
					methodParameters = $"{inputTypeName} i";
					break;
			}
		}

		private void ProcessMethodOutputType(out string methodReturnType, out string methodSemantic)
		{
			var registers = _registers.MethodOutputRegisters.Values;
			switch(registers.Count)
			{
				case 0:
					throw new InvalidOperationException();
				case 1:
					methodReturnType = registers.First().TypeName;
					string semantic = registers.First().Semantic;
					methodSemantic = $" : {semantic}";
					break;
				default:
					WriteStructureDeclaration(registers, "Output", out methodReturnType);
					methodSemantic = string.Empty;
					break;
			};
		}

		private void WriteAst(HlslAst ast)
		{
			var compiler = new NodeCompiler(_registers);

			var rootGroups = ast.Roots.GroupBy(r => r.Key.RegisterKey);
			if(_registers.MethodOutputRegisters.Count == 1)
			{
				var rootGroup = rootGroups.Single();
				var registerKey = rootGroup.Key;
				var roots = rootGroup.OrderBy(r => r.Key.ComponentIndex).Select(r => r.Value).ToList();
				string statement = compiler.Compile(roots, 4);

				WriteLine($"return {statement};");
			}
			else
			{
				foreach(var rootGroup in rootGroups)
				{
					var registerKey = rootGroup.Key;
					var roots = rootGroup.OrderBy(r => r.Key.ComponentIndex).Select(r => r.Value).ToList();
					RegisterDeclaration outputRegister = _registers.MethodOutputRegisters[registerKey];
					string statement = compiler.Compile(roots, roots.Count);

					WriteLine($"o.{outputRegister.Name} = {statement};");
				}

				WriteLine();
				WriteLine($"return o;");
			}
		}

		private void WriteInstructionList()
		{

			foreach(InstructionToken instruction in _shader.Tokens.OfType<InstructionToken>())
			{
				WriteInstruction(instruction);
			}
			WriteLine();
		}
	}
}
