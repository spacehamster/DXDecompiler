using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// Describes a Root Constants entry in the root signature
	/// Based on D3D12_ROOT_CONSTANTS.
	/// </summary>
	public class RootConstants : RootParameter
	{
		public uint ShaderRegister { get; private set; }
		public uint RegisterSpace { get; private set; }
		public uint Num32BitValues { get; private set; }
		public static RootConstants Parse(BytecodeReader constantsReader)
		{
			return new RootConstants()
			{
				ShaderRegister = constantsReader.ReadUInt32(),
				RegisterSpace = constantsReader.ReadUInt32(),
				Num32BitValues = constantsReader.ReadUInt32(),
			};
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"\tParameterType {ParameterType}");
			sb.AppendLine($"\tShaderRegister {ShaderRegister}");
			sb.AppendLine($"\tRegisterSpace {RegisterSpace}");
			sb.AppendLine($"\tNum32BitValues {Num32BitValues}");
			sb.AppendLine($"\tShaderVisibility {ShaderVisibility}");
			return sb.ToString();
		}
	}
}
