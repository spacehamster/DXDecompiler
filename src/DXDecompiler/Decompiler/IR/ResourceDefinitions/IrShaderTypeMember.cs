using DXDecompiler.Chunks.Rdef;
using System.Linq;

namespace DXDecompiler.Decompiler.IR.ResourceDefinitions
{
	public class IrShaderTypeMember
	{
		public string Name;
		public uint Offset;
		public IrShaderType Type;

		public uint GetCBVarSize(bool wholeArraySize = false)
		{
			var member = this;
			uint size;
			if(member.Type.VariableClass == ShaderVariableClass.Struct)
			{
				if(member.Type.Members.Count == 0)
				{
					size = 0;
				}
				else
				{
					var last = member.Type.Members.Last();
					size = last.Offset + last.GetCBVarSize(true);
				}
			}
			else if(member.Type.VariableClass == ShaderVariableClass.MatrixRows)
			{
				var columns = (uint)member.Type.Columns;
				var rows = (uint)member.Type.Rows;
				size = (rows - 1) * 16 + columns * 4;
			}
			else if(member.Type.VariableClass == ShaderVariableClass.MatrixColumns)
			{
				var columns = (uint)member.Type.Columns;
				var rows = (uint)member.Type.Rows;
				size = (columns - 1) * 16 + rows * 4;
			}
			else
			{
				size = (uint)member.Type.Columns * (uint)member.Type.Rows * 4;
			}
			if(wholeArraySize && member.Type.ElementCount > 1)
			{
				uint paddedSize = ((size + 15) / 16) * 16; // Arrays are padded to float4 size
				size = ((uint)member.Type.ElementCount - 1) * paddedSize + size; // Except the last element
			}
			return size;
		}
	}
}
