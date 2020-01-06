using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace SlimShader.DX9Shader
{

	public enum ShaderType
	{
		Vertex = 0xFFFE,
		Pixel = 0xFFFF
	}

	public class ShaderModel
	{
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public ShaderType Type { get; private set; }

		public IList<Token> Tokens { get; private set; }
		public ConstantTable ConstantTable { get; private set; }
		public IEnumerable<InstructionToken> Instructions => Tokens.OfType<InstructionToken>();

		public ShaderModel(int majorVersion, int minorVersion, ShaderType type)
		{
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			Type = type;

			Tokens = new List<Token>();
		}

		static string ReadStringNullTerminated(Stream stream)
		{
			StringBuilder builder = new StringBuilder();
			char b;
			while ((b = (char)stream.ReadByte()) != 0)
			{
				builder.Append(b.ToString());
			}
			return builder.ToString();
		}

		internal ConstantTable ParseConstantTable()
		{
			var constantDeclarations = new List<ConstantDeclaration>();

			byte[] constantTable = GetConstantTableData();
			if (constantTable == null)
			{
				return null;
			}

			var ctabStream = new MemoryStream(constantTable);
			using (var ctabReader = new BinaryReader(ctabStream))
			{
				int ctabSize = ctabReader.ReadInt32();
				System.Diagnostics.Debug.Assert(ctabSize == 0x1C);
				long creatorPosition = ctabReader.ReadInt32();

				int minorVersion = ctabReader.ReadByte();
				int majorVersion = ctabReader.ReadByte();
				System.Diagnostics.Debug.Assert(majorVersion == MajorVersion);
				System.Diagnostics.Debug.Assert(minorVersion == MinorVersion);

				var shaderType = (ShaderType)ctabReader.ReadUInt16();
				System.Diagnostics.Debug.Assert(shaderType == Type);

				int numConstants = ctabReader.ReadInt32();
				long constantInfoPosition = ctabReader.ReadInt32();
				ShaderFlags shaderFlags = (ShaderFlags)ctabReader.ReadInt32();

				long shaderModelPosition = ctabReader.ReadInt32();
				//Console.WriteLine("ctabStart = {0}, shaderModelPosition = {1}", ctabStart, shaderModelPosition);


				ctabStream.Position = creatorPosition;
				string compilerInfo = ReadStringNullTerminated(ctabStream);

				ctabStream.Position = shaderModelPosition;
				string shaderModel = ReadStringNullTerminated(ctabStream);


				for (int i = 0; i < numConstants; i++)
				{
					ctabStream.Position = constantInfoPosition + i * 20;
					ConstantDeclaration declaration = ReadConstantDeclaration(ctabReader);
					constantDeclarations.Add(declaration);
				}
				var ct = new ConstantTable(compilerInfo, shaderModel, majorVersion, minorVersion, constantDeclarations);
				ConstantTable = ct;
				return ConstantTable;
			}
		}

		private byte[] GetConstantTableData()
		{
			int ctabToken = FourCC.Make("CTAB");
			var ctabComment = Tokens.FirstOrDefault(x => x.Opcode == Opcode.Comment && x.Data[0] == ctabToken);
			if (ctabComment == null)
			{
				return null;
			}

			byte[] constantTable = new byte[ctabComment.Data.Length * 4];
			for (int i = 1; i < ctabComment.Data.Length; i++)
			{
				constantTable[i * 4 - 4] = (byte)(ctabComment.Data[i] & 0xFF);
				constantTable[i * 4 - 3] = (byte)((ctabComment.Data[i] >> 8) & 0xFF);
				constantTable[i * 4 - 2] = (byte)((ctabComment.Data[i] >> 16) & 0xFF);
				constantTable[i * 4 - 1] = (byte)((ctabComment.Data[i] >> 24) & 0xFF);
			}

			return constantTable;
		}

		private ConstantDeclaration ReadConstantDeclaration(BinaryReader ctabReader)
		{
			var ctabStream = ctabReader.BaseStream;

			// D3DXSHADER_CONSTANTINFO
			int nameOffset = ctabReader.ReadInt32();
			RegisterSet registerSet = (RegisterSet)ctabReader.ReadInt16();
			short registerIndex = ctabReader.ReadInt16();
			short registerCount = ctabReader.ReadInt16();
			ctabStream.Position += sizeof(short); // Reserved
			int typeInfoOffset = ctabReader.ReadInt32();
			int defaultValueOffset = ctabReader.ReadInt32();
			List<float> defaultValue = new List<float>(); ;

			ctabStream.Position = nameOffset;
			string name = ReadStringNullTerminated(ctabStream);
			
			if(defaultValueOffset != 0)
			{
				//Note: thre are corrisponding def instructions. TODO: check that they are the same
				ctabStream.Position = defaultValueOffset;
				for(int i = 0; i < 4; i++)
				{
					defaultValue.Add(ctabReader.ReadSingle());
				}
			}

			// D3DXSHADER_TYPEINFO
			ctabStream.Position = typeInfoOffset;
			ParameterClass cl = (ParameterClass)ctabReader.ReadInt16();
			ParameterType type = (ParameterType)ctabReader.ReadInt16();
			short rows = ctabReader.ReadInt16();
			short columns = ctabReader.ReadInt16();
			short numElements = ctabReader.ReadInt16();
			short numStructMembers = ctabReader.ReadInt16();
			int structMemberInfoOffset = ctabReader.ReadInt32();
			//System.Diagnostics.Debug.Assert(numElements == 1);
			System.Diagnostics.Debug.Assert(structMemberInfoOffset == 0);

			return new ConstantDeclaration(name, registerSet, registerIndex, registerCount, cl, type, rows, columns, numElements, defaultValue);
		}
	}
}
