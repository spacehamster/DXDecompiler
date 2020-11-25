using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DXDecompiler.Tests
{
	public class ShaderCompiler
	{
		public class CommandResult
		{
			public int ResultCode;
			public string Stdout;
			public string Stderr;
		}
		public class CompileConfig
		{
			public string Name;
			public string Profile;
			public string EntryPoint;
			public string ExtraArgs;
		}
		private static string FxcDir => $@"{TestContext.CurrentContext.TestDirectory}/FXC";
		private static string LogFilePath => $@"{TestContext.CurrentContext.TestDirectory}/CompileLog.txt";
		private static Regex FxcPragmaPatten = new Regex(
			@"#pragma FXC (\w+) (?:((?:fx|lib)_[^\s]*)|(\w+) (\w+)) ?(.*)",
			RegexOptions.Compiled);
		private static Regex ProfilePatten = new Regex(
			@"(\w+)_(\d)_(\d)",
			RegexOptions.Compiled);
		private static void Log(string msg, params object[] args)
		{
			File.AppendAllText(LogFilePath, string.Format(msg, args) + "\n");
		}
		public static IEnumerable<CompileConfig> ExtractDirectives(string filePath)
		{
			using(var file = new StreamReader(filePath))
			{
				string line = null;
				while((line = file.ReadLine()) != null)
				{
					if(!line.StartsWith("#pragma FXC"))
					{
						yield break;
					}
					var match = FxcPragmaPatten.Match(line);
					var options = new CompileConfig()
					{
						Name = match.Groups[1].Value,
						Profile = match.Groups[2].Success ? match.Groups[2].Value : match.Groups[3].Value,
						EntryPoint = match.Groups[4].Value,
						ExtraArgs = match.Groups[5].Value
					};
					if(options.Name == "" || options.Profile == "") throw new InvalidOperationException($"Could not parse FXC directive: {line}");
					yield return options;
				}
			}
		}
		public static CommandResult Command(string exe, string args, string pwd)
		{
			Log($"    Command {exe} {args}");
			var stdout = new StringBuilder();
			var stderr = new StringBuilder();
			Process process = new Process();
			process.StartInfo.FileName = exe;
			process.StartInfo.Arguments = args;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WorkingDirectory = pwd;
			process.OutputDataReceived += (sender, e) => { stdout.AppendLine(e.Data); };
			process.ErrorDataReceived += (sender, e) => { stderr.AppendLine(e.Data); };
			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.WaitForExit(30000);
			if(process.ExitCode != 0)
			{
				Log("FXC Error");
				Log($"ErrorCode: {process.ExitCode}");
				Log("STDOUT");
				Log(stdout.ToString());
				Log("STDERR");
				Log(stderr.ToString());
			}
			return new CommandResult()
			{
				ResultCode = process.ExitCode,
				Stdout = stdout.ToString(),
				Stderr = stderr.ToString()
			};
		}
		public static void CompileShader(string sourcePath, CompileConfig config)
		{
			var targetDirectory = Path.GetDirectoryName(sourcePath);
			var objectPath = Path.Combine(targetDirectory, $"{config.Name}.o");

			DateTime srcTime = File.GetLastWriteTime(sourcePath);
			DateTime dstTime = File.GetLastWriteTime(objectPath);
			if(dstTime > srcTime)
			{
				return;
			}
			var sourceName = Path.GetFileName(sourcePath);
			var fxc10 = Path.Combine(FxcDir, "fxc10.exe");

			var sb = new StringBuilder();
			sb.Append(sourceName);
			sb.AppendFormat(" /Fo {0}.o", config.Name);
			sb.AppendFormat(" /Fc {0}.asm", config.Name);
			sb.AppendFormat(" /T {0}", config.Profile);
			sb.Append(" /nologo");
			if(config.EntryPoint != "") sb.AppendFormat(" /E {0}", config.EntryPoint); 
			if(config.ExtraArgs != "") sb.AppendFormat(" {0}", config.ExtraArgs);
			var result = Command(fxc10, sb.ToString(), targetDirectory);
			if(result.ResultCode != 0)
			{
				throw new InvalidOperationException($"fxc {sb} returned error code {result.ResultCode}\n{result.Stderr}");
			}
			//FXC 10.1 cannot disassemble fx_2_0 shaders
			if(config.EntryPoint != "") return;
			var profileMatch = ProfilePatten.Match(config.Profile);
			if(profileMatch.Groups[2].Value != "2" && profileMatch.Groups[2].Value != "3") return;
			var fxc9 = Path.Combine(FxcDir, "fxc9.exe");
			sb = new StringBuilder();
			sb.AppendFormat("/dumpbin {0}.o", config.Name);
			sb.AppendFormat(" /Fc {0}.asm", config.Name);
			sb.AppendFormat(" /T {0}", config.Profile);
			sb.Append(" /nologo");
			result = Command(fxc9, sb.ToString(), targetDirectory);
			if(result.ResultCode != 0)
			{
				throw new InvalidOperationException($"fxc {sb} returned error code {result.ResultCode}\n{result.Stderr}");
			}
		}
		public static void ProcessShaders(string directory)
		{
			if(File.Exists(LogFilePath))
			{
				File.Delete(LogFilePath);
			}
			Log("Processing shaders");
			foreach(var file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
			{
				var ext = Path.GetExtension(file);
				if(ext != ".hlsl" && ext != ".fx" && ext != ".vsh" && ext != ".psh") continue;
				Log($"    Processing {file}");
				foreach(var options in ExtractDirectives(file))
				{
					CompileShader(file, options);
				}
			}
			Log($"Finished processing shaders");
		}
	}
}
