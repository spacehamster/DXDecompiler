using System.Collections.Generic;
using DXDecompiler.Chunks.Aon9;
using DXDecompiler.Chunks.Fx10;
using DXDecompiler.Chunks.Fxlvm;
using DXDecompiler.Chunks.Ifce;
using DXDecompiler.Chunks.Libf;
using DXDecompiler.Chunks.Priv;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Chunks.RTS0;
using DXDecompiler.Chunks.Sfi0;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Spdb;
using DXDecompiler.Chunks.Stat;
using DXDecompiler.Chunks.Xsgn;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks
{
	public abstract class BytecodeChunk
	{
		private static readonly Dictionary<uint, ChunkType> KnownChunkTypes = new Dictionary<uint, ChunkType>
		{
			{ "IFCE".ToFourCc(), ChunkType.Ifce },
			{ "ISGN".ToFourCc(), ChunkType.Isgn },
			{ "OSGN".ToFourCc(), ChunkType.Osgn },
			{ "OSG5".ToFourCc(), ChunkType.Osg5 },
			{ "PCSG".ToFourCc(), ChunkType.Pcsg },
			{ "RDEF".ToFourCc(), ChunkType.Rdef },
			{ "SDBG".ToFourCc(), ChunkType.Sdbg },
			{ "SFI0".ToFourCc(), ChunkType.Sfi0 },
			{ "SHDR".ToFourCc(), ChunkType.Shdr },
			{ "SHEX".ToFourCc(), ChunkType.Shex },
			{ "SPDB".ToFourCc(), ChunkType.Spdb },
			{ "STAT".ToFourCc(), ChunkType.Stat },
			{ "ISG1".ToFourCc(), ChunkType.Isg1 },
			{ "OSG1".ToFourCc(), ChunkType.Osg1 },
			{ "Aon9".ToFourCc(), ChunkType.Aon9 },
			{ "XNAS".ToFourCc(), ChunkType.Xnas },
			{ "XNAP".ToFourCc(), ChunkType.Xnap },
			{ "PRIV".ToFourCc(), ChunkType.Priv },
			{ "RTS0".ToFourCc(), ChunkType.Rts0 },
			{ "LIBF".ToFourCc(), ChunkType.Libf },
			{ "LIBH".ToFourCc(), ChunkType.Libh },
			{ "LFS0".ToFourCc(), ChunkType.Lfs0 },
			{ "FX10".ToFourCc(), ChunkType.Fx10 },
			{ "CTAB".ToFourCc(), ChunkType.Ctab },
			{ "CLI4".ToFourCc(), ChunkType.Cli4 },
			{ "FXLC".ToFourCc(), ChunkType.Fxlc }
		};

		public BytecodeContainer Container { get; private set; }
		public uint FourCc { get; private set; }
		public ChunkType ChunkType { get; private set; }
		public uint ChunkSize { get; private set; }

		public static BytecodeChunk ParseChunk(BytecodeReader chunkReader, BytecodeContainer container)
		{
			// Type of chunk this is.
			uint fourCc = chunkReader.ReadUInt32();

			// Total length of the chunk in bytes.
			uint chunkSize = chunkReader.ReadUInt32();

			ChunkType chunkType;
			if (KnownChunkTypes.ContainsKey(fourCc))
			{
				chunkType = KnownChunkTypes[fourCc];
			}
			else
			{
				System.Diagnostics.Debug.Assert(false, "Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");
				System.Diagnostics.Debug.WriteLine("Chunk type '" + fourCc.ToFourCcString() + "' is not yet supported.");
				return null;
			}

			var chunkContentReader = chunkReader.CopyAtCurrentPosition((int) chunkSize);
			BytecodeChunk chunk;
			switch (chunkType)
			{
				case ChunkType.Ifce :
					chunk = InterfacesChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Isgn:
				case ChunkType.Osgn:
				case ChunkType.Osg5:
				case ChunkType.Pcsg:
				case ChunkType.Isg1:
				case ChunkType.Osg1:
					chunk = InputOutputSignatureChunk.Parse(chunkContentReader, chunkType,
						container.Version.ProgramType);
					break;
				case ChunkType.Rdef:
					chunk = ResourceDefinitionChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Sdbg :
				case ChunkType.Spdb :
					chunk = DebuggingChunk.Parse(chunkContentReader, chunkType, chunkSize);
					break;
				case ChunkType.Sfi0:
					chunk = Sfi0Chunk.Parse(chunkContentReader, container.Version, chunkSize);
					break;
				case ChunkType.Shdr:
				case ChunkType.Shex:
					chunk = ShaderProgramChunk.Parse(chunkContentReader);
					break;
				case ChunkType.Stat:
					chunk = StatisticsChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Xnas:
				case ChunkType.Xnap:
				case ChunkType.Aon9:
					chunk = Level9ShaderChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Priv:
					chunk = PrivateChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Rts0:
					chunk = RootSignatureChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Libf:
					chunk = LibfChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Libh:
					chunk = LibHeaderChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Lfs0:
					chunk = LibraryParameterSignatureChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Fx10:
					chunk = EffectChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Ctab:
					chunk = CtabChunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Cli4:
					chunk = Cli4Chunk.Parse(chunkContentReader, chunkSize);
					break;
				case ChunkType.Fxlc:
					chunk = FxlcChunk.Parse(chunkContentReader, chunkSize, container);
					break;
				default :
					throw new ParseException("Invalid chunk type: " + chunkType);
			}

			chunk.Container = container;
			chunk.FourCc = fourCc;
			chunk.ChunkSize = chunkSize;
			chunk.ChunkType = chunkType;

			return chunk;
		}
	}
}