using SlimShader.Chunks.Rdef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Decompiler
{
	public partial class DXDecompiler
	{
		/*
		 * This should be merged with Declrations
		 * Resource Definitions and InputOutput Signatures have corrispondting declration tokens
		 * 
		 */ 
		void WriteResoureDefinitions()
		{
			var rdef = Container.ResourceDefinition;
			Output.AppendLine("// ConstantBuffers");
			foreach (var constantBuffer in rdef.ConstantBuffers)
			{
				WriteConstantBuffer(constantBuffer);
			}
			
			Output.AppendLine("// ResourceBindings");
			foreach (var resourceBinding in rdef.ResourceBindings)
			{
				WriteResourceBinding(resourceBinding);
			}
			Output.AppendLine();
		}
		void WriteConstantBuffer(ConstantBuffer constantBuffer)
		{
			if (constantBuffer.Name == "$Globals")
			{
				Output.AppendLine("// Globals");
				foreach (var variable in constantBuffer.Variables)
				{
					WriteVariable(variable);
				}
				Output.AppendLine("");
			}
			else if (constantBuffer.Name == "$ThisPointer")
			{
				Output.AppendLine("// ThisPointer");
				foreach (var variable in constantBuffer.Variables)
				{
					WriteVariable(variable);
				}
				Output.AppendLine("");
			}
			else if (constantBuffer.Name == "$Params")
			{
			}
			else
			{
				var resourceBinding = Container.ResourceDefinition.ResourceBindings.FirstOrDefault(rb => 
					rb.Type == ShaderInputType.CBuffer && 
					rb.Name == constantBuffer.Name);
				string bindpoint = resourceBinding != null ? resourceBinding.BindPoint.ToString() : "Unknown";
				Output.AppendLine($"cbuffer {constantBuffer.Name} : register(b{bindpoint})");
				Output.AppendLine("{");
				indent++;
				foreach (var variable in constantBuffer.Variables)
				{
					WriteVariable(variable);
				}
				indent--;
				Output.AppendLine("}");
				Output.AppendLine("");
			}
		}
		void WriteResourceBinding(ResourceBinding resourceBinding)
		{
			Output.AppendLine(resourceBinding.ToString());
			if (resourceBinding.Type == ShaderInputType.Texture)
			{
				Output.AppendLine($"Texture2D {resourceBinding.Name};");
			}
			if (resourceBinding.Type == ShaderInputType.Sampler)
			{
				string samplerType = resourceBinding.Flags.HasFlag(ShaderInputFlags.ComparisonSampler) ?
					"SamplerComparisonState" : "SamplerState";
				Output.AppendLine($"{samplerType} {resourceBinding.Name};");
			}
		}
		void WriteVariable(ShaderVariable variable)
		{
			AddIndent();
			Output.Append($"{GetShaderTypeName(variable.ShaderType)} {variable.Name}");
			if(variable.DefaultValue != null)
			{
				Output.Append($" = {variable.DefaultValue}");
			}
			Output.Append($"; // Offset {variable.StartOffset} Size {variable.Size}");
			if (variable.Flags.HasFlag(ShaderVariableFlags.Used))
			{
				Output.Append($" [unused]");
			}
			Output.AppendLine();
		}
		internal string GetShaderTypeName(ShaderType variable)
		{
			return variable.ToString().Replace("//", "").Trim(); // TODO: Cleanup
		}
	}
}
