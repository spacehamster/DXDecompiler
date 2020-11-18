using System;

namespace DXDecompiler.DX9Shader
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
