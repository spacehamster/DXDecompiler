using DXDecompiler.Chunks.Xsgn;

namespace DXDecompiler.Decompiler.IR
{
	public class IrSignature
	{

		internal InputOutputSignatureChunk Chunk;
		public string Name;
		public IrSignatureType SignatureType;
		public IrSignature(InputOutputSignatureChunk chunk, string name, IrSignatureType signatureType)
		{
			Name = name;
			Chunk = chunk;
			SignatureType = signatureType;
		}
	}
}
