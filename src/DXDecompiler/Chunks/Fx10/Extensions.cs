using DXDecompiler.Chunks.Fx10.Assignemnt;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Chunks.Shex;
using System;
using System.Linq;

namespace DXDecompiler.Chunks.Fx10
{
	public static class Extensions
	{
		public static Type GetAssignmentType(this EffectAssignmentType type)
		{
			return type.GetAttributeValue<AssignmentTypeAttribute, Type>((a, v) =>
			{
				if (!a.Any())
					return null;
				return a.First().Type;
			});
		}

		public static ShaderVariableType ToShaderVariableType(this EffectObjectType effectType)
		{
			switch (effectType)
			{
				case EffectObjectType.Void:
					return ShaderVariableType.Void;
				case EffectObjectType.Float:
					return ShaderVariableType.Float;
				case EffectObjectType.UInt:
					return ShaderVariableType.UInt;
				case EffectObjectType.Int:
					return ShaderVariableType.Int;
				case EffectObjectType.Bool:
					return ShaderVariableType.Bool;

				case EffectObjectType.String:
					return ShaderVariableType.String;
				case EffectObjectType.Blend:
					return ShaderVariableType.Blend;
				case EffectObjectType.DepthStencil:
					return ShaderVariableType.DepthStencil;
				case EffectObjectType.Rasterizer:
					return ShaderVariableType.Rasterizer;
				case EffectObjectType.PixelShader:
					return ShaderVariableType.PixelShader;
				case EffectObjectType.VertexShader:
					return ShaderVariableType.VertexShader;
				case EffectObjectType.GeometryShader:
					return ShaderVariableType.GeometryShader;
				case EffectObjectType.GeometryShaderWithStream:
					return ShaderVariableType.GeometryShader;
				case EffectObjectType.Texture:
					return ShaderVariableType.Texture;
				case EffectObjectType.Texture1D:
					return ShaderVariableType.Texture1D;
				case EffectObjectType.Texture1DArray:
					return ShaderVariableType.Texture1DArray;
				case EffectObjectType.Texture2D:
					return ShaderVariableType.Texture2D;
				case EffectObjectType.Texture2DArray:
					return ShaderVariableType.Texture2DArray;
				case EffectObjectType.Texture2DMultiSampled:
					return ShaderVariableType.Texture2DMultiSampled;
				case EffectObjectType.Texture2DMultiSampledArray:
					return ShaderVariableType.Texture2DMultiSampledArray;
				case EffectObjectType.Texture3D:
					return ShaderVariableType.Texture3D;
				case EffectObjectType.TextureCube:
					return ShaderVariableType.TextureCube;
				case EffectObjectType.RenderTargetView:
					return ShaderVariableType.RenderTargetView;
				case EffectObjectType.DepthStencilView:
					return ShaderVariableType.DepthStencilView;
				case EffectObjectType.Sampler:
					return ShaderVariableType.Sampler;
				case EffectObjectType.Buffer:
					return ShaderVariableType.Buffer;
				case EffectObjectType.TextureCubeArray:
					return ShaderVariableType.TextureCubeArray;
				case EffectObjectType.PixelShader5:
					return ShaderVariableType.PixelShader;
				case EffectObjectType.VertexShader5:
					return ShaderVariableType.VertexShader;
				case EffectObjectType.GeometryShader5:
					return ShaderVariableType.GeometryShader;
				case EffectObjectType.ComputeShader5:
					return ShaderVariableType.ComputeShader;
				case EffectObjectType.HullShader5:
					return ShaderVariableType.HullShader;
				case EffectObjectType.DomainShader5:
					return ShaderVariableType.DomainShader;
				//TODO: Bring RW and ReadWrite notation into alignment
				case EffectObjectType.RWTexture1D:
					return ShaderVariableType.ReadWriteTexture1D;
				case EffectObjectType.RWTexture1DArray:
					return ShaderVariableType.ReadWriteTexture1DArray;
				case EffectObjectType.RWTexture2D:
					return ShaderVariableType.ReadWriteTexture2D;
				case EffectObjectType.RWTexture2DArray:
					return ShaderVariableType.ReadWriteTexture2DArray;
				case EffectObjectType.RWTexture3D:
					return ShaderVariableType.ReadWriteTexture3D;
				case EffectObjectType.RWBuffer:
					return ShaderVariableType.ReadWriteBuffer;
				case EffectObjectType.ByteAddressBuffer:
					return ShaderVariableType.ByteAddressBuffer;
				case EffectObjectType.RWByteAddressBuffer:
					return ShaderVariableType.ReadWriteByteAddressBuffer;
				case EffectObjectType.StructuredBuffer:
					return ShaderVariableType.StructuredBuffer;
				case EffectObjectType.RWStructuredBuffer:
					return ShaderVariableType.ReadWriteStructuredBuffer;
				case EffectObjectType.AppendStructuredBuffer:
					return ShaderVariableType.AppendStructuredBuffer;
				case EffectObjectType.ConsumeStructuredBuffer:
					return ShaderVariableType.ConsumeStructuredBuffer;
				default:
					return ShaderVariableType.Void;
			}
		}
		public static bool IsArrayAssignemnt(this EffectAssignmentType assignmentType)
		{
			switch (assignmentType)
			{
				case EffectAssignmentType.BlendEnable:
				case EffectAssignmentType.SrcBlend:
				case EffectAssignmentType.DestBlend:
				case EffectAssignmentType.BlendOp:
				case EffectAssignmentType.SrcBlendAlpha:
				case EffectAssignmentType.DestBlendAlpha:
				case EffectAssignmentType.BlendOpAlpha:
				case EffectAssignmentType.RenderTargetWriteMask:
					return true;
				default:
					return false;
			}
		}
		public static NumberType ToNumberType(this EffectScalarType scalarType)
		{
			switch (scalarType)
			{
				case EffectScalarType.Bool:
					return NumberType.Bool;
				case EffectScalarType.Float:
					return NumberType.Float;
				case EffectScalarType.Int:
					return NumberType.Int;
				case EffectScalarType.UInt:
					return NumberType.UInt;
				default:
					return NumberType.Unknown;
			}
		}
	}
}
