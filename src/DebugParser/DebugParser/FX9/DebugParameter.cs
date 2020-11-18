using DXDecompiler.DX9Shader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugParameter
	{
		public ParameterType ParameterType { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint Elements { get; private set; }
		public uint Rows { get; private set; }
		public uint Columns { get; private set; }
		public uint StructMemberCount { get; private set; }
		public List<DebugParameter> StructMembers = new List<DebugParameter>();

		public uint NameOffset;
		public uint SemanticOffset;

		public static DebugParameter Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader)
		{
			var result = new DebugParameter();
			result.ParameterType = variableReader.ReadEnum32<ParameterType>("ParameterType");
			result.ParameterClass = variableReader.ReadEnum32<ParameterClass>("ParameterClass");
			result.NameOffset = variableReader.ReadUInt32("NameOffset");

			var nameReader = reader.CopyAtOffset("NameReader", variableReader, (int)result.NameOffset);
			result.Name = nameReader.TryReadString("Name");

			result.SemanticOffset = variableReader.ReadUInt32("SemanticOffset");

			var semanticReader = reader.CopyAtOffset("SemanticReader", variableReader, (int)result.SemanticOffset);
			result.Semantic = semanticReader.TryReadString("Semantic");

			if(result.ParameterClass == ParameterClass.Scalar ||
				result.ParameterClass == ParameterClass.Vector ||
				result.ParameterClass == ParameterClass.MatrixRows ||
				result.ParameterClass == ParameterClass.MatrixColumns)
			{
				result.Elements = variableReader.ReadUInt32("ElementCount");
				result.Rows = variableReader.ReadUInt32("Rows");
				result.Columns = variableReader.ReadUInt32("Columns");
			}
			if(result.ParameterClass == ParameterClass.Struct)
			{
				result.Elements = variableReader.ReadUInt32("ElementCount");
				result.StructMemberCount = variableReader.ReadUInt32("StructMemberCount");
				for(int i = 0; i < result.StructMemberCount; i++)
				{
					result.StructMembers.Add(DebugParameter.Parse(reader, variableReader));
				}

			}
			if(result.ParameterClass == ParameterClass.Object)
			{
				result.Elements = variableReader.ReadUInt32("Elements");
			}
			return result;
		}

		public uint GetSize()
		{
			var elementCount = Math.Max(1, Elements);
			switch(ParameterClass)
			{
				case ParameterClass.Scalar:
					return 4 * elementCount;
				case ParameterClass.Vector:
					return Rows * 4 * elementCount;
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					return Rows * Columns * 4 * elementCount;
				case ParameterClass.Struct:
					return (uint)StructMembers.Sum(m => m.GetSize()) * elementCount;
				case ParameterClass.Object:
					return 4 * elementCount;
				default:
					return 0;
			}
		}
	}
}