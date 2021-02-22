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
		private static string DXBCShaders => $"{TestContext.CurrentContext.TestDirectory}/Shaders";
		private static string DX9Shaders => $"{TestContext.CurrentContext.TestDirectory}/ShadersDX9";

		[Test]
		public void CompileShaders()
		{
			ShaderCompiler.ProcessShaders(DXBCShaders);
			ShaderCompiler.ProcessShaders(DX9Shaders);
		}
	}
}