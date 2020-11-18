using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler
{
	public class Register
	{
		public Register(string name)
		{
			Name = name;
		}
		public string Name { get; private set; }
		public override string ToString()
		{
			return Name;
		}
	}
}
