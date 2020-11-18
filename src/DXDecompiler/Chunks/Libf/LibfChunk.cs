using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.Libf
{
	/// <summary>
	/// Might be related to D3D11_FUNCTION_DESC
	/// </summary>
	public class LibfChunk : BytecodeChunk
	{
		public BytecodeContainer LibraryContainer { get; private set; }
		public static LibfChunk Parse(BytecodeReader reader, uint chunkSize)
		{
			var result = new LibfChunk();
			var chunkReader = reader.CopyAtCurrentPosition();
			var data = chunkReader.ReadBytes((int)chunkSize);
			result.LibraryContainer = new BytecodeContainer(data);
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine(LibraryContainer.ToString());
			return sb.ToString();
		}
	}
}
