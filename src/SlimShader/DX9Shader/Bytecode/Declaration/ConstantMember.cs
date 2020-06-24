using SlimShader.Util;

namespace SlimShader.DX9Shader.Bytecode.Declaration
{
	public class ConstantMember
	{
		public string Name;
		public ConstantType Type;
		public static ConstantMember Parse(BytecodeReader reader, BytecodeReader memberReader)
		{
			var result = new ConstantMember();
			var nameOffset = memberReader.ReadUInt32();
			var typeOffset = memberReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var typeReader = reader.CopyAtOffset((int)typeOffset);
			result.Type = ConstantType.Parse(reader, typeReader);
			return result;
		}
	}
}
