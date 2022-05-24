using DXDecompiler.DX9Shader.Bytecode.Ctab;
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
				case ParameterType.TextureCube:
					return "textureCUBE";
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
		public static bool ReturnsScalar(this Opcode opcode)
		{
			switch(opcode)
			{
				// TODO: there could be more parallel instructions
				case Opcode.DP2Add:
				case Opcode.Dp3:
				case Opcode.Dp4:
					return true;
				default:
					return false;
			}
		}
		/// <summary>
		/// Check if the instruction is "parallel", that is, 
		/// each component will be processed independently without affecting other components.
		/// <br/>
		/// This is useful for clipping source swizzles with destination write masks:
		/// Since the instruction is parallel, it's safe to clip source swizzles as they will not have any effect.
		/// </summary>
		/// <param name="opcode">The instruction to be checked</param>
		/// <param name="shader">The current shader</param>
		/// <returns>true if instruction is parallel</returns>
		public static bool IsParallel(this Opcode opcode, ShaderModel shader)
		{
			switch(opcode)
			{
				// TODO: there could be more parallel instructions
				case Opcode.Abs:
				case Opcode.Add:
				case Opcode.Cmp:
				case Opcode.Cnd when shader is { MajorVersion: 1, MinorVersion: >= 4 } or { MajorVersion: > 1 }:
				case Opcode.DSX:
				case Opcode.DSY:
				case Opcode.Exp:
				case Opcode.ExpP:
				case Opcode.Frc:
				case Opcode.Log:
				case Opcode.LogP:
				case Opcode.Lrp:
				case Opcode.Mad:
				case Opcode.Max:
				case Opcode.Min:
				case Opcode.Mov:
				case Opcode.MovA when shader.MajorVersion >= 2:
				case Opcode.Mul:
				case Opcode.Pow:
				case Opcode.Rcp:
				case Opcode.Rsq:
				case Opcode.SetP:
				case Opcode.Sge:
				case Opcode.Sgn:
				case Opcode.Slt:
				case Opcode.Sub:
				case Opcode.TexKill:
					return true;
				default:
					return false;
			}
		}
		/// <summary>
		/// Get the size of instruction in shader model 1.
		/// </summary>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <returns>The number of tokens inside this instruction.</returns>
		public static int GetShaderModel1OpcodeSize(this Opcode opcode, int minorVersion)
		{
			switch(opcode)
			{
				case Opcode.Abs:
					return 2;
				case Opcode.Add:
					return 3;
				case Opcode.Bem:
					return 3;
				case Opcode.Breakp:
					return 1;
				case Opcode.Cmp:
					return 4;
				case Opcode.Cnd:
					return 4;
				case Opcode.Crs:
					return 3;
				case Opcode.Dcl:
					return 2;
				case Opcode.Def:
					return 5;
				case Opcode.DefB:
					return 2;
				case Opcode.DefI:
					return 5;
				case Opcode.Dp3:
					return 3;
				case Opcode.Dp4:
					return 3;
				case Opcode.Dst:
					return 3;
				case Opcode.Exp:
					return 2;
				case Opcode.ExpP:
					return 2;
				case Opcode.Frc:
					return 2;
				case Opcode.Lit:
					return 2;
				case Opcode.Log:
					return 2;
				case Opcode.LogP:
					return 2;
				case Opcode.Lrp:
					return 4;
				case Opcode.M3x2:
					return 3;
				case Opcode.M3x3:
					return 3;
				case Opcode.M3x4:
					return 3;
				case Opcode.M4x3:
					return 3;
				case Opcode.M4x4:
					return 3;
				case Opcode.Mad:
					return 4;
				case Opcode.Max:
					return 3;
				case Opcode.Min:
					return 3;
				case Opcode.Mov:
					return 2;
				case Opcode.Mul:
					return 3;
				case Opcode.Nop:
					return 0;
				case Opcode.Nrm:
					return 2;
				case Opcode.Phase:
					return 0;
				case Opcode.Pow:
					return 3;
				case Opcode.Rcp:
					return 2;
				case Opcode.Rsq:
					return 2;
				case Opcode.SetP:
					return 3;
				case Opcode.Sge:
					return 3;
				case Opcode.Slt:
					return 3;
				case Opcode.Sub:
					return 3;
				case Opcode.Tex:
					if(minorVersion < 4)
					{
						return 1;
					}
					else
					{
						return 2;
					}
				case Opcode.TexBem:
					return 2;
				case Opcode.TexBeml:
					return 2;
				case Opcode.TexCoord:
					if(minorVersion < 4)
					{
						return 1;
					}
					else
					{
						return 2;
					}
				case Opcode.TexDepth:
					return 1;
				case Opcode.TexDP3:
					return 2;
				case Opcode.TexDP3Tex:
					return 2;
				case Opcode.TexKill:
					return 1;
				case Opcode.TexM3x2Depth:
					return 2;
				case Opcode.TeXM3x2Pad:
					return 2;
				case Opcode.TexM3x2Tex:
					return 2;
				case Opcode.TexM3x3:
					return 2;
				case Opcode.TexM3x3Diff:
					return 2;
				case Opcode.TeXM3x3Pad:
					return 2;
				case Opcode.TexM3x3Spec:
					return 3;
				case Opcode.TexM3x3Tex:
					return 2;
				case Opcode.TexM3x3VSpec:
					return 2;
				case Opcode.TexReg2AR:
					return 2;
				case Opcode.TexReg2GB:
					return 2;
				case Opcode.TexReg2RGB:
					return 2;
				case Opcode.End:
					return 0;
				default:
					throw new NotImplementedException(opcode.ToString());
			}
		}
	}
}
