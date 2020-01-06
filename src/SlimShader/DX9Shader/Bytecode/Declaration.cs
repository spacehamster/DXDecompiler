using System;
using System.Collections.Generic;

namespace SlimShader.DX9Shader
{
	// D3DXREGISTER_SET
	public enum RegisterSet
	{
		Bool,
		Int4,
		Float4,
		Sampler
	}

	// D3DXPARAMETER_CLASS
	public enum ParameterClass
	{
		Scalar,
		Vector,
		MatrixRows,
		MatrixColumns,
		Object,
		Struct
	}

	// D3DXPARAMETER_TYPE
	public enum ParameterType
	{
		Void,
		Bool,
		Int,
		Float,
		String,
		Texture,
		Texture1D,
		Texture2D,
		Texture3D,
		TextureCube,
		Sampler,
		Sampler1D,
		Sampler2D,
		Sampler3D,
		SamplerCube,
		PixelShader,
		VertexShader,
		PixelFragment,
		VertexFragment,
		Unsupported
	}

	// D3DDECLUSAGE
	public enum DeclUsage
	{
		Position,
		BlendWeight,
		BlendIndices,
		Normal,
		PSize,
		TexCoord,
		Tangent,
		Binormal,
		TessFactor,
		PositionT,
		Color,
		Fog,
		Depth,
		Sample
	}

	// D3DSAMPLER_TEXTURE_TYPE
	public enum SamplerTextureType
	{
		Unknown = 1,
		TwoD = 2,
		Cube = 3,
		Volume = 4
	}

	public class ConstantTable
	{
		public string Creator { get; private set; }
		public string ShaderModel { get; private set; }
		public int MajorVersion { get; private set; }
		public int MinorVersion { get; private set; }
		public List<ConstantDeclaration> ConstantDeclarations { get; private set; }
		public ConstantTable(string creator, string shaderModel, int majorVersion, int minorVersion, List<ConstantDeclaration> constantDeclarations)
		{
			Creator = creator;
			ShaderModel = shaderModel;
			MajorVersion = majorVersion;
			MinorVersion = minorVersion;
			ConstantDeclarations = constantDeclarations;
		}
	}

	public class ConstantDeclaration
	{
		public string Name { get; private set; }
		public RegisterSet RegisterSet { get; private set; }
		public short RegisterIndex { get; private set; }
		public short RegisterCount { get; private set; }
		public ParameterClass ParameterClass { get; private set; }
		public ParameterType ParameterType { get; private set; }
		public int Rows { get; private set; }
		public int Columns { get; set; }
		public int Elements { get; private set; }
		public List<float> DefaultValue { get; set; }

		public ConstantDeclaration(string name, RegisterSet registerSet, short registerIndex, short registerCount,
			ParameterClass parameterClass, ParameterType parameterType, int rows, int columns, int elements, List<float> defaultValue)
		{
			Name = name;
			RegisterSet = registerSet;
			RegisterIndex = registerIndex;
			RegisterCount = registerCount;
			ParameterClass = parameterClass;
			ParameterType = parameterType;
			Rows = rows;
			Columns = columns;
			Elements = elements;
			DefaultValue = defaultValue;
		}

		public bool ContainsIndex(int index)
		{
			return (index >= RegisterIndex) && (index < RegisterIndex + RegisterCount);
		}

		public override string ToString()
		{
			return Name;
		}
		public string GetTypeName()
		{
			if (ParameterClass == ParameterClass.Vector)
			{
				if (Columns > 1)
				{
					return $"{ParameterType.GetDescription()}{Columns}";
				}
				else
				{
					return $"{ParameterType.GetDescription()}";
				}
			}
			else if (ParameterClass == ParameterClass.MatrixColumns)
			{
				return $"{ParameterType.GetDescription()}{Rows}x{Columns}";
			}
			else if (ParameterClass == ParameterClass.MatrixRows)
			{
				return $"{ParameterType.GetDescription()}{Columns}x{Rows}";
			}
			else
			{
				return ParameterType.GetDescription();
			}
		}
		public string GetRegisterName()
		{
			return $"{RegisterSet.GetDescription()}{RegisterIndex}";
		}
	}

	// https://msdn.microsoft.com/en-us/library/windows/hardware/ff549176(v=vs.85).aspx
	public class RegisterDeclaration
	{
		private readonly int _maskedLength;

		public RegisterDeclaration(Token declInstruction)
		{
			RegisterKey = declInstruction.GetParamRegisterKey(1);
			Semantic = declInstruction.GetDeclSemantic();
			_maskedLength = declInstruction.GetDestinationMaskedLength();
		}

		public RegisterDeclaration(RegisterKey registerKey)
		{
			RegisterType type = registerKey.Type;
			if (type != RegisterType.ColorOut &&
				type != RegisterType.Const &&
				type != RegisterType.Temp &&
				type != RegisterType.RastOut &&
				type != RegisterType.ConstInt && 
				type != RegisterType.Addr &&
				type != RegisterType.Output &&
				type != RegisterType.AttrOut &&
				type != RegisterType.ConstBool)
			{
				throw new ArgumentException($"Register type {type} requires declaration instruction,", nameof(registerKey));
			}

			RegisterKey = registerKey;
			switch (registerKey.Number)
			{
				case 0:
					Semantic = "COLOR";
					break;
				default:
					Semantic = "COLOR" + registerKey.Number;
					break;
			}
			_maskedLength = 4;
		}

		public RegisterKey RegisterKey { get; }
		public string Semantic { get; }
		public string Name => Semantic.ToLower();

		public string TypeName
		{
			get
			{
				switch (_maskedLength)
				{
					case 1:
						return "float";
					case 2:
						return "float2";
					case 3:
						return "float3";
					case 4:
						return "float4";
					default:
						throw new InvalidOperationException();
				}
			}
		}

		public override string ToString()
		{
			return RegisterKey.Type + " " + Name;
		}
	}
}
