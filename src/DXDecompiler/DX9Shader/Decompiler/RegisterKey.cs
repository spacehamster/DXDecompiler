namespace DXDecompiler.DX9Shader
{
	public class RegisterKey
	{
		public RegisterKey(RegisterType registerType, uint registerNumber)
		{
			Type = registerType;
			Number = registerNumber;
		}

		public uint Number { get; }
		public RegisterType Type { get; }


		public override bool Equals(object obj)
		{
			if(!(obj is RegisterKey other))
			{
				return false;
			}
			return
				other.Number == Number &&
				other.Type == Type;
		}

		public override int GetHashCode()
		{
			int hashCode =
				Number.GetHashCode() ^
				Type.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return $"{Type.ToString().ToLower()}{Number}";
		}
	}
}
