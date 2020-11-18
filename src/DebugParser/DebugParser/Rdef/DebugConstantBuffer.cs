using DXDecompiler.Chunks.Rdef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugConstantBuffer
	{
		public string Name { get; private set; }
		public ConstantBufferType BufferType { get; private set; }
		public List<DebugShaderVariable> Variables { get; private set; }
		public uint Size { get; private set; }
		public ConstantBufferFlags Flags { get; private set; }
		public DebugConstantBuffer()
		{
			Variables = new List<DebugShaderVariable>();
		}
		public static DebugConstantBuffer Parse(
			DebugBytecodeReader reader, DebugBytecodeReader constantBufferReader,
			DebugShaderVersion target)
		{
			uint nameOffset = constantBufferReader.ReadUInt32("nameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", constantBufferReader, (int)nameOffset);
			var name = nameReader.ReadString("name");
			uint variableCount = constantBufferReader.ReadUInt32("variableCount");
			uint variableOffset = constantBufferReader.ReadUInt32("variableOffset");

			var result = new DebugConstantBuffer
			{
				Name = name
			};

			var variableReader = reader.CopyAtOffset("variableReader", constantBufferReader, (int)variableOffset);
			for (int i = 0; i < variableCount; i++)
			{
				variableReader.AddIndent($"Variable {i}");
				result.Variables.Add(DebugShaderVariable.Parse(reader, variableReader, target, i == 0));
				variableReader.RemoveIndent();
			}
			result.Size = constantBufferReader.ReadUInt32("size");
			result.Flags = (ConstantBufferFlags)constantBufferReader.ReadUInt32("Flags");
			result.BufferType = (ConstantBufferType)constantBufferReader.ReadUInt32("BufferType");

			return result;
		}
	}
}
