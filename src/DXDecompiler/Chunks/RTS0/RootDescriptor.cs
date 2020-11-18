using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Describes a CBV, SRV or UAV entry in the root signature
	/// Based on D3D12_ROOT_DESCRIPTOR and D3D12_ROOT_DESCRIPTOR1.
	/// </summary>
	public class RootDescriptor : RootParameter
	{
		public uint ShaderRegister { get; private set; }
		public uint RegisterSpace { get; private set; }
		/// <summary>
		/// Only in D3D12_ROOT_DESCRIPTOR1
		/// </summary>
		public RootDescriptorFlags Flags { get; private set; }
		public static RootDescriptor Parse(BytecodeReader descReader, RootSignatureVersion version)
		{
			var result = new RootDescriptor()
			{
				ShaderRegister = descReader.ReadUInt32(),
				RegisterSpace = descReader.ReadUInt32(),
			};
			if (version == RootSignatureVersion.Version1_1)
			{
				result.Flags = (RootDescriptorFlags)descReader.ReadUInt32();
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"\tParameterType {ParameterType}({(int)ParameterType})");
			sb.AppendLine($"\tShaderVisibility {ShaderVisibility}");
			sb.AppendLine($"\tShaderRegister {ShaderRegister}");
			sb.AppendLine($"\tRegisterSpace {RegisterSpace}");
			sb.AppendLine($"\tFlags {Flags}");
			return sb.ToString();
		}
	}
}
