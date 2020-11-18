using DXDecompiler.DX9Shader.Decompiler;
using DXDecompiler.DX9Shader.FX9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader
{
	public class EffectHLSLWriter : DecompileWriter
	{
		EffectContainer EffectChunk;
		Dictionary<object, string> ShaderNames = new Dictionary<object, string>();
		public EffectHLSLWriter(EffectContainer effectChunk)
		{
			EffectChunk = effectChunk;
		}
		public static string Decompile (EffectContainer effectChunk)
		{
			var asmWriter = new EffectHLSLWriter(effectChunk);
			return asmWriter.Decompile();
		}
		void BuildNameLookup()
		{
			int shaderCount = 0;
			foreach(var blob in EffectChunk.VariableBlobs)
			{
				if (blob.IsShader)
				{
					ShaderNames[blob] = $"Shader{shaderCount++}";
				}
			}
			foreach (var blob in EffectChunk.StateBlobs)
			{
				if (blob.BlobType == StateBlobType.Shader || 
					blob.BlobType == StateBlobType.IndexShader)
				{
					ShaderNames[blob] = $"Shader{shaderCount++}";
				}
			}

		}
		protected override void Write()
		{
			BuildNameLookup();
			foreach (var variable in EffectChunk.Variables)
			{
				WriteVariable(variable);
			}
			foreach(var blob in EffectChunk.StateBlobs)
			{
				if(blob.BlobType == StateBlobType.Shader ||
					blob.BlobType == StateBlobType.IndexShader)
				{
					WriteShader(blob);
				}
			}
			foreach (var technique in EffectChunk.Techniques)
			{
				WriteTechnique(technique);
			}
		}
		void WriteShader(StateBlob blob)
		{
			var shader = blob.Shader;
			WriteLine($"// {ShaderNames[blob]} {shader.Type}_{shader.MajorVersion}_{shader.MinorVersion} Has PRES {shader.Preshader != null}");
			var funcName = ShaderNames[blob];
			var text = "";
			if (blob.Shader.Type == ShaderType.Expression)
			{
				text = ExpressionHLSLWriter.Decompile(blob.Shader, funcName);
			}
			else
			{
				text = HlslWriter.Decompile(blob.Shader);
				text = text.Replace("main(", $"{funcName}(");
			}
			WriteLine(text);
		}
		public string StateBlobToString(Assignment key)
		{
			if (!EffectChunk.StateBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = EffectChunk.StateBlobLookup[key];
			if(data == null)
			{
				return "Blob is NULL";
			}
			if (data.BlobType == StateBlobType.Shader)
			{
				if (data.Shader.Type == ShaderType.Expression)
				{
					var funcName = ShaderNames[data];
					return $"{funcName}";
				}
				else
				{
					var funcName = ShaderNames[data];
					return $"compile {data.Shader.Type.GetDescription()}_{data.Shader.MajorVersion}_{data.Shader.MinorVersion} {funcName}()";
				}
			}
			if (data.BlobType == StateBlobType.Variable)
			{
				if (string.IsNullOrEmpty(data.VariableName))
				{
					return "NULL";
				} else
				{
					return $"<{data.VariableName}>";
				}
			}
			if (data.BlobType == StateBlobType.IndexShader)
			{
				var funcName = ShaderNames[data];
				return $"{data.VariableName}[{funcName}()]";
			}
			throw new ArgumentException();
		}
		public string VariableBlobToString(FX9.Parameter key, int index = 0)
		{
			if (!EffectChunk.VariableBlobLookup.ContainsKey(key))
			{
				return $"Key not found";
			}
			var data = EffectChunk.VariableBlobLookup[key][index];
			if (data == null)
			{
				return "Blob is NULL";
			}
			if (data.IsShader)
			{
				var funcName = ShaderNames[data];
				return $"compile {data.Shader.Type.GetDescription()}_{data.Shader.MajorVersion}_{data.Shader.MinorVersion} {funcName}()";
			}
			else if(key.ParameterType == ParameterType.String)
			{
				return $"\"{data.Value}\"";
			} else
			{
				return $"<{data.Value}>";
			}
		}
		public void WriteVariable(Variable variable)
		{
			var param = variable.Parameter;
			WriteIndent();
			Write(param.GetDecleration());
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
				for(int i = 0; i < variable.SamplerStates.Count; i++) {
					var state = variable.SamplerStates[i];
					WriteIndent();
					WriteLine("sampler_state");
					WriteIndent();
					WriteLine("{");
					Indent++;
					foreach (var assignment in state.Assignments)
					{
						WriteIndent();
						if (assignment.Type == StateType.Texture)
						{
							var data = StateBlobToString(assignment);
							WriteLine("{0} = <{1}>; // {2}", assignment.Type, data, assignment.Value[0].UInt);
						}
						else
						{
							WriteLine("{0} = {1};", assignment.Type, assignment.Value[0].UInt);
						}
					}
					Indent--;
					WriteIndent();
					Write("}");
					if (variable.SamplerStates.Count == 1)
					{
						WriteLine(";");
					} else if(i < variable.SamplerStates.Count - 1)
					{
						WriteLine(",");
					} else
					{
						WriteLine();
					}
				}
				if (variable.SamplerStates.Count > 1)
				{
					Indent--;
					WriteIndent();
					WriteLine("};");
				}
			} else
			{
				if (param.ParameterType.HasVariableBlob())
				{
					var data = VariableBlobToString(variable.Parameter);
					if (string.IsNullOrEmpty(data))
					{
						WriteLine("; // {0}", variable.DefaultValue[0].UInt);
					}
					else
					{
						WriteLine(" = {0}; // {1}", data, variable.DefaultValue[0].UInt);
					}
				}
				else if (variable.DefaultValue.All(d => d.UInt == 0))
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
				if (annotation.Parameter.ParameterType.HasVariableBlob())
				{
					Write("{0} {1} = {2};", 
						annotation.Parameter.GetTypeName(), 
						annotation.Parameter.Name, 
						VariableBlobToString(annotation.Parameter));
				} else
				{
					Write("{0} {1} = {2};", annotation.Parameter.GetTypeName(), annotation.Parameter.Name, value);
				}
				if (i < annotations.Count - 1) Write(" ");
			}
			Write(">");
		}
		public void WriteTechnique(Technique technique)
		{
			Write("technique {0}", technique.Name);
			if (technique.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(technique.Annotations);
			}
			WriteLine();
			WriteLine("{");
			Indent++;
			foreach (var pass in technique.Passes)
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
			if (pass.Annotations.Count > 0)
			{
				Write(" ");
				WriteAnnotations(pass.Annotations);
			}
			WriteLine();
			WriteIndent();
			WriteLine("{");
			Indent++;
			var shaderAssignments = pass.Assignments;
			foreach (var assignment in shaderAssignments)
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
			if (assignment.Type.RequiresIndex())
			{
				index = string.Format("[{0}]", assignment.ArrayIndex);
			}
			string value;
			if(assignment.Value.Count > 1)
			{
				value = string.Format("{{ {0} }}", string.Join(", ", assignment.Value));
			} else if(EffectChunk.StateBlobLookup.ContainsKey(assignment))
			{
				value = StateBlobToString(assignment);
			} else
			{
				value = assignment.Value[0].ToString();
			}
			Write("{0}{1} = {2};", assignment.Type.ToString(), index, value);
			if (EffectChunk.StateBlobLookup.ContainsKey(assignment))
			{
				Write(" // {0}", assignment.Value[0].UInt);
			}
			WriteLine();
		}
	}
}
