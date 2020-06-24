using SlimShader.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class ShaderReader : BinaryReader
	{
		public ShaderReader(Stream input, bool leaveOpen = false)
			: base(input, new UTF8Encoding(false, true))
		{
		}
		static public ShaderModel ReadShader(byte[] data)
		{
			byte minorVersion = data[0];
			byte majorVersion = data[1];
			ShaderType shaderType = (ShaderType)BitConverter.ToUInt16(data, 2);
			if (shaderType == ShaderType.Effect)
			{
				var _shader = new ShaderModel(majorVersion, minorVersion, shaderType);
				var bytecodeReader = new BytecodeReader(data, 4, data.Length - 4);
				_shader.EffectChunk = FX9.EffectContainer.Parse(bytecodeReader, (uint)(data.Length - 4));
				return _shader;
			}
			var reader = new BytecodeReader(data, 0, data.Length);
			return ShaderModel.Parse(reader);
		}

		private Token ReadInstruction(ShaderModel shaderModel)
		{

			uint instructionToken = ReadUInt32();
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
			Debug.Assert(opcode <= Opcode.Breakp || (opcode >= Opcode.Phase && opcode <= Opcode.End), $"Invalid opcode {opcode}");
			int size;
			if (opcode == Opcode.Comment)
			{
				size = (int)((instructionToken >> 16) & 0x7FFF);
			}
			else
			{
				size = (int)((instructionToken >> 24) & 0x0f);
			}
			Token token = null;
			if(opcode == Opcode.Comment)
			{
				token = new CommentToken(opcode, size, shaderModel);
				for (int i = 0; i < size; i++)
				{
					token.Data[i] = ReadUInt32();
				}
			} else
			{
				token = new InstructionToken(opcode, size, shaderModel);
				var inst = token as InstructionToken;
				for (int i = 0; i < size; i++)
				{
					token.Data[i] = ReadUInt32();
					if(opcode == Opcode.Def || opcode == Opcode.DefB || opcode == Opcode.DefI)
					{

					}
					else if(opcode == Opcode.Dcl)
					{
						if (i == 0)
						{
							inst.Operands.Add(new DeclarationOperand(token.Data[i]));
						} else
						{
							inst.Operands.Add(new DestinationOperand(token.Data[i]));
						}
					} else if(i == 0 && opcode != Opcode.BreakC && opcode != Opcode.IfC && opcode != Opcode.If)
					{
						inst.Operands.Add(new DestinationOperand(token.Data[i]));
					} else if ((token.Data[i] & (1 << 13)) != 0)
					{
						//Relative Address mode
						token.Data[i+1] = ReadUInt32();
						inst.Operands.Add(new SourceOperand(token.Data[i], token.Data[i+1]));
						i++;
					} else
					{
						inst.Operands.Add(new SourceOperand(token.Data[i]));
					}
				}
			}

			if (opcode != Opcode.Comment)
			{
				token.Modifier = (int)((instructionToken >> 16) & 0xff);
				token.Predicated = (instructionToken & 0x10000000) != 0;
				token.CoIssue = (instructionToken & 0x40000000) != 0;
				Debug.Assert((instructionToken & 0xA0000000) == 0, $"Instruction has unexpected bits set {instructionToken & 0xE0000000}");
			}

			return token;
		}
	}
}
