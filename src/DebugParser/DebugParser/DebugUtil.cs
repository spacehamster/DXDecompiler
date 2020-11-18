using System.Text;

namespace DXDecompiler.DebugParser
{
	public static class DebugUtil
	{
		public static string ToReadable(string text)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				var c = (char)text[i];
				if (char.IsControl(c) || char.IsWhiteSpace(c))
				{
					sb.Append(".");
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
		public static string ToReadable(byte[] data)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				var c = (char)data[i];
				sb.Append(CharToReadable(c));
			}
			return sb.ToString();
		}
		public static string CharToReadable(char c)
		{
			if (char.IsControl(c) || char.IsWhiteSpace(c) || c > 176)
			{
				return ".";
			}
			else
			{
				return c.ToString();
			}
		}
	}
}
