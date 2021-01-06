using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Psv0
{
	public class PipelineStateValidationChunk : BytecodeChunk
	{
		private BytecodeReader Reader;
		public ValidationInfo Info { get; private set; }
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

		public static PipelineStateValidationChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new PipelineStateValidationChunk();
			result.Reader = reader.CopyAtCurrentPosition();
			return result;
		}
		internal void UpdateVersion(ShaderVersion version)
		{
			if(Reader == null) return;
			Reader.ReadUInt32(); //Unknown
			switch(version.ProgramType)
			{
				case ProgramType.VertexShader:
					Info = VSInfo.Parse(Reader);
					break;
				case ProgramType.HullShader:
					Info = HSInfo.Parse(Reader);
					break;
				case ProgramType.DomainShader:
					Info = DSInfo.Parse(Reader);
					break;
				case ProgramType.GeometryShader:
					Info = GSInfo.Parse(Reader);
					break;
				case ProgramType.PixelShader:
					Info = PSInfo.Parse(Reader);
					break;
				default:
					Reader.ReadBytes(ValidationInfo.UnionSize);
					break;
			}
			if(Info != null && Info.StructSize < ValidationInfo.UnionSize)
			{
				//Padding
				Reader.ReadBytes(ValidationInfo.UnionSize - Info.StructSize);
			}
			MinimumExpectedWaveLaneCount = Reader.ReadUInt32();
			MaximumExpectedWaveLaneCount = Reader.ReadUInt32();
			if(ChunkSize > 20)
			{
				var shaderKind = (PSVShaderKind)Reader.ReadByte();
				UsesViewID = Reader.ReadByte();
				GSMaxVertexCount = Reader.ReadByte();
				SigInputElements = Reader.ReadByte();
				SigOutputElements = Reader.ReadByte();
				SigPatchConstOrPrimElements = Reader.ReadByte();
				SigInputVectors = Reader.ReadByte();
				SigOutputVectors = Reader.ReadBytes(4);
			}
			Reader = null;
		}
	}
}
