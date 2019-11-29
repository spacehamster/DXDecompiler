using SlimShader.Chunks.Xsgn;
using SlimShader.Chunks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;

namespace SlimShader.Decompiler
{
	public partial class DXDecompiler
	{
		void WriteSignatures()
		{
			WriteInputSignature();
			WriteOutputSignature();
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
		void WriteOutputSignature()
		{
			var signature = Container.OutputSignature;
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
		void WriteSignatureParamater(SignatureParameterDescription param)
		{
			
			AddIndent();
			var fieldType = GetFieldType(param);
			Output.Append($"{fieldType} field{param.Register} : {param.SemanticName};");
			DebugSignatureParamater(param);

		}
		string GetFieldType(SignatureParameterDescription param)
		{
			string fieldType = param.ComponentType.GetDescription();
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
