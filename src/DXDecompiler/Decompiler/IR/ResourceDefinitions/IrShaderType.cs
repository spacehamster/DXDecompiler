using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public class IrShaderType
	{
		/// <summary>
		/// Identifies the variable class as one of scalar, vector, matrix or object.
		/// </summary>
		public ShaderVariableClass VariableClass;

		/// <summary>
		/// The variable type.
		/// </summary>
		public IrShaderVariableType VariableType;

		/// <summary>
		/// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Rows;

		/// <summary>
		/// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0.
		/// </summary>
		public ushort Columns;

		/// <summary>
		/// Number of elements in an array; otherwise 0.
		/// </summary>
		public ushort ElementCount;

		public List<IrShaderTypeMember> Members;

		/// <summary>
		/// Parent Interface. 
		/// TODO: This is a guess, confirm that this is the parent interface
		/// </summary>
		public IrShaderType SubType;

		/// <summary>
		/// TODO: Find out what this is for
		/// </summary>
		public IrShaderType BaseClass;

		/// <summary>
		/// The interface types a concrete class inherits from 
		/// </summary>
		public List<IrShaderType> Interfaces;

		/// <summary>
		/// Name of the shader-variable type. This member can be NULL if it isn't used. This member supports 
		/// dynamic shader linkage interface types, which have names.
		/// TODO: Is this the right description?
		/// </summary>
		public string BaseTypeName;
	}
}