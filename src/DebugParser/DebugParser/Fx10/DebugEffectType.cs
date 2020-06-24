using SlimShader.Chunks.Fx10;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// Describes a effect variable type
	/// Note, has a stride of 28 bytes
	/// Based on D3D10_EFFECT_TYPE_DESC and SType
	/// </summary>
	public class DebugEffectType
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
		public List<DebugEffectMember> Members { get; private set; }
		public uint BaseClassType { get; private set; }
		public uint InterfaceCount { get; private set; }
		public List<DebugEffectType> InterfaceTypes { get; private set; }
		public uint TypeNameOffset;

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
		public DebugEffectType()
		{
			Members = new List<DebugEffectMember>();
			InterfaceTypes = new List<DebugEffectType>();
		}
		public static DebugEffectType Parse(DebugBytecodeReader reader, DebugBytecodeReader typeReader, DebugShaderVersion version)
		{
			var result = new DebugEffectType();
			var typeNameOffset = result.TypeNameOffset = typeReader.ReadUInt32("TypeNameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", typeReader, (int)typeNameOffset);
			result.TypeName = nameReader.ReadString("TypeName");
			result.EffectVariableType = typeReader.ReadEnum32<EffectVariableType>("EffectVariableType");
			result.ElementCount = typeReader.ReadUInt32("ElementCount");
			result.UnpackedSize = typeReader.ReadUInt32("UnpackedSize");
			result.Stride = typeReader.ReadUInt32("Stride");
			result.PackedSize = typeReader.ReadUInt32("PackedSize");
			if (result.EffectVariableType == EffectVariableType.Numeric)
			{
				var type = result.PackedType = typeReader.ReadUInt32("PackedType");
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
				var type = result.PackedType = typeReader.ReadUInt32("PackedType");
				result.VariableClass = ShaderVariableClass.Object;
				result.ObjectType = (EffectObjectType)type;
			}
			else if (result.EffectVariableType == EffectVariableType.Struct)
			{
				result.ObjectType = EffectObjectType.Void;
				result.VariableClass = ShaderVariableClass.Struct;
				result.MemberCount = typeReader.ReadUInt32("MemberCount");
				for (int i = 0; i < result.MemberCount; i++)
				{
					typeReader.AddIndent($"Member {i}");
					result.Members.Add(DebugEffectMember.Parse(reader, typeReader, version));
					typeReader.RemoveIndent();
				}
				if (version.MajorVersion == 5)
				{
					result.BaseClassType = typeReader.ReadUInt32("BaseClassType");
					result.InterfaceCount = typeReader.ReadUInt32("InterfaceCount");
					for (int i = 0; i < result.InterfaceCount; i++)
					{
						var interfaceOffset = typeReader.ReadUInt32($"InterfaceOffset{i}");
						var interfaceReader = reader.CopyAtOffset($"Interface{i}", typeReader, (int)interfaceOffset);
						result.InterfaceTypes.Add(DebugEffectType.Parse(reader, interfaceReader, version));
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
