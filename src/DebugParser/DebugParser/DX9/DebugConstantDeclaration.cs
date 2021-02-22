using DebugParser.DebugParser.DX9;
using DXDecompiler.DX9Shader.Bytecode.Ctab;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.DX9
{
	public class DebugConstantDeclaration
	{
		public DebugConstantType Type;
		public RegisterSet RegisterSet;
		public ushort RegisterIndex;
		public ushort RegisterCount;
		List<float> DefaultValue = new List<float>();
		public string Name;
		public static DebugConstantDeclaration Parse(DebugBytecodeReader reader, DebugBytecodeReader decReader)
		{
			var result = new DebugConstantDeclaration();
			var nameOffset = decReader.ReadUInt32("NameOffset");
			result.RegisterSet = decReader.ReadEnum16<RegisterSet>("RegisterSet");
			result.RegisterIndex = decReader.ReadUInt16("RegisterIndex");
			result.RegisterCount = decReader.ReadUInt16("RegisterCount");
			decReader.ReadUInt16("Reserved");
			var typeInfoOffset = decReader.ReadUInt32("TypeInfoOffset");
			var defaultValueOffset = decReader.ReadUInt32("DefaultValueOffset");

			var nameReader = reader.CopyAtOffset("NameReader", decReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");

			var typeReader = reader.CopyAtOffset("TypeReader", decReader, (int)typeInfoOffset);
			result.Type = DebugConstantType.Parse(reader, typeReader);

			if(defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				var defaultValueReader = reader.CopyAtOffset("DefaultValueReader", decReader, (int)defaultValueOffset);
				var elementCount = result.Type.GetSize() / 4;
				for(int i = 0; i < elementCount; i++)
				{
					result.DefaultValue.Add(defaultValueReader.ReadSingle($"DefaultValue {i}"));
				}
			}

			return result;
		}
	}
}
