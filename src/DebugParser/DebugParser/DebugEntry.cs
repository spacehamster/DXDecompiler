using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SlimShader.DebugParser
{
	public class DebugEntry : IDumpable
	{
		public string Name;
		public string DisplayValue;
		public string Value { get; set; }
		public int Indent { get; set; }
		public uint RelativeIndex { get; set; }
		public uint AbsoluteIndex { get; set; }
		public string Type { get; set; }
		public uint Size { get; set; }
		public List<string> ExtraNotes = new List<string>();
		private bool formatHex;
		public DebugEntry(bool formatHex = true)
		{
			this.formatHex = formatHex;
		}
		public void AddNote(string key, object value)
		{
			ExtraNotes.Add($"{key}: {value}");
		}
		//Format absStart:AbsEnd[relStart:relEnd]
		public string Dump()
		{
			var member = this;
			var indent = new string(' ', (int)member.Indent * 2);
			var sb = new StringBuilder();
			sb.Append(indent);
			if (DebugBytecodeReader.DumpOffsets)
			{
				var absIndex = member.AbsoluteIndex;
				var absOffset = member.AbsoluteIndex + member.Size - 1;
				var relIndex = member.RelativeIndex;
				var relOffset = member.RelativeIndex + member.Size - 1;
				if (formatHex)
				{
					sb.Append($"{absIndex.ToString("X4")}:{absOffset.ToString("X4")}[{relIndex.ToString("X4")}:{relOffset.ToString("X4")}] - ");
				} else
				{
					sb.Append($"{absIndex}:{absOffset}[{relIndex}:{relOffset}] - ");
				}
			}
			if (formatHex && uint.TryParse(member.Value, out uint intVal))
			{
				sb.Append($"{member.Name}=0x{intVal.ToString("X4")}\n");
			}
			else
			{
				sb.Append($"{member.Name}={member.Value}\n");
			}
			
			return sb.ToString();
		}
		public string DumpInline()
		{
			var member = this;
			var sb = new StringBuilder();
			sb.Append($"{member.Name}={member.Value}\n");
			return sb.ToString();
		}
		public string GetOffsets()
		{
			var member = this;
			var absIndex = member.AbsoluteIndex;
			var absOffset = member.AbsoluteIndex + member.Size - 1;
			var relIndex = member.RelativeIndex;
			var relOffset = member.RelativeIndex + member.Size - 1;
			if (formatHex)
			{
				return $"{absIndex.ToString("X4")}:{absOffset.ToString("X4")}[{relIndex.ToString("X4")}:{relOffset.ToString("X4")}]";
			}
			else
			{
				return $"{absIndex}:{absOffset}[{relIndex}:{relOffset}]";
			}
		}
	}
}
