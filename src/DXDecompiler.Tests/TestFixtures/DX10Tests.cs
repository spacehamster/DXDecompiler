using DXDecompiler.Chunks.Fx10;
using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Chunks.Spdb;
using DXDecompiler.Chunks.Xsgn;
using DXDecompiler.Decompiler;
using DXDecompiler.Tests.Util;
using NUnit.Framework;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DXDecompiler.Tests
{
	[TestFixture]
	public class DX10Tests
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			if(Thread.CurrentThread.Name == null)
			{
				Thread.CurrentThread.Name = "TestThread";
			}
			ShaderCompiler.ProcessShaders(ShaderDirectory);
		}
		[SetUp]
		public void SetUp()
		{
			Directory.CreateDirectory(OutputDir);
			AssertTraceListener.Init();
		}
		private static string OutputDir => $@"{TestContext.CurrentContext.TestDirectory}/Shaders";
		private static string ShaderDirectory => $"{TestContext.CurrentContext.TestDirectory}/Shaders";
		private static IEnumerable<TestCaseData> TestShaders
		{
			get
			{
				HashSet<string> Ignore = new HashSet<string>() {
					@"HlslCrossCompiler\\gs5\\stream",
					@"Sdk\\Direct3D11\\BC6HBC7EncoderDecoder11\\BC7Encode_02",
					@"Sdk\\Direct3D11\\BC6HBC7EncoderDecoder11\\BC7Encode_137"
				};
				string shadersDirectory = ShaderDirectory;
				foreach(var pathWithExt in Directory.GetFiles(shadersDirectory, "*.o", SearchOption.AllDirectories))
				{
					var path = Path.GetDirectoryName(pathWithExt) + @"\" + Path.GetFileNameWithoutExtension(pathWithExt);
					var relPath = FileUtil.GetRelativePath(path, ShaderDirectory);
					if(Ignore.Contains(relPath))
					{
						yield return new TestCaseData(relPath).Ignore("FXC quirk");
					}
					else
					{
						yield return new TestCaseData(relPath);
					}
				}
			}
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void AsmMatchesFxc(string relPath)
		{
			var file = $"{ShaderDirectory}/{relPath}";
			// Arrange.
			var asmFileText = string.Join(Environment.NewLine,
				File.ReadAllLines(file + ".asm").Select(x => x.Trim()));

			// Act.
			var bytecode = File.ReadAllBytes(file + ".o");
			var container = BytecodeContainer.Parse(bytecode);
			var decompiledAsmText = container.ToString();

			File.WriteAllText($"{file}.d.asm", decompiledAsmText);
			File.WriteAllText($"{file}.x", FileUtil.FormatReadable(bytecode));

			decompiledAsmText = string.Join(Environment.NewLine, decompiledAsmText
				.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
				.Select(x => x.Trim()));

			decompiledAsmText = TestUtils.NormalizeAssembly(decompiledAsmText);
			asmFileText = TestUtils.NormalizeAssembly(asmFileText);

			// Assert.
			if(container.Chunks.OfType<DebuggingChunk>().Any())
			{
				Warn.If(true, "Debugging information is ignored during dissasembly");
			}
			else
			{
				Assert.That(decompiledAsmText, Is.EqualTo(asmFileText));
			}
		}
		public static string GetSourceNameFromObject(string path)
		{
			path = path.Replace(".o", "");
			var dir = Path.GetDirectoryName(path);
			var file = Path.GetFileName(path);
			var parts = file.Split('_');
			for(int i = parts.Length; i > 0; i--)
			{
				file = string.Join("_", parts.Take(i));
				if(File.Exists($"{dir}/{file}")) return file;
				if(File.Exists($"{dir}/{file}.fx")) return $"{file}.fx";
				if(File.Exists($"{dir}/{file}.hlsl")) return $"{file}.hlsl";
			}
			return Path.GetFileName(path);
		}
		/// <summary>
		/// Sanity check for exceptions while decompling.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void DecompileShaders(string relPath)
		{
			// Arrange.
			var file = $"{ShaderDirectory}/{relPath}";
			var relDir = Path.GetDirectoryName(relPath);
			Directory.CreateDirectory($"{OutputDir}/{relDir}");
			var sourceName = GetSourceNameFromObject($"{ShaderDirectory}/{relPath}.o");
			if(ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relDir}/{sourceName}", $"{OutputDir}/{relDir}/{sourceName}", true);

			// Act.
			var shaderCode = HLSLDecompiler.Decompile(File.ReadAllBytes(file + ".o"));
			File.WriteAllText($"{OutputDir}/{relPath}.d.hlsl", shaderCode);

			// Assert.
		}

		/// <summary>
		/// Compare ASM output produced by fxc.exe and SlimShader.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void RecompileTest(string relPath)
		{
			// Arrange.
			var file = $"{ShaderDirectory}/{relPath}";
			var relDir = Path.GetDirectoryName(relPath);
			Directory.CreateDirectory($"{OutputDir}/{relDir}");
			var sourceName = GetSourceNameFromObject($"{ShaderDirectory}/{relPath}.o");
			if(ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relDir}/{sourceName}", $"{OutputDir}/{relDir}/{sourceName}", true);
			if(ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relPath}.asm", $"{OutputDir}/{relPath}.asm", true);

			var asmFileText = string.Join(Environment.NewLine,
				File.ReadAllLines(file + ".asm").Select(x => x.Trim()));

			// Act.
			var binaryFileBytes = File.ReadAllBytes(file + ".o");
			using(var shaderBytecode = ShaderBytecode.FromStream(new MemoryStream(binaryFileBytes)))
			{
				var shaderVersion = shaderBytecode.GetVersion();
				var shaderCode = OldHLSLDecompiler.Decompile(File.ReadAllBytes(file + ".o"));
				File.WriteAllText($"{OutputDir}/{relPath}.d.hlsl", shaderCode);
				var entryPoint = TestUtils.GetShaderEntryPoint(shaderVersion);
				var profile = TestUtils.GetShaderProfile(shaderVersion);
				var compiledShader = ShaderBytecode.Compile(shaderCode, entryPoint, profile);
				var disassembly = shaderBytecode.Disassemble();
				var redisassembly = compiledShader.Bytecode.Disassemble();
				File.WriteAllText($"{OutputDir}/{relPath}.d1.asm", disassembly);
				File.WriteAllText($"{OutputDir}/{relPath}.d2.asm", redisassembly);

				// Assert.
				Warn.If(disassembly, Is.EqualTo(redisassembly));
				//Assert.Warn("Dissassembly does not match");
			}

		}

		[TestCaseSource("TestShaders")]
		public void DumpStructure(string relPath)
		{
			// Arrange.
			var file = $"{ShaderDirectory}/{relPath}";
			var relDir = Path.GetDirectoryName(relPath);
			Directory.CreateDirectory($"{OutputDir}/{relDir}");
			var sourceName = GetSourceNameFromObject($"{ShaderDirectory}/{relPath}.o");
			if(ShaderDirectory != OutputDir) File.Copy($"{ShaderDirectory}/{relDir}/{sourceName}", $"{OutputDir}/{relDir}/{sourceName}", true);
			var binaryFileBytes = File.ReadAllBytes(file + ".o");

			// Act.
			var bytecodeContainer = DebugParser.DebugBytecodeContainer.Parse(binaryFileBytes);
			var result = bytecodeContainer.Dump();
			foreach(var exception in bytecodeContainer.Exceptions)
			{
				result += "\n" + exception.ToString();
			}

			File.WriteAllText($"{OutputDir}/{relPath}.d.txt", result);

			var html = bytecodeContainer.DumpHTML();
			File.WriteAllText($"{OutputDir}/{relPath}.d.html", html);

			// Assert.
			if(bytecodeContainer.Exceptions.Count > 0)
			{
				throw new Exception($"Found {bytecodeContainer.Exceptions.Count} exception parsing",
					bytecodeContainer.Exceptions.First());
			}
			Assert.That(string.IsNullOrEmpty(bytecodeContainer.ParseErrors), bytecodeContainer.ParseErrors);
			Assert.That(!result.Contains("Unread Memory"), "Unread memory found");
		}

		/// <summary>
		/// Compare with actual Direct3D reflected objects.
		/// </summary>
		[TestCaseSource("TestShaders")]
		public void ShaderReflectionMatchesDirect3DReflection(string relPath)
		{
			// Arrange.
			var file = $"{ShaderDirectory}/{relPath}";
			var binaryFileBytes = File.ReadAllBytes(file + ".o");

			// Act.
			if(binaryFileBytes[0] == 0x01 &&
				binaryFileBytes[1] == 0x20 &&
				binaryFileBytes[2] == 0xFF &&
				binaryFileBytes[3] == 0xFE)
			{
				Effects11.CompareEffect(null, binaryFileBytes, Path.GetFileNameWithoutExtension(relPath));
				return;
			}
			var container = BytecodeContainer.Parse(binaryFileBytes);
			if(container.Chunks.OfType<LibHeaderChunk>().Any())
			{
				CompareLibrary(container, binaryFileBytes);
				return;
			}
			if(container.Chunks.OfType<EffectChunk>().Any())
			{
				Effects10.CompareEffect(container, binaryFileBytes, Path.GetFileNameWithoutExtension(relPath));
				return;
			}
			using(var shaderBytecode = ShaderBytecode.FromStream(new MemoryStream(binaryFileBytes)))
			using(var shaderReflection = new ShaderReflection(shaderBytecode))
			{
				var desc = shaderReflection.Description;

				// Assert.
				Assert.AreEqual(shaderReflection.BitwiseInstructionCount, 0); // TODO
				Assert.AreEqual(shaderReflection.ConditionalMoveInstructionCount, container.Statistics.MovCInstructionCount);
				Assert.AreEqual(shaderReflection.ConversionInstructionCount, container.Statistics.ConversionInstructionCount);
				Assert.AreEqual((int)shaderReflection.GeometryShaderSInputPrimitive, (int)container.Statistics.InputPrimitive);
				Assert.AreEqual(shaderReflection.InterfaceSlotCount, container.ResourceDefinition.InterfaceSlotCount);
				Assert.AreEqual((bool)shaderReflection.IsSampleFrequencyShader, container.Statistics.IsSampleFrequencyShader);
				Assert.AreEqual(shaderReflection.MoveInstructionCount, container.Statistics.MovInstructionCount);

				var flags = ShaderRequiresFlags.None;
				if(container.Version.MajorVersion >= 5)
				{
					if(container.Sfi0 != null)
					{
						flags = (ShaderRequiresFlags)container.Sfi0.Flags;
					}
					else
					{
						var dcl = container.Shader.DeclarationTokens
							.OfType<GlobalFlagsDeclarationToken>()
							.FirstOrDefault();
						var globals = dcl?.Flags ?? 0;
						flags = (ShaderRequiresFlags)Chunks.Sfi0.Sfi0Chunk.GlobalFlagsToRequireFlags(globals);
					}
				}
				Assert.AreEqual(shaderReflection.RequiresFlags, flags);

				int expectedSizeX, expectedSizeY, expectedSizeZ;
				uint actualSizeX, actualSizeY, actualSizeZ;
				shaderReflection.GetThreadGroupSize(out expectedSizeX, out expectedSizeY, out expectedSizeZ);
				container.Shader.GetThreadGroupSize(out actualSizeX, out actualSizeY, out actualSizeZ);
				Assert.AreEqual(expectedSizeX, actualSizeX);
				Assert.AreEqual(expectedSizeY, actualSizeY);
				Assert.AreEqual(expectedSizeZ, actualSizeZ);


				SharpDX.Direct3D.FeatureLevel featureLevel = 0;
				if(container.Chunks.OfType<Chunks.Aon9.Level9ShaderChunk>().Any())
				{
					var level9Chunk = container.Chunks
						.OfType<Chunks.Aon9.Level9ShaderChunk>()
						.First();
					featureLevel = level9Chunk.ShaderModel.MinorVersion == 1
						? SharpDX.Direct3D.FeatureLevel.Level_9_3 :
						SharpDX.Direct3D.FeatureLevel.Level_9_1;
				}
				else if(container.Version.MajorVersion == 4 && container.Version.MinorVersion == 0)
				{
					featureLevel = SharpDX.Direct3D.FeatureLevel.Level_10_0;
				}
				else if(container.Version.MajorVersion == 4 && container.Version.MinorVersion == 1)
				{
					featureLevel = SharpDX.Direct3D.FeatureLevel.Level_10_1;
				}
				else if(container.Version.MajorVersion == 5)
				{
					featureLevel = SharpDX.Direct3D.FeatureLevel.Level_11_0;
					if(flags.HasFlag(ShaderRequiresFlags.ShaderRequires64UnorderedAccessViews))
					{
						featureLevel = SharpDX.Direct3D.FeatureLevel.Level_11_1;
					}
				}

				Assert.AreEqual(shaderReflection.MinFeatureLevel, featureLevel); // TODO

				Assert.AreEqual(desc.ArrayInstructionCount, container.Statistics.ArrayInstructionCount);
				Assert.AreEqual(desc.BarrierInstructions, container.Statistics.BarrierInstructions);
				Assert.AreEqual(desc.BoundResources, container.ResourceDefinition.ResourceBindings.Count);
				Assert.AreEqual(desc.ConstantBuffers, container.ResourceDefinition.ConstantBuffers.Count);
				Assert.AreEqual(desc.ControlPoints, container.Statistics.ControlPoints);
				Assert.AreEqual(desc.Creator, container.ResourceDefinition.Creator);
				Assert.AreEqual(desc.CutInstructionCount, container.Statistics.CutInstructionCount);
				Assert.AreEqual(desc.DeclarationCount, container.Statistics.DeclarationCount);
				Assert.AreEqual(desc.DefineCount, container.Statistics.DefineCount);
				Assert.AreEqual(desc.DynamicFlowControlCount, container.Statistics.DynamicFlowControlCount);
				Assert.AreEqual(desc.EmitInstructionCount, container.Statistics.EmitInstructionCount);
				Assert.AreEqual((int)desc.Flags, (int)container.ResourceDefinition.Flags);
				Assert.AreEqual(desc.FloatInstructionCount, container.Statistics.FloatInstructionCount);
				Assert.AreEqual(desc.GeometryShaderInstanceCount, container.Statistics.GeometryShaderInstanceCount);
				Assert.AreEqual(desc.GeometryShaderMaxOutputVertexCount, container.Statistics.GeometryShaderMaxOutputVertexCount);
				Assert.AreEqual((int)desc.GeometryShaderOutputTopology, (int)container.Statistics.GeometryShaderOutputTopology);
				Assert.AreEqual((int)desc.HullShaderOutputPrimitive, (int)container.Statistics.HullShaderOutputPrimitive);
				Assert.AreEqual((int)desc.HullShaderPartitioning, (int)container.Statistics.HullShaderPartitioning);
				Assert.AreEqual(desc.InputParameters, container.InputSignature.Parameters.Count);
				Assert.AreEqual((int)desc.InputPrimitive, (int)container.Statistics.InputPrimitive);
				Assert.AreEqual(desc.InstructionCount, container.Statistics.InstructionCount);
				Assert.AreEqual(desc.InterlockedInstructions, container.Statistics.InterlockedInstructions);
				Assert.AreEqual(desc.IntInstructionCount, container.Statistics.IntInstructionCount);
				Assert.AreEqual(desc.MacroInstructionCount, container.Statistics.MacroInstructionCount);
				Assert.AreEqual(desc.OutputParameters, container.OutputSignature.Parameters.Count);
				Assert.AreEqual(desc.PatchConstantParameters, (container.PatchConstantSignature != null)
					? container.PatchConstantSignature.Parameters.Count
					: 0);
				Assert.AreEqual(desc.StaticFlowControlCount, container.Statistics.StaticFlowControlCount);
				Assert.AreEqual(desc.TempArrayCount, container.Statistics.TempArrayCount);
				Assert.AreEqual(desc.TempRegisterCount, container.Statistics.TempRegisterCount);
				Assert.AreEqual((int)desc.TessellatorDomain, (int)container.Statistics.TessellatorDomain);
				Assert.AreEqual(desc.TextureBiasInstructions, container.Statistics.TextureBiasInstructions);
				Assert.AreEqual(desc.TextureCompInstructions, container.Statistics.TextureCompInstructions);
				Assert.AreEqual(desc.TextureGradientInstructions, container.Statistics.TextureGradientInstructions);
				Assert.AreEqual(desc.TextureLoadInstructions, container.Statistics.TextureLoadInstructions);
				Assert.AreEqual(desc.TextureNormalInstructions, container.Statistics.TextureNormalInstructions);
				Assert.AreEqual(desc.TextureStoreInstructions, container.Statistics.TextureStoreInstructions);
				Assert.AreEqual(desc.UintInstructionCount, container.Statistics.UIntInstructionCount);

				var version = Chunks.Common.ShaderVersion.FromShexToken((uint)desc.Version);
				Assert.AreEqual(version.ToString(), container.ResourceDefinition.Target.ToString());

				for(int i = 0; i < shaderReflection.Description.ConstantBuffers; i++)
					CompareConstantBuffer(shaderReflection.GetConstantBuffer(i),
						container.ResourceDefinition.ConstantBuffers[i]);

				for(int i = 0; i < shaderReflection.Description.BoundResources; i++)
					CompareResourceBinding(shaderReflection.GetResourceBindingDescription(i),
						container.ResourceDefinition.ResourceBindings[i]);

				for(int i = 0; i < shaderReflection.Description.InputParameters; i++)
					CompareParameter(shaderReflection.GetInputParameterDescription(i),
						container.InputSignature.Parameters[i]);

				for(int i = 0; i < shaderReflection.Description.OutputParameters; i++)
					CompareParameter(shaderReflection.GetOutputParameterDescription(i),
						container.OutputSignature.Parameters[i]);

				for(int i = 0; i < shaderReflection.Description.PatchConstantParameters; i++)
					CompareParameter(shaderReflection.GetPatchConstantParameterDescription(i),
						container.PatchConstantSignature.Parameters[i]);
			}
		}
		private static void CompareLibrary(BytecodeContainer container, byte[] shaderBytecode)
		{
			var libReflection = new LibraryReflection(shaderBytecode);
			var libHeader = container.Chunks.OfType<LibHeaderChunk>().First();
			var desc = libReflection.Description;
			Assert.AreEqual(desc.Creator, libHeader.CreatorString);
			Assert.AreEqual(desc.FunctionCount, libHeader.FunctionDescs.Count);
			Assert.AreEqual(desc.Flags, 0);
		}

		private static void CompareConstantBuffer(SharpDX.D3DCompiler.ConstantBuffer expected, Chunks.Rdef.ConstantBuffer actual)
		{
			Assert.AreEqual((int)expected.Description.Flags, (int)actual.Flags);
			Assert.AreEqual(expected.Description.Name, actual.Name);
			Assert.AreEqual(expected.Description.Size, actual.Size);
			Assert.AreEqual((int)expected.Description.Type, (int)actual.BufferType);
			Assert.AreEqual(expected.Description.VariableCount, actual.Variables.Count);

			for(int i = 0; i < expected.Description.VariableCount; i++)
				CompareConstantBufferVariable(expected.GetVariable(i), actual.Variables[i]);
		}

		private static void CompareConstantBufferVariable(ShaderReflectionVariable expected,
			ShaderVariable actual)
		{
			//Assert.AreEqual(expected.Description.DefaultValue, actual.DefaultValue); // TODO
			Assert.AreEqual((int)expected.Description.Flags, (int)actual.Flags);
			Assert.AreEqual(expected.Description.Name, actual.Name);
			Assert.AreEqual(expected.Description.SamplerSize, actual.SamplerSize);
			Assert.AreEqual(expected.Description.Size, actual.Size);
			Assert.AreEqual((uint)expected.Description.StartOffset, actual.StartOffset);
			if(expected.Description.StartSampler != -1 && actual.StartSampler != 0)
				Assert.AreEqual(expected.Description.StartSampler, actual.StartSampler);
			if(expected.Description.StartTexture != -1 && actual.StartTexture != 0)
				Assert.AreEqual(expected.Description.StartTexture, actual.StartTexture);
			Assert.AreEqual(expected.Description.TextureSize, actual.TextureSize);

			CompareConstantBufferVariableType(expected.GetVariableType(), actual.ShaderType);
		}

		private static void CompareConstantBufferVariableType(ShaderReflectionType expected,
			ShaderType actual)
		{
			Assert.AreEqual(expected.BaseClass?.Description.Name, actual.BaseClass?.BaseTypeName);
			if(expected.BaseClass != null && actual.BaseClass != null)
			{
				CompareConstantBufferVariableType(expected.BaseClass, actual.BaseClass);
			}
			Assert.AreEqual(expected.SubType?.Description.Name, actual.SubType?.BaseTypeName);
			if(expected.SubType != null && actual.SubType != null)
			{
				CompareConstantBufferVariableType(expected.SubType, actual.SubType);
			}
			Assert.AreEqual((int)expected.Description.Class, (int)actual.VariableClass);
			Assert.AreEqual(expected.Description.ColumnCount, actual.Columns);
			Assert.AreEqual(expected.Description.ElementCount, actual.ElementCount);
			Assert.AreEqual(expected.Description.MemberCount, actual.Members.Count);
			Assert.AreEqual(expected.Description.Name, actual.BaseTypeName);
			Assert.AreEqual(expected.Description.Offset, 0); // TODO
			Assert.AreEqual(expected.Description.RowCount, actual.Rows);
			Assert.AreEqual((int)expected.Description.Type, (int)actual.VariableType);
			Assert.AreEqual(expected.NumInterfaces, actual.Interfaces.Count);
			for(int i = 0; i < expected.NumInterfaces && i < actual.Interfaces.Count; i++)
			{
				var expectedInterface = expected.GetInterface(i);
				CompareConstantBufferVariableType(expectedInterface, actual.Interfaces[i]);
			}
		}

		private static void CompareParameter(ShaderParameterDescription expected,
			SignatureParameterDescription actual)
		{
			Assert.AreEqual((int)expected.ComponentType, (int)actual.ComponentType);
			//Assert.AreEqual((int) expected.ReadWriteMask, (int) actual.ReadWriteMask); // TODO: Bug in SharpDX?
			if(expected.Register != -1 || actual.Register != uint.MaxValue)
				Assert.AreEqual(expected.Register, actual.Register);
			Assert.AreEqual(expected.SemanticIndex, actual.SemanticIndex);
			Assert.AreEqual(expected.SemanticName, actual.SemanticName);
			Assert.AreEqual(expected.Stream, actual.Stream);
			Assert.AreEqual((int)expected.SystemValueType, (int)actual.SystemValueType);
			Assert.AreEqual((int)expected.UsageMask, (int)actual.Mask);
		}

		private static void CompareResourceBinding(InputBindingDescription expected,
			ResourceBinding actual)
		{
			Assert.AreEqual((uint)expected.BindCount, actual.BindCount);
			Assert.AreEqual(expected.BindPoint, actual.BindPoint);
			Assert.AreEqual((int)expected.Dimension, (int)actual.Dimension);
			Assert.AreEqual((int)expected.Flags, (int)actual.Flags);
			Assert.AreEqual(expected.Name, actual.Name);
			if(expected.NumSamples != -1 || actual.NumSamples != uint.MaxValue)
				Assert.AreEqual(expected.NumSamples, actual.NumSamples);
			Assert.AreEqual((int)expected.ReturnType, (int)actual.ReturnType);
			Assert.AreEqual((int)expected.Type, (int)actual.Type);
		}
	}
}