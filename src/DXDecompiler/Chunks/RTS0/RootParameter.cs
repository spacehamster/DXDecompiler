using DXDecompiler.Util;
using System;

namespace DXDecompiler.Chunks.RTS0
{
	/// <summary>
	/// A general entry in the root signature.
	/// Based on D3D12_ROOT_PARAMETER and D3D12_ROOT_PARAMETER1.
	/// </summary>
	public class RootParameter
	{
		public RootParameterType ParameterType { get; private set; }
		public ShaderVisibility ShaderVisibility;
		public static RootParameter Parse(BytecodeReader reader, BytecodeReader paramReader, RootSignatureVersion version)
		{
			var type = (RootParameterType)paramReader.ReadUInt32();
			var visibility = (ShaderVisibility)paramReader.ReadUInt32();
			var bodyOffset = paramReader.ReadUInt32();
			var bodyReader = reader.CopyAtOffset((int)bodyOffset);
			RootParameter result;
			switch(type)
			{
				case RootParameterType.DescriptorTable:
					result = RootDescriptorTable.Parse(reader, bodyReader, version);
					break;
				case RootParameterType._32BitConstants:
					result = RootConstants.Parse(bodyReader);
					break;
				case RootParameterType.Srv:
				case RootParameterType.Uav:
				case RootParameterType.Cbv:
					result = RootDescriptor.Parse(bodyReader, version);
					break;
				default:
					throw new InvalidOperationException($"Can't parse Root Parameter of type {type}");
			}
			result.ParameterType = type;
			result.ShaderVisibility = visibility;
			return result;
		}
	}
}
