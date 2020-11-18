using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;
using System.Diagnostics;

namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugShaderType
	{
		private readonly int _indent;
		private readonly bool _isFirst;
		public ShaderVariableClass VariableClass { get; private set; }
		public ShaderVariableType VariableType { get; private set; }
		public ushort Rows { get; private set; }
		public ushort Columns { get; private set; }
		public ushort ElementCount { get; private set; }
		public List<DebugShaderTypeMember> Members { get; private set; }
		public DebugShaderType SubType { get; private set; }
		public DebugShaderType BaseClass { get; private set; }
		public List<DebugShaderType> Interfaces { get; private set; }
		public string BaseTypeName { get; private set; }
		public uint NumberOfInterfaces { get; private set; }

		public DebugShaderType(int indent, bool isFirst)
		{
			_indent = indent;
			_isFirst = isFirst;
			Members = new List<DebugShaderTypeMember>();
			Interfaces = new List<DebugShaderType>();
		}

		public static DebugShaderType Parse(DebugBytecodeReader reader, DebugBytecodeReader typeReader, DebugShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var result = new DebugShaderType(indent, isFirst)
			{
				VariableClass = typeReader.ReadEnum16<ShaderVariableClass>("VariableClass"),
				VariableType = typeReader.ReadEnum16<ShaderVariableType>("VariableType"),
				Rows = typeReader.ReadUInt16("Rows"),
				Columns = typeReader.ReadUInt16("Columns"),
				ElementCount = typeReader.ReadUInt16("ElementCount")
			};

			var memberCount = typeReader.ReadUInt16("memberCount");
			var memberOffset = typeReader.ReadUInt32("memberOffset");

			if(target.MajorVersion >= 5)
			{
				var subTypeOffset = typeReader.ReadInt32("subTypeOffset"); // Guessing
				if(subTypeOffset != 0)
				{
					var parentInterfaceReader = reader.CopyAtOffset("subtypeReader", typeReader, (int)subTypeOffset);
					result.SubType = DebugShaderType.Parse(reader, parentInterfaceReader, target,
						indent + 4, true, parentOffset);
				}

				var baseClassOffset = typeReader.ReadUInt32("baseClassOffset");
				if(baseClassOffset != 0)
				{
					var baseClassReader = reader.CopyAtOffset("baseClassReader", typeReader, (int)baseClassOffset);
					result.BaseClass = DebugShaderType.Parse(reader, baseClassReader, target,
						indent + 4, true, parentOffset);
				}

				result.NumberOfInterfaces = typeReader.ReadUInt32("NumberOfInterfaces");

				var interfaceSectionOffset = typeReader.ReadUInt32("InterfaceSectionOffset");
				if(interfaceSectionOffset != 0)
				{
					var interfaceSectionReader = reader.CopyAtOffset("interfaceSectionReader", typeReader, (int)interfaceSectionOffset);
					for(int i = 0; i < result.NumberOfInterfaces; i++)
					{
						var interfaceTypeOffset = interfaceSectionReader.ReadUInt32($"UnkInterface{i}");
						var interfaceReader = reader.CopyAtOffset($"InterfaceReader {i}", typeReader, (int)interfaceTypeOffset);
						result.Interfaces.Add(DebugShaderType.Parse(reader, interfaceReader,
							target, indent + 4, i == 0, parentOffset));
					}
				}

				var parentNameOffset = typeReader.ReadUInt32("parentNameOffset");
				if(parentNameOffset > 0)
				{
					var parentNameReader = reader.CopyAtOffset("parentNameOffset", typeReader, (int)parentNameOffset);
					result.BaseTypeName = parentNameReader.ReadString("BaseTypeName");
				}
			}

			if(memberCount > 0)
			{

				var memberReader = reader.CopyAtOffset("memberReader", typeReader, (int)memberOffset);
				for(int i = 0; i < memberCount; i++)
				{
					memberReader.AddIndent($"Member {i}");
					result.Members.Add(DebugShaderTypeMember.Parse(reader, memberReader, target, indent + 4, i == 0,
						parentOffset));
					memberReader.RemoveIndent();
				}
			}

			if(target.ProgramType == ProgramType.LibraryShader && target.MajorVersion == 4)
			{
				var unk1 = typeReader.ReadUInt32("Unk1");
				var unk2 = typeReader.ReadUInt32("Unk2");
				var unk3 = typeReader.ReadUInt32("Unk3");
				var unk4 = typeReader.ReadUInt32("Unk4");
				var typeNameOffset = typeReader.ReadUInt32("typeNameoffset");
				var typeNameReader = reader.CopyAtOffset("TypeNameReader", typeReader, (int)typeNameOffset);
				typeNameReader.ReadString("TypeName");
				Debug.Assert(unk1 == 0, $"ShaderType.Unk1={unk1}");
				Debug.Assert(unk2 == 0, $"ShaderType.Unk2={unk2}");
				Debug.Assert(unk3 == 0, $"ShaderType.Unk3={unk3}");
				Debug.Assert(unk4 == 0, $"ShaderType.Unk4={unk4}");
			}

			return result;
		}
	}
}