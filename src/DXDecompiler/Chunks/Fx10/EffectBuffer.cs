using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectBuffer
	{
		public string Name { get; private set; }
		public uint VariableCount { get; private set; }
		public uint BufferSize { get; private set; }
		public ConstantBufferType BufferType { get; private set; }
		public ShaderVariableType Type => BufferType == ConstantBufferType.ConstantBuffer ?
			ShaderVariableType.CBuffer : ShaderVariableType.TBuffer;
		public uint RegisterNumber { get; private set; }
		public List<EffectNumericVariable> Variables { get; private set; }

		uint NameOffset;
		public EffectBuffer()
		{
			Variables = new List<EffectNumericVariable>();
		}
		public static EffectBuffer Parse(BytecodeReader reader, BytecodeReader bufferReader, ShaderVersion version, bool isShared)
		{
			var result = new EffectBuffer();
			var nameOffset = result.NameOffset = bufferReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			result.BufferSize = bufferReader.ReadUInt32();
			result.BufferType = (ConstantBufferType)bufferReader.ReadUInt32();
			result.VariableCount = bufferReader.ReadUInt32();
			result.RegisterNumber = bufferReader.ReadUInt32();
			if (!isShared)
			{
				var unknown0 = bufferReader.ReadUInt32();
				//TODO: Unknown0
				//Debug.Assert(result.Unknown0 == 0, $"EffectBuffer.Unknown0: {result.Unknown0}");
			}
			for (int i = 0; i < result.VariableCount; i++)
			{
				result.Variables.Add(EffectNumericVariable.Parse(reader, bufferReader, version, isShared));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			var bufferType = BufferType.GetDescription();
			var registerPrefix = BufferType == ConstantBufferType.ConstantBuffer ? "b" : "t";
			sb.Append(string.Format("{0} {1}", 
				bufferType, Name));
			if(RegisterNumber != uint.MaxValue)
			{
				sb.Append(string.Format(" : register({0}{1})", registerPrefix, RegisterNumber));
			}
			sb.AppendLine();
			sb.AppendLine("{");
			foreach (var variable in Variables)
			{
				sb.AppendLine(variable.ToString());
			}
			sb.AppendLine("}");
			return sb.ToString();
		}
	}
}