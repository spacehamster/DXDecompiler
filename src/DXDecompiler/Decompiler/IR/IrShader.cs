using DXDecompiler.Decompiler.IR.ResourceDefinitions;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR
{
	public class IrShader
	{
		public List<IrPass> Passes = new List<IrPass>();
		public IrEffect Effect;
		public IrResourceDefinition ResourceDefinition;
		public IrInterfaceManger InterfaceManger;
		public List<IrSignature> Signatures = new List<IrSignature>();
		public List<string> PreComments = new List<string>();
		public List<string> PostComments = new List<string>();
		public IrShader()
		{
		}
	}
}
