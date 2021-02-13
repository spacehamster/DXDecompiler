using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;
using NUnit.Framework;
using System;
using System.IO;

namespace DXDecompiler.Tests.Util
{
	[TestFixture]
	public class CompileShadersFixture
	{
		private static string ShaderDirectory => $"{TestContext.CurrentContext.TestDirectory}/Shaders";

		[Test]
		public void CompileShaders()
		{
			ShaderCompiler.ProcessShaders(ShaderDirectory);
		}
	}
}