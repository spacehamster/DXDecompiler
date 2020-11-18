using DXDecompiler.Chunks.Xsgn;
using DXDecompiler.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DXDecompiler.Chunks.Common;

namespace DXDecompiler.Decompiler
{
	public partial class HLSLDecompiler
	{
		void WriteSignatures()
		{
			if (Container.InputSignature != null)
			{
				WriteInputSignature();
			}
			if (Container.OutputSignature != null)
			{
				WriteOutputSignature();
			}
			WritePatchConstant();
		}
		void WriteInputSignature()
		{
			var signature = Container.InputSignature;
			Output.AppendLine("struct ShaderInput");
			Output.AppendLine("{");
			indent++;
			foreach(var param in signature.Parameters)
			{
				WriteSignatureParamater(param);
			}
			indent--;
			Output.AppendLine("};");
			Output.AppendLine();
		}
		void WritePatchConstant()
		{
			if (Container.PatchConstantSignature == null) return;
			Output.AppendLine("struct PatchConstant");
			Output.AppendLine("{");
			indent++;
			foreach (var group in Container.PatchConstantSignature.Parameters.GroupBy(p => p.SemanticName))
			{
				var count = group.Count();
				var param = group.First();
				AddIndent();
				var fieldType = GetFieldType(param);
				string array = count > 1 ? $"[{count}]" : "";
				Output.Append($"{fieldType} field{param.Register}{param.ReadWriteMask.GetDescription().Trim()}{array} : {GetSemanticName(param)};");
				DebugSignatureParamater(param);
			}
			indent--;
			Output.AppendLine("};");
			Output.AppendLine();
		}
		void WriteOutputSignature()
		{
			var signature = Container.OutputSignature;
			if (Container.Shader.Version.ProgramType == ProgramType.GeometryShader)
			{
				foreach(var group in signature.Parameters.GroupBy(p => p.Stream)){
					Output.AppendLine($"struct Stream{group.Key}Output");
					Output.AppendLine("{");
					indent++;
					foreach (var param in group)
					{
						WriteSignatureParamater(param);
					}
					indent--;
					Output.AppendLine("};");
					Output.AppendLine();
				}
			}
			else
			{
				Output.AppendLine("struct ShaderOutput");
				Output.AppendLine("{");
				indent++;
				foreach (var param in signature.Parameters)
				{
					WriteSignatureParamater(param);
				}
				indent--;
				Output.AppendLine("};");
				Output.AppendLine();
			}
		}
		void WriteSignatureParamater(SignatureParameterDescription param)
		{
			
			AddIndent();
			var fieldType = GetFieldType(param);
			Output.Append($"{fieldType} {param.GetName()} : {GetSemanticName(param)};");
			DebugSignatureParamater(param);
		}
		string GetSemanticName(SignatureParameterDescription param)
		{
			if(param.SemanticName == "TEXCOORD")
			{
				return $"{param.SemanticName}{param.SemanticIndex}";
			}
			return param.SemanticName;
		}
		string GetFieldType(SignatureParameterDescription param)
		{
			string fieldType = param.ComponentType.GetDescription();
			if (param.MinPrecision != MinPrecision.None)
			{
				fieldType = param.MinPrecision.GetTypeName();
			}
			int componentCount = 0;
			if (param.Mask.HasFlag(ComponentMask.X)) componentCount += 1;
			if (param.Mask.HasFlag(ComponentMask.Y)) componentCount += 1;
			if (param.Mask.HasFlag(ComponentMask.Z)) componentCount += 1;
			if (param.Mask.HasFlag(ComponentMask.W)) componentCount += 1;
			switch (componentCount)
			{
				case 1:
					return $"{fieldType}";
				case 2:
					return $"{fieldType}2";
				case 3:
					return $"{fieldType}3";
				case 4:
					return $"{fieldType}4";
				default:
					throw new Exception($"Invalid ComponentMask {param.Mask}");
			}
		}
		void DebugSignatureParamater(SignatureParameterDescription param)
		{
			Output.Append(" // ");
			Output.AppendLine(param.ToString());
		}
	}
}
