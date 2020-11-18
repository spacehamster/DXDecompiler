using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;

namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugResourceBinding
	{
		public string Name { get; private set; }
		public ShaderInputType Type { get; private set; }
		public uint BindPoint { get; private set; }
		public uint BindCount { get; private set; }
		public uint ID { get; private set; }
		public ShaderInputFlags Flags { get; private set; }
		public ShaderResourceViewDimension Dimension { get; private set; }
		public ResourceReturnType ReturnType { get; private set; }
		public uint NumSamples { get; private set; }
		public uint SpaceIndex { get; private set; }

		public static DebugResourceBinding Parse(DebugBytecodeReader reader, DebugBytecodeReader resourceBindingReader, DebugShaderVersion target)
		{
			uint nameOffset = resourceBindingReader.ReadUInt32("nameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", resourceBindingReader, (int)nameOffset);
			var result = new DebugResourceBinding
			{
				Name = nameReader.ReadString("Name"),
				Type = (ShaderInputType)resourceBindingReader.ReadUInt32("Type"),
				ReturnType = (ResourceReturnType)resourceBindingReader.ReadUInt32("ReturnType"),
				Dimension = (ShaderResourceViewDimension)resourceBindingReader.ReadUInt32("Dimension"),
				NumSamples = resourceBindingReader.ReadUInt32("NumSamples"),
				BindPoint = resourceBindingReader.ReadUInt32("BindPoint"),
				BindCount = resourceBindingReader.ReadUInt32("BindCount"),
				Flags = (ShaderInputFlags)resourceBindingReader.ReadUInt32("Flags")
			};
			if(target.MajorVersion == 5 && target.MinorVersion == 1)
			{
				result.SpaceIndex = resourceBindingReader.ReadUInt32("SpaceIndex");
				result.ID = resourceBindingReader.ReadUInt32("ID");
			}
			else
			{
				result.ID = result.BindPoint;
			}
			return result;
		}
	}
}