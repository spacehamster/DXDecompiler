using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugResourceDeclarationToken : DebugDeclarationToken
	{
		public ResourceDimension ResourceDimension;
		public byte SampleCount;
		public DebugResourceReturnTypeToken ReturnType;
		public uint SpaceIndex;
		private bool IsSM51 => Operand.IndexDimension == OperandIndexDimension._3D;

		public static DebugResourceDeclarationToken Parse(DebugBytecodeReader reader, DebugShaderVersion version)
		{
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);
			var resourceDimension = token0.DecodeValue<ResourceDimension>(11, 15);
			reader.AddNote("resourceDimension", resourceDimension);

			byte sampleCount;
			switch (resourceDimension)
			{
				case ResourceDimension.Texture2DMultiSampled:
				case ResourceDimension.Texture2DMultiSampledArray:
					sampleCount = token0.DecodeValue<byte>(16, 22);
					reader.AddNote("SampleCount", sampleCount);
					break;
				default:
					sampleCount = 0;
					break;
			}

			var operand = DebugOperand.Parse(reader, token0.DecodeValue<OpcodeType>(0, 10));
			var returnType = DebugResourceReturnTypeToken.Parse(reader);
			var result = new DebugResourceDeclarationToken
			{
				ResourceDimension = resourceDimension,
				SampleCount = sampleCount,
				Operand = operand,
				ReturnType = returnType
			};
			if (version.IsSM51)
			{
				result.SpaceIndex = reader.ReadUInt32("SpaceIndex");
			}
			return result;
		}
	}
}
