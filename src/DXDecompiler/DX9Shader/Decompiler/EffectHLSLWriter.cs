using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.DX9Shader.Decompiler;
using DXDecompiler.DX9Shader.FX9;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader
{
	public class EffectHLSLWriter : DecompileWriter
	{
		private readonly Dictionary<object, string> _shaderNames = new();
		private readonly EffectContainer _effectChunk;

		public Dictionary<string, DecompiledConstantDeclaration> CommonConstantDeclarations { get; } = new();


		public EffectHLSLWriter(EffectContainer effectChunk)
		{
			_effectChunk = effectChunk;
		}
		public static string Decompile(EffectContainer effectChunk)
		{
			var asmWriter = new EffectHLSLWriter(effectChunk);
			return asmWriter.Decompile();
		}
		void BuildNameLookup()
		{
			string MakeShaderName(ShaderModel shader) => shader.Type switch
			{
				ShaderType.Pixel => "PixelShader",
				ShaderType.Vertex => "VertexShader",
				ShaderType.Expression => "Expression",
				_ => "Shader"
			} + $"{_shaderNames.Count + 1}";

			foreach(var blob in _effectChunk.VariableBlobs)
			{
				if(blob.IsShader)
				{
					_shaderNames[blob] = MakeShaderName(blob.Shader);
				}
			}
			foreach(var blob in _effectChunk.StateBlobs)
			{
				if(blob.BlobType == StateBlobType.Shader ||
					blob.BlobType == StateBlobType.IndexShader)
				{

					_shaderNames[blob] = MakeShaderName(blob.Shader);
				}
			}
		}
		void FindCommonConstantDeclarations()
		{
			var variableShaders = _effectChunk.VariableBlobs
				.Where(x => x.IsShader)
				.Select(x => x.Shader);
			var blobShaders = _effectChunk.StateBlobs
				.Where(x => x.BlobType is StateBlobType.Shader or StateBlobType.IndexShader)
				.Select(x => x.Shader);
			foreach(var shader in variableShaders.Concat(blobShaders))
			{
				var declarations = shader.ConstantTable?.ConstantDeclarations
					?? Enumerable.Empty<ConstantDeclaration>();
				var decompiled = declarations.Select(c => ConstantTypeWriter.Decompile(c, shader));
				foreach(var declaration in decompiled)
				{
					// assuming every shader have com
					if(!CommonConstantDeclarations.TryGetValue(declaration.Name, out var existing))
					{
						CommonConstantDeclarations[declaration.Name] = declaration;
					}
					// this means we don't have common constant declaration
					else if(existing.Code != declaration.Code)
					{
						CommonConstantDeclarations.Remove(declaration.Name);
					}
					// check if two declarations only differs on register...
					else
					{
						if(!declaration.RegisterAssignments.Any())
						{
							continue;
						}
						// sanity check
						if(declaration.RegisterAssignments.Count > 1)
						{
							throw new InvalidOperationException();
						}

						var registerAssignment = declaration.RegisterAssignments.First();
						if(existing.RegisterAssignments.TryGetValue(registerAssignment.Key, out var existingRegister))
						{
							// if register number of the same constant is different
							// betwwen two shaders of same shader profile
							if(registerAssignment.Value != existingRegister)
							{
								// then probably there weren't a register assignment at all.
								// I don't think there is a way to specify two different registers 
								// for the same variable with the same shader profile in DX9's effect.
								existing.RegisterAssignments.Remove(registerAssignment.Key);
							}
						}
						else
						{
							existing.RegisterAssignments[registerAssignment.Key] = registerAssignment.Value;
						}
					}
				}
			}
		}
		protected override void Write()
		{
			BuildNameLookup();
			FindCommonConstantDeclarations();
			foreach(var variable in _effectChunk.Variables)
			{
				WriteVariable(variable);
			}
			foreach(var blob in _effectChunk.StateBlobs)
			{
				if(blob.BlobType == StateBlobType.Shader ||
					blob.BlobType == StateBlobType.IndexShader)
				{
					WriteShader(blob);
				}
			}
			foreach(var technique in _effectChunk.Techniques)
			{
				WriteTechnique(technique);
			}
		}
		void WriteShader(StateBlob blob) => WriteShader(_shaderNames[blob], blob.Shader);
		void WriteShader(string shaderName, ShaderModel shader)
		{
			WriteLine($"// {shaderName} {shader.Type}_{shader.MajorVersion}_{shader.MinorVersion} Has PRES {shader.Preshader != null}");
			var funcName = shaderName;
			string text;
			if(shader.Type == ShaderType.Expression)
			{
				text = ExpressionHLSLWriter.Decompile(shader, funcName);
			}
			else
			{
				text = HlslWriter.Decompile(shader, funcName, this);
			}
			WriteLine(text);
		}
		public string StateBlobToString(Assignment key)
		{
			if(!_effectChunk.StateBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = _effectChunk.StateBlobLookup[key];
			if(data == null)
			{
				return "Blob is NULL";
			}
			if(data.BlobType == StateBlobType.Shader)
			{
				if(data.Shader.Type == ShaderType.Expression)
				{
					var funcName = _shaderNames[data];
					return $"{funcName}()";
				}
				else
				{
					var funcName = _shaderNames[data];
					return $"compile {data.Shader.Type.GetDescription()}_{data.Shader.MajorVersion}_{data.Shader.MinorVersion} {funcName}()";
				}
			}
			if(data.BlobType == StateBlobType.Variable)
			{
				if(string.IsNullOrEmpty(data.VariableName))
				{
					return "NULL";
				}
				else
				{
					return $"<{data.VariableName}>";
				}
			}
			if(data.BlobType == StateBlobType.IndexShader)
			{
				var funcName = _shaderNames[data];
				return $"{data.VariableName}[{funcName}()]";
			}
			throw new ArgumentException();
		}
		public string VariableBlobToString(FX9.Parameter key, int index = 0)
		{
			if(!_effectChunk.VariableBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = _effectChunk.VariableBlobLookup[key][index];
			if(data == null)
			{
				return "Blob is NULL";
			}
			if(data.IsShader)
			{
				var funcName = _shaderNames[data];
				return $"compile {data.Shader.Type.GetDescription()}_{data.Shader.MajorVersion}_{data.Shader.MinorVersion} {funcName}()";
			}
			else if(key.ParameterType == ParameterType.String)
			{
				return $"\"{data.Value}\"";
			}
			else if(string.IsNullOrEmpty(data.Value))
			{
				return string.Empty;
			}
			else
			{
				return $"<{data.Value}>";
			}
		}
		public void WriteVariable(Variable variable)
		{
			var param = variable.Parameter;
			WriteIndent();
			var isShaderArray = param.ParameterClass == ParameterClass.Object &&
				(param.ParameterType == ParameterType.PixelShader || param.ParameterType == ParameterType.VertexShader);
			Dictionary<uint, string> shaderArrayElements = null;
			if(isShaderArray)
			{
				shaderArrayElements = new Dictionary<uint, string>();
				var blobs = _effectChunk.VariableBlobLookup[param];
				var index = 0;
				foreach(var blob in blobs)
				{
					var name = $"{variable.Parameter.Name}_Shader_{index}";
					shaderArrayElements.Add(blob.Index, name);
					WriteShader(name, blob.Shader);
					++index;
				}
			}
			if(CommonConstantDeclarations.TryGetValue(variable.Parameter.Name, out var decompiled))
			{
				var semantic = string.IsNullOrEmpty(param.Semantic)
					? string.Empty
					: $" : {param.Semantic}";
				WriteLine($"{decompiled.Code}{semantic}{decompiled.RegisterAssignmentString}");
			}
			// shader's constant declaration might differ from the effect variable's parameter declaration
			// in that case, we should prefer shader's one.
			// So we write parameter's declaration only if the variable isn't inside the common constant declaration.
			else
			{
				Write(param.GetDeclaration());
			}
			if(variable.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(variable.Annotations);
			}
			if(param.ParameterType.IsSampler())
			{
				WriteLine(" =");
				if(variable.SamplerStates.Count > 1)
				{
					WriteLine("{");
					Indent++;
				}
				for(int i = 0; i < variable.SamplerStates.Count; i++)
				{
					var state = variable.SamplerStates[i];
					WriteIndent();
					WriteLine("sampler_state");
					WriteIndent();
					WriteLine("{");
					Indent++;
					foreach(var assignment in state.Assignments)
					{
						WriteIndent();
						if(assignment.Type == StateType.Texture)
						{
							var data = StateBlobToString(assignment);
							WriteLine("{0} = {1}; // {2}", assignment.Type, data, assignment.Value[0].UInt);
						}
						else
						{
							WriteLine("{0} = {1};", assignment.Type, assignment.Value[0].UInt);
						}
					}
					Indent--;
					WriteIndent();
					Write("}");
					if(variable.SamplerStates.Count == 1)
					{
						WriteLine(";");
					}
					else if(i < variable.SamplerStates.Count - 1)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
				if(variable.SamplerStates.Count > 1)
				{
					Indent--;
					WriteIndent();
					WriteLine("};");
				}
			}
			else
			{
				if(param.ParameterType.HasVariableBlob())
				{
					if(isShaderArray)
					{
						WriteLine(" = {");
						Indent++;
						foreach(var idx in variable.DefaultValue)
						{
							WriteIndent();
							WriteLine("{0}, // {1}", shaderArrayElements[idx.UInt], idx.UInt);
						}
						Indent--;
						WriteLine("};");
					}
					else
					{
						var data = VariableBlobToString(variable.Parameter);
						if(string.IsNullOrEmpty(data))
						{
							WriteLine("; // {0}", variable.DefaultValue[0].UInt);
						}
						else
						{
							WriteLine(" = {0}; // {1}", data, variable.DefaultValue[0].UInt);
						}
					}
				}
				else if(variable.DefaultValue.All(d => d.UInt == 0))
				{
					WriteLine(";");
				}
				else
				{
					var defaultValue = string.Join(", ", variable.DefaultValue);
					WriteLine(" = {{ {0} }};", defaultValue);
				}
			}
		}
		public void WriteAnnotations(List<Annotation> annotations)
		{
			Write("<");
			for(int i = 0; i < annotations.Count; i++)
			{
				var annotation = annotations[i];
				var value = string.Join(", ", annotation.Value);
				if(annotation.Parameter.ParameterType.HasVariableBlob())
				{
					Write("{0} {1} = {2};",
						annotation.Parameter.GetTypeName(),
						annotation.Parameter.Name,
						VariableBlobToString(annotation.Parameter));
				}
				else
				{
					Write("{0} {1} = {2};", annotation.Parameter.GetTypeName(), annotation.Parameter.Name, value);
				}
				if(i < annotations.Count - 1) Write(" ");
			}
			Write(">");
		}
		public void WriteTechnique(Technique technique)
		{
			Write("technique {0}", technique.Name);
			if(technique.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(technique.Annotations);
			}
			WriteLine();
			WriteLine("{");
			Indent++;
			foreach(var pass in technique.Passes)
			{
				WritePass(pass);
			}
			Indent--;
			WriteLine("}");
			WriteLine("");
		}
		public void WritePass(Pass pass)
		{
			WriteIndent();
			Write("pass {0}", pass.Name);
			if(pass.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(pass.Annotations);
			}
			WriteLine();
			WriteIndent();
			WriteLine("{");
			Indent++;
			var shaderAssignments = pass.Assignments;
			foreach(var assignment in shaderAssignments)
			{
				WriteAssignment(assignment);
			}
			Indent--;
			WriteIndent();
			WriteLine("}");
		}
		public void WriteAssignment(Assignment assignment)
		{
			WriteIndent();
			string index = "";
			if(assignment.Type.RequiresIndex())
			{
				index = string.Format("[{0}]", assignment.ArrayIndex);
			}
			string value;
			if(assignment.Value.Count > 1)
			{
				value = string.Format("{{ {0} }}", string.Join(", ", assignment.Value));
			}
			else if(_effectChunk.StateBlobLookup.ContainsKey(assignment))
			{
				value = StateBlobToString(assignment);
			}
			else
			{
				value = assignment.Value[0].ToString();
			}
			Write("{0}{1} = {2};", assignment.Type.ToString(), index, value);
			if(_effectChunk.StateBlobLookup.ContainsKey(assignment))
			{
				Write(" // {0}", assignment.Value[0].UInt);
			}
			WriteLine();
		}
	}
}
