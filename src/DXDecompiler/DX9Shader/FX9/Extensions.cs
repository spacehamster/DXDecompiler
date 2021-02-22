using DXDecompiler.DX9Shader.Bytecode.Ctab;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.DX9Shader.FX9
{
	public static class Extensions
	{
		public static bool IsSampler(this ParameterType type)
		{
			switch(type)
			{
				case ParameterType.Sampler:
				case ParameterType.Sampler1D:
				case ParameterType.Sampler2D:
				case ParameterType.Sampler3D:
				case ParameterType.SamplerCube:
					return true;
				default:
					return false;
			}
		}
		public static bool IsObjectType(this ParameterType type)
		{
			switch(type)
			{
				case ParameterType.Texture:
				case ParameterType.PixelShader:
				case ParameterType.VertexShader:
					return true;
				default:
					return false;
			}
		}
		public static bool HasVariableBlob(this ParameterType type)
		{
			switch(type)
			{
				case ParameterType.Texture:
				case ParameterType.Texture1D:
				case ParameterType.Texture2D:
				case ParameterType.Texture3D:
				case ParameterType.TextureCube:
				case ParameterType.PixelShader:
				case ParameterType.VertexShader:
				case ParameterType.String:
					return true;
				default:
					return false;
			}
		}
		public static bool HasStateBlob(this StateType type)
		{
			switch(type)
			{
				case StateType.Texture:
				case StateType.Sampler:
				case StateType.VertexShader:
				case StateType.PixelShader:
					return true;
				default:
					return false;
			}
		}
		public static bool RequiresIndex(this StateType type)
		{
			switch(type)
			{
				case StateType.ColorOp:
				case StateType.ColorArg0:
				case StateType.ColorArg1:
				case StateType.ColorArg2:
				case StateType.AlphaOp:
				case StateType.AlphaArg0:
				case StateType.AlphaArg1:
				case StateType.AlphaArg2:
				case StateType.ResultArg:
				case StateType.BumpEnvMat00:
				case StateType.BumpEnvMat01:
				case StateType.BumpEnvMat10:
				case StateType.BumpEnvMat11:
				case StateType.TexCoordIndex:
				case StateType.BumpEnvLScale:
				case StateType.BumpEnvLOffset:
				case StateType.TextureTransformFlags:
				case StateType.Constant:
				case StateType.TextureTransform:
				case StateType.LightType:
				case StateType.LightDiffuse:
				case StateType.LightSpecular:
				case StateType.LightAmbient:
				case StateType.LightPosition:
				case StateType.LightDirection:
				case StateType.LightRange:
				case StateType.LightFalloff:
				case StateType.LightAttenuation0:
				case StateType.LightAttenuation1:
				case StateType.LightAttenuation2:
				case StateType.LightTheta:
				case StateType.LightPhi:
				case StateType.LightEnable:
				case StateType.Texture:
				case StateType.AddressU:
				case StateType.AddressV:
				case StateType.AddressW:
				case StateType.BorderColor:
				case StateType.MagFilter:
				case StateType.MinFilter:
				case StateType.MipFilter:
				case StateType.MipMapLodBias:
				case StateType.MaxMipLevel:
				case StateType.MaxAnisotropy:
				case StateType.Sampler:
					return true;
				default:
					return false;
			}
		}
		public static string TryReadString(this BytecodeReader reader)
		{
			try
			{
				var length = reader.ReadUInt32();
				if(length == 0)
				{
					return "";
				}
				var bytes = reader.ReadBytes((int)length);
				return Encoding.UTF8.GetString(bytes, 0, bytes.Length - 1);
			}
			catch(Exception ex)
			{
				return "Error reading string";
			}
		}
		public static List<Number> ReadParameterValue(this Parameter parameter, BytecodeReader valueReader)
		{
			var result = new List<Number>();
			if(parameter.ParameterClass == ParameterClass.Object)
			{
				var elementCount = parameter.ElementCount == 0 ? 1 : parameter.ElementCount;
				for(int i = 0; i < elementCount; i++)
				{
					result.Add(Number.Parse(valueReader));
				}
			}
			else
			{
				var defaultValueCount = parameter.GetSize() / 4;
				for(int i = 0; i < defaultValueCount; i++)
				{
					result.Add(Number.Parse(valueReader));
				}
			}
			return result;
		}
	}
}
