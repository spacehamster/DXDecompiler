using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DX9Shader.FX9
{
	public class Parameter
	{
		public ParameterType ParameterType { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint ElementCount { get; private set; }
		public uint Rows { get; private set; }
		public uint Columns { get; private set; }
		public uint StructMemberCount { get; private set; }
		public List<Parameter> StructMembers = new List<Parameter>();

		public static Parameter Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Parameter();
			result.ParameterType = (ParameterType)variableReader.ReadUInt32();
			result.ParameterClass = (ParameterClass)variableReader.ReadUInt32();
			var nameOffset = variableReader.ReadUInt32();
			var semanticOffset = variableReader.ReadUInt32();
			if (result.ParameterClass == ParameterClass.Scalar ||
				result.ParameterClass == ParameterClass.Vector ||
				result.ParameterClass == ParameterClass.MatrixRows ||
				result.ParameterClass == ParameterClass.MatrixColumns)
			{
				result.ElementCount = variableReader.ReadUInt32();
				result.Rows = variableReader.ReadUInt32();
				result.Columns = variableReader.ReadUInt32();
			}
			if (result.ParameterClass == ParameterClass.Struct)
			{
				result.ElementCount = variableReader.ReadUInt32();
				result.StructMemberCount = variableReader.ReadUInt32();
				for(int i = 0; i < result.StructMemberCount; i++)
				{
					result.StructMembers.Add(Parameter.Parse(reader, variableReader));
				}
			}
			if (result.ParameterClass == ParameterClass.Object)
			{
				result.ElementCount = variableReader.ReadUInt32();
			}

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.TryReadString();

			var semanticReader = reader.CopyAtOffset((int)semanticOffset);
			result.Semantic = semanticReader.TryReadString();
			return result;
		}
		public uint GetSize()
		{
			var elementCount = Math.Max(1, ElementCount);
			switch (ParameterClass)
			{
				case ParameterClass.Object:
					return 4 * elementCount;
				case ParameterClass.Scalar:
					return 4 * elementCount;
				case ParameterClass.Vector:
					return Rows * 4 * elementCount;
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					return Rows * Columns * 4 * elementCount;
				case ParameterClass.Struct:
					return (uint)StructMembers.Sum(m => m.GetSize()) * elementCount;
				default:
					return 0;
			}
		}
		public string GetDecleration(int indentLevel = 0)
		{
			string arrayDecl = "";
			string semanticDecl = "";
			if(ElementCount > 0)
			{
				arrayDecl = string.Format("[{0}]", ElementCount);
			}
			if (!string.IsNullOrEmpty(Semantic))
			{
				semanticDecl = string.Format(" : {0}", Semantic);
			}
			return string.Format("{0} {1}{2}{3}", GetTypeName(indentLevel), Name, arrayDecl, semanticDecl);
		}
		public string GetTypeName(int indentLevel = 0)
		{
			var sb = new StringBuilder();
			string indent = new string(' ', indentLevel * 4);
			sb.Append(indent);
			switch (ParameterClass)
			{
				case ParameterClass.Scalar:
					sb.Append(ParameterType.ToString().ToLower());
					break;
				case ParameterClass.Vector:
					sb.Append(ParameterType.ToString().ToLower());
					sb.Append(Rows);
					break;
				case ParameterClass.MatrixColumns:
					sb.Append("column_major ");
					sb.Append(ParameterType.ToString().ToLower());
					sb.Append(string.Format("{0}x{1}", Columns, Rows));
					break;
				case ParameterClass.MatrixRows:
					sb.Append("row_major ");
					sb.Append(ParameterType.ToString().ToLower());
					sb.Append(string.Format("{0}x{1}", Rows, Columns));
					break;
				case ParameterClass.Struct:
					{
						sb.AppendLine("struct {");
						foreach(var member in StructMembers)
						{
							sb.AppendLine(string.Format("{0};", member.GetDecleration(indentLevel + 1)));
						}
						sb.Append(indent);
						sb.Append("}");
					}
					break;
				case ParameterClass.Object:
					sb.Append(ParameterType.ToString().ToLower());
					break;
				default:
					break;
			}
			return sb.ToString();
		}
	}
}
