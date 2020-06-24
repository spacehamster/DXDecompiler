using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.DX9Shader
{
	public class DecompileWriter
	{
		public int Indent;
		StreamWriter Writer;
		protected void WriteIndent()
		{
			Writer.Write(new string(' ', Indent * 4));
		}
		protected void WriteLine()
		{
			Writer.WriteLine();
		}
		protected void WriteLine(string value)
		{
			Writer.WriteLine(value);
		}

		protected void WriteLine(string format, params object[] args)
		{
			Writer.WriteLine(format, args);
		}
		protected void Write(string value)
		{
			Writer.Write(value);
		}
		protected void Write(string format, params object[] args)
		{
			Writer.Write(format, args);
		}
		protected virtual void Write()
		{

		}
		public string Decompile()
		{
			using (var stream = new MemoryStream())
			{
				Writer = new StreamWriter(stream);
				Write();
				Writer.Flush();
				stream.Position = 0;
				using (var reader = new StreamReader(stream, Encoding.UTF8))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
