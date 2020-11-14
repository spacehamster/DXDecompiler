using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugIndent : IDumpable
	{
		public string Name;
		public int Indent { get; set; }
		public List<IDumpable> Members = new List<IDumpable>();
		public uint AbsoluteIndex => Members.Count == 0 ? 0 : Members.First().AbsoluteIndex;
		public uint RelativeIndex => Members.Count == 0 ? 0 : Members.First().RelativeIndex;
		public string Value => "";
		public string Type => "Indent";
		public List<string> ExtraNotes = new List<string>();
		public uint Size
		{
			get
			{
				if(Members.Count == 0)
				{
					return 0;
				}
				if(Members.Count == 1)
				{
					return Members.First().Size;
				}
				var first = Members.First();
				var last = Members.Last();
				return last.AbsoluteIndex - first.AbsoluteIndex + last.Size;
			}
		}
		public string Dump()
		{
			var member = this;
			var indent = new string(' ', (int)member.Indent * 2);
			var sb = new StringBuilder();
			sb.Append(indent);
			sb.Append($"Indent: {Name}\n");
			return sb.ToString();
		}

		public void AddNote(string key, object value)
		{
			ExtraNotes.Add($"{key}: {value}");
		}
	}
}
