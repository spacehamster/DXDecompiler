using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class IrPass
	{
		public enum PassType
		{
			PixelShader,
			VertexShader,
			GeometryShader,
			DomainShader,
			ComputeShader,
			FunctionBody,
			HullShaderControlPointPhase,
			HullShaderPatchConstantPhase,
			HullShaderForkPhase,
			HullShaderJoinPhase,
		}
		internal List<DeclarationToken> Declarations = new List<DeclarationToken>();
		public List<IrInstruction> Instructions = new List<IrInstruction>();
		public List<IrAttribute> Attributes = new List<IrAttribute>();
		public string Name;
		public PassType Type;
		public IrSignature InputSignature;
		public IrSignature OutputSignature;
		public IrPass(string name, PassType type)
		{
			Name = name;
			Type = type;
		}
		public IrPass(List<OpcodeToken> instructions, string name, PassType type)
		{
			Name = name;
			Type = type;
		}

	}
}
