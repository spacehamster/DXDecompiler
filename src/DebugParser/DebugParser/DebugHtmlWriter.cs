using DebugParser.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace DXDecompiler.DebugParser
{
	public class DebugHtmlWriter
	{
		DebugBytecodeReader Reader;
		byte[] buffer;
		StringBuilder stringBuilder;
		private List<IDumpable> Members = new List<IDumpable>();
		public DebugHtmlWriter(DebugBytecodeReader reader, byte[] buffer, List<IDumpable> members)
		{
			this.Reader = reader;
			this.buffer = buffer;
			this.Members = members;
		}
		public string ToHtml()
		{
			stringBuilder = new StringBuilder();
			Write();
			var result = stringBuilder.ToString();
			result = Regex.Replace(result, @"<\/span>\s+<span", @"</span><span", RegexOptions.Multiline);
			return result;
		}
		private void Write()
		{
			string css = Resources.DebugCSS;
			var javascriptTag = new XElement
				(
				   "script",
				   new XAttribute("type", @"text/javascript"),
				   "//",
				   new XCData("\n" + Resources.DebugJavascript + "\n//")
				);
			var styleTag = new XElement
				(
					"style",
					"/*",
					new XCData("*/\n" + Resources.DebugCSS + "\n/*"),
					"*/"
				);
			var xDocument = new XDocument(
				new XDocumentType("html", null, null, null),
				new XElement("html",
					new XElement("head",
						new XElement("meta",
							new XAttribute("charset", "utf-8")),
						styleTag,
						javascriptTag),
					new XElement("body",
						new XElement("div",
							new XAttribute("class", "panel-row"),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "treeview")
							),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "hexview")
							),
							new XElement("div",
								new XAttribute("class", "panel-column"),
								new XAttribute("id", "detailview"),
								""
							)
						)
					)
				)
			);
			var treeView = xDocument.Root.Descendants("div")
				.Where(e => (string)e.Attribute("id") == "treeview")
				.First();
			WriteTreeView(treeView, Members);
			var hexview = xDocument.Root.Descendants("div")
				.Where(e => (string)e.Attribute("id") == "hexview")
				.First();
			WriteHexView(hexview);
			var settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = true,
				Indent = true,
				IndentChars = "\t",
				Encoding = Encoding.UTF8
			};

			using(var writer = XmlWriter.Create(stringBuilder, settings))
			{
				xDocument.WriteTo(writer);
			}
		}
		private void WriteTreeView(XElement treeView, List<IDumpable> entries)
		{
			var stack = new Stack<XElement>();
			var ul = new XElement("ul", new XAttribute("id", "myUL"));
			treeView.Add(ul);
			stack.Push(ul);
			for(int i = 0; i < entries.Count; i++)
			{
				var entry = entries[i];
				var nextEntry = i < entries.Count - 1 ? entries[i + 1] : null;
				var span = new XElement("span", new XAttribute("class", "tree-item"), "");
				XElement label = null;
				if(entry is DebugEntry de)
				{
					var valueLength = 10;
					var value = de.Value.Length > valueLength ?
						(de.Value.Substring(0, valueLength - 3) + "...") :
						de.Value;
					var text = $"{de.Name}={value}";
					var noteText = string.Join(";", de.ExtraNotes);
					label = new XElement("span", text,
						new XAttribute("class", "tree-label"),
						new XAttribute("data-start", de.AbsoluteIndex),
						new XAttribute("data-end", de.AbsoluteIndex + de.Size),
						new XAttribute("id", "member_" + entry.GetHashCode()),
						new XAttribute("name", de.Name),
						new XAttribute("value", de.Value),
						new XAttribute("size", de.Size),
						new XAttribute("rel-start", de.RelativeIndex),
						new XAttribute("rel-end", de.RelativeIndex + de.Size),
						new XAttribute("type", de.Type),
						new XAttribute("notes", noteText));
				}
				if(entry is DebugIndent di)
				{
					var noteText = string.Join(";", di.ExtraNotes);
					label = new XElement("span", di.Name,
						new XAttribute("class", "tree-label"),
						new XAttribute("data-start", di.AbsoluteIndex),
						new XAttribute("data-end", di.AbsoluteIndex + di.Size),
						new XAttribute("id", "member_" + entry.GetHashCode()),
						new XAttribute("name", di.Name),
						new XAttribute("value", ""),
						new XAttribute("size", di.Size),
						new XAttribute("rel-start", di.RelativeIndex),
						new XAttribute("rel-end", di.RelativeIndex + di.Size),
						new XAttribute("type", "Indent"),
						new XAttribute("notes", noteText)
						);
				}
				if(entry is DebugBytecodeReader dr)
				{
					var labelText = dr.Name;
					var value = "";
					if(dr.LocalMembers.Count == 1 && dr.LocalMembers.First().Type == "String")
					{
						var valueLength = 10;
						var child = dr.LocalMembers.First();
						var truncatedValue = child.Value.Length > valueLength ?
							(child.Value.Substring(0, valueLength - 3) + "...") :
							child.Value;
						labelText += $"={truncatedValue}";
						value = child.Value;
					}
					label = new XElement("span", $"{labelText}",
						new XAttribute("class", "tree-label"),
						new XAttribute("data-start", dr.AbsoluteIndex),
						new XAttribute("data-end", dr.AbsoluteIndex + dr.Size),
						new XAttribute("id", "member_" + entry.GetHashCode()),
						new XAttribute("name", dr.Name),
						new XAttribute("value", value),
						new XAttribute("size", dr.Size),
						new XAttribute("rel-start", dr.RelativeIndex),
						new XAttribute("rel-end", dr.RelativeIndex + dr.Size),
						new XAttribute("type", "Container"),
						new XAttribute("notes", "")
						);
				}
				var container = stack.Peek();
				var li = new XElement("li");
				container.Add(li);
				li.Add(span);
				var nextIndent = nextEntry != null ?
					((nextEntry is DebugBytecodeReader) ? nextEntry.Indent - 1 : nextEntry.Indent) : 0;
				var indent = (entry is DebugBytecodeReader) ? entry.Indent - 1 : entry.Indent;
				if(nextEntry != null && nextIndent > indent)
				{
					var caret = new XElement("span", new XAttribute("class", "caret"), "");
					span.Add(caret);
					span.Add(label);
					if(nextIndent - indent > 1) throw new Exception("Unbalanced Indents");
					var subList = new XElement("ul", new XAttribute("class", "nested"));
					li.Add(subList);
					stack.Push(subList);
				}
				else
				{
					span.Add(label);
				}
				if(nextEntry != null && nextIndent < indent)
				{
					for(int j = 0; j < indent - nextIndent; j++)
					{
						stack.Pop();
					}
				}
			}
		}
		private void WriteHexView(XElement element)
		{
			var nbsp = "\u00A0";
			var used = BuildUsedLookup();
			for(int i = 0; i < buffer.Length; i += 16)
			{
				var row = new XElement("div",
					new XAttribute("row", i / 16),
					new XAttribute("class", "hex_row monospace"),
					new XElement("span", $"{i.ToString("X4")}:{nbsp}{nbsp}"));
				element.Add(row);
				for(int j = i; j < i + 16; j++)
				{

					var text = j < buffer.Length ? buffer[j].ToString("X2") : $"{nbsp}{nbsp}";
					var hexElement = new XElement("span", text,
						new XAttribute("id", "b" + j.ToString()),
						new XAttribute("index", j.ToString()),
						new XAttribute("class", "hex_byte"));
					if(j < used.Length && used[j] == null)
					{
						hexElement.Attribute("class").Value += " unused";
					}
					else if(j < used.Length && used[j] != null)
					{
						hexElement.Add(new XAttribute("member", "member_" + used[j].GetHashCode()));
					}
					row.Add(hexElement);
					if(j % 4 == 3 && j % 16 != 15)
					{
						row.Add(new XElement("span", nbsp));
					}
				}
				row.Add(new XElement("span", $"{nbsp}{nbsp}"));
				for(int j = i; j < i + 16; j++)
				{
					var asciiText = j < buffer.Length ? DebugUtil.CharToReadable((char)buffer[j]) : nbsp;
					var asciiElement = new XElement("span",
							asciiText,
							new XAttribute("id", "a" + j.ToString()),
							new XAttribute("index", j.ToString()),
							new XAttribute("class", "hex_ascii"));
					row.Add(asciiElement);
					if(j < used.Length && used[j] == null)
					{
						asciiElement.Attribute("class").Value += " unused";
					}
					else if(j < used.Length && used[j] != null)
					{
						asciiElement.Add(new XAttribute("member", "member_" + used[j].GetHashCode()));
					}
				}
			}
		}
		DebugEntry[] BuildUsedLookup()
		{
			var entries = Members.OfType<DebugEntry>();
			var used = new DebugEntry[buffer.Length];
			foreach(var entry in entries)
			{
				for(var i = entry.AbsoluteIndex; i < entry.AbsoluteIndex + entry.Size; i++)
				{
					if(used[i] == null)
					{
						used[i] = entry;
					}
					else if(entry.Indent > used[i].Indent)
					{
						used[i] = entry;
					}
				}
			}
			return used;
		}
	}
}
