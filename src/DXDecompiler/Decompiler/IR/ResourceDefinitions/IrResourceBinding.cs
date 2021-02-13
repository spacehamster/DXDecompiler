using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using System;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public class IrResourceBinding
	{
		public string Name;
		public IrResourceKind Kind;
		public IrResourceClass Class;
		/// <summary>
		/// If the input is a texture, the return type.
		/// </summary>
		public IrShaderType ReturnType;
		/// <summary>
		/// Number of elements in the return type
		/// </summary>
		public uint Dimension;
		public uint BindPoint;

		/// <summary>
		/// Number of contiguous bind points for arrays.
		/// </summary>
		public uint BindCount;

		public string Debug;
		public IrResourceBinding(ResourceBinding binding)
		{
			Name = binding.Name;
		}
		public IrResourceBinding()
		{

		}

		public string GetBindPointDescription()
		{
			switch(Class)
			{
				case IrResourceClass.CBuffer:
					return $"cb{BindPoint}";
				case IrResourceClass.Sampler:
					return $"s{BindPoint}";
				case IrResourceClass.SRV:
					return $"t{BindPoint}";
				case IrResourceClass.UAV:
					return $"u{BindPoint}";
				default:
					throw new InvalidOperationException($"Invalid ResourceClass {Class}");
			}
		}
	}
}
