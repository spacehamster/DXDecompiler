using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader.Bytecode.Declaration
{
	public class ConstantDeclaration
	{
		public string Name { get; private set; }
		public RegisterSet RegisterSet { get; private set; }
		public ushort RegisterIndex { get; private set; }
		public ushort RegisterCount { get; private set; }
		public ConstantType Type { get; private set; }
		public List<float> DefaultValue { get; private set; }

		public uint Rows => Type.Rows;
		public uint Columns => Type.Columns;
		public ParameterType ParameterType => Type.ParameterType;
		public ParameterClass ParameterClass => Type.ParameterClass;
		public uint Elements => Type.Elements;

		public ConstantDeclaration(string name, RegisterSet registerSet, short registerIndex, short registerCount,
			ParameterClass parameterClass, ParameterType parameterType, int rows, int columns, int elements, List<float> defaultValue)
		{
			Name = name;
			RegisterSet = registerSet;
			RegisterIndex = (ushort)registerIndex;
			RegisterCount = (ushort)registerCount;
			Type = new ConstantType()
			{
				ParameterClass = parameterClass,
				ParameterType = parameterType,
				Rows = (uint)rows,
				Columns = (uint)columns,
				Elements = (uint)elements,
			};
			DefaultValue = defaultValue;
		}
		public ConstantDeclaration()
		{
			DefaultValue = new List<float>();
		}
		public static ConstantDeclaration Parse(BytecodeReader reader, BytecodeReader decReader)
		{
			var result = new ConstantDeclaration();
			var nameOffset = decReader.ReadUInt32();
			result.RegisterSet = (RegisterSet)decReader.ReadUInt16();
			result.RegisterIndex = decReader.ReadUInt16();
			result.RegisterCount = decReader.ReadUInt16();
			decReader.ReadUInt16(); //Reserved
			var typeInfoOffset = decReader.ReadUInt32();
			var defaultValueOffset = decReader.ReadUInt32();

			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			var typeReader = reader.CopyAtOffset((int)typeInfoOffset);
			result.Type = ConstantType.Parse(reader, typeReader);
			var memberInfoOffset = typeReader.ReadUInt32();

			if (defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset((int)defaultValueOffset);
				for (int i = 0; i < 4; i++)
				{
					result.DefaultValue.Add(defaultValueReader.ReadSingle());
				}
			}
			return result;
		}

		public bool ContainsIndex(int index)
		{
			return (index >= RegisterIndex) && (index < RegisterIndex + RegisterCount);
		}

		public override string ToString()
		{
			return Name;
		}
		public string GetTypeName()
		{
			return Type.GetTypeName();
		}
		public string GetRegisterName()
		{
			return $"{RegisterSet.GetDescription()}{RegisterIndex}";
		}
	}
}
