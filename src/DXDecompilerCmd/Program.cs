using DXDecompiler;
using DXDecompiler.DebugParser;
using DXDecompiler.DebugParser.DX9;
using DXDecompiler.DebugParser.FX9;
using DXDecompiler.Decompiler;
using DXDecompiler.Util;
using System;
using System.IO;

namespace DXDecompilerCmd
{
	class Program
	{
		public static ProgramType GetProgramType(byte[] data)
		{
			if(data.Length < 4)
			{
				return ProgramType.Unknown;
			}
			var dxbcHeader = BitConverter.ToUInt32(data, 0);
			if(dxbcHeader == "DXBC".ToFourCc())
			{
				return ProgramType.DXBC;
			}
			if(dxbcHeader == 0xFEFF2001)
			{
				return ProgramType.DXBC;
			}
			var dx9ShaderType = (DXDecompiler.DX9Shader.ShaderType)BitConverter.ToUInt16(data, 2);
			if(dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Vertex ||
				dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Pixel ||
				dx9ShaderType == DXDecompiler.DX9Shader.ShaderType.Effect)
			{
				return ProgramType.DX9;
			}
			return ProgramType.Unknown;
		}
		static StreamWriter GetStream(Options options)
		{
			if(string.IsNullOrEmpty(options.DestPath))
			{
				var sw = new StreamWriter(Console.OpenStandardOutput());
				sw.AutoFlush = true;
				Console.SetOut(sw);
				return sw;
			}
			try
			{
				return new StreamWriter(options.DestPath);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("Error creating output file");
				Console.Error.WriteLine(ex);
				Environment.Exit(1);
				return null;
			}
		}
		static void Main(string[] args)
		{
			var options = new Options();
			for(int i = 0; i < args.Length; i++)
			{
				switch(args[i])
				{
					case "-O":
						if(args.Length <= i + 1)
						{
							Console.Error.WriteLine("No output path specified");
							return;
						}
						options.DestPath = args[i + 1];
						i += 1;
						break;
					case "-a":
						options.Mode = DecompileMode.Dissassemble;
						break;
					case "-d":
						options.Mode = DecompileMode.Debug;
						break;
					case "-h":
						options.Mode = DecompileMode.DebugHtml;
						break;
					default:
						options.SourcePath = args[i];
						break;
				}
			}
			if(string.IsNullOrEmpty(options.SourcePath))
			{
				Console.Error.WriteLine("No source path specified");
				Environment.Exit(1);
			}

			byte[] data = null;
			try
			{
				data = File.ReadAllBytes(options.SourcePath);
			}
			catch(Exception ex)
			{
				Console.Error.WriteLine("Error reading source");
				Console.Error.WriteLine(ex);
				Environment.Exit(1);
			}
			var programType = GetProgramType(data);
			using(var sw = GetStream(options))
			{
				if(programType == ProgramType.Unknown)
				{
					Console.Error.WriteLine($"Unable to identify shader object format");
					Environment.Exit(1);
				}
				else if(programType == ProgramType.DXBC)
				{
					if(options.Mode == DecompileMode.Dissassemble)
					{
						var container = new BytecodeContainer(data);
						sw.Write(container.ToString());
					}
					else if(options.Mode == DecompileMode.Decompile)
					{
						var hlsl = HLSLDecompiler.Decompile(data);
						sw.Write(hlsl);
					}
					else if(options.Mode == DecompileMode.Debug)
					{
						sw.WriteLine(string.Join(" ", args));
						var shaderBytecode = DebugBytecodeContainer.Parse(data);
						var result = shaderBytecode.Dump();
						sw.Write(result);
					}
					else if(options.Mode == DecompileMode.DebugHtml)
					{
						var shaderBytecode = DebugBytecodeContainer.Parse(data);
						var result = shaderBytecode.DumpHTML();
						sw.Write(result);
					}
				}
				else if(programType == ProgramType.DX9)
				{
					if(options.Mode == DecompileMode.Dissassemble)
					{
						var disasm = DXDecompiler.DX9Shader.AsmWriter.Disassemble(data);
						sw.Write(disasm);
					}
					else if(options.Mode == DecompileMode.Decompile)
					{
						var hlsl = DXDecompiler.DX9Shader.HlslWriter.Decompile(data);
						sw.Write(hlsl);
					}
					else if(options.Mode == DecompileMode.Debug)
					{
						sw.WriteLine(string.Join(" ", args));
						var shaderType = (DXDecompiler.DX9Shader.ShaderType)BitConverter.ToUInt16(data, 2);
						if(shaderType == DXDecompiler.DX9Shader.ShaderType.Effect)
						{
							var reader = new DebugBytecodeReader(data, 0, data.Length);
							string error = "";
							try
							{
								reader.ReadByte("minorVersion");
								reader.ReadByte("majorVersion");
								reader.ReadUInt16("shaderType");
								DebugEffectChunk.Parse(reader, (uint)(data.Length - 4));
							}
							catch(Exception ex)
							{
								error = ex.ToString();
							}
							var dump = reader.DumpStructure();
							if(!string.IsNullOrEmpty(error))
							{
								dump += "\n" + error;
							}
							sw.Write(dump);
						}
						else
						{
							var reader = new DebugBytecodeReader(data, 0, data.Length);
							string error = "";
							try
							{
								DebugShaderModel.Parse(reader);
							}
							catch(Exception ex)
							{
								error = ex.ToString();
							}
							var dump = reader.DumpStructure();
							if(!string.IsNullOrEmpty(error))
							{
								dump += "\n" + error;
							}
							sw.Write(dump);
						}
					}
					else if(options.Mode == DecompileMode.DebugHtml)
					{
						var shaderType = (DXDecompiler.DX9Shader.ShaderType)BitConverter.ToUInt16(data, 2);
						if(shaderType == DXDecompiler.DX9Shader.ShaderType.Effect)
						{
							var reader = new DebugBytecodeReader(data, 0, data.Length);
							string error = "";
							try
							{
								reader.ReadByte("minorVersion");
								reader.ReadByte("majorVersion");
								reader.ReadUInt16("shaderType");
								DebugEffectChunk.Parse(reader, (uint)(data.Length - 4));
							}
							catch(Exception ex)
							{
								error = ex.ToString();
							}
							var dump = reader.DumpHtml();
							if(!string.IsNullOrEmpty(error))
							{
								dump += "\n" + error;
							}
							sw.Write(dump);
						}
						else
						{
							var reader = new DebugBytecodeReader(data, 0, data.Length);
							string error = "";
							try
							{
								DebugShaderModel.Parse(reader);
							}
							catch(Exception ex)
							{
								error = ex.ToString();
							}
							var dump = reader.DumpHtml();
							if(!string.IsNullOrEmpty(error))
							{
								dump += "\n" + error;
							}
							sw.Write(dump);
						}
					}
				}
			}
		}
	}
}
