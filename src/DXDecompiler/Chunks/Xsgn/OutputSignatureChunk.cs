using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Xsgn
{
	public class OutputSignatureChunk : InputOutputSignatureChunk
	{
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine("// Output signature:");
			sb.AppendLine("//");
			sb.Append(base.ToString());

			if (!Parameters.Any())
				sb.AppendLine("// no Output");

			return sb.ToString();
		}
	}
}