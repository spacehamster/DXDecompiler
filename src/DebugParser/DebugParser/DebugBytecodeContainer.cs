using DXDecompiler.Chunks;
using DXDecompiler.DebugParser.Chunks.Fx10;
using DXDecompiler.DebugParser.Rdef;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser
{
	public class DebugBytecodeContainer
	{
		private byte[] _rawBytes;

		public byte[] RawBytes
		{
			get { return _rawBytes; }
		}
		public DebugBytecodeContainerHeader Header { get; private set; }
		public List<DebugBytecodeChunk> Chunks { get; private set; }

		public DebugResourceDefinitionChunk ResourceDefinition => Chunks
			.OfType<DebugResourceDefinitionChunk>()
			.FirstOrDefault();

		DebugBytecodeReader _reader;
		string Error = "";
		public Exception Exception = null;
		public DebugBytecodeContainer(byte[] rawBytes)
		{
			try
			{
				_rawBytes = rawBytes;
				var reader = new DebugBytecodeReader(rawBytes, 0, rawBytes.Length);

				Chunks = new List<DebugBytecodeChunk>();

				_reader = reader;
				var magicNumber = reader.PeakUint32();
				if (magicNumber == 0xFEFF2001)
				{
					Chunks.Add(DebugEffectChunk.Parse(reader, (uint)rawBytes.Length));
					return;
				}

				Header = DebugBytecodeContainerHeader.Parse(reader);

				for (uint i = 0; i < Header.ChunkCount; i++)
				{
					uint chunkOffset = reader.ReadUInt32("chunkOffset");
					var fourCC = DebugUtil.ToReadable(reader.PeakUInt32At((int)chunkOffset).ToFourCcString());
					var chunkReader = reader.CopyAtOffset($"Chunk{fourCC}", reader, (int)chunkOffset);

					var chunk = DebugBytecodeChunk.ParseChunk(chunkReader, this);
					if (chunk != null)
						Chunks.Add(chunk);
				}
			} catch (Exception ex)
			{
				Exception = ex;
				Error = ex.ToString();
			}
		}
		public DebugBytecodeContainer(DebugBytecodeReader reader)
		{
			try
			{
				Chunks = new List<DebugBytecodeChunk>();

				_reader = reader;
				var magicNumber = reader.PeakUint32();
				if (magicNumber == 0xFEFF2001)
				{
					Chunks.Add(DebugEffectChunk.Parse(reader, (uint)reader.Count));
					return;
				}
				Header = DebugBytecodeContainerHeader.Parse(reader);

				for (uint i = 0; i < Header.ChunkCount; i++)
				{
					uint chunkOffset = reader.ReadUInt32("chunkOffset");
					var fourCC = DebugUtil.ToReadable(reader.PeakUInt32At((int)chunkOffset).ToFourCcString());
					var chunkReader = reader.CopyAtOffset($"Chunk{fourCC}", reader, (int)chunkOffset);

					var chunk = DebugBytecodeChunk.ParseChunk(chunkReader, this);
					if (chunk != null)
						Chunks.Add(chunk);
				}
			}
			catch (Exception ex)
			{
				Exception = ex;
				Error = ex.ToString();
			}
		}
		public static DebugBytecodeContainer Parse(byte[] bytes)
		{
			var result = new DebugBytecodeContainer(bytes);
			result._rawBytes = bytes;
			return result;
		}
		public static DebugBytecodeContainer Parse(DebugBytecodeReader reader)
		{
			return new DebugBytecodeContainer(reader);
		}
		public string ParseErrors {
			get
			{
				var sb = new StringBuilder();
				if (!string.IsNullOrEmpty(Error))
				{
					sb.AppendLine(Error);
				}
				var libraryChunks = Chunks.OfType<Libf.DebugLibfChunk>().ToList();
				foreach (var chunk in libraryChunks)
				{
					if (!string.IsNullOrEmpty(chunk.LibraryContainer.Error))
					{
						var msg = $"Error in Library {libraryChunks.IndexOf(chunk)}\n" + chunk.LibraryContainer.Error;
						sb.AppendLine(Error);
					}
				}
				return sb.ToString();
			}
		}
		public List<Exception> Exceptions
		{
			get
			{
				var result = new List<Exception>();
				if(this.Exception != null)
				{
					result.Add(Exception);
				}
				var libraryChunks = Chunks.OfType<Libf.DebugLibfChunk>().ToList();
				foreach (var chunk in libraryChunks)
				{
					if (chunk.LibraryContainer.Exception != null)
					{
						result.Add(chunk.LibraryContainer.Exception);
					}
				}
				return result;
			}
		}
		public string Dump()
		{
			var dump = _reader.DumpStructure();
			if (!string.IsNullOrEmpty(ParseErrors))
			{
				dump += $"\n{ParseErrors}";
			}
			return dump;
		}
		public string DumpHTML()
		{
			var dump = _reader.DumpHtml();
			return dump;
		}
	}
}
