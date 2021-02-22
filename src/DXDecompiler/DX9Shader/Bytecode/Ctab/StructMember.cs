using DXDecompiler.Util;

namespace DXDecompiler.DX9Shader.Bytecode.Ctab
{
	/// <summary>
	/// Refer D3DXSHADER_STRUCTMEMBERINFO d3dx9shader.h
	/// https://docs.microsoft.com/en-us/windows/win32/direct3d9/d3dxshader-structmemberinfo
	/// </summary>
	public class StructMember
	{
		public string Name;
		public ConstantType Type;
		public static StructMember Parse(BytecodeReader reader, BytecodeReader memberReader)
		{
			var result = new StructMember();
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
