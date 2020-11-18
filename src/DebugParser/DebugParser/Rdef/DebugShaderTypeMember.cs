namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugShaderTypeMember
	{
		private readonly uint _parentOffset;
		public string Name { get; internal set; }
		public uint Offset { get; internal set; }
		public DebugShaderType Type { get; internal set; }
		public DebugShaderTypeMember(uint parentOffset)
		{
			_parentOffset = parentOffset;
		}
		public static DebugShaderTypeMember Parse(DebugBytecodeReader reader, DebugBytecodeReader memberReader, DebugShaderVersion target,
			int indent, bool isFirst, uint parentOffset)
		{
			var nameOffset = memberReader.ReadUInt32("nameOffset");
			var nameReader = reader.CopyAtOffset("nameReader", memberReader, (int)nameOffset);
			var name = nameReader.ReadString("name");

			var memberTypeOffset = memberReader.ReadUInt32("memberTypeOffset");

			var offset = memberReader.ReadUInt32("offset");

			var memberTypeReader = reader.CopyAtOffset("memberTypeReader", memberReader, (int)memberTypeOffset);
			var memberType = DebugShaderType.Parse(reader, memberTypeReader, target, indent, isFirst, parentOffset + offset);

			return new DebugShaderTypeMember(parentOffset)
			{
				Name = name,
				Type = memberType,
				Offset = offset
			};
		}
	}
}