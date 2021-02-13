using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	/// <summary>
	/// VariableDeclaration
	/// Variable declaration, similar to a structure member declaration. This can be any HLSL type or effect object (except a texture or a sampler object).
	/// </summary>
	public class IrShaderVariable
	{
		public IrShaderTypeMember Member;

		/// <summary>
		/// The variable name.
		/// </summary>
		public string Name
		{
			get { return Member.Name; }
		}

		/// <summary>
		/// Offset from the start of the parent structure, to the beginning of the variable.
		/// </summary>
		public uint StartOffset
		{
			get { return Member.Offset; }
		}

		/// <summary>
		/// Get a shader-variable type.
		/// </summary>
		public IrShaderType ShaderType
		{
			get { return Member.Type; }
		}

		/// <summary>
		/// Gets the name of the base class.
		/// </summary>
		public string BaseType;

		/// <summary>
		/// Size of the variable (in bytes).
		/// </summary>
		public uint Size;

		/// <summary>
		/// Flags, which identify shader-variable properties.
		/// </summary>
		public ShaderVariableFlags Flags;

		/// <summary>
		/// The default value for initializing the variable.
		/// </summary>
		public List<Number> DefaultValue;

		/// <summary>
		/// First texture index (or -1 if no textures used).
		/// </summary>
		public int StartTexture;

		/// <summary>
		/// Number of texture slots possibly used.
		/// </summary>
		public int TextureSize;

		/// <summary>
		/// First sampler index (or -1 if no textures used)
		/// </summary>
		public int StartSampler;

		/// <summary>
		/// Number of sampler slots possibly used.
		/// </summary>
		public int SamplerSize;
	}
}