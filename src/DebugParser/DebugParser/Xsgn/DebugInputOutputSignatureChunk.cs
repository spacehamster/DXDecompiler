using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Xsgn;
using System;

namespace DXDecompiler.DebugParser.Xsgn
{
	public class DebugInputOutputSignatureChunk : DebugBytecodeChunk
	{
		public DebugSignatureParameterDescriptionCollection Parameters { get; private set; }

		protected DebugInputOutputSignatureChunk()
		{
			Parameters = new DebugSignatureParameterDescriptionCollection();
		}

		public static DebugInputOutputSignatureChunk Parse(DebugBytecodeReader reader, ChunkType chunkType,
			ProgramType programType)
		{
			DebugInputOutputSignatureChunk result;
			switch(chunkType)
			{
				case ChunkType.Isgn:
				case ChunkType.Isg1:
					result = new DebugInputSignatureChunk();
					break;
				case ChunkType.Osgn:
				case ChunkType.Osg1:
				case ChunkType.Osg5:
					result = new DebugOutputSignatureChunk();
					break;
				case ChunkType.Pcsg:
					result = new DebugPatchConstantSignatureChunk();
					break;
				default:
					throw new ArgumentOutOfRangeException("chunkType", "Unrecognised chunk type: " + chunkType);
			}

			var chunkReader = reader.CopyAtCurrentPosition("ChunkReader", reader);
			var elementCount = chunkReader.ReadUInt32("ElementCount");
			var uniqueKey = chunkReader.ReadUInt32("UniqueKey");

			SignatureElementSize elementSize;
			switch(chunkType)
			{
				case ChunkType.Isgn:
				case ChunkType.Osgn:
				case ChunkType.Pcsg:
					elementSize = SignatureElementSize._6;
					break;
				case ChunkType.Osg5:
					elementSize = SignatureElementSize._7;
					break;
				case ChunkType.Osg1:
				case ChunkType.Isg1:
					elementSize = SignatureElementSize._8;
					break;
				default:
					throw new ArgumentOutOfRangeException("chunkType", "Unrecognised chunk type: " + chunkType);
			}

			for(int i = 0; i < elementCount; i++)
			{
				var indent = reader.AddIndent($"{i}");
				var parameter = DebugSignatureParameterDescription.Parse(reader, chunkReader, chunkType, elementSize,
					programType);
				result.Parameters.Add(parameter);
				reader.RemoveIndent();
				indent.Name += $": {parameter.SemanticName}";
			}

			return result;
		}
	}
}
