﻿using DXDecompiler.DX9Shader.Bytecode;
using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.DX9Shader.Bytecode.Fxlvm;
using DXDecompiler.DX9Shader.FX9;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DXDecompiler.DX9Shader
{

	public enum ShaderType
	{
		Vertex = 0xFFFE,
		Pixel = 0xFFFF,
		Effect = 0xFEFF,
		/// <summary>
		/// Represents a Fxlc expression chunk that contains FXLVM tokens
		/// </summary>
		Expression = 0x4658,
		Lib4Vertex = 0x4c56,
		Lib4Pixel = 0x4c50
	}

	public class ShaderModel
	{
		static ShaderModel()
		{
			System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
		}

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
		public EffectContainer EffectChunk { get; set; }
		public IList<Token> Tokens { get; private set; }
		public ConstantTable ConstantTable { get; private set; }
		public FxlcBlock Fxlc { get; set; }
		public CliToken Cli { get; set; }
		public Preshader Preshader { get; set; }
		public PrsiToken Prsi { get; set; }
		public IEnumerable<InstructionToken> Instructions => Tokens.OfType<InstructionToken>();

		public string Profile
		{
			get
			{
				var version = $"{MajorVersion}_{MinorVersion}";
				version = version switch
				{
					"2_1" => "2_a",
					"2_255" => "2_sw",
					"3_255" => "3_sw",
					_ => version
				};
				return $"{Type.GetDescription()}_{version}";
			}
		}

		public ShaderModel(int majorVersion, int minorVersion, ShaderType type)
		{
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			Type = type;

			Tokens = new List<Token>();
		}
		private ShaderModel()
		{
			Tokens = new List<Token>();
		}
		/*
		 * There are a few token types:
		 * comment_token fourCC data
		 * def_token dest_param literal_param literal_param literal_param literal_param
		 * dcl_token decl_param dest_param
		 * inst_token dest_param [src_param ...]
		 * end_token
		 */
		public static ShaderModel Parse(BytecodeReader reader)
		{
			var result = new ShaderModel();
			result.MinorVersion = reader.ReadByte();
			result.MajorVersion = reader.ReadByte();
			result.Type = (ShaderType)reader.ReadUInt16();
			while(true)
			{
				var instruction = result.ReadInstruction(reader);
				if(instruction == null) continue;
				result.Tokens.Add(instruction);
				if(instruction.Opcode == Opcode.End) break;
			}
			return result;
		}
		Token ReadInstruction(BytecodeReader reader)
		{
			uint instructionToken = reader.ReadUInt32();
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			int size;
			if(opcode == Opcode.Comment)
			{
				size = (int)((instructionToken >> 16) & 0x7FFF);
			}
			else
			{
				if(MajorVersion > 1)
				{
					size = (int)((instructionToken >> 24) & 0x0f);
				}
				else
				{
					size = opcode.GetShaderModel1OpcodeSize(MinorVersion);
				}
			}
			Token token = null;
			if(opcode == Opcode.Comment)
			{
				var fourCC = reader.ReadUInt32();
				if(KnownCommentTypes.ContainsKey(fourCC))
				{
					var commentReader = reader.CopyAtCurrentPosition();
					reader.ReadBytes(size * 4 - 4);
					switch(KnownCommentTypes[fourCC])
					{
						case CommentType.CTAB:
							ConstantTable = ConstantTable.Parse(commentReader);
							return null;
						case CommentType.CLIT:
							Cli = CliToken.Parse(commentReader);
							return null;
						case CommentType.FXLC:
							Fxlc = FxlcBlock.Parse(commentReader);
							return null;
						case CommentType.PRES:
							if(size > 1)
							{
								Preshader = Preshader.Parse(commentReader);
							}
							return null;
						case CommentType.PRSI:
							Prsi = PrsiToken.Parse(commentReader);
							return null;
					}
				}
				token = new CommentToken(opcode, size, this);
				token.Data[0] = fourCC;
				for(int i = 1; i < size; i++)
				{
					token.Data[i] = reader.ReadUInt32();
				}
			}
			else
			{
				token = new InstructionToken(opcode, size, this);
				var inst = token as InstructionToken;

				for(int i = 0; i < size; i++)
				{
					token.Data[i] = reader.ReadUInt32();
					if(opcode == Opcode.Def || opcode == Opcode.DefB || opcode == Opcode.DefI)
					{
						if(i == 0)
						{
							inst.Operands.Add(new DestinationOperand(token.Data[i]));
						}
					}
					else if(opcode == Opcode.Dcl)
					{
						if(i == 0)
						{
							inst.Operands.Add(new DeclarationOperand(token.Data[i]));
						}
						else
						{
							inst.Operands.Add(new DestinationOperand(token.Data[i]));
						}
					}
					else if(i == 0 && opcode.HasDestination())
					{
						if((token.Data[i] & (1 << 13)) != 0)
						{
							//Relative Address mode
							if(MajorVersion < 2)
							{
								// special handling for SM1 shaders
								token.AddData();
								token.Data[i + 1] = 0xB0000000;
								size++;
							}
							else
							{
								token.Data[i + 1] = reader.ReadUInt32();
							}
							inst.Operands.Add(new DestinationOperand(token.Data[i], token.Data[i + 1]));
							i++;
						}
						else
						{
							inst.Operands.Add(new DestinationOperand(token.Data[i]));
						}
					}
					else if((token.Data[i] & (1 << 13)) != 0)
					{
						//Relative Address mode
						if(MajorVersion < 2)
						{
							// special handling for SM1 shaders
							token.AddData();
							token.Data[i + 1] = 0xB0000000;
							size++;
						}
						else
						{
							token.Data[i + 1] = reader.ReadUInt32();
						}
						inst.Operands.Add(new SourceOperand(token.Data[i], token.Data[i + 1]));
						i++;
					}
					else
					{
						inst.Operands.Add(new SourceOperand(token.Data[i]));
					}
				}
				if(opcode != Opcode.Comment)
				{
					token.Modifier = (int)((instructionToken >> 16) & 0xff);
					token.Predicated = (instructionToken & 0x10000000) != 0;
					token.CoIssue = (instructionToken & 0x40000000) != 0;
					Debug.Assert((instructionToken & 0xA0000000) == 0, $"Instruction has unexpected bits set {instructionToken & 0xE0000000}");
				}
			}
			return token;
		}
	}
}
