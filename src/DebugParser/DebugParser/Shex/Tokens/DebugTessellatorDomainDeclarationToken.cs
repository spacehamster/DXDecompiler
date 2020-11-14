using SlimShader.Chunks.Common;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlimShader.DebugParser.Shex.Tokens
{
	class DebugTessellatorDomainDeclarationToken : DebugDeclarationToken
	{
		public TessellatorDomain Domain;

		public static DebugTessellatorDomainDeclarationToken Parse(DebugBytecodeReader reader)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			reader.AddNote("Domain", token0.DecodeValue<TessellatorDomain>(11, 12));
			return new DebugTessellatorDomainDeclarationToken
			{
				Domain = token0.DecodeValue<TessellatorDomain>(11, 12)
			};
		}
	}
}
