using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimShader.Compiler
{
	public class FileUtils
	{
		public static void WriteFile(string path, byte[] data)
		{
			File.WriteAllBytes(path, data);
		}
	}
}
