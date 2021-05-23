using System;

namespace DXDecompiler.DX9Shader
{
	// https://msdn.microsoft.com/en-us/library/windows/hardware/ff549176(v=vs.85).aspx
	public class RegisterDeclaration
	{
		private readonly string _semantic;

		public RegisterDeclaration(InstructionToken declInstruction)
		{
			RegisterKey = declInstruction.GetParamRegisterKey(1);
			_semantic = declInstruction.GetDeclSemantic();
			MaskedLength = declInstruction.GetDestinationMaskedLength();
		}

		public RegisterDeclaration(RegisterKey registerKey, int maskedLength = 4)
		{
			RegisterType type = registerKey.Type;
			_semantic = GuessSemanticByRegisterType(type);
			RegisterKey = registerKey;
			if(_semantic != null && RegisterKey.Number != 0)
			{
				_semantic += registerKey.Number;
			}
			MaskedLength = maskedLength;
		}

		public RegisterKey RegisterKey { get; }
		public int MaskedLength { get; }
		public string Semantic => _semantic ?? throw new NotSupportedException();
		public string Name => _semantic?.ToLower() ?? RegisterKey.ToString();


		public string TypeName
		{
			get
			{
				switch(MaskedLength)
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

		private static string GuessSemanticByRegisterType(RegisterType type)
		{
			switch(type)
			{
				case RegisterType.ColorOut:
				case RegisterType.AttrOut: // in vs_2_0 (and / or below?), as output color register
					return "COLOR";
				case RegisterType.RastOut: // in vs_2_0 (and / or below?), as output position register
					return "POSITION";
				case RegisterType.Const:
				case RegisterType.Temp:
				case RegisterType.ConstInt:
				case RegisterType.Addr:
				case RegisterType.ConstBool:
					return null;
				case RegisterType.TexCoordOut:
					return "TEXCOORD";
				default:
					throw new ArgumentException($"Register type {type} requires declaration instruction");
			}
		}
	}
}
