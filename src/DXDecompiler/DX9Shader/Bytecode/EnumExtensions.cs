using System;

namespace DXDecompiler.DX9Shader
{
	public static class EnumExtensions
	{
		public static string GetDescription(this ParameterType value)
		{
			switch(value)
			{
				case ParameterType.Sampler1D:
					return "sampler1D";
				case ParameterType.Sampler2D:
					return "sampler2D";
				case ParameterType.Sampler3D:
					return "sampler3D";
				case ParameterType.SamplerCube:
					return "samplerCUBE";
				case ParameterType.Texture1D:
					return "texture1D";
				case ParameterType.Texture2D:
					return "texture2D";
				case ParameterType.Texture3D:
					return "texture3D";
				default:
					return value.ToString().ToLower();
			}
		}
		public static string GetDescription(this RegisterSet value)
		{
			switch(value)
			{
				case RegisterSet.Bool:
					return "b";
				case RegisterSet.Float4:
					return "c";
				case RegisterSet.Int4:
					return "i";
				case RegisterSet.Sampler:
					return "s";
				default:
					throw new InvalidOperationException();
			}
		}
		public static string GetDescription(this ShaderType value)
		{
			switch(value)
			{
				case ShaderType.Effect:
					return "fx";
				case ShaderType.Pixel:
					return "ps";
				case ShaderType.Vertex:
					return "vs";
				default:
					throw new InvalidOperationException();
			}
		}
		public static bool HasDestination(this Opcode opcode)
		{
			switch(opcode)
			{
				case Opcode.Abs:
				case Opcode.Add:
				case Opcode.Bem:
				case Opcode.Cmp:
				case Opcode.Cnd:
				case Opcode.Crs:
				case Opcode.Dcl:
				case Opcode.Def:
				case Opcode.DefB:
				case Opcode.DefI:
				case Opcode.DP2Add:
				case Opcode.Dp3:
				case Opcode.Dp4:
				case Opcode.Dst:
				case Opcode.DSX:
				case Opcode.DSY:
				case Opcode.Exp:
				case Opcode.ExpP:
				case Opcode.Frc:
				case Opcode.Lit:
				case Opcode.Log:
				case Opcode.LogP:
				case Opcode.Lrp:
				case Opcode.M3x2:
				case Opcode.M3x3:
				case Opcode.M3x4:
				case Opcode.M4x3:
				case Opcode.M4x4:
				case Opcode.Mad:
				case Opcode.Max:
				case Opcode.Min:
				case Opcode.Mov:
				case Opcode.MovA:
				case Opcode.Mul:
				case Opcode.Nrm:
				case Opcode.Pow:
				case Opcode.Rcp:
				case Opcode.Rsq:
				case Opcode.SetP:
				case Opcode.Sge:
				case Opcode.Sgn:
				case Opcode.SinCos:
				case Opcode.Slt:
				case Opcode.Sub:
				case Opcode.Tex:
				case Opcode.TexBem:
				case Opcode.TexBeml:
				case Opcode.TexCoord:
				case Opcode.TexDepth:
				case Opcode.TexDP3:
				case Opcode.TexDP3Tex:
				case Opcode.TexKill:
				case Opcode.TexLDD:
				case Opcode.TexLDL:
				case Opcode.TexM3x2Depth:
				case Opcode.TeXM3x2Pad:
				case Opcode.TexM3x2Tex:
				case Opcode.TexM3x3:
				case Opcode.TexM3x3Diff:
				case Opcode.TeXM3x3Pad:
				case Opcode.TexM3x3Spec:
				case Opcode.TexM3x3Tex:
				case Opcode.TexM3x3VSpec:
				case Opcode.TexReg2AR:
				case Opcode.TexReg2GB:
				case Opcode.TexReg2RGB:
					return true;
				default:
					return false;
			}
		}
		public static bool IsTextureOperation(this Opcode opcode)
		{
			switch(opcode)
			{
				case Opcode.Tex:
				case Opcode.TexBem:
				case Opcode.TexBeml:
				case Opcode.TexCoord:
				case Opcode.TexDepth:
				case Opcode.TexDP3:
				case Opcode.TexDP3Tex:
				case Opcode.TexKill:
				case Opcode.TexLDD:
				case Opcode.TexLDL:
				case Opcode.TexM3x2Depth:
				case Opcode.TeXM3x2Pad:
				case Opcode.TexM3x2Tex:
				case Opcode.TexM3x3:
				case Opcode.TexM3x3Diff:
				case Opcode.TeXM3x3Pad:
				case Opcode.TexM3x3Spec:
				case Opcode.TexM3x3Tex:
				case Opcode.TexM3x3VSpec:
				case Opcode.TexReg2AR:
				case Opcode.TexReg2GB:
				case Opcode.TexReg2RGB:
					return true;
				default:
					return false;
			}
		}
	}
}
