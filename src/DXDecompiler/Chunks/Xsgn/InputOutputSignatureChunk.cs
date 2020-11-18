using System;
using System.Linq;
using System.Text;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Xsgn
{
	public abstract class InputOutputSignatureChunk : BytecodeChunk
	{
		public SignatureParameterDescriptionCollection Parameters { get; private set; }

		protected InputOutputSignatureChunk()
		{
			Parameters = new SignatureParameterDescriptionCollection();
		}

		public static InputOutputSignatureChunk Parse(BytecodeReader reader, ChunkType chunkType,
			ProgramType programType)
		{
			InputOutputSignatureChunk result;
			switch (chunkType)
			{
				case ChunkType.Isgn :
				case ChunkType.Isg1:
					result = new InputSignatureChunk();
					break;
				case ChunkType.Osgn :
				case ChunkType.Osg1:
				case ChunkType.Osg5 :
					result = new OutputSignatureChunk();
					break;
				case ChunkType.Pcsg :
					result = new PatchConstantSignatureChunk();
					break;
				default :
					throw new ArgumentOutOfRangeException("chunkType", "Unrecognised chunk type: " + chunkType);
			}

			var chunkReader = reader.CopyAtCurrentPosition();
			var elementCount = chunkReader.ReadUInt32();
			var uniqueKey = chunkReader.ReadUInt32();

			SignatureElementSize elementSize;
			switch (chunkType)
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

			for (int i = 0; i < elementCount; i++)
				result.Parameters.Add(SignatureParameterDescription.Parse(reader, chunkReader, chunkType, elementSize,
					programType));

			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("// Name                 Index   Mask Register SysValue  Format   Used");
			sb.AppendLine("// -------------------- ----- ------ -------- -------- ------- ------");
			bool includeStreams = Parameters.Any(p => p.Stream > 0);
			foreach (var parameter in Parameters)
				sb.AppendLine("// " + parameter.ToString(includeStreams));

			if (Parameters.Any())
				sb.AppendLine("//");

			return sb.ToString();
		}
	}
}