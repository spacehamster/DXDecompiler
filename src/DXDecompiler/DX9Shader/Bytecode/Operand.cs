namespace DXDecompiler.DX9Shader
{
	public class Operand
	{
		public static string GetParamRegisterName(RegisterType registerType, uint registerNumber)
		{
			string registerTypeName;
			switch(registerType)
			{
				case RegisterType.Addr:
					registerTypeName = "a";
					break;
				case RegisterType.Const:
					registerTypeName = "c";
					break;
				case RegisterType.Const2:
					registerTypeName = "c";
					registerNumber += 2048;
					break;
				case RegisterType.Const3:
					registerTypeName = "c";
					registerNumber += 4096;
					break;
				case RegisterType.Const4:
					registerTypeName = "c";
					registerNumber += 6144;
					break;
				case RegisterType.ConstBool:
					registerTypeName = "b";
					break;
				case RegisterType.ConstInt:
					registerTypeName = "i";
					break;
				case RegisterType.Input:
					registerTypeName = "v";
					break;
				case RegisterType.Output:
					registerTypeName = "o";
					break;
				case RegisterType.RastOut:
					{
						return "oPos";
					}
				case RegisterType.Temp:
					registerTypeName = "r";
					break;
				case RegisterType.Sampler:
					registerTypeName = "s";
					break;
				case RegisterType.ColorOut:
					registerTypeName = "oC";
					break;
				case RegisterType.DepthOut:
					registerTypeName = "oDepth";
					break;
				case RegisterType.AttrOut:
					registerTypeName = "oD";
					break;
				case RegisterType.MiscType:
					if(registerNumber == 0)
					{
						return "vFace";
					}
					else if(registerNumber == 1)
					{
						return "vPos";
					}
					else
					{
						return $"Invalid MiscType {registerNumber}";
					}
				default:
					return $"Invalid register type {registerType}";
			}

			return $"{registerTypeName}{registerNumber}";
		}
	}
}
