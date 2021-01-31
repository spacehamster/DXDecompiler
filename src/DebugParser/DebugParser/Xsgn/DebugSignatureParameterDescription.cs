using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Xsgn;

namespace DXDecompiler.DebugParser.Xsgn
{
	public class DebugSignatureParameterDescription
	{
		public string SemanticName { get; private set; }
		public uint SemanticIndex { get; private set; }
		public uint Register { get; private set; }
		public Name SystemValueType { get; private set; }
		public RegisterComponentType ComponentType { get; private set; }
		public ComponentMask Mask { get; private set; }
		public ComponentMask ReadWriteMask { get; private set; }
		public uint Stream { get; private set; }
		public MinPrecision MinPrecision { get; private set; }

		public DebugSignatureParameterDescription(string semanticName, uint semanticIndex,
			Name systemValueType, RegisterComponentType componentType, uint register,
			ComponentMask mask, ComponentMask readWriteMask)
		{
			SemanticName = semanticName;
			SemanticIndex = semanticIndex;
			Register = register;
			SystemValueType = systemValueType;
			ComponentType = componentType;
			Mask = mask;
			ReadWriteMask = readWriteMask;
		}

		public DebugSignatureParameterDescription()
		{

		}

		public static DebugSignatureParameterDescription Parse(DebugBytecodeReader reader, DebugBytecodeReader parameterReader,
			ChunkType chunkType, SignatureElementSize size, ProgramType programType)
		{
			uint stream = 0;
			if(size == SignatureElementSize._7 || size == SignatureElementSize._8)
				stream = parameterReader.ReadUInt32("Stream");
			uint nameOffset = parameterReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", parameterReader, (int)nameOffset);

			var result = new DebugSignatureParameterDescription
			{
				SemanticName = nameReader.ReadString("SemanticName"),
				SemanticIndex = parameterReader.ReadUInt32("SemanticIndex"),
				SystemValueType = parameterReader.ReadEnum32<Name>("SystemValueType"),
				ComponentType = parameterReader.ReadEnum32<RegisterComponentType>("ComponentType"),
				Register = parameterReader.ReadUInt32("Register"),
				Stream = stream,
			};

			result.Mask = parameterReader.ReadEnum8<ComponentMask>("Mask");
			result.ReadWriteMask = parameterReader.ReadEnum8<ComponentMask>("ReadWriteMask");
			parameterReader.ReadUInt16("MaskPadding");

			if(size == SignatureElementSize._8)
			{
				MinPrecision minPrecision = parameterReader.ReadEnum32<MinPrecision>("MinPrecision");
				result.MinPrecision = minPrecision;
			}

			// This is my guesswork, but it works so far...
			if(chunkType == ChunkType.Osg5 ||
				chunkType == ChunkType.Osgn ||
				chunkType == ChunkType.Osg1 ||
				(chunkType == ChunkType.Pcsg && programType == ProgramType.HullShader))
			{
				result.ReadWriteMask = (ComponentMask)(ComponentMask.All - result.ReadWriteMask);
			}

			// Vertex and pixel shaders need special handling for SystemValueType in the output signature.
			if((programType == ProgramType.PixelShader || programType == ProgramType.VertexShader)
				&& (chunkType == ChunkType.Osg5 || chunkType == ChunkType.Osgn || chunkType == ChunkType.Osg1))
			{
				if(result.Register == 0xffffffff)
					switch(result.SemanticName.ToUpper())
					{
						case "SV_DEPTH":
							result.SystemValueType = Name.Depth;
							break;
						case "SV_COVERAGE":
							result.SystemValueType = Name.Coverage;
							break;
						case "SV_DEPTHGREATEREQUAL":
							result.SystemValueType = Name.DepthGreaterEqual;
							break;
						case "SV_DEPTHLESSEQUAL":
							result.SystemValueType = Name.DepthLessEqual;
							break;
						case "SV_STENCILREF":
							result.SystemValueType = Name.StencilRef;
							break;
					}
				else if(programType == ProgramType.PixelShader)
					result.SystemValueType = Name.Target;
			}

			return result;
		}
	}
}