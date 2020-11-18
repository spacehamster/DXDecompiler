using DXDecompiler.DX9Shader;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.DX9
{
	public class DebugConstantTable
	{
		public List<DebugConstantDeclaration> ConstantDeclarations = new List<DebugConstantDeclaration>();
		public static DebugConstantTable Parse(DebugBytecodeReader reader)
		{
			var result = new DebugConstantTable();

			var ctabReader = reader.CopyAtCurrentPosition("CTabReader", reader);
			var ctabSize = ctabReader.ReadUInt32("CtabSize");
			var creatorOffset = ctabReader.ReadInt32("CreatorPosition");
			var minorVersion = ctabReader.ReadByte("MinorVersion");
			var majorVersion = ctabReader.ReadByte("MajorVersion");
			var shaderType = ctabReader.ReadEnum16<ShaderType>("ShaderType");
			var numConstants = ctabReader.ReadInt32("NumConstants");
			var constantInfoOffset = ctabReader.ReadInt32("ConstantInfoOffset");
			var shaderFlags = ctabReader.ReadEnum32<ShaderFlags>("ShaderFlags");
			var shaderModelOffset = ctabReader.ReadInt32("ShaderModelOffset");

			for (int i = 0; i < numConstants; i++)
			{
				var decReader = ctabReader.CopyAtOffset($"Declaration {i}", ctabReader, constantInfoOffset + i * 20);
				DebugConstantDeclaration declaration = DebugConstantDeclaration.Parse(ctabReader, decReader);
				result.ConstantDeclarations.Add(declaration);
			}

			var shaderModelReader = ctabReader.CopyAtOffset("ShaderModelReader", ctabReader, shaderModelOffset);
			var shaderModel = shaderModelReader.ReadString("ShaderModel");

			var creatorReader = ctabReader.CopyAtOffset("CreaterReader", ctabReader, creatorOffset);
			var creatorString = creatorReader.ReadString("CreatorString");
			return result;
		}
	}
}
