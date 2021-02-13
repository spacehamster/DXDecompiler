using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Decompiler.IR;
using DXDecompiler.Decompiler.IR.ResourceDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Decompiler.DxbcParser
{
	public class ResourceDefinitionParser
	{
		public static void Parse(IrShader shader, ResourceDefinitionChunk chunk)
		{
			var rdef = new IrResourceDefinition(chunk);
			shader.ResourceDefinition = rdef;
			rdef.ResourceBindingsDebug = DisassembleBindings(chunk.ResourceBindings, chunk.Target.IsSM51);
			rdef.ResourceBindings = new List<IrResourceBinding>();
			foreach(var binding in chunk.ResourceBindings)
			{
				rdef.ResourceBindings.Add(ParseBinding(binding));
			}
			rdef.ConstantBuffersDebug = DisassembleBuffers(chunk.ConstantBuffers);
			rdef.ConstantBuffers = new List<IrConstantBuffer>();
			foreach(var cbuffer in chunk.ConstantBuffers)
			{
				rdef.ConstantBuffers.Add(ParseBuffer(cbuffer));
			}
		}

		static IrResourceBinding ParseBinding(ResourceBinding binding)
		{
			var irBinding = new IrResourceBinding()
			{
				Name = binding.Name,
				Class = GetResourceClass(binding),
				Kind = GetResourceKind(binding),
				Dimension = binding.NumSamples == uint.MaxValue ? 0 : binding.NumSamples,
				BindPoint = binding.BindPoint,
				BindCount = binding.BindCount,
				Debug = binding.ToString()
			};
			if((irBinding.Class == IrResourceClass.SRV || irBinding.Class == IrResourceClass.UAV) 
				&& irBinding.Kind != IrResourceKind.TBuffer)
			{
				irBinding.ReturnType = ParseShaderReturnType(binding);
			}
			return irBinding;
		}

		static IrShaderType ParseShaderReturnType(ResourceBinding binding)
		{
			var returnType = new IrShaderType();
			switch(binding.ReturnType)
			{
				case ResourceReturnType.Float:
					returnType.VariableType = IrShaderVariableType.F32;
					break;
				case ResourceReturnType.Double:
					returnType.VariableType = IrShaderVariableType.F64;
					break;
				case ResourceReturnType.SInt:
					returnType.VariableType = IrShaderVariableType.I32;
					break;
				case ResourceReturnType.UInt:
					returnType.VariableType = IrShaderVariableType.U32;
					break;
				case ResourceReturnType.UNorm:
					returnType.VariableType = IrShaderVariableType.UNormF32;
					break;
				case ResourceReturnType.SNorm:
					returnType.VariableType = IrShaderVariableType.SNormF32;
					break;
				case ResourceReturnType.Mixed:
					switch(binding.Type)
					{
						case ShaderInputType.Structured:
						case ShaderInputType.UavRwStructured:
						case ShaderInputType.UavAppendStructured:
						case ShaderInputType.UavConsumeStructured:
						case ShaderInputType.UavRwStructuredWithCounter:
							break;
						case ShaderInputType.ByteAddress:
						case ShaderInputType.UavRwByteAddress:
							//TODO: Byte type?
							break;
						default:
							throw new NotImplementedException();
					}
					break;
				default:
					throw new NotImplementedException();
			}
			switch(binding.Type)
			{
				case ShaderInputType.Structured:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
					returnType.VariableClass = ShaderVariableClass.Struct;
					returnType.BaseTypeName = "TODO";
					break;
				default:
					ushort componentCount = 1;
					if(binding.Flags.HasFlag(ShaderInputFlags.TextureComponent0))
					{
						componentCount += 1;
					}
					if(binding.Flags.HasFlag(ShaderInputFlags.TextureComponent1))
					{
						componentCount += 2;
					}
					returnType.Columns = componentCount;
					returnType.VariableClass = componentCount == 1 ?
						ShaderVariableClass.Scalar :
						ShaderVariableClass.Vector;
					break;
			}
			return returnType;
		}
		static IrConstantBuffer ParseBuffer(ConstantBuffer buffer)
		{
			return new IrConstantBuffer()
			{
				Name = buffer.Name,
				BufferType = buffer.BufferType,
				Variables = buffer.Variables
					.Select(v => ParseVariable(v))
					.ToList()
			};
		}

		static IrShaderVariable ParseVariable(ShaderVariable variable)
		{
			return new IrShaderVariable()
			{
				Member = ParseShaderTypeMember(variable.Member),
				BaseType = variable.BaseType,
				Size = variable.Size,
				Flags = variable.Flags,
				DefaultValue = variable.DefaultValue,
				StartTexture = variable.StartTexture,
				TextureSize = variable.TextureSize,
				StartSampler = variable.StartSampler,
				SamplerSize = variable.SamplerSize 
			};
		}

		static IrShaderTypeMember ParseShaderTypeMember(ShaderTypeMember member)
		{
			return new IrShaderTypeMember()
			{
				Name = member.Name,
				Offset = member.Offset,
				Type = ParseShaderType(member.Type)
			};
		}

		static IrShaderType ParseShaderType(ShaderType type)
		{
			if(type == null) return null;
			return new IrShaderType()
			{
				VariableClass = type.VariableClass,
				VariableType = GetShaderVariableType(type.VariableType),
				Rows = type.Rows,
				Columns = type.Columns,
				ElementCount = type.ElementCount,
				Members = type.Members
					.Select(m => ParseShaderTypeMember(m))
					.ToList(),
				SubType = ParseShaderType(type.SubType),
				BaseClass = ParseShaderType(type.BaseClass),
				Interfaces = type.Interfaces
					.Select(t => ParseShaderType(t))
					.ToList(),
			};
		}
		static IrShaderVariableType GetShaderVariableType(ShaderVariableType type)
		{
			switch(type)
			{
				case ShaderVariableType.Void:
					return IrShaderVariableType.Void;
				case ShaderVariableType.Bool:
					return IrShaderVariableType.I1;
				case ShaderVariableType.Int:
					return IrShaderVariableType.I32;
				case ShaderVariableType.Float:
					return IrShaderVariableType.F32;
				case ShaderVariableType.Double:
					return IrShaderVariableType.F64;
				case ShaderVariableType.UInt:
					return IrShaderVariableType.U32;
				case ShaderVariableType.InterfacePointer:
					return IrShaderVariableType.InterfacePointer;
				default:
					throw new ArgumentException($"Unexpected shader variable type {type}");
			}
		}
		public static string DisassembleBindings(List<ResourceBinding> bindings, bool IsSM51)
		{
			var sb = new StringBuilder();
			if(bindings.Any())
			{
				if(!IsSM51)
				{
					sb.AppendLine("// Name                                 Type  Format         Dim      HLSL Bind  Count");
					sb.AppendLine("// ------------------------------ ---------- ------- ----------- -------------- ------");
				}
				else
				{
					sb.AppendLine("// Name                                 Type  Format         Dim      ID      HLSL Bind  Count");
					sb.AppendLine("// ------------------------------ ---------- ------- ----------- ------- -------------- ------");

				}
				foreach(var resourceBinding in bindings)
					sb.AppendLine(resourceBinding.ToString());
			}
			return sb.ToString();
		}

		public static string DisassembleBuffers(List<ConstantBuffer> buffers)
		{
			return string.Join("\n", buffers.Select(b => b.ToString()));
		}

		static IrResourceClass GetResourceClass(ResourceBinding binding)
		{
			switch(binding.Type)
			{
				case ShaderInputType.CBuffer:
					return IrResourceClass.CBuffer;
				case ShaderInputType.Sampler:
					return IrResourceClass.Sampler;
				case ShaderInputType.TBuffer:
				case ShaderInputType.Texture:
				case ShaderInputType.Structured:
				case ShaderInputType.ByteAddress:
					return IrResourceClass.SRV;
				case ShaderInputType.UavRwTyped:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavRwByteAddress:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
					return IrResourceClass.UAV;
				default:
					throw new ArgumentException($"Unexpected Binding Type {binding.Type}");
			}
		}
		static IrResourceKind GetResourceKind(ResourceBinding binding)
		{
			switch(binding.Type)
			{
				case ShaderInputType.CBuffer:
					return IrResourceKind.CBuffer;
				case ShaderInputType.TBuffer:
					return IrResourceKind.TBuffer;
				case ShaderInputType.Texture:
				case ShaderInputType.UavRwTyped:
					switch(binding.Dimension)
					{
						case ShaderResourceViewDimension.Texture1D:
							return IrResourceKind.Texture1D;
						case ShaderResourceViewDimension.Texture1DArray:
							return IrResourceKind.Texture1DArray;
						case ShaderResourceViewDimension.Texture2D:
							return IrResourceKind.Texture2D;
						case ShaderResourceViewDimension.Texture2DArray:
							return IrResourceKind.Texture2DArray;
						case ShaderResourceViewDimension.Texture2DMultiSampled:
							return IrResourceKind.Texture2DMS;
						case ShaderResourceViewDimension.Texture2DMultiSampledArray:
							return IrResourceKind.Texture2DMSArray;
						case ShaderResourceViewDimension.Texture3D:
							return IrResourceKind.Texture3D;
						case ShaderResourceViewDimension.TextureCube:
							return IrResourceKind.TextureCube;
						case ShaderResourceViewDimension.TextureCubeArray:
							return IrResourceKind.TextureCubeArray;
						case ShaderResourceViewDimension.Buffer:
							return IrResourceKind.TypedBuffer;
						default:
							throw new ArgumentException($"Unexpected Binding Dimension: {binding.Dimension}");
					}
				case ShaderInputType.ByteAddress:
				case ShaderInputType.UavRwByteAddress:
					return IrResourceKind.RawBuffer;
				case ShaderInputType.Structured:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
					return IrResourceKind.StructuredBuffer;
				case ShaderInputType.Sampler:
					return IrResourceKind.Sampler;
				default:
					throw new ArgumentException($"Unexpected Binding Type: {binding.Type}");
			}
		}

	}
}
