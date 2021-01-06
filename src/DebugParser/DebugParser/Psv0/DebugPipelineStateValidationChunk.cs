using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Psv0;

namespace DXDecompiler.DebugParser.Psv0
{
	public class DebugPipelineStateValidationChunk : DebugBytecodeChunk
	{
		private DebugBytecodeReader Reader;
		public DebugValidationInfo Info { get; private set; }
		public uint MinimumExpectedWaveLaneCount { get; private set; }
		public uint MaximumExpectedWaveLaneCount { get; private set; }
		public byte UsesViewID { get; private set; }
		public ushort GSMaxVertexCount { get; private set; }
		public byte SigPatchConstOrPrimVectors { get; private set; }
		public uint SigPrimVectors { get; private set; }
		public uint MeshOutputTopology { get; private set; }

		// PSVSignatureElement counts
		public byte SigInputElements { get; private set; }
		public byte SigOutputElements { get; private set; }
		public byte SigPatchConstOrPrimElements { get; private set; }

		// Number of packed vectors per signature
		public byte SigInputVectors { get; private set; }
		public byte[] SigOutputVectors { get; private set; } = new byte[4];      // Array for GS Stream Out Index

		public static DebugPipelineStateValidationChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var result = new DebugPipelineStateValidationChunk();
			result.Reader = reader.CopyAtCurrentPosition("DebugPipelineStateValidationChunk", reader);
			return result;
		}
		internal void UpdateVersion(DebugShaderVersion version)
		{
			if(Reader == null) return;
			Reader.ReadUInt32("Unknown");
			switch(version.ProgramType)
			{
				case ProgramType.VertexShader:
					Info = DebugVSInfo.Parse(Reader);
					break;
				case ProgramType.HullShader:
					Info = DebugHSInfo.Parse(Reader);
					break;
				case ProgramType.DomainShader:
					Info = DebugDSInfo.Parse(Reader);
					break;
				case ProgramType.GeometryShader:
					Info = DebugGSInfo.Parse(Reader);
					break;
				case ProgramType.PixelShader:
					Info = DebugPSInfo.Parse(Reader);
					break;
				default:
					Reader.ReadBytes("Unknown", 32);
					break;
			}
			if(Info != null && Info.StructSize < DebugValidationInfo.UnionSize)
			{
				Reader.ReadBytes("Padding", DebugValidationInfo.UnionSize - Info.StructSize);
			}
			MinimumExpectedWaveLaneCount = Reader.ReadUInt32("MinimumExpectedWaveLaneCount");
			MaximumExpectedWaveLaneCount = Reader.ReadUInt32("MaximumExpectedWaveLaneCount");
			if(ChunkSize > 20)
			{
				Reader.ReadEnum8<PSVShaderKind>("ShaderStage");
				UsesViewID = Reader.ReadByte("UsesViewID");
				GSMaxVertexCount = Reader.ReadByte("MaxVertexCount");
				SigInputElements = Reader.ReadByte("SigInputElements");
				SigOutputElements = Reader.ReadByte("SigOutputElements");
				SigPatchConstOrPrimElements = Reader.ReadByte("SigPatchConstOrPrimElements");
				SigInputVectors = Reader.ReadByte("SigInputVectors");
				SigOutputVectors = Reader.ReadBytes("SigOutputVectors", 4);
			}
			Reader = null;
		}
	}
}
