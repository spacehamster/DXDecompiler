using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Priv
{
	/// <summary>
	/// Contains user specified private data.
	/// </summary>
	public class PrivateChunk : BytecodeChunk
	{
		/// <summary>
		/// Raw data bytes that user included during complimation.
		/// </summary>
		public byte[] PrivateData { get; private set; }

		public static PrivateChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new PrivateChunk();

			result.PrivateData = reader.ReadBytes((int)chunkSize);

			return result;
		}
	}
}