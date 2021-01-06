using DebugParser.DebugParser.DX9;
using DXDecompiler.DX9Shader;
using DXDecompiler.DX9Shader.Bytecode;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DebugParser.DX9
{
	public class DebugShaderModel
	{
		private static readonly Dictionary<uint, CommentType> KnownCommentTypes =
			new Dictionary<uint, CommentType>
		{
			{ "CTAB".ToFourCc(), CommentType.CTAB },
			{ "CLIT".ToFourCc(), CommentType.CLIT },
			{ "FXLC".ToFourCc(), CommentType.FXLC },
			{ "PRES".ToFourCc(), CommentType.PRES },
			{ "PRSI".ToFourCc(), CommentType.PRSI }
		};
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public ShaderType Type { get; private set; }
		public List<IDebugToken> Tokens = new List<IDebugToken>();
		public DebugConstantTable ConstantTable;
		public DebugCliToken Cli;
		public DebugPreshader Preshader;
		public DebugPrsiToken Prsi;
		public DebugFxlc Fxlc;
		public static DebugShaderModel Parse(DebugBytecodeReader reader)
		{
			var result = new DebugShaderModel();
			result.MinorVersion = reader.ReadByte("MinorVersion");
			result.MajorVersion = reader.ReadByte("MajorVersion");
			result.Type = reader.ReadEnum16<ShaderType>("ShaderType");
			while(true)
			{
				var token = reader.PeakUint32();
				Opcode opcode = (Opcode)(token & 0xffff);
				if(opcode == Opcode.Comment && result.ReadCommentToken(reader))
				{
					continue;
				}
				reader.AddIndent($"T{result.Tokens.Count}");

				var indent = reader.LocalMembers.OfType<DebugIndent>().Last();
				IDebugToken instruction = result.ReadInstruction(reader);
				result.Tokens.Add(instruction);
				indent.Name += $" {instruction.Opcode} {string.Join(" ", instruction.Operands)}";
				reader.RemoveIndent();
				if(instruction.Opcode == Opcode.End) break;
			}
			return result;
		}
		bool ReadCommentToken(DebugBytecodeReader reader)
		{
			var fourCC = reader.PeakUInt32Ahead(4);
			if(KnownCommentTypes.ContainsKey(fourCC))
			{
				reader.AddIndent(KnownCommentTypes[fourCC].ToString());
			}
			else
			{
				return false;
			}
			var instructionToken = reader.ReadUInt32("Token");
			var startPosition = reader._reader.BaseStream.Position;
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			reader.AddNote("TokenOpcode", opcode);
			var size = (int)((instructionToken >> 16) & 0x7FFF);
			reader.AddNote("TokenSize", size);
			reader.ReadBytes("FourCC", 4);

			switch(KnownCommentTypes[fourCC])
			{
				case CommentType.CTAB:
					ConstantTable = DebugConstantTable.Parse(reader);
					break;
				case CommentType.CLIT:
					Cli = DebugCliToken.Parse(reader);
					break;
				case CommentType.FXLC:
					Fxlc = DebugFxlc.Parse(reader, (uint)size * 4);
					break;
				case CommentType.PRES:
					Preshader = DebugPreshader.Parse(reader);
					break;
				case CommentType.PRSI:
					Prsi = DebugPrsiToken.Parse(reader, (uint)size);
					break;
				default:
					return false;
			}
			reader.RemoveIndent();
			reader._reader.BaseStream.Position = startPosition + size * 4;
			return true;
		}
		IDebugToken ReadInstruction(DebugBytecodeReader reader)
		{
			uint instructionToken = reader.ReadUInt32("Token");
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			uint size;
			if(opcode == Opcode.Comment)
			{
				size = (uint)((instructionToken >> 16) & 0x7FFF);
			}
			else
			{
				size = (uint)((instructionToken >> 24) & 0x0f);
			}
			var entry = reader.LocalMembers.Last();
			entry.AddNote("TokenOpcode", opcode.ToString());
			entry.AddNote("TokenSize", size.ToString());
			IDebugToken result;
			if(opcode == Opcode.Comment)
			{
				var token = new DebugToken();
				token.Token = instructionToken;
				token.Opcode = opcode;
				token.Data = reader.ReadBytes("CommentData", (int)size * 4);
				result = token;
			}
			else
			{
				result = DebugInstructionToken.Parse(reader, instructionToken, opcode, size);
			}
			if(opcode != Opcode.Comment)
			{
				var token = result as DebugInstructionToken;
				token.Modifier = (int)((instructionToken >> 16) & 0xff);
				token.Predicated = (instructionToken & 0x10000000) != 0;
				token.CoIssue = (instructionToken & 0x40000000) != 0;
				entry.AddNote("Modifer", token.Modifier);
				entry.AddNote("Predicated", token.Predicated);
				entry.AddNote("CoIssue", token.CoIssue);
			}
			return result;
		}
	}
}
