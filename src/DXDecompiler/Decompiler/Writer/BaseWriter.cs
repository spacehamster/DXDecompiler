using System;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.Writer
{
	public class BaseWriter
	{
		public DecompileContext Context;
		public BaseWriter(DecompileContext context)
		{
			Context = context;
		}
		protected void WriteIndent()
		{
			Context.Writer.Write(new string('\t', Context.Indent));
		}
		protected void Write(string text)
		{
			Context.Writer.Write(text);
		}
		protected void WriteLine(string text)
		{
			Context.Writer.WriteLine(text);
		}
		protected void WriteLine()
		{
			Context.Writer.WriteLine();
		}
		protected void WriteFormat(string fmt, params object[] args)
		{
			Context.Writer.Write(fmt, args);
		}
		protected void WriteLineFormat(string fmt, params object[] args)
		{
			Context.Writer.WriteLine(fmt, args);
		}
		protected void IncreaseIndent()
		{
			Context.Indent += 1;
		}
		protected void DecreaseIndent()
		{
			Context.Indent -= 1;
		}
		protected void Join(string seperator, IEnumerable<object> objects, Action<object> func)
		{
			var enumerator = objects.GetEnumerator();
			if(!enumerator.MoveNext()) return;
			while(true)
			{
				func(enumerator.Current);
				if(enumerator.MoveNext())
				{
					Write(seperator);
				}
				else
				{
					break;
				}
			}
		}
	}
}
