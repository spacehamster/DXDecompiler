using System;
using System.Collections.Generic;
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

			var shader = new ShaderModel(majorVersion, minorVersion, shaderType);

			while (true)
			{
				Token instruction = ReadInstruction(shader);
				InstructionVerifier.Verify(instruction);
				shader.Instructions.Add(instruction);
				if (instruction.Opcode == Opcode.End) break;
			}

			return shader;
		}

		private Token ReadInstruction(ShaderModel shaderModel)
		{

			uint instructionToken = ReadUInt32();
			Opcode opcode = (Opcode)(instructionToken & 0xffff);
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
					inst.Params.Add(new Parameter(token.Data[i]));
				}
			}

			if (opcode != Opcode.Comment)
			{
				token.Modifier = (int)((instructionToken >> 16) & 0xff);
				token.Predicated = (instructionToken & 0x10000000) != 0;
				System.Diagnostics.Debug.Assert((instructionToken & 0xE0000000) == 0);
			}

			return token;
		}
	}
}
