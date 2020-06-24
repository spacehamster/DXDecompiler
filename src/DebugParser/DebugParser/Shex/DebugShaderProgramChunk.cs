using SlimShader.Chunks.Shex;
using SlimShader.DebugParser.Shex.Tokens;
using SlimShader.Util;
using System;
using System.Collections.Generic;

namespace SlimShader.DebugParser.Shex
{
	public class DebugShaderProgramChunk : DebugBytecodeChunk
	{
		public DebugShaderVersion Version { get; private set; }
		public uint Length { get; private set; }
		public List<DebugOpcodeToken> Tokens { get; private set; }

		public DebugShaderProgramChunk()
		{
			Tokens = new List<DebugOpcodeToken>();
		}
		public static DebugShaderProgramChunk Parse(DebugBytecodeReader reader)
		{
			var program = new DebugShaderProgramChunk
			{
				Version = DebugShaderVersion.ParseShex(reader),
				Length = reader.ReadUInt32("Length")
			};
			while (!reader.EndOfBuffer)
			{
				var opcodeIndex = program.Tokens.Count;
				var opcodeToken0 = reader.PeakUint32();
				var opcodeHeader = new DebugOpcodeHeader
				{
					OpcodeType = opcodeToken0.DecodeValue<OpcodeType>(0, 10),
					Length = opcodeToken0.DecodeValue(24, 30),
					IsExtended = (opcodeToken0.DecodeValue(31, 31) == 1)
				};
				DebugOpcodeToken opcodeToken = null;
				if(opcodeHeader.Length == 0 && opcodeHeader.OpcodeType != OpcodeType.CustomData)
				{
					throw new Exception("Error parsing shader");
				}
				if (opcodeHeader.OpcodeType == OpcodeType.CustomData)
				{
					//opcodeToken = DebugCustomDataToken.Parse(reader, opcodeToken0);
					var customDataClass = opcodeToken0.DecodeValue<CustomDataClass>(11, 31);
					var length = reader.PeakUInt32Ahead(4);
					if (length == 0)
					{
						throw new Exception("Error parsing shader");
					}
					var data = reader.ReadBytes($"Opcode{opcodeIndex}({opcodeHeader.OpcodeType}-{customDataClass})", (int)length * 4);
				}
				else if (opcodeHeader.OpcodeType.IsDeclaration())
				{
					var data = reader.ReadBytes($"Opcode{opcodeIndex}({opcodeHeader.OpcodeType})", (int)opcodeHeader.Length * 4);
				}
				else // Not custom data or declaration, so must be instruction.
				{
					var data = reader.ReadBytes($"Opcode{opcodeIndex}({opcodeHeader.OpcodeType})", (int)opcodeHeader.Length * 4);
				}
				program.Tokens.Add(opcodeToken);
			}
			return program;
		}
	}
}
