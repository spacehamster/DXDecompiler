using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectBuffer
	{
		public string Name { get; private set; }
		public uint VariableCount { get; private set; }
		public uint BufferSize { get; private set; }
		public ConstantBufferType BufferType { get; private set; }
		public ShaderVariableType Type => BufferType == ConstantBufferType.ConstantBuffer ?
			ShaderVariableType.CBuffer : ShaderVariableType.TBuffer;
		public uint RegisterNumber { get; private set; }
		public uint Unknown0 { get; private set; }
		public List<DebugEffectNumericVariable> Variables { get; private set; }

		uint NameOffset;
		public DebugEffectBuffer()
		{
			Variables = new List<DebugEffectNumericVariable>();
		}
		public static DebugEffectBuffer Parse(DebugBytecodeReader reader, DebugBytecodeReader bufferReader, DebugShaderVersion version, bool isShared)
		{
			var result = new DebugEffectBuffer();
			var nameOffset = result.NameOffset = bufferReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", bufferReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			result.BufferSize = bufferReader.ReadUInt32("BufferSize");
			result.BufferType = (ConstantBufferType)bufferReader.ReadUInt32("BufferType");
			result.VariableCount = bufferReader.ReadUInt32("VariableCount");
			result.RegisterNumber = bufferReader.ReadUInt32("RegisterNumber");
			if (!isShared)
			{
				result.Unknown0 = bufferReader.ReadUInt32("Unknown0");
			}
			//TODO: Unknown0
			//Debug.Assert(result.Unknown0 == 0, $"EffectBuffer.Unknown0: {result.Unknown0}");
			for (int i = 0; i < result.VariableCount; i++)
			{
				bufferReader.AddIndent($"BufferVariable {i}");
				result.Variables.Add(DebugEffectNumericVariable.Parse(reader, bufferReader, version, isShared));
				bufferReader.RemoveIndent();
			}
			return result;
		}
	}
}