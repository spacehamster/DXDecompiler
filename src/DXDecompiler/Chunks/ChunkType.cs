namespace DXDecompiler.Chunks
{
	public enum ChunkType
	{
		Unknown,

		/// <summary>
		/// Interfaces
		/// </summary>
		Ifce,

		/// <summary>
		/// Input signature
		/// </summary>
		Isgn,

		/// <summary>
		/// Output signature (SM5)
		/// </summary>
		Osg5,

		/// <summary>
		/// Output signature
		/// </summary>
		Osgn,

		/// <summary>
		/// Patch constant signature
		/// </summary>
		Pcsg,

		/// <summary>
		/// Resource definition
		/// </summary>
		Rdef,

		/// <summary>
		/// Shader debugging info (old-style)
		/// </summary>
		Sdbg,

		/// <summary>
		/// ?
		/// </summary>
		Sfi0,

		/// <summary>
		/// Shader (SM 4.0)
		/// </summary>
		Shdr,

		/// <summary>
		/// Shader (SM 5.0)
		/// </summary>
		Shex,

		/// <summary>
		/// Shader debugging info (new-style)
		/// </summary>
		Spdb,

		/// <summary>
		/// Statistics
		/// </summary>
		Stat,

		/// <summary>
		/// ?
		/// </summary>
		Isg1,

		/// <summary>
		/// ?
		/// </summary>
		Osg1,

		/// <summary>
		/// ?
		/// </summary>
		Psg1,

		/// <summary>
		/// Level 9 Shader Chunk
		/// </summary>
		Aon9,

		/// <summary>
		/// Level 9 XNA Prepass Shader Chunk
		/// </summary>
		Xnap,

		/// <summary>
		/// Level 9 XNA Shader Chunk
		/// </summary>
		Xnas,

		/// <summary>
		/// User Specified Private Data Chunk
		/// </summary>
		Priv,

		/// <summary>
		/// Root Signature Chunk
		/// </summary>
		Rts0,

		/// <summary>
		/// Library chunk
		/// </summary>
		Libf,

		/// <summary>
		/// Library chunk
		/// </summary>
		Libh,

		/// <summary>
		/// Library chunk
		/// </summary>
		Lfs0,

		/// <summary>
		/// Effects chunk
		/// </summary>
		Fx10,

		/// <summary>
		/// Effects expression chunk
		/// </summary>
		Ctab,

		/// <summary>
		/// Effects expression chunk
		/// </summary>
		Cli4,

		/// <summary>
		/// Effects expression chunk
		/// </summary>
		Fxlc,

		/// <summary>
		/// DXIL bitcode chunk
		/// </summary>
		Dxil,

		/// <summary>
		/// DXIL shader hash chunk
		/// </summary>
		Hash,

		/// <summary>
		/// Pipeline validation chunk
		/// </summary>
		Psv0,

		/// <summary>
		/// Runtime data chunk
		/// </summary>
		Rdat,

		/// <summary>
		/// Shader Debug Info DXIL chunk
		/// </summary>
		Ildb,

		/// <summary>
		/// Shader Debug Name chunk
		/// </summary>
		Ildn
	}
}