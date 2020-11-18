using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Describes a descriptor table entry for the root signature
	/// Based on D3D12_DESCRIPTOR_RANGE and D3D12_DESCRIPTOR_RANGE1.
	/// </summary>
	public class DescriptorRange
	{
		public DescriptorRangeType RangeType { get; private set; }
		/// <summary>
		/// Unbounded number of descriptions represented with 0xFFFFFFFF (uint.Max)
		/// </summary>
		public uint NumDescriptors { get; private set; }
		public uint BaseShaderRegister { get; private set; }
		public uint RegisterSpace { get; private set; }
		/// <summary>
		/// Only in D3D12_DESCRIPTOR_RANGE1
		/// UAV(u5, flags=DATA_STATIC)
		/// </summary>
		public DescriptorRangeFlags Flags { get; private set; }
		/// <summary>
		/// Default unset value is represented with 0xFFFFFFFF (uint.Max)
		/// UAV(u5, offset=5)
		/// </summary>
		public uint OffsetInDescriptorsFromTableStart { get; private set; }
		public static DescriptorRange Parse(BytecodeReader rangeReader, RootSignatureVersion version)
		{
			var result = new DescriptorRange()
			{
				RangeType = (DescriptorRangeType)rangeReader.ReadUInt32(),
				NumDescriptors = rangeReader.ReadUInt32(),
				BaseShaderRegister = rangeReader.ReadUInt32(),
				RegisterSpace = rangeReader.ReadUInt32(),
			};
			if (version == RootSignatureVersion.Version1_1)
			{
				result.Flags = (DescriptorRangeFlags)rangeReader.ReadUInt32();
			}
			result.OffsetInDescriptorsFromTableStart = rangeReader.ReadUInt32();
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"\t\tRangeType {RangeType}");
			sb.AppendLine($"\t\tNumDescriptors {NumDescriptors}");
			sb.AppendLine($"\t\tBaseShaderRegister {BaseShaderRegister}");
			sb.AppendLine($"\t\tRegisterSpace {RegisterSpace}");
			sb.AppendLine($"\t\tFlags {Flags}");
			sb.AppendLine($"\t\tOffsetInDescriptorsFromTableStart {OffsetInDescriptorsFromTableStart}");
			return sb.ToString();
		}
	}
}