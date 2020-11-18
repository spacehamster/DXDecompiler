using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugShaderVariable
	{
		public DebugShaderTypeMember Member { get; private set; }
		public string Name
		{
			get { return Member.Name; }
		}
		public uint StartOffset
		{
			get { return Member.Offset; }
		}
		public DebugShaderType ShaderType
		{
			get { return Member.Type; }
		}
		public string BaseType { get; private set; }
		public uint Size { get; private set; }
		public ShaderVariableFlags Flags { get; private set; }
		public List<Number> DefaultValue { get; private set; }
		public int StartTexture { get; private set; }
		public int TextureSize { get; private set; }
		public int StartSampler { get; private set; }
		public int SamplerSize { get; private set; }
		internal static DebugShaderVariable Parse(DebugBytecodeReader reader, 
				DebugBytecodeReader variableReader, DebugShaderVersion target, bool isFirst)
		{
			uint nameOffset = variableReader.ReadUInt32("nameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", variableReader, (int)nameOffset);
			var name = nameReader.ReadString("name");

			var startOffset = variableReader.ReadUInt32("startOffset");
			uint size = variableReader.ReadUInt32("size");
			var flags = variableReader.ReadEnum32<ShaderVariableFlags>("flags");

			var typeOffset = variableReader.ReadUInt32("typeOffset");
			var typeReader = reader.CopyAtOffset("typeReader", variableReader, (int)typeOffset);
			var shaderType = DebugShaderType.Parse(reader, typeReader, target, 2, isFirst, startOffset);

			var defaultValueOffset = variableReader.ReadUInt32("defaultValueOffset");
			List<Number> defaultValue = null;
			if (defaultValueOffset != 0)
			{
				defaultValue = new List<Number>();
				var defaultValueReader = reader.CopyAtOffset("defaultValueReader", variableReader, (int)defaultValueOffset);
				if (size % 4 != 0)
					throw new ParseException("Can only deal with 4-byte default values at the moment.");
				for (int i = 0; i < size; i += 4)
					defaultValue.Add(new Number(defaultValueReader.ReadBytes($"defaultValue{i}", 4)));
			}


			var result = new DebugShaderVariable
			{
				DefaultValue = defaultValue,
				Member = new DebugShaderTypeMember(0)
				{
					Name = name,
					Offset = startOffset,
					Type = shaderType
				},
				BaseType = name,
				Size = size,
				Flags = flags
			};

			if (target.MajorVersion >= 5 || target.ProgramType == ProgramType.LibraryShader)
			{
				result.StartTexture = variableReader.ReadInt32("startTexture");
				result.TextureSize = variableReader.ReadInt32("textureSize");
				result.StartSampler = variableReader.ReadInt32("startSampler");
				result.SamplerSize = variableReader.ReadInt32("samplerSize");
			}

			return result;
		}
	}
}