using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Describes a shader-variable type.
	/// Based on D3D12_SHADER_TYPE_DESC.
	/// </summary>
	public class ShaderType
	{
		private readonly int _indent;
		private readonly bool _isFirst;

		/// <summary>
		/// Identifies the variable class as one of scalar, vector, matrix or object.
		/// </summary>
		public ShaderVariableClass VariableClass { get; private set; }

		/// <summary>
		/// The variable type.
		/// </summary>
		public ShaderVariableType VariableType { get; private set; }

		/// <summary>
		/// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Rows { get; private set; }

		/// <summary>
		/// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Columns { get; private set; }

		/// <summary>
		/// Number of elements in an array; otherwise 0.
		/// </summary>
		public ushort ElementCount { get; private set; }

		public List<ShaderTypeMember> Members { get; private set; }

		/// <summary>
		/// Parent Interface. 
		/// TODO: This is a guess, confirm that this is the parent interface
		/// </summary>
		public ShaderType SubType { get; private set; }

		/// <summary>
		/// TODO: Find out what this is for
		/// </summary>
		public ShaderType BaseClass { get; private set; }

		/// <summary>
		/// The interface types a concrete class inherits from 
		/// </summary>
		public List<ShaderType> Interfaces { get; private set; }

		/// <summary>
		/// Name of the shader-variable type. This member can be NULL if it isn't used. This member supports 
		/// dynamic shader linkage interface types, which have names.
		/// TODO: Is this the right description?
		/// </summary>
		public string BaseTypeName { get; private set; }

		public ShaderType(int indent, bool isFirst)
		{
			_indent = indent;
			_isFirst = isFirst;
			Members = new List<ShaderTypeMember>();
			Interfaces = new List<ShaderType>();
		}

		public static ShaderType Parse(BytecodeReader reader, BytecodeReader typeReader, ShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var result = new ShaderType(indent, isFirst)
			{
				VariableClass = (ShaderVariableClass) typeReader.ReadUInt16(),
				VariableType = (ShaderVariableType) typeReader.ReadUInt16(),
				Rows = typeReader.ReadUInt16(),
				Columns = typeReader.ReadUInt16(),
				ElementCount = typeReader.ReadUInt16()
			};

			var memberCount = typeReader.ReadUInt16();
			var memberOffset = typeReader.ReadUInt32();

			if (target.MajorVersion >= 5)
			{
				var subTypeOffset = typeReader.ReadUInt32(); // Guessing
				if (subTypeOffset != 0)
				{
					var parentTypeReader = reader.CopyAtOffset((int)subTypeOffset);
					result.SubType = ShaderType.Parse(reader, parentTypeReader, target,
						indent + 4, true, parentOffset);
					Debug.Assert(
							result.SubType.VariableClass == ShaderVariableClass.Vector ||
							result.SubType.VariableClass == ShaderVariableClass.InterfaceClass);
				}

				var baseClassOffset = typeReader.ReadUInt32();
				if (baseClassOffset != 0)
				{
					var baseClassReader = reader.CopyAtOffset((int)baseClassOffset);
					result.BaseClass = ShaderType.Parse(reader, baseClassReader, target,
						indent + 4, true, parentOffset);
					Debug.Assert(
						result.BaseClass.VariableClass == ShaderVariableClass.Scalar ||
						result.BaseClass.VariableClass == ShaderVariableClass.Struct);
				}

				var interfaceCount = typeReader.ReadUInt32();
				var interfaceSectionOffset = typeReader.ReadUInt32();
				if (interfaceSectionOffset != 0)
				{
					var interfaceSectionReader = reader.CopyAtOffset((int)interfaceSectionOffset);
					for (int i = 0; i < interfaceCount; i++)
					{
						var interfaceTypeOffset = interfaceSectionReader.ReadUInt32();
						var interfaceReader = reader.CopyAtOffset((int)interfaceTypeOffset);
						result.Interfaces.Add(ShaderType.Parse(reader, interfaceReader, target, 
							indent + 4, i == 0, parentOffset));
					}
				}

				var parentNameOffset = typeReader.ReadUInt32();
				if (parentNameOffset > 0)
				{
					var parentNameReader = reader.CopyAtOffset((int) parentNameOffset);
					result.BaseTypeName = parentNameReader.ReadString();
				}
			}

			if (memberCount > 0)
			{
				var memberReader = reader.CopyAtOffset((int) memberOffset);
				for (int i = 0; i < memberCount; i++)
					result.Members.Add(ShaderTypeMember.Parse(reader, memberReader, target, indent + 4, i == 0,
						parentOffset));
			}

			if (target.ProgramType == ProgramType.LibraryShader && target.MajorVersion == 4)
			{
				var unk1 = typeReader.ReadUInt32();
				var unk2 = typeReader.ReadUInt32();
				var unk3 = typeReader.ReadUInt32();
				var unk4 = typeReader.ReadUInt32();
				var typeNameOffset = typeReader.ReadUInt32();
				var typeNameReader = reader.CopyAtOffset((int)typeNameOffset);
				typeNameReader.ReadString();
				Debug.Assert(unk1 == 0, $"ShaderType.Unk1={unk1}");
				Debug.Assert(unk2 == 0, $"ShaderType.Unk2={unk2}");
				Debug.Assert(unk3 == 0, $"ShaderType.Unk3={unk3}");
				Debug.Assert(unk4 == 0, $"ShaderType.Unk4={unk4}");
			}

			return result;
		}

		public override string ToString()
		{
			var indentString = "// " + new string(Enumerable.Repeat(' ', _indent).ToArray());
			var sb = new StringBuilder();
			if (_isFirst)
				sb.AppendLine(indentString);
			switch (VariableClass)
			{
				case ShaderVariableClass.InterfacePointer:
				case ShaderVariableClass.MatrixColumns:
				case ShaderVariableClass.MatrixRows:
				{
					sb.Append(indentString);
					if (!string.IsNullOrEmpty(BaseTypeName)) // BaseTypeName is only populated in SM 5.0
					{
						sb.Append(string.Format("{0}{1}", VariableClass.GetDescription(), BaseTypeName));
					}
					else
					{
						sb.Append(VariableClass.GetDescription());
						sb.Append(VariableType.GetDescription());
						if (Columns > 1)
						{
							sb.Append(Columns);
							if (Rows > 1)
								sb.Append("x" + Rows);
						}
					}
					break;
				}
				case ShaderVariableClass.Vector:
				{
					sb.Append(indentString + VariableType.GetDescription());
					sb.Append(Columns);
					break;
				}
				case ShaderVariableClass.Struct:
					{
						if (!_isFirst)
							sb.AppendLine(indentString);
						sb.AppendLine(indentString + "struct " + BaseTypeName);
						sb.AppendLine(indentString + "{");
						foreach (var member in Members)
							sb.AppendLine(member.ToString());
						sb.AppendLine("//");
						sb.Append(indentString + "}");
						break;
					}
				case ShaderVariableClass.Scalar:
					{
						sb.Append(indentString + VariableType.GetDescription());
						break;
					}
				default:
					throw new InvalidOperationException(string.Format("Variable class '{0}' is not currently supported.", VariableClass));
			}
			return sb.ToString();
		}
	}
}