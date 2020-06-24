using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Util
{
	public static class FormatUtil
	{
		public static string FormatBytes(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i += 16)
			{
				sb.AppendFormat("// {0}:  ", i.ToString("X4"));
				for (int j = i; j < i + 16; j++)
				{
					if (j < data.Length)
					{
						sb.Append(data[j].ToString("X2"));
					}
					else
					{
						sb.Append("  ");
					}
					if ((j + 1) % 4 == 0)
					{
						sb.Append("  ");
					}
				}
				for (int j = i; j < i + 16 && j < data.Length; j++)
				{
					var c = (char)data[j];
					if (char.IsControl(c))
					{
						sb.Append("_");
					}
					else if (c > 0x7E)
					{
						sb.Append('.');
					}
					else if (char.IsWhiteSpace(c))
					{
						sb.Append('.');
					}
					else
					{
						sb.Append(c);
					}
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}
		public static string BytesToAscii(byte[] data)
		{
			var sb = new StringBuilder();
			foreach(var b in data)
			{
				if(b >= 32 && b <= 126)
				{
					sb.Append((char)b);
				} else
				{
					sb.Append((char)219);
				}
			}
			return sb.ToString();
		}
	}
}
