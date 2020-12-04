using System;

namespace DXDecompiler.DX9Shader
{
	public static class Statistics
	{
		[Flags]
		public enum SlotType
		{
			None = 0,
			Setup = 1,
			Arithmetic = 2,
			Texture = 4,
			FlowControl = 8,
			New = 16
		}
		//Refer https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx9-graphics-reference-asm-vs-instructions-vs-3-0
		//Refer https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx9-graphics-reference-asm-ps-instructions-ps-3-0
		public static int GetInstructionSlotCount(this InstructionToken token)
		{
			switch(token.Opcode)
			{
				case Opcode.Dcl:
				case Opcode.Def:
				case Opcode.DefB:
				case Opcode.DefI:
				case Opcode.Label:
				case Opcode.Comment:
				case Opcode.End:
					return 0;
				case Opcode.Abs:
				case Opcode.Add:
				case Opcode.Break:
				case Opcode.Dp3:
				case Opcode.Dp4:
				case Opcode.Dst:
				case Opcode.Else:
				case Opcode.Endif:
				case Opcode.Exp:
				case Opcode.Frc:
				case Opcode.Log:
				case Opcode.LogP:
				case Opcode.Mad:
				case Opcode.Max:
				case Opcode.Min:
				case Opcode.Mov:
				case Opcode.MovA:
				case Opcode.Mul:
				case Opcode.Nop:
				case Opcode.Rcp:
				case Opcode.Ret:
				case Opcode.Rsq:
				case Opcode.SetP:
				case Opcode.Sge:
				case Opcode.Slt:
				case Opcode.Sub:
				case Opcode.Cmp:
					return 1;
				case Opcode.Lrp:
					return 1;
				case Opcode.Call:
				case Opcode.Crs:
				case Opcode.EndLoop:
				case Opcode.EndRep:
				case Opcode.M3x2:
				case Opcode.DP2Add:
				case Opcode.DSX:
				case Opcode.DSY:
				case Opcode.TexKill:
					return 2;
				case Opcode.CallNZ:
				case Opcode.BreakC:
				case Opcode.Breakp:
				case Opcode.If:
				case Opcode.IfC:
				case Opcode.Lit:
				case Opcode.Loop:
				case Opcode.M3x3:
				case Opcode.M4x3:
				case Opcode.Nrm:
				case Opcode.Pow:
				case Opcode.Rep:
				case Opcode.Sgn:
				case Opcode.TexLDD:
					return 3;
				case Opcode.M3x4:
				case Opcode.M4x4:
					return 4;
				//TODO: can't find opcode
				//case Opcode.TexLdb:
				//	return 6
				case Opcode.SinCos:
					return 8;
				case Opcode.TexLDL:
					{
						var textureType = token.GetDeclSamplerTextureType();
						if(textureType == SamplerTextureType.Cube)
						{
							return 5;
						}
						else
						{
							return 2;
						}
					}
				//TexLD
				case Opcode.Tex:
					{
						var textureType = token.GetDeclSamplerTextureType();
						if(textureType == SamplerTextureType.Cube)
						{
							return 4;
						}
						else
						{
							return 1;
						}
					}
				//Guessing
				case Opcode.ExpP:
					return 1;
				default:
					throw new NotImplementedException($"Instruction not implemented {token.Opcode}");
			}
		}
		public static SlotType GetSlotType(this Token token)
		{
			switch(token.Opcode)
			{
				case Opcode.Nop:
					return SlotType.Arithmetic;
				case Opcode.Mov:
					return SlotType.Arithmetic;
				case Opcode.Add:
					return SlotType.Arithmetic;
				case Opcode.Sub:
					return SlotType.Arithmetic;
				case Opcode.Mad:
					return SlotType.Arithmetic;
				case Opcode.Mul:
					return SlotType.Arithmetic;
				case Opcode.Rcp:
					return SlotType.Arithmetic;
				case Opcode.Rsq:
					return SlotType.Arithmetic;
				case Opcode.Dp3:
					return SlotType.Arithmetic;
				case Opcode.Dp4:
					return SlotType.Arithmetic;
				case Opcode.Min:
					return SlotType.Arithmetic;
				case Opcode.Max:
					return SlotType.Arithmetic;
				case Opcode.Slt:
					return SlotType.Arithmetic;
				case Opcode.Sge:
					return SlotType.Arithmetic;
				case Opcode.Exp:
					return SlotType.Arithmetic;
				case Opcode.Log:
					return SlotType.Arithmetic;
				case Opcode.Lit:
					return SlotType.Arithmetic;
				case Opcode.Dst:
					return SlotType.Arithmetic;
				case Opcode.Lrp:
					return SlotType.Arithmetic;
				case Opcode.Frc:
					return SlotType.Arithmetic;
				case Opcode.M4x4:
					return SlotType.Arithmetic;
				case Opcode.M4x3:
					return SlotType.Arithmetic;
				case Opcode.M3x4:
					return SlotType.Arithmetic;
				case Opcode.M3x3:
					return SlotType.Arithmetic;
				case Opcode.M3x2:
					return SlotType.Arithmetic;
				case Opcode.Call:
					return SlotType.FlowControl;
				case Opcode.CallNZ:
					return SlotType.FlowControl;
				case Opcode.Loop:
					return SlotType.FlowControl;
				case Opcode.Ret:
					return SlotType.FlowControl;
				case Opcode.EndLoop:
					//Note endloop in ps is both FlowControl and New
					return SlotType.FlowControl;
				case Opcode.Label:
					return SlotType.FlowControl;
				case Opcode.Dcl:
					//Note: dcl_samplerType is both setup and new
					return SlotType.Setup;
				case Opcode.Pow:
					return SlotType.Arithmetic;
				case Opcode.Crs:
					return SlotType.Arithmetic;
				case Opcode.Sgn:
					return SlotType.Arithmetic;
				case Opcode.Abs:
					return SlotType.Arithmetic;
				case Opcode.Nrm:
					return SlotType.Arithmetic;
				case Opcode.SinCos:
					return SlotType.Arithmetic;
				case Opcode.Rep:
					return SlotType.FlowControl;
				case Opcode.EndRep:
					return SlotType.FlowControl;
				case Opcode.If:
					return SlotType.FlowControl;
				case Opcode.IfC:
					return SlotType.FlowControl;
				case Opcode.Else:
					return SlotType.FlowControl;
				case Opcode.Endif:
					return SlotType.FlowControl;
				case Opcode.Break:
					return SlotType.FlowControl;
				case Opcode.BreakC:
					return SlotType.FlowControl;
				case Opcode.MovA:
					return SlotType.Arithmetic;
				case Opcode.DefB:
					return SlotType.Setup;
				case Opcode.DefI:
					return SlotType.Setup;
				case Opcode.TexCoord:
					return SlotType.Texture;
				case Opcode.TexKill:
					return SlotType.Texture;
				case Opcode.Tex:
					return SlotType.Texture;
				case Opcode.TexBem:
					return SlotType.Texture;
				case Opcode.TexBeml:
					return SlotType.Texture;
				case Opcode.TexReg2AR:
					return SlotType.Texture;
				case Opcode.TexReg2GB:
					return SlotType.Texture;
				case Opcode.TeXM3x2Pad:
					return SlotType.Texture;
				case Opcode.TexM3x2Tex:
					return SlotType.Texture;
				case Opcode.TeXM3x3Pad:
					return SlotType.Texture;
				case Opcode.TexM3x3Tex:
					return SlotType.Texture;
				case Opcode.TexM3x3Diff:
					return SlotType.Texture;
				case Opcode.TexM3x3Spec:
					return SlotType.Texture;
				case Opcode.TexM3x3VSpec:
					return SlotType.Texture;
				case Opcode.ExpP:
					return SlotType.Arithmetic;
				case Opcode.LogP:
					return SlotType.Arithmetic;
				case Opcode.Cnd:
					return SlotType.None;
				case Opcode.Def:
					return SlotType.Setup; ;
				case Opcode.TexReg2RGB:
					return SlotType.Texture;
				case Opcode.TexDP3Tex:
					return SlotType.Texture;
				case Opcode.TexM3x2Depth:
					return SlotType.Texture;
				case Opcode.TexDP3:
					return SlotType.Texture;
				case Opcode.TexM3x3:
					return SlotType.Texture;
				case Opcode.TexDepth:
					return SlotType.Texture;
				case Opcode.Cmp:
					return SlotType.Arithmetic;
				case Opcode.Bem:
					return SlotType.None;
				case Opcode.DP2Add:
					return SlotType.Arithmetic;
				case Opcode.DSX:
					return SlotType.Arithmetic;
				case Opcode.DSY:
					return SlotType.Arithmetic;
				case Opcode.TexLDD:
					return SlotType.Texture;
				case Opcode.SetP:
					return SlotType.FlowControl;
				case Opcode.TexLDL:
					return SlotType.Texture | SlotType.New;
				case Opcode.Breakp:
				default:
					return SlotType.None;
			}
		}
	}
}
