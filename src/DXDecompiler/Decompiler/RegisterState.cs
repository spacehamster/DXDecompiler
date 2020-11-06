using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using SlimShader.Chunks.Xsgn;
using SlimShader.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Decompiler
{
	public class RegisterState
	{
		private BytecodeContainer Container;
		Dictionary<string, List<Register>> Registers = new Dictionary<string, List<Register>>();
		//Contains types used by structured buffers (struct/scalar/matrix)

		Dictionary<string, ConstantBuffer> ResourceBindings = new Dictionary<string, ConstantBuffer>();
		public List<Register> InputRegisters = new List<Register>();
		public List<Register> OutputRegisters = new List<Register>();
		public RegisterState(BytecodeContainer container)
		{
			Container = container;
			InitContantBuffers();
			InitResources();
			InitDeclarations();
			if (Container.LibrarySignature != null)
			{
				InitLibraryParams();
			}
			else
			{
				InitInputAndOutput();
			}
			InitResourceBindings();
		}
		public void AddRegister(string key, Register register)
		{
			if (Registers.ContainsKey(key))
			{
				Registers[key].Add(register);
			} else
			{
				Registers[key] = new List<Register>() { register };
			}
		}
		void InitContantBuffers()
		{
			foreach (var cb in Container.ResourceDefinition
				.ConstantBuffers.Where(cb => cb.BufferType == ConstantBufferType.ConstantBuffer || 
				cb.BufferType == ConstantBufferType.TextureBuffer))			{
				var registerName = cb.BufferType == ConstantBufferType.ConstantBuffer ?
					"cb" : "t";
				var bindingType = cb.BufferType == ConstantBufferType.ConstantBuffer ?
					ShaderInputType.CBuffer : ShaderInputType.TBuffer;
				var binding = Container.ResourceDefinition.ResourceBindings.First(rb =>
					rb.Type == bindingType &&
					rb.Name == cb.Name);
				InitConstantBuffer(cb, registerName, binding);
			}
		}
		void InitConstantBuffer(ConstantBuffer cb, string registerName, ResourceBinding binding)
		{
			foreach (var variable in cb.Variables)
			{
				InitMember(variable.Member, registerName, binding.BindPoint);
			}
		}
		void InitMember(ShaderTypeMember member, string registerName, uint bindPoint, uint parentOffset = 0, string prefix = "")
		{
			uint thisOffset = parentOffset + member.Offset;
			uint thisSize = member.GetCBVarSize();
			var elementCount = member.Type.ElementCount == 0 ? 1 : member.Type.ElementCount;
			for (uint i = 0; i < elementCount; i++)
			{
				string arrayIndex = member.Type.ElementCount > 1 ? $"[{i}]" : "";
				if (member.Type.VariableClass == ShaderVariableClass.MatrixColumns)
				{
					for (int j = 0; j < member.Type.Columns; j++)
					{
						var key = $"{registerName}{bindPoint}[{thisOffset / 16 + i * member.Type.Columns + j}]";
						AddRegister(key, new Register($"{prefix}{member.Name}{arrayIndex}[{j}]"));
					}
				} else if(member.Type.VariableClass == ShaderVariableClass.MatrixRows)
				{
					for (int j = 0; j < member.Type.Rows; j++)
					{
						var key = $"{registerName}{bindPoint}[{thisOffset / 16 + i * member.Type.Rows + j}]";
						AddRegister(key, new Register($"{prefix}{member.Name}{arrayIndex}[{j}]"));
					}
				}
				else if (member.Type.VariableClass == ShaderVariableClass.Struct)
				{
					uint paddedSize = ((thisSize + 15) / 16) * 16;
					uint structOffset = thisOffset + paddedSize * i;
					foreach (var child in member.Type.Members)
					{
						InitMember(child, registerName, bindPoint, structOffset, $"{prefix}{member.Name}{arrayIndex}.");
					}
				}
				else
				{
					var key = $"{registerName}{bindPoint}[{thisOffset / 16 + i}]";
					AddRegister(key, new Register($"{prefix}{member.Name}{arrayIndex}"));
				}
			}
		}
		void InitResources()
		{
			foreach (var rb in Container.ResourceDefinition.ResourceBindings.Where(rb => rb.Type != ShaderInputType.CBuffer))
			{
				var key = $"{rb.GetBindPointDescription()}";
				AddRegister(key, new Register($"{rb.Name}"));
			}
		}
		void InitLibraryParams()
		{
			foreach (var param in Container.LibrarySignature
				.Parameters
				.Where(p => p.FirstInRegister != uint.MaxValue))
			{
				string key;
				string name;
				key = $"v{param.FirstInRegister}";
				name = $"{param.Name}";
				var register = new Register(name);
				AddRegister(key, register);
				InputRegisters.Add(register);
			}
			foreach (var param in Container.LibrarySignature
				.Parameters
				.Where(p => p.FirstOutRegister != uint.MaxValue))
			{
				string key;
				string name;
				key = $"o{param.FirstOutRegister}";
				name = $"{param.Name}";
				var register = new Register(name);
				AddRegister(key, register);
				InputRegisters.Add(register);
			}
		}
		void InitInputAndOutput()
		{
			foreach(var param in Container.InputSignature.Parameters)
			{
				string key;
				string name;
				if (param.SystemValueType.RequiresMask())
				{
					key = $"v{param.Register}";
					name = $"{param.GetName()}";
				} else
				{
					key = $"{param.SystemValueType}";
					name = $"{param.GetName()}";
				}
				var register = new Register(name);
				AddRegister(key, register);
				InputRegisters.Add(register);
			}
			foreach (var param in Container.OutputSignature.Parameters)
			{
				string key;
				string name;
				if (param.SystemValueType.RequiresMask())
				{
					key = $"o{param.Register}";
					name = $"{param.GetName()}";
				}
				else
				{
					key = $"{param.SystemValueType}";
					name = $"{param.GetName()}";
				}
				var register = new Register(name);
				AddRegister(key, register);
				OutputRegisters.Add(register);
			}
		}
		void InitDeclarations()
		{
			foreach(var token in Container.Shader.DeclarationTokens)
			{
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.DclTemps:
						{
							var dcl = token as TempRegisterDeclarationToken;
							for(int i = 0; i < dcl.TempCount; i++)
							{
								AddRegister($"r{i}", new Register($"r{i}"));
							}
							break;
						}
					case OpcodeType.DclIndexableTemp:
						{
							var dcl = token as IndexableTempRegisterDeclarationToken;
							for (int i = 0; i < dcl.RegisterCount; i++)
							{
								AddRegister($"x{dcl.RegisterIndex}[{i}]", new Register($"x{dcl.RegisterIndex}[{i}]"));
							}
							break;
						}
					case OpcodeType.DclThreadGroupSharedMemoryRaw:
						{
							AddRegister($"g{token.Operand.Indices[0].Value}", new Register($"g{token.Operand.Indices[0].Value}"));
							break;
						}
					case OpcodeType.DclThreadGroupSharedMemoryStructured:
						{
							AddRegister($"g{token.Operand.Indices[0].Value}", new Register($"g{token.Operand.Indices[0].Value}"));
							break;
						}
					case OpcodeType.DclOutput:
						{
							InitOutputDeclaration(token);
							break;
						}
					case OpcodeType.DclInput:
						{
							InitInputDeclaration(token);
							break;
						}
				}
			}
			foreach(var token in Container.Shader.Tokens.OfType<CustomDataToken>())
			{
				switch (token.Header.OpcodeType)
				{
					case OpcodeType.DclConstantBuffer:
						{
							var dcl = token as ImmediateConstantBufferDeclarationToken;
							for (int i = 0; i < dcl.Data.Length; i++)
							{
								AddRegister($"icb[{i}]", new Register($"icb[{i}]"));
							}
							break;
						}
				}
			}
		}
		void InitOutputDeclaration(DeclarationToken token)
		{
			var key = token.Operand.OperandType.GetDescription();
			switch (token.Operand.OperandType)
			{
				case OperandType.OutputDepth:
					{
						var register = new Register("depth");
						AddRegister(key, register);
						OutputRegisters.Add(register);
						break;
					}
				case OperandType.OutputCoverageMask:
					{
						var register = new Register("coverageMask");
						AddRegister(key, register);
						OutputRegisters.Add(register);
						break;
					}
				case OperandType.OutputDepthGreaterEqual:
					{
						var register = new Register("depthGE");
						AddRegister(key, register);
						OutputRegisters.Add(register);
						break;
					}
				case OperandType.OutputDepthLessEqual:
					{
						var register = new Register("depthLE");
						AddRegister(key, register);
						OutputRegisters.Add(register);
						break;
					}
				case OperandType.StencilRef:
					{
						var register = new Register("oStencilRef");
						AddRegister(key, register);
						OutputRegisters.Add(register);
						break;
					}
				case OperandType.Output:
					break;
				default:

					throw new NotImplementedException($"{token}");
			}
		}
		void InitInputDeclaration(DeclarationToken token)
		{
			var key = token.Operand.OperandType.GetDescription();
			switch (token.Operand.OperandType)
			{
				case OperandType.InputCoverageMask:
					{
						var register = new Register("coverage");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.OutputDepth:
					{
						var register = new Register("depth");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.OutputCoverageMask:
					{
						var register = new Register("coverageMask");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputThreadGroupID:
					{
						var register = new Register("threadGroupID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputThreadID:
					{
						var register = new Register("threadID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputThreadIDInGroup:
					{
						var register = new Register("threadIDInGroup");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputThreadIDInGroupFlattened:
					{
						var register = new Register("threadIDInGroupFlattened");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputPrimitiveID:
					{
						var register = new Register("inputPrimitiveID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputForkInstanceID:
					{
						var register = new Register("forkInstanceID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputGSInstanceID:
					{
						var register = new Register("gsInstanceID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.InputDomainPoint:
					{
						var register = new Register("domain");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.OutputControlPointID:
					{
						var register = new Register("outputControlPointID");
						AddRegister(key, register);
						InputRegisters.Add(register);
						break;
					}
				case OperandType.Input:
					{
						// TODO
						break;
					}
				case OperandType.InputControlPoint:
					{
						// TODO
						break;
					}
				case OperandType.OutputControlPoint:
					{
						// TODO
						break;
					}
				case OperandType.InputPatchConstant:
					{
						// TODO
						break;
					}
				case OperandType.InnerCoverage:
					{
						// TODO
						break;
					}
				case OperandType.StencilRef:
					{
						// TODO
						break;
					}
				default:
					throw new NotImplementedException($"{token}");
			}
		}
		void InitResourceBindings()
		{
			foreach(var cb in Container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.ResourceBindInformation))
			{
				ResourceBindings[cb.Name] = cb;
			}
		}
		public Register GetRegister(Operand operand)
		{
			switch(operand.OperandType){
				case OperandType.ConstantBuffer:
					{
						int bindPoint = (int)operand.Indices[0].Value;
						int fieldIndex = (int)operand.Indices[1].Value;
						var regs = Registers[$"cb{bindPoint}[{fieldIndex}]"];
						return regs.First();
					}
				case OperandType.Interface:
					{
						int bindPoint = (int)operand.Indices[0].Value;
						int fieldIndex = (int)operand.Indices[1].Value;

						Registers.TryGetValue($"fp{bindPoint}[{fieldIndex}]", out var regs);
						return regs?.First();
					}
				case OperandType.InputCoverageMask:
					{
						var name = operand.OperandType.GetDescription();
						Registers.TryGetValue(name, out var regs);
						return regs?.First();
					}
				default:
					{
						var name = operand.OperandType.GetDescription();
						var index = operand.Indices[0].Value;
						Registers.TryGetValue($"{name}{index}", out var regs);
						return regs?.First();
					}
			}
		}
		public Register GetRegister(InstructionToken token)
		{
			switch (token.Header.OpcodeType)
			{
				case OpcodeType.InterfaceCall:
					{
						var operand = token.Operands[0];
						int functionBodyIndex = (int)token.FunctionIndex;
						int interfaceIndex = (int)operand.Indices[0].Value;
						int elementIndex = (int)operand.Indices[1].Value;
						Registers.TryGetValue($"fp{interfaceIndex}[{functionBodyIndex}]", out var regs);
						return regs?.First();
					}
				default:
					throw new Exception($"Couldn't find register {token}");
			}
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine("// RegisterState");
			foreach(var kv in Registers)
			{
				var names = string.Join(", ", kv.Value);
				sb.AppendLine($"// {kv.Key} = {names}");
			}
			foreach (var kv in this.ResourceBindings)
			{
				sb.AppendLine($"// RB! {kv.Key} = {kv.Value.Variables[0].ShaderType}");
			}
			return sb.ToString();
		}
	}
}
