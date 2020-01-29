using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class ShaderReader : BinaryReader
	{
		public ShaderReader(Stream input, bool leaveOpen = false)
			: base(input, new UTF8Encoding(false, true))
		{
		}
		static public ShaderModel ReadShader(Stream stream)
		{
			using (var ss = new ShaderReader(stream))
			{
				return ss.ReadShader();
			}
		}
		static public ShaderModel ReadShader(byte[] data)
		{
			using(var ms = new MemoryStream(data))
			using (var ss = new ShaderReader(ms))
			{
				return ss.ReadShader();
			}
		}
		virtual public ShaderModel ReadShader()
		{
			// Version token
			byte minorVersion = ReadByte();
			byte majorVersion = ReadByte();
			ShaderType shaderType = (ShaderType)ReadUInt16();
			if(shaderType == ShaderType.Fx)
			{
				throw new Exception("FX shaders currently not supported");
			}
			Debug.Assert(shaderType == ShaderType.Pixel || shaderType == ShaderType.Vertex || shaderType == ShaderType.Fx,
				$"Shader does not contain a valid shader type {shaderType}");
			var shader = new ShaderModel(majorVersion, minorVersion, shaderType);

			while (true)
			{
				Token instruction = ReadInstruction(shader);
				InstructionVerifier.Verify(instruction);
				shader.Tokens.Add(instruction);
				if (instruction.Opcode == Opcode.End) break;
			}
			shader.ParseConstantTable();
			return shader;
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
