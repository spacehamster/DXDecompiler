using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class Parameter
	{
		private uint p;
		public Parameter(uint data)
		{
			p = data;
		}
		RegisterType RegisterType  {
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
		public int DestinationWriteMask
		{
			get
			{
				return (int)((p >> 16) & 0xF);
			}
		}

		public int DestinationMaskedLength
		{
			get 
			{
				int writeMask = DestinationWriteMask;
				for (int i = 3; i != 0; i--)
				{
					if ((writeMask & (1 << i)) != 0)
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
				int writeMask = DestinationWriteMask;
				int length = 0;
				for (int i = 0; i < 4; i++)
				{
					if ((writeMask & (1 << i)) != 0)
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
		public byte[] GetSourceSwizzleComponents
		{
			get
			{
				int swizzle = SourceSwizzle;
				byte[] swizzleArray = new byte[4];
				for (int i = 0; i < 4; i++)
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
	}
}
