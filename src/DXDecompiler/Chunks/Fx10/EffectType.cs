using SlimShader.Chunks.Common;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC and SType
	/// </summary>
	public class EffectType
	{
		public string TypeName { get; private set; }
		public EffectVariableType EffectVariableType { get; private set; }
		public uint ElementCount { get; private set; }
		public uint PackedSize { get; private set; }
		public uint UnpackedSize { get; private set; }
		public uint Stride { get; private set; }
		public uint PackedType { get; private set; }
		public uint MemberCount { get; private set; }
		public EffectObjectType ObjectType { get; private set; }
		public EffectNumericType NumericType { get; private set; }
		public ShaderVariableClass VariableClass { get; private set; }
		public List<EffectMember> Members { get; private set; }
		public uint BaseClassType { get; private set; }
		public uint InterfaceCount { get; private set; }
		public List<EffectType> InterfaceTypes { get; private set; }

		public uint Rows => EffectVariableType == EffectVariableType.Numeric ?
				NumericType.Rows : 0;
		public uint Columns => EffectVariableType == EffectVariableType.Numeric ?
			NumericType.Columns : 0;
		public ShaderVariableType VariableType
		{
			get {
				//TODO: remove numeric and interface value from ObjectType
				switch (EffectVariableType){
					case EffectVariableType.Numeric:
					case EffectVariableType.Object:
						return ObjectType.ToShaderVariableType();
					case EffectVariableType.Struct:
						return ShaderVariableType.Void;
					case EffectVariableType.Interface:
						return ShaderVariableType.InterfacePointer;
					default:
						return 0;
						//throw new Exception($"Unexpected EffectVariableType {EffectVariableType}");
				}
			}
		}
		public EffectType()
		{
			Members = new List<EffectMember>();
			InterfaceTypes = new List<EffectType>();
		}
		public static EffectType Parse(BytecodeReader reader, BytecodeReader typeReader, ShaderVersion version)
		{
			var result = new EffectType();
			var typeNameOffset = typeReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)typeNameOffset);
			result.TypeName = nameReader.ReadString();
			//I suspect this is object type, 1 for numeric, 2 for object, 3 for struct
			result.EffectVariableType = (EffectVariableType)typeReader.ReadUInt32();
			result.ElementCount = typeReader.ReadUInt32();
			result.UnpackedSize = typeReader.ReadUInt32();
			result.Stride = typeReader.ReadUInt32();
			result.PackedSize = typeReader.ReadUInt32();
			if (result.EffectVariableType == EffectVariableType.Numeric)
			{
				var type = result.PackedType = typeReader.ReadUInt32();
				var numericType = result.NumericType = EffectNumericType.Parse(type);
				switch (numericType.NumericLayout)
				{
					case EffectNumericLayout.Scalar:
						result.VariableClass = ShaderVariableClass.Scalar;
						break;
					case EffectNumericLayout.Vector:
						result.VariableClass = ShaderVariableClass.Vector;
						break;
					case EffectNumericLayout.Matrix:
						result.VariableClass = type.DecodeValue(14, 14) == 1 ?
							ShaderVariableClass.MatrixColumns :
							ShaderVariableClass.MatrixRows;
						break;
				}
				switch (numericType.ScalarType)
				{
					case EffectScalarType.Float:
						result.ObjectType = EffectObjectType.Float;
						break;
					case EffectScalarType.Int:
						result.ObjectType = EffectObjectType.Int;
						break;
					case EffectScalarType.UInt:
						result.ObjectType = EffectObjectType.UInt;
						break;
					case EffectScalarType.Bool:
						result.ObjectType = EffectObjectType.Bool;
						break;
				}
			}
			else if (result.EffectVariableType == EffectVariableType.Object)
			{
				var type = result.PackedType = typeReader.ReadUInt32();
				result.VariableClass = ShaderVariableClass.Object;
				result.ObjectType = (EffectObjectType)type;
			}
			else if (result.EffectVariableType == EffectVariableType.Struct)
			{
				result.ObjectType = EffectObjectType.Void;
				result.VariableClass = ShaderVariableClass.Struct;
				result.MemberCount = typeReader.ReadUInt32();
				for (int i = 0; i < result.MemberCount; i++)
				{
					result.Members.Add(EffectMember.Parse(reader, typeReader, version));
				}
				if (version.MajorVersion == 5)
				{
					result.BaseClassType = typeReader.ReadUInt32();
					result.InterfaceCount = typeReader.ReadUInt32();
					for (int i = 0; i < result.InterfaceCount; i++)
					{
						var interfaceOffset = typeReader.ReadUInt32();
						var interfaceReader = reader.CopyAtOffset((int)interfaceOffset);
						result.InterfaceTypes.Add(EffectType.Parse(reader, interfaceReader, version));
					}
				}
			}
			else if (result.EffectVariableType == EffectVariableType.Interface)
			{
				result.VariableClass = ShaderVariableClass.InterfaceClass;
				result.ObjectType = EffectObjectType.Interface;
			}
			return result;
		}
	}
}
