using SlimShader.Chunks.Libf;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Libf
{
	/// <summary>
	/// Describes a library function's input/output parameter
	/// Based on D3D12_PARAMETER_DESC
	/// </summary>
	public class DebugLibraryParameterDescription
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Parameter semantic name (+index).
		/// </summary>
		public string SemanticName { get; private set; }

		/// <summary>
		/// Element type.
		/// </summary>
		public ShaderVariableType VariableType { get; private set; }

		/// <summary>
		/// Scalar/Vector/Matrix.
		/// </summary>
		public ShaderVariableClass VariableClass { get; private set; }

		/// <summary>
		/// Rows are for matrix parameters.
		/// </summary>
		public uint Rows { get; private set; }

		/// <summary>
		/// Components or Columns in matrix.
		/// </summary>
		public uint Column { get; private set; }

		/// <summary>
		/// Interpolation mode.
		/// </summary>
		public InterpolationMode InterpolationMode { get; private set; }

		/// <summary>
		/// Parameter modifiers.
		/// </summary>
		public ParameterFlags Flags { get; private set; }

		/// <summary>
		/// The first input register for this parameter.
		/// </summary>
		public uint FirstInRegister { get; private set; }

		/// <summary>
		/// The first input register component for this parameter.
		/// </summary>
		public uint FirstInComponent { get; private set; }

		/// <summary>
		/// The first output register for this parameter.
		/// </summary>
		public uint FirstOutRegister { get; private set; }

		/// <summary>
		/// The first output register component for this parameter.
		/// </summary>
		public uint FirstOutComponent { get; private set; }

		public static DebugLibraryParameterDescription Parse(DebugBytecodeReader reader, 
			DebugBytecodeReader paramReader)
		{
			var nameOffset = paramReader.ReadUInt32("NameOffset");
			var semanticNameOffset = paramReader.ReadUInt32("SemanticNameOffset");
			var result = new DebugLibraryParameterDescription()
			{
				VariableType = paramReader.ReadEnum32<ShaderVariableType>("VariableType"),
				VariableClass = paramReader.ReadEnum32<ShaderVariableClass>("VariableClass"),
				Rows = paramReader.ReadUInt32("Rows"),
				Column = paramReader.ReadUInt32("Column"),
				InterpolationMode = paramReader.ReadEnum32< InterpolationMode>("InterpolationMode"),
				Flags = paramReader.ReadEnum32<ParameterFlags>("Flags"),
				FirstInRegister = paramReader.ReadUInt32("FirstInRegister"),
				FirstInComponent = paramReader.ReadUInt32("FirstInComponent"),
				FirstOutRegister = paramReader.ReadUInt32("FirstOutRegister"),
				FirstOutComponent = paramReader.ReadUInt32("FirstOutComponent"),
			};
			var nameReader = reader.CopyAtOffset("NameReader", paramReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");

			if (semanticNameOffset != 0)
			{
				var semanticNameReader = reader.CopyAtOffset("SemanticNameReader", paramReader, (int)semanticNameOffset);
				result.SemanticName = semanticNameReader.ReadString("SemanticName");
			} else
			{
				result.SemanticName = "";
			}
			return result;
		}
	}
}
