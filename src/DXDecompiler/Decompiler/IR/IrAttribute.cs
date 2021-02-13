using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR
{
	public class IrAttribute
	{
		public string Name;
		public List<string> Arguments = new List<string>();
		public static IrAttribute Create(string name, params object[] args)
		{
			var result = new IrAttribute();
			result.Name = name;
			foreach(var arg in args)
			{
				if(arg is string)
				{
					result.Arguments.Add($"\"{arg}\"");
				}
				else
				{
					result.Arguments.Add(arg.ToString());
				}
			}
			return result;
		}
	}
}
