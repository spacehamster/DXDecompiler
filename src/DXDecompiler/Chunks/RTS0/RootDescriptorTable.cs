using DXDecompiler.Util;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Decribes an Descripter Table entry in the root signature
	/// Based on D3D12_ROOT_DESCRIPTOR_TABLE and D3D12_ROOT_DESCRIPTOR_TABLE1.
	/// </summary>
	public class RootDescriptorTable : RootParameter
	{
		public List<DescriptorRange> DescriptorRanges { get; private set; }
		public RootDescriptorTable()
		{
			DescriptorRanges = new List<DescriptorRange>();
		}

		public new static RootDescriptorTable Parse(BytecodeReader reader, BytecodeReader tableReader, RootSignatureVersion version)
		{
			var result = new RootDescriptorTable();
			var numDescriptorRanges = tableReader.ReadUInt32();
			var descriptorRangesOffset = tableReader.ReadUInt32();
			var rangeReader = reader.CopyAtOffset((int)descriptorRangesOffset);
			for(int i = 0; i < numDescriptorRanges; i++)
			{
				result.DescriptorRanges.Add(DescriptorRange.Parse(rangeReader, version));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"\tParameterType {ParameterType}");
			sb.AppendLine($"\tShaderVisibility {ShaderVisibility}");
			for(int i = 0; i < DescriptorRanges.Count; i++)
			{
				sb.AppendLine(DescriptorRanges[i].ToString());
			}
			return sb.ToString();
		}
	}
}
