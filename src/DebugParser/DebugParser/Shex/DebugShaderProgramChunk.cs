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
				reader.AddIndent(opcodeHeader.OpcodeType.ToString());
				if(opcodeHeader.Length == 0 && opcodeHeader.OpcodeType != OpcodeType.CustomData)
				{
					throw new Exception("Error parsing shader");
				}
				DebugOpcodeToken opcodeToken;
				if (opcodeHeader.OpcodeType == OpcodeType.CustomData)
				{
					opcodeToken = DebugCustomDataToken.Parse(reader, opcodeToken0);
				}
				else if (opcodeHeader.OpcodeType.IsDeclaration())
				{
					opcodeToken = DebugDeclarationToken.Parse(reader, opcodeHeader.OpcodeType, program.Version);
				}
				else // Not custom data or declaration, so must be instruction.
				{
					opcodeToken = DebugInstructionToken.Parse(reader, opcodeHeader);
				}
				program.Tokens.Add(opcodeToken);
				reader.RemoveIndent();
			}
			return program;
		}
	}
}
