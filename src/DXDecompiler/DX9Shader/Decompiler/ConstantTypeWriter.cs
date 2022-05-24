using DXDecompiler.DX9Shader.Bytecode.Ctab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.Decompiler
{
	public class DecompiledConstantDeclaration
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public Dictionary<string, string> RegisterAssignments { get; } = new(); // example: key = "ps_2_0", value = "c0"
		public string DefaultValue { get; set; }

		public string RegisterAssignmentString =>
			string.Concat(RegisterAssignments.Select(t => $" : register({t.Key}, {t.Value})"));
	}

	class ConstantTypeWriter : DecompileWriter
	{
		private readonly ConstantType _initialType;
		private readonly string _initialName;
		private readonly bool _addTrailingSemicolon;
		private readonly bool _printMatrixOrientation;

		public static string Decompile(ConstantType type, string name, bool isHlsl, int indent = 0)
		{
			var decompiler = new ConstantTypeWriter(type, name, !isHlsl, isHlsl)
			{
				Indent = indent
			};
			return decompiler.Decompile();
		}
		public static DecompiledConstantDeclaration Decompile(ConstantDeclaration declaration, ShaderModel shader)
		{

			var defaultValue = declaration.DefaultValue.All(v => v == 0)
				? null
				: string.Format("{{ {0} }}", string.Join(", ", declaration.DefaultValue));
			var decompiled = new DecompiledConstantDeclaration
			{
				Name = declaration.Name,
				Code = Decompile(declaration.Type, declaration.Name.TrimStart('$'), true),
				DefaultValue = defaultValue,
			};
			if(shader.Type != ShaderType.Expression)
			{
				var register = declaration.RegisterSet.GetDescription() + declaration.RegisterIndex;
				decompiled.RegisterAssignments[shader.Profile] = register;
			}
			return decompiled;
		}
		private ConstantTypeWriter(ConstantType initialType, string initialName, bool addTrailingSemicolon, bool printMatrixOrientation)
		{
			_initialType = initialType;
			_initialName = initialName;
			_addTrailingSemicolon = addTrailingSemicolon;
			_printMatrixOrientation = printMatrixOrientation;
		}
		protected override void Write()
		{
			Write(_initialType, _initialName, _addTrailingSemicolon);
		}
		private void Write(ConstantType type, string name, bool addSemicolon = false)
		{
			string typeName = GetConstantTypeName(type);
			WriteIndent();
			Write("{0}", typeName);
			if(type.ParameterClass == ParameterClass.Struct)
			{
				WriteLine(string.Empty);
				WriteIndent();
				WriteLine("{");
				Indent++;
				foreach(var member in type.Members)
				{
					Write(member.Type, member.Name, true);
				}
				Indent--;
				WriteIndent();
				Write("}");
			}
			Write(" {0}", name);
			if(type.Elements > 1)
			{
				Write("[{0}]", type.Elements);
			}
			if(addSemicolon)
			{
				WriteLine(";");
			}
		}
		private string GetConstantTypeName(ConstantType type)
		{
			switch(type.ParameterClass)
			{
				case ParameterClass.Scalar:
					return type.ParameterType.GetDescription();
				case ParameterClass.Vector:
					return type.ParameterType.GetDescription() + type.Columns;
				case ParameterClass.Struct:
					return "struct";
				case ParameterClass.MatrixColumns:
				case ParameterClass.MatrixRows:
					var prefix = string.Empty;
					if(_printMatrixOrientation)
					{
						prefix = type.ParameterClass == ParameterClass.MatrixColumns
							? "column_major "
							: "row_major ";
					}
					return $"{prefix}{type.ParameterType.GetDescription()}{type.Rows}x{type.Columns}";
				case ParameterClass.Object:
					switch(type.ParameterType)
					{
						case ParameterType.Sampler1D:
						case ParameterType.Sampler2D:
						case ParameterType.Sampler3D:
						case ParameterType.SamplerCube:
							return type.ParameterType.GetDescription();
						default:
							throw new NotImplementedException();
					}
			}
			throw new NotImplementedException();
		}
	}
}
