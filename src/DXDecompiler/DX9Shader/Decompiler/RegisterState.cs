using DXDecompiler.DX9Shader.Bytecode.Ctab;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DXDecompiler.DX9Shader
{
	public sealed class RegisterState
	{
		public readonly bool ColumnMajorOrder = true;

		private readonly CultureInfo _culture = CultureInfo.InvariantCulture;
		private readonly ShaderType _shaderType;

		private ICollection<Constant> _constantDefinitions = new List<Constant>();
		private ICollection<ConstantInt> _constantIntDefinitions = new List<ConstantInt>();

		public IDictionary<RegisterKey, RegisterDeclaration> RegisterDeclarations { get; } = new Dictionary<RegisterKey, RegisterDeclaration>();
		public IDictionary<RegisterKey, RegisterDeclaration> MethodInputRegisters { get; } = new Dictionary<RegisterKey, RegisterDeclaration>();
		public IDictionary<RegisterKey, RegisterDeclaration> MethodOutputRegisters { get; } = new Dictionary<RegisterKey, RegisterDeclaration>();
		public ICollection<ConstantDeclaration> ConstantDeclarations { get; private set; }

		public HashSet<uint> CtabOverride { get; set; }

		public RegisterState(ShaderModel shader)
		{
			_shaderType = shader.Type;
			Load(shader);
		}



		private void Load(ShaderModel shader)
		{
			ConstantDeclarations = shader.ConstantTable.ConstantDeclarations;
			foreach(var constantDeclaration in ConstantDeclarations)
			{
				RegisterType registerType;
				switch(constantDeclaration.RegisterSet)
				{
					case RegisterSet.Bool:
						registerType = RegisterType.ConstBool;
						break;
					case RegisterSet.Float4:
						registerType = RegisterType.Const;
						break;
					case RegisterSet.Int4:
						registerType = RegisterType.ConstInt;
						break;
					case RegisterSet.Sampler:
						registerType = RegisterType.Sampler;
						break;
					default:
						throw new InvalidOperationException();
				}
				if(registerType == RegisterType.Sampler)
				{
					// Use declaration from declaration instruction instead
					continue;
				}
				for(uint r = 0; r < constantDeclaration.RegisterCount; r++)
				{
					var registerKey = new RegisterKey(registerType, constantDeclaration.RegisterIndex + r);
					var registerDeclaration = new RegisterDeclaration(registerKey);
					RegisterDeclarations.Add(registerKey, registerDeclaration);
				}
			}

			foreach(var instruction in shader.Tokens.OfType<InstructionToken>().Where(i => i.HasDestination))
			{
				if(instruction.Opcode == Opcode.Dcl)
				{
					var registerDeclaration = new RegisterDeclaration(instruction);
					RegisterKey registerKey = registerDeclaration.RegisterKey;

					RegisterDeclarations.Add(registerKey, registerDeclaration);

					switch(registerKey.Type)
					{
						case RegisterType.Input:
						case RegisterType.MiscType:
						case RegisterType.Texture when shader.Type == ShaderType.Pixel:
							MethodInputRegisters.Add(registerKey, registerDeclaration);
							break;
						case RegisterType.Output:
						case RegisterType.ColorOut:
						case RegisterType.AttrOut when shader.MajorVersion == 3 && shader.Type == ShaderType.Vertex:
							MethodOutputRegisters.Add(registerKey, registerDeclaration);
							break;
						case RegisterType.Sampler:
						case RegisterType.Addr:
							break;
						default:
							throw new Exception($"Unexpected dcl {registerKey.Type}");
					}
				}
				else if(instruction.Opcode == Opcode.Def)
				{
					var constant = new Constant(
						instruction.GetParamRegisterNumber(0),
						instruction.GetParamSingle(1),
						instruction.GetParamSingle(2),
						instruction.GetParamSingle(3),
						instruction.GetParamSingle(4));
					_constantDefinitions.Add(constant);
				}
				else if(instruction.Opcode == Opcode.DefI)
				{
					var constantInt = new ConstantInt(instruction.GetParamRegisterNumber(0),
						instruction.Data[1],
						instruction.Data[2],
						instruction.Data[3],
						instruction.Data[4]);
					_constantIntDefinitions.Add(constantInt);
				}
				else
				{
					// Find all assignments to color outputs, because pixel shader outputs are not declared.
					int destIndex = instruction.GetDestinationParamIndex();
					RegisterType registerType = instruction.GetParamRegisterType(destIndex);
					var registerNumber = instruction.GetParamRegisterNumber(destIndex);
					var registerKey = new RegisterKey(registerType, registerNumber);
					if(RegisterDeclarations.ContainsKey(registerKey) == false)
					{
						var reg = new RegisterDeclaration(registerKey);
						RegisterDeclarations[registerKey] = reg;
						switch(registerType)
						{
							case RegisterType.AttrOut:
							case RegisterType.ColorOut:
							case RegisterType.DepthOut:
							case RegisterType.Output:
							case RegisterType.RastOut:
								MethodOutputRegisters[registerKey] = reg;
								break;

						}
					}

				}
			}
		}

		public string GetDestinationName(InstructionToken instruction, out string writeMaskName)
		{
			int destIndex = instruction.GetDestinationParamIndex();
			RegisterKey registerKey = instruction.GetParamRegisterKey(destIndex);

			string registerName = GetRegisterName(registerKey);
			registerName = registerName ?? instruction.GetParamRegisterName(destIndex);
			var registerLength = GetRegisterFullLength(registerKey);
			writeMaskName = instruction.GetDestinationWriteMaskName(registerLength, true);

			return registerName;
		}

		public string GetSourceName(InstructionToken instruction, int srcIndex, out string swizzle, out string modifier)
		{
			string sourceRegisterName;
			var registerKey = instruction.GetParamRegisterKey(srcIndex);
			var registerType = instruction.GetParamRegisterType(srcIndex);
			var registerNumber = instruction.GetParamRegisterNumber(srcIndex);
			switch(registerType)
			{
				case RegisterType.Const:
				case RegisterType.Const2:
				case RegisterType.Const3:
				case RegisterType.Const4:
					if(CtabOverride?.Contains(registerNumber) is true)
					{
						swizzle = instruction.GetSourceSwizzleName(srcIndex, true);
						modifier = GetModifier(instruction.GetSourceModifier(srcIndex));
						return $"expr{registerNumber}";
					}
					goto case RegisterType.ConstInt;
				case RegisterType.ConstBool:
				case RegisterType.ConstInt:
					sourceRegisterName = GetSourceConstantName(instruction, srcIndex);
					if(sourceRegisterName != null)
					{
						swizzle = string.Empty;
						modifier = "{0}";
						return sourceRegisterName;
					}

					RegisterSet registerSet;
					switch(registerType)
					{
						case RegisterType.Const:
						case RegisterType.Const2:
						case RegisterType.Const3:
						case RegisterType.Const4:
							registerSet = RegisterSet.Float4;
							break;
						case RegisterType.ConstBool:
							registerSet = RegisterSet.Bool;
							break;
						case RegisterType.ConstInt:
							registerSet = RegisterSet.Int4;
							break;
						default:
							throw new NotImplementedException();
					}
					var decl = FindConstant(registerSet, registerNumber);
					if(decl == null)
					{
						// Constant register not found in def statements nor the constant table
						//TODO:
						swizzle = "Error";
						modifier = "{0}";
						return $"Error {registerType}{registerNumber}";
						//throw new NotImplementedException();
					}
					string indexer = null;
					if(instruction.IsRelativeAddressMode(srcIndex))
					{
						indexer = GetSourceName(instruction, srcIndex + 1, out var indexSwizzle, out var indexModifier);
						indexer = string.Format(indexModifier, indexer + indexSwizzle);
					}
					sourceRegisterName = decl.GetConstantNameByRegisterNumber(registerNumber, indexer);
					break;
				default:
					sourceRegisterName = GetRegisterName(registerKey);
					break;
			}

			sourceRegisterName ??= instruction.GetParamRegisterName(srcIndex);

			swizzle = instruction.GetSourceSwizzleName(srcIndex, true);
			modifier = GetModifier(instruction.GetSourceModifier(srcIndex));
			return sourceRegisterName;
		}

		public uint GetRegisterFullLength(RegisterKey registerKey)
		{
			// TODO: handle other cases as well
			if(registerKey.Type == RegisterType.Const)
			{
				var constant = FindConstant(RegisterSet.Float4, registerKey.Number);
				var data = constant.GetRegisterTypeByOffset(registerKey.Number - constant.RegisterIndex);
				if(data.Type.ParameterType != ParameterType.Float)
				{
					throw new NotImplementedException();
				}
				if(data.Type.ParameterClass == ParameterClass.MatrixColumns)
				{
					return data.Type.Rows;
				}
				return data.Type.Columns;
			}

			RegisterDeclaration decl = RegisterDeclarations[registerKey];
			switch(decl.TypeName)
			{
				case "float":
					return 1;
				case "float2":
					return 2;
				case "float3":
					return 3;
				case "float4":
					return 4;
				default:
					throw new InvalidOperationException();
			}
		}

		public string GetRegisterName(RegisterKey registerKey)
		{
			var decl = RegisterDeclarations[registerKey];
			switch(registerKey.Type)
			{
				case RegisterType.Input:
				case RegisterType.Addr when _shaderType is ShaderType.Pixel:
					return (MethodInputRegisters.Count == 1) ? decl.Name : ("i." + decl.Name);
				case RegisterType.RastOut:
				case RegisterType.Output:
				case RegisterType.AttrOut:
				case RegisterType.ColorOut:
					return (MethodOutputRegisters.Count == 1) ? decl.Name : ("o." + decl.Name);
				case RegisterType.Const:
					throw new NotSupportedException($"Use {nameof(GetSourceName)} instead");
				case RegisterType.Sampler:
					ConstantDeclaration samplerDecl = FindConstant(RegisterSet.Sampler, registerKey.Number);
					if(samplerDecl != null)
					{
						var offset = registerKey.Number - samplerDecl.RegisterIndex;
						return samplerDecl.GetMemberNameByOffset(offset);
					}
					else
					{
						throw new NotImplementedException();
					}
				case RegisterType.MiscType:
					switch(registerKey.Number)
					{
						case 0:
							return "vFace";
						case 1:
							return "vPos";
						default:
							throw new NotImplementedException();
					}
				case RegisterType.Addr when _shaderType is not ShaderType.Pixel:
				case RegisterType.Temp:
					return registerKey.ToString();
				default:
					throw new NotImplementedException();
			}
		}

		public ConstantDeclaration FindConstant(RegisterInputNode register)
		{
			if(register.RegisterComponentKey.Type != RegisterType.Const)
			{
				return null;
			}
			return FindConstant(RegisterSet.Float4, register.RegisterComponentKey.Number);
		}

		public ConstantDeclaration FindConstant(RegisterSet set, uint index)
		{
			return ConstantDeclarations.FirstOrDefault(c =>
				c.RegisterSet == set &&
				c.ContainsIndex(index));
		}

		public ConstantDeclaration FindConstant(ParameterType type, uint index)
		{
			return ConstantDeclarations.FirstOrDefault(c =>
				c.ParameterType == type &&
				c.ContainsIndex(index));
		}

		private string GetSourceConstantName(InstructionToken instruction, int srcIndex)
		{
			var registerType = instruction.GetParamRegisterType(srcIndex);
			var registerNumber = instruction.GetParamRegisterNumber(srcIndex);

			string type;
			string[] constant;
			switch(registerType)
			{
				case RegisterType.ConstBool:
					//throw new NotImplementedException();
					return null;
				case RegisterType.ConstInt:
					type = "int";
					var constantInt = _constantIntDefinitions.FirstOrDefault(x => x.RegisterIndex == registerNumber);
					if(constantInt == null)
					{
						return null;
					}
					var intValues = instruction.GetSourceSwizzleComponents(srcIndex)
						.Select(s => constantInt[s])
						.ToArray();

					switch(instruction.GetSourceModifier(srcIndex))
					{
						case SourceModifier.None:
							break;
						case SourceModifier.Negate:
							throw new NotImplementedException();
						/*
						for (int i = 0; i < 4; i++)
						{
							intValues[i] = -intValues[i];
						}
						break;*/
						default:
							throw new NotImplementedException();
					}
					constant = intValues.Select(i => i.ToString(_culture)).ToArray();
					break;
				case RegisterType.Const:
				case RegisterType.Const2:
				case RegisterType.Const3:
				case RegisterType.Const4:
					type = "float";
					var constantRegister = _constantDefinitions.FirstOrDefault(x => x.RegisterIndex == registerNumber);
					if(constantRegister == null)
					{
						return null;
					}

					var floatValues = instruction.GetSourceSwizzleComponents(srcIndex)
						.Select(s => constantRegister[s])
						.ToArray();

					switch(instruction.GetSourceModifier(srcIndex))
					{
						case SourceModifier.None:
							break;
						case SourceModifier.Negate:
							for(int i = 0; i < floatValues.Length; i++)
							{
								floatValues[i] = -floatValues[i];
							}
							break;
						case SourceModifier.Abs:
							for(int i = 0; i < floatValues.Length; i++)
							{
								floatValues[i] = Math.Abs(floatValues[i]);
							}
							break;
						default:
							throw new NotImplementedException();
					}
					constant = floatValues.Select(f => f.ToString(_culture)).ToArray();
					break;
				default:
					return null;
			}

			if(instruction.Opcode == Opcode.If || instruction.Opcode == Opcode.IfC)
			{
				// TODO
			}

			return string.Format("{0}{1}({2})", type, constant.Length, string.Join(", ", constant));
		}


		private static string GetModifier(SourceModifier modifier)
		{
			switch(modifier)
			{
				case SourceModifier.None:
					return "{0}";
				case SourceModifier.Negate:
					return "-{0}";
				case SourceModifier.Bias:
					return "{0}_bias";
				case SourceModifier.BiasAndNegate:
					return "-{0}_bias";
				case SourceModifier.Sign:
					return "{0}_bx2";
				case SourceModifier.SignAndNegate:
					return "-{0}_bx2";
				case SourceModifier.Complement:
					throw new NotImplementedException();
				case SourceModifier.X2:
					return "(2 * {0})";
				case SourceModifier.X2AndNegate:
					return "(-2 * {0})";
				case SourceModifier.DivideByZ:
					return "{0}_dz";
				case SourceModifier.DivideByW:
					return "{0}_dw";
				case SourceModifier.Abs:
					return "abs({0})";
				case SourceModifier.AbsAndNegate:
					return "-abs({0})";
				case SourceModifier.Not:
					throw new NotImplementedException();
				default:
					throw new NotImplementedException();
			}
		}
	}
}
