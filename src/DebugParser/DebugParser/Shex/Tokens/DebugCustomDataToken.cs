using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;
using System.Linq;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public abstract class DebugCustomDataToken : DebugOpcodeToken
	{
		public CustomDataClass CustomDataClass;

		public static DebugCustomDataToken Parse(DebugBytecodeReader reader, uint token0)
		{
			DebugOpcodeHeader.AddNotes(reader, token0);
			var customDataClass = token0.DecodeValue<CustomDataClass>(11, 31);
			var member = reader.Members.Last();
			DebugCustomDataToken token;
			switch (customDataClass)
			{
				case CustomDataClass.DclImmediateConstantBuffer:
					token = DebugImmediateConstantBufferDeclarationToken.Parse(reader);
					break;
				case CustomDataClass.ShaderMessage:
					token = DebugShaderMessageDeclarationToken.Parse(reader);
					break;
				default:
					throw new ParseException("Unknown custom data class: " + customDataClass);
			}

			token.CustomDataClass = customDataClass;
			member.AddNote("CustomDataClass", customDataClass);
			return token;
		}
	}
}
