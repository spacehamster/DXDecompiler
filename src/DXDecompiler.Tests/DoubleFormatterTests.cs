using NUnit.Framework;
using SharpDX.D3DCompiler;
using SlimShader.Tests.Util;
using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static SlimShader.Util.DoubleConverter;

namespace SlimShader.Tests
{
	/// <summary>
	/// FXC float formatting is equivilent to std::to_string ( double value ); and 
	/// sprintf("%f", double value); with a special case for 0f 
	/// </summary>
	[TestFixture]
	class DoubleFormatterTests
	{
		private static string OutputDir => $@"{TestContext.CurrentContext.TestDirectory}/Misc";
		[Test]
		public void TestFloatFormattingRandom()
		{
			//Note each test takes approximately 0.0011 seconds
			var rnd = new Random();
			for (uint i = 0; i < 10000; i++)
			{
				int ival = rnd.Next(int.MinValue, int.MaxValue);
				var data = BitConverter.GetBytes(ival);
				float f = BitConverter.ToSingle(data, 0);
				TestFloat(f);
			}
		}
		[Test]
		public void TestFloatFormattingRandomToFile()
		{
			//Note each test takes approximately 0.0011 seconds
			var rnd = new Random();
			var sb = new StringBuilder();
			var values = new object[] {
				0.0000005f,
				0.0000006f,
				0.0000004f,
				0.0000015f,
				0.0000016f,
				0.0000014f,
				0.0000005456f,
				0.0000006456f,
				0.0000004456f,
				0.0000015456f,
				0.0000016456f,
				0.0000014456f,
				-0.0000005f,
				-0.0000006f,
				-0.0000004f,
				-0.0000015f,
				-0.0000016f,
				-0.0000014f,
				-0.0000005456f,
				-0.0000006456f,
				-0.0000004456f,
				-0.0000015456f,
				-0.0000016456f,
				-0.0000014456f,
				//Tests
				889599933,
				//Random
				107429.0390625f,
				0.000000510792f,
				11.97355747f,
				-91233.5078125f
			};
			foreach(var val in values)
			{
				int ival = val is int ? (int)val : BitConverter.ToInt32(BitConverter.GetBytes((float)val), 0);
				DumpFloat(ival, sb);
			}
			for (uint i = 0; i < 100; i++)
			{
				int ival = rnd.Next(int.MinValue, int.MaxValue);
				var floatValue = ToFloat(ival);
				if(floatValue < 0.0000004 && floatValue > -0.0000004)
				{
					i--;
					continue;
				}
				DumpFloat(ival, sb);
			}
			Directory.CreateDirectory(OutputDir);
			File.WriteAllText($"{OutputDir}/RandomFloats.txt", sb.ToString());
		}
		[Test]
		public void TestFloatFixedRoundTrip()
		{
			TestRoundTrip(-1945768850);
			//Note each test takes approximately 0.0011 seconds
			var rnd = new Random();
			for (uint i = 0; i < 1000000; i++)
			{
				int ival = rnd.Next(int.MinValue, int.MaxValue);
				TestRoundTrip(ival);
			}
		}
		/// <summary>
		/// Note: Round trip formats: 
		/// "G7" - 0x8c05ec6e -> "1.03171E-31" -> 0x8c05ec72
		/// "G9 - Works
		/// "F9" - Doesn't work for small values
		/// </summary>
		/// <param name="expected"></param>
		public static void TestRoundTrip(int expected)
		{
			var data = BitConverter.GetBytes(expected);
			float original = BitConverter.ToSingle(data, 0);
			if (float.IsNaN(original))
			{
				return;
			}
			var floatText = original.ToString("G7");
			var roundTripped = float.Parse(floatText);
			var actual = BitConverter.ToInt32(BitConverter.GetBytes(roundTripped), 0);
			Assert.AreEqual(expected, actual);
		}
		[Test]
		public void TestRounding()
		{
			TestRounding(891359152, "1");
			TestRounding(889599933, "0");
			TestRounding(-1222628349, "10");
			TestRounding(924827245, "10");
			TestRounding(1004706573, "6915");
			TestRounding(893148337, "1");
			TestRounding(902386588, "2");
			TestRounding(-1144724844, "6008");
		}
		[Test]
		public void TestFloatFormatting()
		{
			TestFloat(0.75f);
			//Rounding
			TestFloat(0.0000005f);
			TestFloat(0.0000006f);
			TestFloat(0.0000004f);
			TestFloat(0.0000015f);
			TestFloat(0.0000016f);
			TestFloat(0.0000014f);
			TestFloat(0.0000005456f);
			TestFloat(0.0000006456f);
			TestFloat(0.0000004456f);
			TestFloat(0.0000015456f);
			TestFloat(0.0000016456f);
			TestFloat(0.0000014456f);
			TestFloat(-0.0000005f);
			TestFloat(-0.0000006f);
			TestFloat(-0.0000004f);
			TestFloat(-0.0000015f);
			TestFloat(-0.0000016f);
			TestFloat(-0.0000014f);
			TestFloat(-0.0000005456f);
			TestFloat(-0.0000006456f);
			TestFloat(-0.0000004456f);
			TestFloat(-0.0000015456f);
			TestFloat(-0.0000016456f);
			TestFloat(-0.0000014456f);
			//General
			TestFloat(-1222628349);
			TestFloat(889599933); //0.000010 0.00000952147547650383785367012023926
			TestFloat(1134525186);  //318.960999 318.96099853515625
			TestFloat(1004706573);  //0.006915 00691545614972710609436035156
			TestFloat(-1144724844); //-0.006008
			TestFloat(999877176); //0.00466659292578697204589843
			TestFloat(1191651990); //34602.585938 (34602.5859375)
			TestFloat(1024618613); //0.035752
			TestFloat(1160399031); //2724.544678
			TestFloat(1955948961); //94701494221708683911610226442240.000000
			TestFloat(1.575f);
			TestFloat(-2140039654);
			TestFloat(-1118439282);
			TestFloat(-212173787);
			TestFloat(-1945768850);
			TestFloat(0f);
			TestFloat(1f);
			TestFloat(0.75f);
			TestFloat(551241158);
			TestFloat(0x8c05ec6e);
			TestFloat(0xadaafdb8);
			TestFloat(0xef3197b0);
		}
		public static void TestRounding(float value, string expected)
		{
			var data = BitConverter.GetBytes(value);
			var u = BitConverter.ToInt32(data, 0);
			TestRounding(u, expected);
		}
		public static void TestRounding(int value, string expected)
		{
			var floatValue = ToFloat(value);
			var ad = DoubleUtil.ToArbitaryDecimal(floatValue, -1);
			ad.RoundDigits(6);
			var stringValue = DoubleConverter.ToExactString(floatValue, -1);
			var digits = ad.digits;
			var chr = new char[digits.Length];
			for (int i = 0; i < digits.Length; i++)
			{
				chr[i] = (char)(digits[i] + '0');
			}
			var result = new string(chr);
			Assert.AreEqual(expected, result, $"Error rounding {value} original {stringValue}");
		}
		public static string FormatFloat(float value)
		{
			if (value == 0f) return "0";
			var result = DoubleConverter.ToExactString(value);
			return result;
		}
		public static float ToFloat(int value)
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
		}
		public static void TestFloat(float value)
		{
			var data = BitConverter.GetBytes(value);
			var u = BitConverter.ToInt32(data, 0);
			TestFloat(u);
		}
		public static void TestFloat(int value)
		{
			var floatValue = ToFloat(value);
			if (float.IsNaN(floatValue))
			{
				return;
			}
			string shaderCode = string.Format(@"
				float4 main(): sv_target {{ return asfloat({0}); }}",
				value);
			var compiledShader = ShaderBytecode.Compile(shaderCode, "main", "ps_5_0");
			var shaderBytecode = compiledShader.Bytecode;
			var disassembly = shaderBytecode.Disassemble();
			var bytecode = BytecodeContainer.Parse(shaderBytecode);
			var number = bytecode.Shader.InstructionTokens.First().Operands[1].ImmediateValues;
			StringAssert.Contains("Generated by Microsoft (R) HLSL Shader Compiler 10.1", disassembly);
			var expectedFloat = Regex.Match(disassembly, @"l\((.*?),").Groups[1].Value;
			var actualFloat = FormatFloat(floatValue);
			if(value != number.Int0)
			{
				Directory.CreateDirectory(OutputDir);
				File.WriteAllText($"{OutputDir}/FloatParserDissassembly.asm", disassembly);
				return;
			}
			Assert.AreEqual(value, number.Int0, "Parsed binary represention doesn't match input");
			Assert.AreEqual(expectedFloat, actualFloat, $"String format doesn't match expected for {value} {DoubleConverter.ToExactString(floatValue, -1)}");
		}
		public static void DumpFloat(int value, StringBuilder sb)
		{
			var floatValue = ToFloat(value);
			if (float.IsNaN(floatValue))
			{
				return;
			}
			string shaderCode = string.Format(@"
				float4 main(): sv_target {{ return asfloat({0}); }}",
				value);
			var compiledShader = ShaderBytecode.Compile(shaderCode, "main", "ps_5_0");
			var shaderBytecode = compiledShader.Bytecode;
			var disassembly = shaderBytecode.Disassemble();
			var bytecode = BytecodeContainer.Parse(shaderBytecode);
			var number = bytecode.Shader.InstructionTokens.First().Operands[1].ImmediateValues;
			StringAssert.Contains("Generated by Microsoft (R) HLSL Shader Compiler 10.1", disassembly);
			var expectedFloat = Regex.Match(disassembly, @"l\((.*?),").Groups[1].Value;
			var actualFloat = FormatFloat(floatValue);
			var fullFloat = DoubleConverter.ToExactString(floatValue, -1);
			var correct = expectedFloat == actualFloat;
			if (value != number.Int0)
			{
				return;
			}
			sb.AppendLine(string.Format("{0}\t{1, 50}\t{2, 50}\t{3}\t{4}",
				correct, expectedFloat, actualFloat, fullFloat, value));
		}
	}
}
