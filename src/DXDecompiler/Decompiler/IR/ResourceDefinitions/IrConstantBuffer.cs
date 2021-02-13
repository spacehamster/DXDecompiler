using DXDecompiler.Chunks.Rdef;
using System.Collections.Generic;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	/// <summary>
	/// Represents a constantbuffer or resource bind infomation
	/// https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-constants
	/// BufferType [Name] [: register(b#)] { VariableDeclaration [: packoffset(c#.xyzw)]; ... };
	/// There are two default constant buffers available, $Global and $Param
	/// Resource bind infomation represent single struct declrations to be used as concrete types by resource declrations
	/// 
	/// </summary>
	public class IrConstantBuffer
	{
		public string Debug;
		public string Name;
		public ConstantBufferType BufferType;
		public List<IrShaderVariable> Variables;
		public IrConstantBuffer()
		{

		}
	}
}
