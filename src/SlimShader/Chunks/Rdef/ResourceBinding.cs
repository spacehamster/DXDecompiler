using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Diagnostics;

namespace SlimShader.Chunks.Rdef
{
	/// <summary>
	/// Roughly corresponds to the D3D11_SHADER_INPUT_BIND_DESC structure.
	/// </summary>
	public class ResourceBinding
	{
		/// <summary>
		/// Name of the shader resource.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Identifies the type of data in the resource.
		/// </summary>
		public ShaderInputType Type { get; private set; }

		/// <summary>
		/// Starting bind point.
		/// </summary>
		public uint BindPoint { get; private set; }

		/// <summary>
		/// Number of contiguous bind points for arrays.
		/// </summary>
		public uint BindCount { get; private set; }

		/// <summary>
		/// Bindpoint for SM <= 5.0, ID for SM5.1.
		/// </summary>
		public uint ID { get; private set; }

		/// <summary>
		/// Shader input-parameter options.
		/// </summary>
		public ShaderInputFlags Flags { get; private set; }

		/// <summary>
		/// Identifies the dimensions of the bound resource.
		/// </summary>
		public ShaderResourceViewDimension Dimension { get; private set; }

		/// <summary>
		/// If the input is a texture, the return type.
		/// </summary>
		public ResourceReturnType ReturnType { get; private set; }

		/// <summary>
		/// The number of samples for a multisampled texture; otherwise 0.
		/// </summary>
		public uint NumSamples { get; private set; }

		public static ResourceBinding Parse(BytecodeReader reader, BytecodeReader resourceBindingReader, ShaderVersion target)
		{
			uint nameOffset = resourceBindingReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int) nameOffset);
			var result = new ResourceBinding
			{
				Name = nameReader.ReadString(),
				Type = (ShaderInputType) resourceBindingReader.ReadUInt32(),
				ReturnType = (ResourceReturnType) resourceBindingReader.ReadUInt32(),
				Dimension = (ShaderResourceViewDimension) resourceBindingReader.ReadUInt32(),
				NumSamples = resourceBindingReader.ReadUInt32(),
				BindPoint = resourceBindingReader.ReadUInt32(),
				BindCount = resourceBindingReader.ReadUInt32(),
				Flags = (ShaderInputFlags) resourceBindingReader.ReadUInt32()
			};
			if(target.MajorVersion == 5 && target.MinorVersion == 1)
			{
				//TODO: Might be related to spacing?
				var unk0 = resourceBindingReader.ReadUInt32();
				Debug.Assert(unk0 == 0);
				result.ID = resourceBindingReader.ReadUInt32();
			} else
			{
				result.ID = result.BindPoint;
			}
			return result;
		}
		public string GetBindPointDescription()
		{
			string hlslBind;
			switch (Type)
			{
				case ShaderInputType.CBuffer:
					hlslBind = $"cb{ID}";
					break;
				case ShaderInputType.Sampler:
					hlslBind = $"s{ID}";
					break;
				case ShaderInputType.Texture:
				case ShaderInputType.Structured:
				case ShaderInputType.ByteAddress:
				case ShaderInputType.TBuffer:
					hlslBind = $"t{ID}";
					break;
				case ShaderInputType.UavRwTyped:
				case ShaderInputType.UavRwStructured:
				case ShaderInputType.UavRwByteAddress:
				case ShaderInputType.UavAppendStructured:
				case ShaderInputType.UavConsumeStructured:
				case ShaderInputType.UavRwStructuredWithCounter:
					hlslBind = $"u{ID}";
					break;
				default:
					hlslBind = $"unk{ID}";
					break;
			}
			return hlslBind;
		}
		public override string ToString()
		{
			string returnType = ReturnType.GetDescription(Type);
			if (Flags.HasFlag(ShaderInputFlags.TextureComponent0) && !Flags.HasFlag(ShaderInputFlags.TextureComponent1))
				returnType += "2";
			if (!Flags.HasFlag(ShaderInputFlags.TextureComponent0) && Flags.HasFlag(ShaderInputFlags.TextureComponent1))
				returnType += "3";
			if (Flags.HasFlag(ShaderInputFlags.TextureComponent0) && Flags.HasFlag(ShaderInputFlags.TextureComponent1))
				returnType += "4";
			string typeDescription = Type.GetDescription();
			if (Flags.HasFlag(ShaderInputFlags.ComparisonSampler))
				typeDescription += "_c";
			string hlslBindPoint = GetBindPointDescription();
			return string.Format("// {0,-30} {1,10} {2,7} {3,11} {4,14} {5,6}",
				Name, typeDescription, returnType,
				Dimension.GetDescription(Type, ReturnType) + (Dimension.IsMultiSampled() && NumSamples > 0 ? NumSamples.ToString() : string.Empty),
				hlslBindPoint, BindCount);
		}
	}
}