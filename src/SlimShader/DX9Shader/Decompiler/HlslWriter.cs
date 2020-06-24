using SlimShader.DX9Shader.Bytecode.Declaration;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class HlslWriter : DecompileWriter
	{
		private readonly ShaderModel _shader;
		private readonly bool _doAstAnalysis;

		public RegisterState _registers;
		string _entryPoint;

		public HlslWriter(ShaderModel shader, bool doAstAnalysis = false, string entryPoint = null)
		{
			_shader = shader;
			_doAstAnalysis = doAstAnalysis;
			if (string.IsNullOrEmpty(entryPoint))
			{
				_entryPoint = $"{_shader.Type}Main";
			} else
			{
				_entryPoint = entryPoint;
			}

		}

		public static string Decompile(byte[] bytecode, string entryPoint = null)
		{
			var shaderModel = ShaderReader.ReadShader(bytecode);
			return Decompile(shaderModel);
		}
		public static string Decompile(ShaderModel shaderModel, string entryPoint = null)
		{
			if (shaderModel.Type == ShaderType.Effect)
			{
				return EffectHLSLWriter.Decompile(shaderModel.EffectChunk);
			}
			var hlslWriter = new HlslWriter(shaderModel, false, entryPoint);
			return hlslWriter.Decompile();
		}

		private string GetDestinationName(Token instruction)
		{
			return _registers.GetDestinationName(instruction);
		}

		private string GetSourceName(Token instruction, int srcIndex)
		{
			return _registers.GetSourceName(instruction, srcIndex);
		}

		private static string GetConstantTypeName(ConstantType type)
		{
			switch (type.ParameterClass)
			{
				case ParameterClass.Scalar:
					return type.ParameterType.GetDescription();
				case ParameterClass.Vector:
					return type.ParameterType.GetDescription() + type.Columns;
				case ParameterClass.Struct:
					return "struct";
				case ParameterClass.MatrixColumns:
					return $"column_major {type.ParameterType.GetDescription()}{type.Rows}x{type.Columns}";
				case ParameterClass.MatrixRows:
					return $"row_major {type.ParameterType.GetDescription()}{type.Rows}x{type.Columns}";
				case ParameterClass.Object:
					switch (type.ParameterType)
					{
						case ParameterType.Sampler1D:
						case ParameterType.Sampler2D:
						case ParameterType.Sampler3D:
						case ParameterType.SamplerCube:
							return "sampler";
						default:
							throw new NotImplementedException();
					}
			}
			throw new NotImplementedException();
		}

		private void WriteInstruction(Token instruction)
		{
			WriteIndent();
			WriteLine($"// {instruction}");
			WriteIndent();
			switch (instruction.Opcode)
			{
				case Opcode.Abs:
					WriteLine("{0} = abs({1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1));
					break;
				case Opcode.Add:
					WriteLine("{0} = {1} + {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Cmp:
					// TODO: should be per-component
					WriteLine("{0} = ({1} >= 0) ? {2} : {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.DP2Add:
					WriteLine("{0} = dot({1}, {2}) + {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Dp3:
					WriteLine("{0} = dot({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Dp4:
					WriteLine("{0} = dot({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Else:
					Indent--;
					WriteLine("} else {");
					Indent++;
					break;
				case Opcode.Endif:
					Indent--;
					WriteLine("}");
					break;
				case Opcode.Exp:
					WriteLine("{0} = exp2({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Frc:
					WriteLine("{0} = frac({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.If:
					WriteLine("if ({0}) {{", GetSourceName(instruction, 0));
					Indent++;
					break;
				case Opcode.IfC:
					if ((IfComparison)instruction.Modifier == IfComparison.GE &&
						instruction.GetSourceModifier(0) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(1) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0) ==
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1))
					{
						WriteLine("if ({0} == 0) {{", instruction.GetParamRegisterName(0) + instruction.GetSourceSwizzleName(0));
					}
					else if ((IfComparison)instruction.Modifier == IfComparison.LT &&
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
						switch ((IfComparison)instruction.Modifier)
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
					WriteLine("{0} = log2({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Lrp:
					WriteLine("{0} = lerp({2}, {3}, {1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Mad:
					WriteLine("{0} = {1} * {2} + {3};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2), GetSourceName(instruction, 3));
					break;
				case Opcode.Max:
					WriteLine("{0} = max({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Min:
					WriteLine("{0} = min({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Mov:
					WriteLine("{0} = {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.MovA:
					WriteLine("{0} = {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Mul:
					WriteLine("{0} = {1} * {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Nrm:
					WriteLine("{0} = normalize({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Pow:
					WriteLine("{0} = pow({1}, {2});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Rcp:
					WriteLine("{0} = 1 / {1};", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Rsq:
					WriteLine("{0} = 1 / sqrt({1});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sge:
					if (instruction.GetSourceModifier(1) == SourceModifier.AbsAndNegate &&
						instruction.GetSourceModifier(2) == SourceModifier.Abs &&
						instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1) ==
						instruction.GetParamRegisterName(2) + instruction.GetSourceSwizzleName(2))
					{
						WriteLine("{0} = ({1} == 0) ? 1 : 0;", GetDestinationName(instruction),
							instruction.GetParamRegisterName(1) + instruction.GetSourceSwizzleName(1));
					}
					else
					{
						WriteLine("{0} = ({1} >= {2}) ? 1 : 0;", GetDestinationName(instruction), GetSourceName(instruction, 1),
							GetSourceName(instruction, 2));
					}
					break;
				case Opcode.Slt:
					WriteLine("{0} = ({1} < {2}) ? 1 : 0;", GetDestinationName(instruction), GetSourceName(instruction, 1),
						GetSourceName(instruction, 2));
					break;
				case Opcode.SinCos:
					WriteLine("sincos({1}, {0}, {0});", GetDestinationName(instruction), GetSourceName(instruction, 1));
					break;
				case Opcode.Sub:
					WriteLine("{0} = {1} - {2};", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Tex:
					if ((_shader.MajorVersion == 1 && _shader.MinorVersion >= 4) || (_shader.MajorVersion > 1))
					{
						WriteLine("{0} = tex2D({2}, {1});", GetDestinationName(instruction),
							GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					}
					else
					{
						WriteLine("{0} = tex2D();", GetDestinationName(instruction));
					}
					break;
				case Opcode.TexLDL:
					WriteLine("{0} = tex2Dlod({2}, {1});", GetDestinationName(instruction),
						GetSourceName(instruction, 1), GetSourceName(instruction, 2));
					break;
				case Opcode.Comment:
					{
						byte[] bytes = new byte[instruction.Data.Length * sizeof(uint)];
						Buffer.BlockCopy(instruction.Data, 0, bytes, 0, bytes.Length);
						var ascii = FormatUtil.BytesToAscii(bytes);
						WriteLine($"// Comment: {ascii}");
						break;
					}
				case Opcode.End:
					break;
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
						if(dest.RegisterType == RegisterType.Temp)
						{

							var registerKey = new RegisterKey(dest.RegisterType, (int)dest.RegisterNumber);
							if (!tempRegisters.ContainsKey(registerKey))
							{
								var reg = new RegisterDeclaration(registerKey);
								_registers._registerDeclarations[registerKey] = reg;
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
			if (tempRegisters.Count == 0) return;
			foreach(IGrouping<int, RegisterKey> group in tempRegisters.GroupBy(
				kv => kv.Value,
				kv => kv.Key))
			{
				int writeMask = group.Key;
				string writeMaskName; switch (writeMask)
				{
					case 0x1:
						writeMaskName = "float";
						break;
					case 0x3:
						writeMaskName = "float2";
						break;
					case 0x7:
						writeMaskName = "float3";
						break;
					case 0xF:
						writeMaskName = "float4";
						break;
					default:
						// TODO
						writeMaskName = "float4";
						break;
						//throw new NotImplementedException();
				}
				WriteLine("{0} {1};", writeMaskName, string.Join(", ", group));
			}
			

		}
		protected override void Write()
		{
			if (_shader.Type == ShaderType.Expression)
			{
				Write($"// Writing expression");
				WriteExpression(_shader);
				return;
			}
			_registers = new RegisterState(_shader);

			WriteConstantDeclarations();

			if (_shader.Preshader != null)
			{
				WriteExpression(_shader.Preshader.Shader);
			}
			if (_registers.MethodInputRegisters.Count > 1)
			{
				WriteInputStructureDeclaration();
			}

			if (_registers.MethodOutputRegisters.Count > 1)
			{
				WriteOutputStructureDeclaration();
			}

			string methodReturnType = GetMethodReturnType();
			string methodParameters = GetMethodParameters();
			string methodSemantic = GetMethodSemantic();
			WriteLine("{0} {1}({2}){3}", 
				methodReturnType, 
				_entryPoint, 
				methodParameters,
				methodSemantic);
			WriteLine("{");
			Indent++;

			if (_registers.MethodOutputRegisters.Count > 1)
			{
				var outputStructType = _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
				WriteIndent();
				WriteLine($"{outputStructType} o;");
			} else
			{
				var output = _registers.MethodOutputRegisters.First().Value;
				WriteLine("{0} {1};", methodReturnType, _registers.GetRegisterName(output.RegisterKey));
			}
			WriteTemps();
			HlslAst ast = null;
			if (_doAstAnalysis)
			{
				var parser = new BytecodeParser();
				ast = parser.Parse(_shader);
				ast.ReduceTree();

				WriteAst(ast);
			}
			else
			{
				WriteInstructionList();

				if (_registers.MethodOutputRegisters.Count > 1)
				{
					WriteLine($"return o;");
				}
				else
				{
					var output = _registers.MethodOutputRegisters.First().Value;
					WriteLine($"return {_registers.GetRegisterName(output.RegisterKey)};");
				}
				
			}
			Indent--;
			WriteLine("}");
		}

		private void WriteConstantDeclarations()
		{
			if (_registers.ConstantDeclarations.Count != 0)
			{
				foreach (ConstantDeclaration declaration in _registers.ConstantDeclarations)
				{
					Write(declaration);
				}
			}
		}
		private void Write(ConstantDeclaration declaration)
		{
			Write(declaration.Type, declaration.Name);
			if (!declaration.DefaultValue.All(v => v == 0))
			{
				Write(" = {{ {0} }}", string.Join(", ", declaration.DefaultValue));
			}
			WriteLine(";");
			WriteLine();
		}
		private void Write(ConstantType type, string name, bool isStructMember = false)
		{
			string typeName = GetConstantTypeName(type);
			WriteIndent();
			Write("{0}", typeName);
			if (type.ParameterClass == ParameterClass.Struct)
			{
				WriteLine("");
				WriteLine("{");
				Indent++;
				foreach (var member in type.Members)
				{
					Write(member.Type, member.Name, true);
				}
				Indent--;
				WriteIndent();
				Write("}");
			}
			Write(" {0}", name);
			if (type.Elements > 1)
			{
				Write("[{0}]", type.Elements);
			}
			if (isStructMember)
			{
				Write(";\n");
			}
		}
		private void WriteInputStructureDeclaration()
		{
			var inputStructType = _shader.Type == ShaderType.Pixel ? "PS_IN" : "VS_IN";
			WriteLine($"struct {inputStructType}");
			WriteLine("{");
			Indent++;
			foreach (var input in _registers.MethodInputRegisters.Values)
			{
				WriteLine($"{input.TypeName} {input.Name} : {input.Semantic};");
			}
			Indent--;
			WriteLine("};");
			WriteLine();
		}

		private void WriteOutputStructureDeclaration()
		{
			var outputStructType = _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
			WriteLine($"struct {outputStructType}");
			WriteLine("{");
			Indent++;
			foreach (var output in _registers.MethodOutputRegisters.Values)
			{
				WriteLine($"// {output.RegisterKey} {Operand.GetParamRegisterName(output.RegisterKey.Type, output.RegisterKey.Number)}");
				WriteLine($"{output.TypeName} {output.Name} : {output.Semantic};");
			}
			Indent--;
			WriteLine("};");
			WriteLine();
		}

		private string GetMethodReturnType()
		{
			switch (_registers.MethodOutputRegisters.Count)
			{
				case 0:
					throw new InvalidOperationException();
				case 1:
					return _registers.MethodOutputRegisters.Values.First().TypeName;
				default:
					return _shader.Type == ShaderType.Pixel ? "PS_OUT" : "VS_OUT";
			}
		}

		private string GetMethodSemantic()
		{
			switch (_registers.MethodOutputRegisters.Count)
			{
				case 0:
					throw new InvalidOperationException();
				case 1:
					string semantic = _registers.MethodOutputRegisters.Values.First().Semantic;
					return $" : {semantic}";
				default:
					return string.Empty;
			}
		}

		private string GetMethodParameters()
		{
			if (_registers.MethodInputRegisters.Count == 0)
			{
				return string.Empty;
			}
			else if (_registers.MethodInputRegisters.Count == 1)
			{
				var input = _registers.MethodInputRegisters.Values.First();
				return $"{input.TypeName} {input.Name} : {input.Semantic}";
			}

			return _shader.Type == ShaderType.Pixel
					? "PS_IN i"
					: "VS_IN i";
		}

		private void WriteAst(HlslAst ast)
		{
			var compiler = new NodeCompiler(_registers);

			var rootGroups = ast.Roots.GroupBy(r => r.Key.RegisterKey);
			if (_registers.MethodOutputRegisters.Count == 1)
			{
				var rootGroup = rootGroups.Single();
				var registerKey = rootGroup.Key;
				var roots = rootGroup.OrderBy(r => r.Key.ComponentIndex).Select(r => r.Value).ToList();
				string statement = compiler.Compile(roots, 4);

				WriteLine($"return {statement};");
			}
			else
			{
				foreach (var rootGroup in rootGroups)
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

			foreach (Token instruction in _shader.Tokens)
			{
				WriteInstruction(instruction);
			}
			WriteLine();
		}
		private void WriteExpression(ShaderModel shader)
		{
			WriteLine("void {0}Preshader(){{", _entryPoint);
			Indent++;
			WriteLine($"// {shader.Type}_{shader.MajorVersion}_{shader.MinorVersion}");
			foreach(var token in shader.Fxlc.Tokens)
			{
				WriteLine($"// {token.ToString(shader.ConstantTable, shader.Cli)}");
			}
			if (shader.Prsi != null)
			{
				WriteLine(shader.Prsi.Dump());
			}
			Indent++;
			WriteLine("}");

		}
	}
}
