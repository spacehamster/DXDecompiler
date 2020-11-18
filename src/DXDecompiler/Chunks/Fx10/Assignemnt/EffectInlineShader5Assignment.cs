using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectInlineShader5Assignment : EffectAssignment
	{
		public BytecodeContainer Shader { get; private set; }
		public List<string> SODecls { get; private set; }
		public uint[] SODeclsOffset { get; private set; }
		public uint SODeclsCount { get; private set; }
		public uint RasterizedStream { get; private set; }
		public List<EffectInterfaceInitializer> InterfaceBindings { get; private set; }

		public EffectInlineShader5Assignment()
		{
			SODeclsOffset = new uint[4];
			SODecls = new List<string>();
			InterfaceBindings = new List<EffectInterfaceInitializer>();
		}
		public new static EffectInlineShader5Assignment Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new EffectInlineShader5Assignment();
			var shaderOffset = variableReader.ReadUInt32();
			result.SODeclsOffset[0] = variableReader.ReadUInt32();
			result.SODeclsOffset[1] = variableReader.ReadUInt32();
			result.SODeclsOffset[2] = variableReader.ReadUInt32();
			result.SODeclsOffset[3] = variableReader.ReadUInt32();
			var SoDeclsCount = result.SODeclsCount = variableReader.ReadUInt32();
			result.RasterizedStream = variableReader.ReadUInt32();
			var interfaceBindingCount = variableReader.ReadUInt32();
			var interfaceBindingOffset = variableReader.ReadUInt32();
			var shaderReader = reader.CopyAtOffset((int)shaderOffset);
			var shaderSize = shaderReader.ReadUInt32();
			if (shaderSize != 0)
			{
				result.Shader = BytecodeContainer.Parse(shaderReader.ReadBytes((int)shaderSize));
			}
			for (int i = 0; i < 4; i++)
			{
				var offset = result.SODeclsOffset[i];
				if (offset != 0)
				{
					var soDeclReader = reader.CopyAtOffset((int)offset);
					result.SODecls.Add(soDeclReader.ReadString());
				}
			}
			var interfaceReader = reader.CopyAtOffset((int)interfaceBindingOffset);
			for (int i = 0; i < interfaceBindingCount; i++)
			{
				result.InterfaceBindings.Add(EffectInterfaceInitializer.Parse(reader, interfaceReader));
			}
			return result;
		}
		public override string ToString()
		{
			if (Shader == null)
			{
				return string.Format("{0} = NULL;", MemberType);
			}
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("{0} = asm {{", MemberType));
			sb.Append("        ");
			var shaderText = Shader.ToString()
				.Replace(Environment.NewLine, $"{Environment.NewLine}        ");
			sb.AppendLine(shaderText);
			sb.Append("    }");
			for(int i = 0; i < InterfaceBindings.Count; i++)
			{
				var binding = InterfaceBindings[i];
				sb.AppendLine();
				sb.Append(string.Format("/* Interface parameter {0} bound to: {1} */", i, binding));
			}
			sb.Append(";");
			return sb.ToString();
		}
	}
}
