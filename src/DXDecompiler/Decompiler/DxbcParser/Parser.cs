using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Fx10;
using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Decompiler.IR;
using System;
using System.Linq;

namespace DXDecompiler.Decompiler.DxbcParser
{
	public class Parser
	{
		public static IrShader Parse(BytecodeContainer container)
		{
			var shader = new IrShader();
			if(container.Version.ProgramType == ProgramType.LibraryShader)
			{
				ParseLibrary(shader, container);
				return shader;
			}
			if(container.Version.ProgramType == ProgramType.EffectsShader)
			{
				ParseEffect(shader, container);
				return shader;
			}
			ResourceDefinitionParser.Parse(shader, container.ResourceDefinition);
			if(container.Interfaces != null)
			{
				shader.InterfaceManger = new IrInterfaceManger(container.Interfaces);
			}
			if(container.InputSignature != null)
			{
				shader.Signatures.Add(new IrSignature(
					container.InputSignature,
					$"{container.Version.ProgramType}Input",
					IrSignatureType.Input));
			}
			if(container.OutputSignature != null)
			{
				shader.Signatures.Add(new IrSignature(
					container.OutputSignature,
					$"{container.Version.ProgramType}Output",
					IrSignatureType.Output));
			}
			ParseShaderChunk(shader, container.Shader);
			return shader;
		}

		/// <summary>
		/// Split instructions into seperate phases.
		/// </summary>
		/// <param name="programChunk"></param>
		static void ParseShaderChunk(IrShader shader, ShaderProgramChunk programChunk)
		{
			Func<OpcodeToken, int, bool> NotSplitToken = (OpcodeToken token, int index) =>
				index == 0 ||
				token.Header.OpcodeType != OpcodeType.HsForkPhase &&
				token.Header.OpcodeType != OpcodeType.HsJoinPhase &&
				token.Header.OpcodeType != OpcodeType.Label;
			int i = 0;
			while(i < programChunk.Tokens.Count)
			{
				var tokens = programChunk.Tokens.Skip(i).TakeWhile(NotSplitToken).ToList();
				if(i == 0)
				{
					var type = programChunk.Version.ProgramType.GetPassType();
					var pass = new IrPass(tokens, programChunk.Version.ProgramType.ToString(), type);
					InstructionParser.ParseTokens(pass, tokens);
					pass.InputSignature = shader.Signatures.First(s => s.SignatureType == IrSignatureType.Input);
					pass.OutputSignature = shader.Signatures.First(s => s.SignatureType == IrSignatureType.Output);
					shader.Passes.Add(pass);
				}
				else if(tokens[0].Header.OpcodeType == OpcodeType.Label)
				{
					var index = (tokens[0] as InstructionToken).Operands[0].Indices[0].Value;
					var pass = new IrPass($"Label{index}", IrPass.PassType.FunctionBody);
					InstructionParser.ParseTokens(pass, tokens);
					shader.Passes.Add(pass);
				}
				else if(tokens[0].Header.OpcodeType == OpcodeType.HsForkPhase)
				{
					var index = shader.Passes.Select(p => p.Type == IrPass.PassType.HullShaderForkPhase).Count();
					var pass = new IrPass($"HullForkPhase{index}", IrPass.PassType.HullShaderForkPhase);
					InstructionParser.ParseTokens(pass, tokens);
					shader.Passes.Add(pass);
				}
				else
				{
					var index = shader.Passes.Select(p => p.Type == IrPass.PassType.HullShaderJoinPhase).Count();
					var pass = new IrPass(tokens, $"HullJoinPhase{index}", IrPass.PassType.HullShaderJoinPhase);
					InstructionParser.ParseTokens(pass, tokens);
					shader.Passes.Add(pass);
				}
				i += tokens.Count;
			}
			shader.InterfaceManger?.Parse(shader, programChunk.Container);
			if(programChunk.Version.ProgramType == ProgramType.HullShader)
			{
				string patchConstantPassName = "HullPatchConstant";
				shader.Passes.Add(new IrPass("HullPatchConstant", IrPass.PassType.HullShaderPatchConstantPhase));
				var ctrlPointPass = shader.Passes.First(p => p.Type == IrPass.PassType.HullShaderControlPointPhase);
				ctrlPointPass.Attributes.Add(IrAttribute.Create("patchconstantfunc", patchConstantPassName));
			}
		}

		static void ParseLibrary(IrShader shader, BytecodeContainer container)
		{
			var libraryHeader = container.Chunks.OfType<LibHeaderChunk>().Single();
			var libraryFunctions = container.Chunks.OfType<LibfChunk>().ToArray();
			for(int i = 0; i < libraryFunctions.Length; i++)
			{
				var pass = new IrPass(libraryHeader.FunctionDescs[i].Name, IrPass.PassType.FunctionBody);
				InstructionParser.ParseTokens(pass, libraryFunctions[i].LibraryContainer.Shader.Tokens);
				shader.Passes.Add(pass);
			}
		}
		static void ParseEffect(IrShader shader, BytecodeContainer container)
		{
			var effect = container.Chunks.OfType<EffectChunk>().Single();
			shader.Effect = new IrEffect(effect);
			foreach(var variable in effect.LocalVariables)
			{
				foreach(var effectShader in variable.ShaderData)
				{
					var chunk = effectShader.Shader?.Shader;
					if(chunk == null) continue;
					//shader.Passes.Add(new IrPass(chunk));
				}
				foreach(var effectShader in variable.ShaderData5)
				{
					var chunk = effectShader.Shader.Shader;
					//shader.Passes.Add(new IrPass(chunk));
				}
			}
		}
	}
}
