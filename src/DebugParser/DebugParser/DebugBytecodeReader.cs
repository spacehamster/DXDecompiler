using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser
{
	public class DebugBytecodeReader : IDumpable
	{
		private static readonly bool FormatHex = true;
		private readonly byte[] _buffer;
		public readonly int Offset;
		public readonly int Count;
		public int ReadCount;
		public bool InheritSize;
		//IDumpable
		public int Indent { get; set; }
		public uint AbsoluteIndex => (uint)Offset;
		public uint RelativeIndex => (uint)(Offset - _parentOffset);
		//public uint Size => (uint)(InheritSize ? ReadCount : Count);
		public uint Size
		{
			get
			{
				if(!InheritSize)
				{
					return (uint)Count;
				}
				if(Members.Count == 0) return 0;
				if(Members.Count == 1) return Members.First().Size;
				var last = Members.Last();
				return last.AbsoluteIndex - AbsoluteIndex + last.Size;
			}
		}
		public string Type => "Container";
		public string Value => "";
		internal readonly BinaryReader _reader;
		private int _parentOffset;

		public List<IDumpable> Members = new List<IDumpable>();
		DebugBytecodeReader Root = null;
		Stack<DebugIndent> Indents = new Stack<DebugIndent>();
		public static bool DumpOffsets = true;
		public string Name;
		public bool EndOfBuffer
		{
			get { return _reader.BaseStream.Position >= _reader.BaseStream.Length; }
		}
		public long CurrentPosition
		{
			get { return _reader.BaseStream.Position; }
		}
		public DebugBytecodeReader(byte[] buffer, int index, int count)
		{
			_buffer = buffer;
			Offset = index;
			Indent = 0;
			_parentOffset = 0;
			Root = this;
			Count = count;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
		}
		public DebugBytecodeReader(byte[] buffer, int index, int count, int parentIndex,
			int indent, string name, DebugBytecodeReader root, bool inheritSize)
		{
			_buffer = buffer;
			Offset = index;
			_parentOffset = parentIndex;
			_reader = new BinaryReader(new MemoryStream(buffer, index, count));
			Name = name;
			Root = root;
			Indent = indent;
			Count = count;
			InheritSize = inheritSize;
		}
		DebugEntry AddEntry(string name, uint size)
		{
			var result = new DebugEntry()
			{
				Name = name,
				RelativeIndex = (uint)Offset - (uint)_parentOffset + (uint)_reader.BaseStream.Position - size,
				AbsoluteIndex = (uint)Offset + (uint)_reader.BaseStream.Position - size,
				Size = size,
				Indent = Indent
			};
			var currentReadCount = (int)(_reader.BaseStream.Position);
			if(currentReadCount > ReadCount)
			{
				ReadCount = currentReadCount;
			}
			if(Indents.Count > 0)
			{
				Indents.Peek().Members.Add(result);
			}
			if(this != Root) Members.Add(result);
			Root.Members.Add(result);
			return result;
		}

		internal void AddIndent(string name)
		{
			var result = new DebugIndent()
			{
				Name = name,
				Indent = Indent
			};
			Indents.Push(result);
			if(this != Root) Members.Add(result);
			Root.Members.Add(result);
			Indent++;
		}
		internal void RemoveIndent()
		{
			Indents.Pop();
			Indent--;
		}
		public uint PeakUint32()
		{
			var result = _reader.ReadUInt32();
			_reader.BaseStream.Position -= 4;
			return result;
		}
		public uint PeakUInt32At(int offset)
		{
			var oldPos = _reader.BaseStream.Position;
			_reader.BaseStream.Position = offset;
			var result = _reader.ReadUInt32();
			_reader.BaseStream.Position = oldPos;
			return result;
		}
		public uint PeakUInt32Ahead(int offset)
		{
			var oldPos = _reader.BaseStream.Position;
			_reader.BaseStream.Position = _reader.BaseStream.Position + offset;
			var result = _reader.ReadUInt32();
			_reader.BaseStream.Position = oldPos;
			return result;
		}
		public uint ReadUInt32(string name)
		{
			var result = _reader.ReadUInt32();
			var entry = AddEntry(name, 4);
			entry.Type = "UInt32";
			entry.Value = result.ToString();
			return result;
		}
		public int ReadInt32(string name)
		{
			var result = _reader.ReadInt32();
			var entry = AddEntry(name, 4);
			entry.Type = "Int32";
			entry.Value = result.ToString();
			return result;
		}
		public ulong ReadUInt64(string name)
		{
			var result = _reader.ReadUInt64();
			var entry = AddEntry(name, 8);
			entry.Type = "UInt64";
			entry.Value = result.ToString();
			return result;
		}
		public long ReadInt64(string name)
		{
			var result = _reader.ReadInt64();
			var entry = AddEntry(name, 8);
			entry.Type = "Int64";
			entry.Value = result.ToString();
			return result;
		}
		public ushort ReadUInt16(string name)
		{
			var result = _reader.ReadUInt16();
			var entry = AddEntry(name, 2);
			entry.Type = "UInt16";
			entry.Value = result.ToString();
			return result;
		}
		public byte ReadByte(string name)
		{
			var result = _reader.ReadByte();
			var entry = AddEntry(name, 1);
			entry.Type = "Byte";
			entry.Value = result.ToString();
			return result;
		}
		public float ReadSingle(string name)
		{
			var result = _reader.ReadSingle();
			var entry = AddEntry(name, 4);
			entry.Type = "Single";
			entry.Value = result.ToString();
			return result;
		}
		public double ReadDouble(string name)
		{
			var result = _reader.ReadDouble();
			var entry = AddEntry(name, 8);
			entry.Type = "Double";
			entry.Value = result.ToString();
			return result;
		}
		public T ReadEnum32<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadUInt32());
			var entry = AddEntry(name, 4);
			entry.Type = "Enum";
			entry.Value = result.ToString();
			return (T)result;
		}

		public T ReadEnum16<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadUInt16());
			var entry = AddEntry(name, 2);
			entry.Type = "Enum16";
			entry.Value = result.ToString();
			return (T)result;
		}

		public T ReadEnum8<T>(string name) where T : System.Enum
		{
			var result = Enum.ToObject(typeof(T), _reader.ReadByte());
			var entry = AddEntry(name, 1);
			entry.Value = result.ToString();
			entry.Type = "Enum8";
			return (T)result;
		}

		public string TryReadString(string name)
		{
			var length = ReadUInt32($"{name}Length");
			string result = "";
			var toRead = length == 0 ? 0 : length;
			if(toRead % 4 != 0)
			{
				toRead += 4 - toRead % 4;
			}
			var extraBytes = new byte[0];
			if(length > 0)
			{
				var stringBytes = _reader.ReadBytes((int)length - 1);
				extraBytes = _reader.ReadBytes((int)(toRead - (length - 1)));
				result = Encoding.UTF8.GetString(stringBytes, 0, stringBytes.Length);
			}
			else
			{
				extraBytes = _reader.ReadBytes((int)toRead);
			}
			string extra = Encoding.UTF8.GetString(extraBytes, 0, extraBytes.Length);
			string extraHex = string.Join(" ", extraBytes.Select(b => b.ToString("X2")));
			var entry = AddEntry(name, toRead);
			entry.Value = $"\"{result}\" ({extraHex})";
			entry.Type = "String";
			return result;
		}

		public string ReadString(string name)
		{
			var sb = new StringBuilder();
			char nextCharacter;
			while(!EndOfBuffer && (nextCharacter = _reader.ReadChar()) != 0)
			{
				sb.Append(nextCharacter);
			}
			int paddingCount = 0;
			while(!EndOfBuffer)
			{
				if(_reader.ReadByte() == 0xAB)
				{
					paddingCount++;
				}
				else
				{
					_reader.BaseStream.Position -= 1;
					break;
				}
			}
			var result = sb.ToString();
			var entry = AddEntry(name, (uint)(result.Length + 1 + paddingCount));
			entry.Value = result;
			entry.Type = "String";
			return result;
		}
		public byte[] ReadBytes(string name, int count)
		{
			var result = _reader.ReadBytes(count);
			var entry = AddEntry(name, (uint)result.Length);
			entry.Value = $"byte[{result.Length}] {FormatBytes(result)}";
			entry.Type = "Byte[]";
			return result;
		}
		private string FormatBytes(byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("hex(");
			for(int i = 0; i < data.Length; i++)
			{
				var hex = data[i].ToString("X2");
				sb.Append(hex);
				if((i + 1) % 4 == 0 && i != 0 && i < data.Length - 1) sb.Append(" ");
			}
			sb.Append("), chr(");
			sb.Append(DebugUtil.ToReadable(data));
			sb.Append(")");
			return sb.ToString();
		}
		public string DumpHtml()
		{
			return new DebugHtmlWriter(this, _buffer, Members).ToHtml();
		}
		public string DumpStructure()
		{
			var sb = new StringBuilder();
			foreach(var member in Members)
			{
				sb.Append(member.Dump());
			}
			sb.AppendLine();
			var entries = Members.OfType<DebugEntry>();
			var used = new bool[_buffer.Length];
			foreach(var entry in entries)
			{
				for(var i = entry.AbsoluteIndex; i < entry.AbsoluteIndex + entry.Size; i++)
				{
					used[i] = true;
				}
			}
			var sorted = entries
				.OrderBy(e => e.AbsoluteIndex)
				.ToList();
			for(int i = 0; i < used.Length; i++)
			{
				if(!used[i])
				{
					int next = 1;
					int fillerCount = _buffer[i] == 0xAB ? 1 : 0;
					while(i + next < used.Length && used[i + next] == false)
					{
						if(_buffer[i + next] == 0xAB) fillerCount++;
						next++;
					}
					//Strings are padded to 4 byte boundary with 0xAB
					if(fillerCount != next)
					{
						var closest = sorted
							.Last(e => e.AbsoluteIndex < i);


						var absIndex = i;
						var absOffset = i + next - 1;
						var relIndex = i - (closest.AbsoluteIndex - closest.RelativeIndex);
						var relOffset = relIndex + next - 1;
						if(FormatHex)
						{
							sb.Append($"Unread Memory {absIndex.ToString("X4")}:{absOffset.ToString("X4")}[{relIndex.ToString("X4")}:{relOffset.ToString("X4")}] (See {closest.AbsoluteIndex.ToString("X4")}:{(closest.AbsoluteIndex + closest.Size).ToString("X4")} - {closest.Name})");
						}
						else
						{
							sb.Append($"Unread Memory {absIndex}:{absOffset}[{relIndex}:{relOffset}] (See {closest.AbsoluteIndex}:{closest.AbsoluteIndex + closest.Size} - {closest.Name})");
						}
						var subset = _buffer.Skip(i).Take(next).ToArray();
						sb.Append(" ");
						sb.AppendLine(FormatBytes(subset));
					}
					i += next;
				}
			}
			return sb.ToString();
		}
		public string Dump()
		{
			var indent = new string(' ', (int)Indent * 2);
			string next = Offset + _buffer.Length == _buffer.Length ?
					"*" :
					FormatHex ?
					(Offset + _buffer.Length - 1).ToString("X4") :
					(Offset + _buffer.Length - 1).ToString();
			var sb = new StringBuilder();
			sb.Append($"{indent}Container: {Name}");
			if(DumpOffsets)
			{
				if(FormatHex)
				{
					sb.Append($"[{Offset.ToString("X4")}:{next}]");
				}
				else
				{
					sb.Append($"[{Offset}:{next}]");
				}
			}
			sb.AppendLine();
			return sb.ToString();
		}
		public DebugBytecodeReader CopyAtCurrentPosition(string name, DebugBytecodeReader parent, int? count = null)
		{
			return CopyAtOffset(name, parent, (int)_reader.BaseStream.Position, count);
		}

		public DebugBytecodeReader CopyAtOffset(string name, DebugBytecodeReader parent, int offset, int? count = null)
		{
			bool inheritOffset = count == null;
			count = count ?? (int)(_reader.BaseStream.Length - offset);
			var result = new DebugBytecodeReader(_buffer, Offset + offset, count.Value, Offset, parent.Indent + 1, name, Root, inheritOffset);
			Root.Members.Add(result);
			if(this != Root)
			{
				Members.Add(result);
			}
			if(Indents.Count > 0)
			{
				Indents.Peek().Members.Add(result);
			}
			return result;
		}

		public void AddNote(string key, object value)
		{
			var debugEntry = Members.Last();
			debugEntry.AddNote(key, value);
		}
	}
}
