using System;

namespace DXDecompiler.DX9Shader
{
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
			if(type != RegisterType.ColorOut &&
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
			switch(registerKey.Number)
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
				switch(_maskedLength)
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
