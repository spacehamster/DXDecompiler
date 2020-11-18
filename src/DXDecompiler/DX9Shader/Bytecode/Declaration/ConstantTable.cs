using DXDecompiler.Util;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.Bytecode.Declaration
{
	public class ConstantTable
	{
		public string Creator { get; private set; }
		public string VersionString { get; private set; }
		public byte MajorVersion { get; private set; }
		public byte MinorVersion { get; private set; }
		public List<ConstantDeclaration> ConstantDeclarations { get; private set; }
		//TODO: Remove
		public ConstantTable(string creator, string versionString, byte majorVersion, byte minorVersion, List<ConstantDeclaration> constantDeclarations)
		{
			Creator = creator;
			VersionString = versionString;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			ConstantDeclarations = constantDeclarations;
		}
		private ConstantTable()
		{
			ConstantDeclarations = new List<ConstantDeclaration>();
		}
		public static ConstantTable Parse(BytecodeReader ctabReader)
		{
			var result = new ConstantTable();
			var ctabSize = ctabReader.ReadUInt32();
			var creatorOffset = ctabReader.ReadInt32();
			result.MajorVersion = ctabReader.ReadByte();
			result.MinorVersion = ctabReader.ReadByte();
			var shaderType = (ShaderType)ctabReader.ReadUInt16();
			var numConstants = ctabReader.ReadInt32();
			var constantInfoOffset = ctabReader.ReadInt32();
			var shaderFlags = (ShaderFlags)ctabReader.ReadUInt32();
			var shaderModelOffset = ctabReader.ReadInt32();

			for(int i = 0; i < numConstants; i++)
			{
				var decReader = ctabReader.CopyAtOffset(constantInfoOffset + i * 20);
				ConstantDeclaration declaration = ConstantDeclaration.Parse(ctabReader, decReader);
				result.ConstantDeclarations.Add(declaration);
			}

			var shaderModelReader = ctabReader.CopyAtOffset(shaderModelOffset);
			result.VersionString = shaderModelReader.ReadString();

			var creatorReader = ctabReader.CopyAtOffset(creatorOffset);
			result.Creator = creatorReader.ReadString();
			return result;
		}

		public string GetVariable(uint elementIndex)
		{
			var decl = ConstantDeclarations
				.FirstOrDefault(d => d.ContainsIndex((int)elementIndex));
			if(decl.ParameterClass == ParameterClass.MatrixColumns ||
				decl.ParameterClass == ParameterClass.MatrixRows ||
				decl.ParameterClass == ParameterClass.Struct ||
				decl.Elements > 1)
			{
				var arrayIndex = elementIndex - decl.RegisterIndex;
				return $"{decl.Name}[{arrayIndex}]";
			}
			if(decl == null)
			{
				return string.Format("var{0}", elementIndex);
			}
			return decl.Name;
		}
	}
}
