using DXDecompiler.Util;
using System;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.Bytecode.Ctab
{
	/// <summary>
	/// Refer D3DXSHADER_TYPEINFO d3dx9shader.h 
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxshader-typeinfo
	/// Note also D3DXCONSTANT_DESC d3dx9shader.h which is a combined ConstantInfo and ConstantType struct
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxconstant-desc
	/// </summary>
	public class ConstantType
	{
		public ParameterClass ParameterClass;
		public ParameterType ParameterType;
		public uint Rows;
		public uint Columns;
		public uint Elements;
		public List<StructMember> Members;
		public ConstantType()
		{
			Members = new List<StructMember>();
		}
		public static ConstantType Parse(BytecodeReader reader, BytecodeReader typeReader)
		{
			var result = new ConstantType();
			result.ParameterClass = (ParameterClass)typeReader.ReadUInt16();
			result.ParameterType = (ParameterType)typeReader.ReadUInt16();
			result.Rows = typeReader.ReadUInt16();
			result.Columns = typeReader.ReadUInt16();
			result.Elements = typeReader.ReadUInt16();
			var memberCount = typeReader.ReadUInt16();
			var memberInfoOffset = typeReader.ReadUInt32();
			if(memberCount != 0)
			{
				var memberReader = reader.CopyAtOffset((int)memberInfoOffset);
				for(int i = 0; i < memberCount; i++)
				{
					result.Members.Add(StructMember.Parse(reader, memberReader));
				}
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
					return 0;
				case ParameterClass.Object:
					return 4 * elementCount;
				default:
					return 0;
			}
		}

		public string GetTypeName()
		{
			if(ParameterClass == ParameterClass.Vector)
			{
				if(Columns > 1)
				{
					return $"{ParameterType.GetDescription()}{Columns}";
				}
				else
				{
					return $"{ParameterType.GetDescription()}";
				}
			}
			else if(ParameterClass == ParameterClass.MatrixColumns)
			{
				return $"{ParameterType.GetDescription()}{Rows}x{Columns}";
			}
			else if(ParameterClass == ParameterClass.MatrixRows)
			{
				return $"{ParameterType.GetDescription()}{Columns}x{Rows}";
			}
			else
			{
				return ParameterType.GetDescription();
			}
		}
	}
}
