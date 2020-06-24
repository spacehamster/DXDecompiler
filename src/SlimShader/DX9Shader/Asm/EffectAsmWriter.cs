using SlimShader.DX9Shader.FX9;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class EffectAsmWriter : DecompileWriter
	{
		FX9.EffectContainer EffectChunk;
		public EffectAsmWriter(FX9.EffectContainer effectChunk)
		{
			EffectChunk = effectChunk;
		}
		public static string Disassemble(FX9.EffectContainer effectChunk)
		{
			var asmWriter = new EffectAsmWriter(effectChunk);
			return asmWriter.Decompile();
		}
		protected override void Write()
		{
			foreach (var variable in EffectChunk.Variables)
			{
				if(variable.Parameter.ParameterType == ParameterType.PixelShader ||
					variable.Parameter.ParameterType == ParameterType.VertexShader)
				{
					WriteShaderVariable(variable);
				}
			}
			WriteLine("//listing of all techniques and passes with embedded asm listings");
			WriteLine();
			foreach(var technique in EffectChunk.Techniques)
			{
				WriteLine("technique {0}", technique.Name);
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
		}
		public void WriteShaderVariable(Variable variable) 
		{

			WriteIndent();
			Write("{0} {1}", 
					variable.Parameter.ParameterType.ToString().ToLower(),
					variable.Parameter.Name);
			var elementCount = variable.Parameter.ElementCount == 0 ? 1 : variable.Parameter.ElementCount;
			if (elementCount > 1)
			{
				Write("[{0}]",
					variable.Parameter.ElementCount);
			}
			Write(" = ");
			if(elementCount > 1)
			{
				WriteLine();
				WriteLine("{");
				for (int i = 0; i < elementCount; i++)
				{
					var blob = EffectChunk.VariableBlobLookup[variable.Parameter][i];
					if (blob.Shader == null)
					{
						WriteIndent();
						WriteLine("null;");
					}
					else
					{
						WriteLine("//listing for: {0}[{1}]", variable.Parameter.Name, i);
						WriteIndent();
						WriteLine("asm {");

						WriteLine(AsmWriter.Disassemble(blob.Shader));
						Write("}");
						if(i < elementCount - 1)
						{
							WriteLine(",");
							WriteLine("");
						}
					}
				}
				WriteLine("};");
				WriteLine();
			} else
			{
				var blob = EffectChunk.VariableBlobLookup[variable.Parameter][0];
				if (blob.Shader == null)
				{
					WriteIndent();
					WriteLine("null;");
				}
				else
				{
					WriteLine();
					WriteIndent();
					WriteLine("asm {");
					WriteLine(AsmWriter.Disassemble(blob.Shader));
					WriteLine("};");
					WriteLine("");
				}
			}
		}
		public void WritePass(Pass pass)
		{
			WriteIndent();
			WriteLine("pass {0}", pass.Name);
			WriteIndent();
			WriteLine("{");
			Indent++;
			var vertexAssignment = pass.Assignments
				.FirstOrDefault(a => a.Type == StateType.VertexShader);
			if(vertexAssignment != null)
			{
				WriteAssignment(vertexAssignment);
			} else
			{
				WriteIndent();
				WriteLine("//No embedded vertex shader");
			}
			var pixelAssignment = pass.Assignments
				.FirstOrDefault(a => a.Type == StateType.PixelShader);
			if (pixelAssignment != null)
			{
				WriteAssignment(pixelAssignment);
			}
			else
			{
				WriteIndent();
				WriteLine("//No embedded pixel shader");
			}
			Indent--;
			WriteIndent();
			WriteLine("}");
		}
		public void WriteAssignment(Assignment assignment)
		{
			WriteIndent();
			EffectChunk.StateBlobLookup.TryGetValue(assignment, out StateBlob stateBlob);
			if (stateBlob != null)
			{
				Write("{0} = ", assignment.Type.ToString().ToLower());
				if(stateBlob.BlobType == StateBlobType.Variable)
				{
					var variable = EffectChunk.Variables.First(v => v.Parameter.Name == stateBlob.VariableName);
					var variableShader = EffectChunk.VariableBlobLookup[variable.Parameter].First();
					WriteLine();
					Indent++;
					WriteIndent();
					WriteLine("asm {");
					//TODO: Dissassembler should accept indent
					var disasm = string.Join("\n", AsmWriter.Disassemble(variableShader.Shader)
						.Split('\n')
						.Select(l => $"{new string(' ', Indent * 4)}{l}"));
					WriteLine(disasm);
					WriteIndent();
					WriteLine("};");
					Indent--;
				} else if(stateBlob.BlobType == StateBlobType.Shader)
				{
					WriteLine();
					Indent++;
					WriteIndent();
					WriteLine("asm {");
					var disasm = string.Join("\n", AsmWriter.Disassemble(stateBlob.Shader)
						.Split('\n')
						.Select(l => $"{new string(' ', Indent * 4)}{l}"));
					WriteLine(disasm);
					WriteIndent();
					WriteLine("};");
					Indent--;
				} else if (stateBlob.BlobType == StateBlobType.IndexShader)
				{
					WriteLine("{0}[TODO];", stateBlob.VariableName);
				}
			}
			else if (assignment.Type == StateType.VertexShader)
			{
				WriteLine("//No embedded vertex shader");
			}
			else if(assignment.Type == StateType.PixelShader)
			{
				WriteLine("//No embedded pixel shader");
			}
		}
	}
}
