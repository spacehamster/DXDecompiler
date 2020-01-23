using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Chunks.Shex;
using SlimShader.Chunks.Shex.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.Chunks;

namespace SlimShader.Decompiler
{
	public partial class DXDecompiler
	{
		StringBuilder Output = new StringBuilder();
		BytecodeContainer Container;
		RegisterState RegisterState;
		Interfaces Interfaces;
		Dictionary<string, string> Functions = new Dictionary<string, string>();
		public bool EmitRegisterDeclarations = true;
		public bool EmitPackingOffset = true;
		public Dictionary<string, ConstantBuffer> m_ConstantBufferLookup;
		public Dictionary<string, ResourceBinding> m_ResourceBindingLookup;
		int indent = 0;
		public static string Decompile(byte[] data)
		{
			var container = new BytecodeContainer(data);
			var decompiler = new DXDecompiler(container);
			return decompiler.Decompile();
		}
		DXDecompiler(BytecodeContainer container)
		{
			RegisterState = new RegisterState(container);
			Container = container;
			m_ConstantBufferLookup = new Dictionary<string, ConstantBuffer>();
			m_ResourceBindingLookup = new Dictionary<string, ResourceBinding>();
			if (Container.Interfaces != null)
			{
				Interfaces = new Interfaces(Container);
				foreach(var kv in Interfaces.GetRegisterMapping())
				{
					RegisterState.AddRegister(kv.Key, new Register(kv.Value));
				}
			}

			foreach (var rb in Container.ResourceDefinition.ResourceBindings)
			{
				m_ResourceBindingLookup.Add(rb.GetBindPointDescription(), rb);
			}

			//Note that resources can have duplicate name and resources of differenttypes can share names
			var resourceBindingLookup = new Dictionary<string, ResourceBinding>();
			var resourceBindingsByType = Container.ResourceDefinition.ResourceBindings
				.GroupBy(rb => rb.Type.ToCBType());
			foreach(var typeGroup in resourceBindingsByType)
			{
				var type = typeGroup.Key;
				var bindingsByName = typeGroup
					.GroupBy(rb => rb.Name);
				foreach (var nameGroup in bindingsByName) {
					var bindings = nameGroup.ToArray();
					for (int i = 0; i < bindings.Length; i++)
					{
						var rb = bindings[i];
						resourceBindingLookup.Add($"{type}${rb.Name}${i}", rb);
					}
				}
			}
			var bufferBindingsByType = Container.ResourceDefinition.ConstantBuffers
				.Where(cb => cb.BufferType != ConstantBufferType.InterfacePointers)
				.GroupBy(cb => cb.BufferType);
			foreach (var typeGroup in bufferBindingsByType)
			{
				var type = typeGroup.Key;
				var bindingsByName = typeGroup
					.GroupBy(cb => cb.Name);
				foreach (var nameGroup in bindingsByName)
				{
					var bindings = nameGroup.ToArray();
					for (int i = 0; i < bindings.Length; i++)
					{
						var cb = bindings[i];
						var key = $"{type}${cb.Name}${i}";
						var rb = resourceBindingLookup[key];
						m_ConstantBufferLookup.Add(rb.GetBindPointDescription(), cb);
					}
				}
			}
		}
		public void AddFunction(string name, string def)
		{
			Functions[name] = def;
		}
		public string GetMainFuncName()
		{
			switch (Container.Shader.Version.ProgramType)
			{
				case ProgramType.VertexShader:
					return "MainVS";
				case ProgramType.PixelShader:
					return "MainPS";
				case ProgramType.HullShader:
					return "MainHS";
				case ProgramType.GeometryShader:
					return "MainGS";
				case ProgramType.DomainShader:
					return "MainDS";
				case ProgramType.ComputeShader:
					return "MainCS";
				default:
					return "Main";
			}
		}
		void WriteMainFuncDec()
		{
			if (Container.Shader.Version.ProgramType == ProgramType.ComputeShader)
			{
				Output.AppendFormat("void {0}(", GetMainFuncName());
				WriteComputeParams();
				Output.AppendLine(")");
				Output.AppendLine("{");
				indent++;
			}
			else if (Container.Shader.Version.ProgramType == ProgramType.GeometryShader)
			{
				Output.AppendFormat("void {0}(", GetMainFuncName());
				WriteGeometryParams();
				Output.AppendLine(")");
				Output.AppendLine("{");
				indent++;
				AddIndent();
				Output.AppendLine("ShaderOutput output;");
			}
			else if (Container.Shader.Version.ProgramType == ProgramType.DomainShader)
			{
				Output.AppendFormat("ShaderOutput {0}(", GetMainFuncName());
				WriteDomainParams();
				Output.AppendLine(")");
				Output.AppendLine("{");
				indent++;
				AddIndent();
				Output.AppendLine("ShaderOutput output;");
			}
			else
			{
				Output.AppendFormat("ShaderOutput {0}(ShaderInput input", GetMainFuncName());
				WriteParams();
				Output.AppendLine(")");
				Output.AppendLine("{");
				indent++;
				AddIndent();
				Output.AppendLine("ShaderOutput output;");
			}
		}
		//refer bool ToGLSL::Translate()
		private string Decompile()
		{
#if DEBUG
			Output.AppendLine(RegisterState.Dump());
#endif
			if (Container.Shader.Version.ProgramType == ProgramType.HullShader)
			{
				DecompileHullShader();
				return Output.ToString();
			}
			WriteResoureDefinitions();
			WriteSignatures();
			if (Interfaces != null)
			{
				Output.AppendLine(Interfaces.Dump());
			}
			LogDeclaration(Container.Shader.DeclarationTokens);
			WriteDeclarationAnnotations(Container.Shader.DeclarationTokens);
			WriteMainFuncDec();
			WriteTempRegisters();
			WriteDeclarationVariables(Container.Shader.DeclarationTokens);
			TranslateInstructions();
			indent--;
			Output.AppendLine("}");
			string functionDefs = string.Join("\n", Functions.Values);
			return string.Format("{0}\n{1}", functionDefs, Output.ToString());
		}
		void DecompileHullShader()
		{
			var hsDecls = new List<OpcodeToken>();
			var controlPointPhase = new List<OpcodeToken>();
			var forkPhase = new List<List<OpcodeToken>>();
			var joinPhase = new List<List<OpcodeToken>>();
			List<OpcodeToken> currentPhase = null;
			foreach(var token in Container.Shader.Tokens)
			{
				if(token.Header.OpcodeType == OpcodeType.HsDecls)
				{
					currentPhase = hsDecls;
				} else if(token.Header.OpcodeType == OpcodeType.HsControlPointPhase)
				{
					currentPhase = controlPointPhase;
				} else if(token.Header.OpcodeType == OpcodeType.HsForkPhase)
				{
					currentPhase = new List<OpcodeToken>();
					forkPhase.Add(currentPhase);
				}
				else if (token.Header.OpcodeType == OpcodeType.HsJoinPhase)
				{
					currentPhase = new List<OpcodeToken>();
					joinPhase.Add(currentPhase);
				}
				currentPhase.Add(token);
			}
			WriteResoureDefinitions();
			WriteSignatures();
			WriteHullMainFunc(hsDecls);
			WriteHullControlPhase(controlPointPhase);
			foreach (var phase in forkPhase)
			{
				WriteHullForkPhase(phase);
				indent++;
				var declTokens = phase.OfType<DeclarationToken>();
				LogDeclaration(declTokens);
				WriteDeclarationVariables(declTokens);
				foreach (var token in phase.OfType<InstructionToken>())
				{
					TranslateInstruction(token);
				}
				indent--;
				Output.AppendLine("}");
			}
			foreach (var phase in joinPhase)
			{
				WriteHullJoinPhase(phase);
				indent++;
				var declTokens = phase.OfType<DeclarationToken>();
				LogDeclaration(declTokens);
				WriteDeclarationVariables(declTokens);
				foreach (var token in phase.OfType<InstructionToken>())
				{
					TranslateInstruction(token);
				}
				indent--;
				Output.AppendLine("}");
			}
		}
		void WriteHullMainFunc(List<OpcodeToken> tokens)
		{
			Output.AppendLine("ShaderOutput MainHS(){");
			indent++;
			foreach (var token in tokens)
			{
				DebugLog(token);
			}
			indent--;
			Output.AppendLine("}");
		}
		void WriteHullControlPhase(List<OpcodeToken> tokens)
		{
			Output.AppendLine("ShaderOutput ControlPhase(){");
			indent++;
			foreach (var token in tokens)
			{
				DebugLog(token);
			}
			indent--;
			Output.AppendLine("}");
		}
		void WriteHullForkPhase(List<OpcodeToken> tokens)
		{
			Output.AppendLine("ShaderOutput ForkPhase(){");
		}
		void WriteHullJoinPhase(List<OpcodeToken> tokens)
		{
			Output.AppendLine("ShaderOutput JoinPhase(){");
		}
		void WriteTempRegisters()
		{
			//Note: Hull shaders can declare multiple temp registers
			TempRegisterDeclarationToken temps = Container.Shader.DeclarationTokens
				.OfType<TempRegisterDeclarationToken>()
				.FirstOrDefault();
			if (temps == null) return;
			AddIndent();
			Output.Append("float4 ");
			for (int i = 0; i < temps.TempCount; i++){
				Output.AppendFormat("r{0}", i);
				if (i < temps.TempCount - 1) Output.Append(", ");
			}
			Output.AppendLine(";");
			//Note: CustomDataToken
			var immediateCBs = Container.Shader.Tokens
				.OfType<ImmediateConstantBufferDeclarationToken>()
				.ToArray();
			foreach(var cb in immediateCBs)
			{
				AddIndent();
				Output.AppendLine("const float4 icb[] = { ");
				Output.Append(new string(' ', 30));
				for (int i = 0; i < cb.Data.Length; i += 4)
				{
					if (i > 0)
						Output.Append("," + Environment.NewLine + new string(' ', 30));
					Output.AppendFormat("{{ {0}, {1}, {2}, {3}}}",
						cb.Data[i], cb.Data[i + 1], cb.Data[i + 2], cb.Data[i + 3]);
				}
				Output.AppendLine("};");
			}
		}
		void WriteComputeParams()
		{
			var inputs = Container.Shader.DeclarationTokens
				.OfType<InputRegisterDeclarationToken>()
				.ToArray();
			if (inputs.Length == 0) return;
			Output.AppendLine("");
			Output.Append("\t");
			for(int i = 0; i < inputs.Length; i++)
			{
				var token = inputs[i];
				WriteInputDeclaration(token);
				if(i < inputs.Length - 1)
				{
					Output.AppendLine(",");
					Output.Append("\t");
				}
			}
			Output.Append(" ");
		}
		void WriteDomainParams()
		{
			var inputs = Container.Shader.DeclarationTokens
				.Where(t => t is InputRegisterDeclarationToken)
				.Cast<InputRegisterDeclarationToken>()
				.ToArray();
			Output.AppendLine("");
			Output.Append("\t");
			Output.AppendFormat("PatchConstant patchConstant");
			for (int i = 0; i < inputs.Length; i++)
			{
				Output.AppendLine(",");
				Output.Append("\t");
				var token = inputs[i];
				if (token.Operand.OperandType == OperandType.InputControlPoint)
				{
					var controlPointCount = Container.Shader.DeclarationTokens
						.Single(c => c is ControlPointCountDeclarationToken) as ControlPointCountDeclarationToken;
					var operand = token.Operand;
					Output.AppendFormat("{0}<{1}, {2}> {3}",
						"OutputPatch",
						"ShaderInput",
						controlPointCount.ControlPointCount,
						operand.OperandType.GetDescription());
				}
				else
				{
					var type = "float4";
					if (token.Operand.OperandType == OperandType.InputDomainPoint)
						type = "float2";
					if (token.Operand.OperandType == OperandType.InputPrimitiveID)
						type = "uint";
					Output.AppendFormat("{0} {1} : {2}",
						type,
						token.Operand.OperandType.GetDescription(),
						GetSemanticName(token.Operand));
				}
			}
			Output.Append(" ");
		}
		void WriteGeometryParams()
		{
			//Input declares each field seperatly, we only care about the size of the input array
			var mainInput = Container.Shader.DeclarationTokens
				.First(t => 
						t is InputRegisterDeclarationToken &&
						t.Operand.OperandType == OperandType.Input);
			var inputPrimitive = Container.Shader.DeclarationTokens
				.OfType<GeometryShaderInputPrimitiveDeclarationToken>()
				.Single();
			Output.AppendLine("");
			Output.Append("\t");
			/*
			 * Streams are declared in groups like
				dcl_stream m0
				dcl_outputtopology pointlist 
				dcl_output o0.xyz
				dcl_stream m1
				dcl_outputtopology pointlist 
				dcl_output o0.xyz
				*/
			Output.AppendFormat("{0} {1} {2}[{3}]",
				inputPrimitive.Primitive.GetDescription(ChunkType.Shex),
				"ShaderInput",
				"input",
				mainInput.Operand.Indices[0].Value);
			var dclTokens = Container.Shader.DeclarationTokens.ToArray();
			for (int i = 0; i < dclTokens.Length; i++)
			{
				var token = dclTokens[i];
				if (token is InputRegisterDeclarationToken && token.Operand.OperandType != OperandType.Input)
				{
					Output.Append(",\n\t");
					var type = "float4";
					if (token.Operand.OperandType == OperandType.InputPrimitiveID)
						type = "uint";
					if (token.Operand.OperandType == OperandType.InputGSInstanceID)
						type = "uint";
					Output.AppendFormat("{0} {1} : {2}",
						type,
						token.Operand.OperandType.GetDescription(),
						GetSemanticName(token.Operand));
				}
				else if(token is StreamDeclarationToken)
				{
					//TODO: analyse bytecode and assign streams and output registers the correct output signature
					Output.Append(",\n\t");
					var outputTopology = (GeometryShaderOutputPrimitiveTopologyDeclarationToken)dclTokens[i + 1];
					var output = (OutputRegisterDeclarationToken)dclTokens[i + 2];
					Output.AppendFormat("{0} {1}<{2}> {3}",
						"inout",
						outputTopology.PrimitiveTopology.GetStreamName(),
						"Stream0Output",
						$"{token.Operand.OperandType.GetDescription()}{token.Operand.Indices[0].Value}");
					i += 2;
				}
			}
			Output.Append(" ");
		}
		void WriteParams()
		{
			var funcParams = Container.ResourceDefinition.ConstantBuffers.FirstOrDefault(c =>
				c.BufferType == ConstantBufferType.ConstantBuffer &&
				c.Name == "$Params");
			if (funcParams == null) return;
			indent++;
			foreach(var variable in funcParams.Variables)
			{
				Output.AppendLine(",");
				AddIndent();
				Output.Append($"uniform {GetShaderTypeName(variable.ShaderType)} {variable.Name}");
			}
			Output.Append(" ");
			indent--;
		}
		void TranslateInstructions()
		{
			var tokens = Container.Shader.Tokens;
			if(Interfaces != null)
			{
				tokens = Container.Shader.Tokens
					.TakeWhile(t => t.Header.OpcodeType != OpcodeType.Label)
					.ToList();
			}
			foreach (var token in tokens)
			{
				if (token is InstructionToken inst)
				{
					TranslateInstruction(inst);
				}
				else if (token is ShaderMessageDeclarationToken message)
				{
					WriteShaderMessage(message);
				} else if (token is CustomDataToken data)
				{
					AddIndent();
					Output.AppendFormat("// {0}\n", data.ToString());
				}
			}
		}
		void AddIndent()
		{
			var indentText = new string('\t', indent);
			Output.Append(indentText);
		}
		void DebugLog(OpcodeToken token)
		{
			var lines  = token.ToString().Replace("\r", "").Split('\n');
			foreach (var line in lines)
			{
				AddIndent();
				Output.AppendLine($"// {line}");
			}
		}
		// TODO: Use OperandType for lookup instead of ShaderInputType
		ConstantBuffer GetConstantBuffer(OperandType type, uint id)
		{
			var hlslBind = $"{type.GetDescription()}{id}";
			return m_ConstantBufferLookup[hlslBind];
		}
		ResourceBinding GetResourceBinding(OperandType type, uint id)
		{
			var hlslBind = $"{type.GetDescription()}{id}";
			return m_ResourceBindingLookup[hlslBind];
		}
	}
}
