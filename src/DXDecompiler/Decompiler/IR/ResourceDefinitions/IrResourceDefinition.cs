using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public class IrResourceDefinition
	{
		internal ResourceDefinitionChunk Chunk;

		public List<IrResourceBinding> ResourceBindings;
		public string ResourceBindingsDebug;
		public List<IrConstantBuffer> ConstantBuffers;
		public string ConstantBuffersDebug;
		public IrResourceDefinition(ResourceDefinitionChunk chunk)
		{
			Chunk = chunk;
		}
		public IrResourceDefinition()
		{

		}
	}
}
