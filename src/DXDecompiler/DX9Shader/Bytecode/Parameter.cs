
namespace DXDecompiler.DX9Shader
{
	public class Parameter
	{
		private uint p;
		Token token;
		public Parameter(uint data)
		{
			p = data;
		}
		RegisterType RegisterType
		{
			get
			{
				return (RegisterType)(((p >> 28) & 0x7) | ((p >> 8) & 0x18));
			}
		}
		int RegisterNumber
		{
			get
			{
				return (int)(p & 0x7FF);
			}
		}
		RegisterType RegisterName
		{
			get
			{
				return (RegisterType)(((p >> 28) & 0x7) | ((p >> 8) & 0x18));
			}
		}
		public ResultModifier DestinationResultModifier
		{
			get
			{
				return (ResultModifier)((p >> 20) & 0xF);
			}
		}
		public SourceModifier GetSourceModifier
		{
			get
			{
				return (SourceModifier)((p >> 24) & 0xF);
			}
		}
		public ComponentFlags DestinationWriteMask
		{
			get
			{
				return (ComponentFlags)((p >> 16) & 0xF);
			}
		}

		public int DestinationMaskedLength
		{
			get
			{
				int writeMask = (int)DestinationWriteMask;
				for(int i = 3; i != 0; i--)
				{
					if((writeMask & (1 << i)) != 0)
					{
						return i + 1;
					}
				}
				return 0;
			}
		}
		public int DestinationMaskLength
		{
			get
			{
				int writeMask = (int)DestinationWriteMask;
				int length = 0;
				for(int i = 0; i < 4; i++)
				{
					if((writeMask & (1 << i)) != 0)
					{
						length++;
					}
				}
				return length;
			}
		}
		public int SourceSwizzle
		{
			get
			{
				return (int)((p >> 16) & 0xFF);
			}
		}
		public byte[] SourceSwizzleComponents
		{
			get
			{
				int swizzle = SourceSwizzle;
				byte[] swizzleArray = new byte[4];
				for(int i = 0; i < 4; i++)
				{
					swizzleArray[i] = (byte)((swizzle >> (i * 2)) & 0x3);
				}
				return swizzleArray;
			}
		}
		// For output, input and texture declarations
		public DeclUsage DeclUsage
		{
			get
			{
				return (DeclUsage)(p & 0x1F);
			}
		}
		public int DeclIndex
		{
			get
			{
				return (int)(p >> 16) & 0x0F;
			}
		}
		public SamplerTextureType GetDeclSamplerTextureType
		{
			get
			{
				return (SamplerTextureType)((p >> 27) & 0xF);
			}
		}
		public bool LastBit
		{
			get
			{
				return (p & 0x80000000) != 0;
			}
		}
		public string DestinationWriteMaskName
		{
			get
			{
				int destinationLength = 4;
				ComponentFlags writeMask = DestinationWriteMask;
				int writeMaskLength = DestinationMaskLength;

				// Check if mask is the same length and of the form .xyzw
				if(writeMaskLength == destinationLength && writeMask == (ComponentFlags)((1 << writeMaskLength) - 1))
				{
					return "";
				}

				string writeMaskName =
					string.Format(".{0}{1}{2}{3}",
					((writeMask & ComponentFlags.X) != 0) ? "x" : "",
					((writeMask & ComponentFlags.Y) != 0) ? "y" : "",
					((writeMask & ComponentFlags.Z) != 0) ? "z" : "",
					((writeMask & ComponentFlags.W) != 0) ? "w" : "");
				return writeMaskName;
			}
		}
		public string GetSourceSwizzleName
		{
			get
			{
				int swizzleLength = 4;

				string swizzleName = "";
				byte[] swizzle = SourceSwizzleComponents;
				for(int i = 0; i < swizzleLength; i++)
				{
					switch(swizzle[i])
					{
						case 0:
							swizzleName += "x";
							break;
						case 1:
							swizzleName += "y";
							break;
						case 2:
							swizzleName += "z";
							break;
						case 3:
							swizzleName += "w";
							break;
					}
				}
				switch(swizzleName)
				{
					case "xxx":
						return ".x";
					case "yyy":
						return ".y";
					case "zzz":
						return ".z";
					case "xyz":
						return "";
					case "xyzw":
						return "";
					case "xxxx":
						return ".x";
					case "yyyy":
						return ".y";
					case "zzzz":
						return ".z";
					case "wwww":
						return ".w";
					default:
						return "." + swizzleName;
				}
			}
		}
	}
}
