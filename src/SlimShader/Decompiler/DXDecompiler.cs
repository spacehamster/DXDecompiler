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
		StringBuilder Output = new StringBuilder();
		BytecodeContainer Container;
		public Dictionary<ConstantBufferType, List<ConstantBuffer>> m_ConstantBufferLookup;
		int indent = 0;
		public static string Decompile(byte[] data)
		{
			var container = new BytecodeContainer(data);
			var decompiler = new DXDecompiler(container);
			return decompiler.Decompile();
		}
		DXDecompiler(BytecodeContainer container)
		{
			Container = container;
			m_ConstantBufferLookup = new Dictionary<ConstantBufferType, List<ConstantBuffer>>();
			
			foreach (ConstantBufferType bufferType in Enum.GetValues(typeof(ConstantBufferType)))
			{
				if (bufferType == ConstantBufferType.InterfacePointers) continue;
				if (bufferType == ConstantBufferType.ResourceBindInformation) continue;
				var rbType =
					bufferType == ConstantBufferType.ConstantBuffer ? ShaderInputType.CBuffer :
					bufferType == ConstantBufferType.TextureBuffer ? ShaderInputType.Texture :
					ShaderInputType.CBuffer;
				var items = container.ResourceDefinition.ConstantBuffers
					.Where(cb => cb.BufferType == bufferType)
					.ToList();
				var bindings = container.ResourceDefinition.ResourceBindings
					.Where(rb => rb.Type == rbType)
					.ToList();
				var elementCount = bindings.Count > 0 ? (int)bindings.Max(rb => rb.BindPoint) + 1 : 0;
				m_ConstantBufferLookup[bufferType] = new List<ConstantBuffer>();
				for(int i = 0; i < elementCount; i++)
				{
					m_ConstantBufferLookup[bufferType].Add(null);
				}
				foreach(var item in items)
				{
					var bindPoint = bindings.First(rb => rb.Name == item.Name).BindPoint;
					m_ConstantBufferLookup[bufferType][(int)bindPoint] = item;
				}
			}
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
		//refer bool ToGLSL::Translate()
		private string Decompile()
		{
			WriteResoureDefinitions();
			WriteSignatures();
			TranslateDeclarations();
			Output.AppendFormat("ShaderOutput {0}(ShaderInput input", GetMainFuncName());
			WriteParams();
			Output.AppendLine(")");
			Output.AppendLine("{");
			indent++;
			AddIndent();
			Output.AppendLine("ShaderOutput output;");
			WriteTempRegisters();
			TranslateInstructions();
			indent--;
			Output.AppendLine("}");

			return Output.ToString();
		}
		void TranslateDeclarations()
		{
			foreach(var token in Container.Shader.DeclarationTokens)
			{
				TranslateDeclaration(token);
			}
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
				Output.Append($"{GetShaderTypeName(variable.ShaderType)} {variable.Name}");
			}
			indent--;
		}
		void TranslateInstructions()
		{
			foreach (var token in Container.Shader.InstructionTokens)
			{
				try
				{
					TranslateInstruction(token);
				}
				catch (NotImplementedException)
				{
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
			AddIndent();
			Output.AppendLine($"//{token}");
		}
		ConstantBuffer GetConstantBuffer(ConstantBufferType type, int bindPoint)
		{
			return m_ConstantBufferLookup[type][bindPoint];
		}
	}
}
