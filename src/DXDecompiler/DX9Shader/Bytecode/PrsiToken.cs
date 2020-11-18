using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DXDecompiler.DX9Shader.Bytecode
{
	public class PrsiToken
	{
		public uint OutputRegisterOffset;
		public uint OutputRegisterCount;
		public List<Tuple<uint, uint>> Mapping = new List<Tuple<uint, uint>>();

		private uint Unknown1;
		private uint Unknown2;
		private uint Unknown3;
		private uint Unknown4;
		private uint Unknown5;
		private uint Unknown6;
		public static PrsiToken Parse(BytecodeReader reader)
		{
			var result = new PrsiToken();
			result.OutputRegisterOffset = reader.ReadUInt32();
			var unknown1 = result.Unknown1 = reader.ReadUInt32();
			var unknown2 = result.Unknown2 = reader.ReadUInt32();
			result.OutputRegisterCount = reader.ReadUInt32();
			var unknown3 = result.Unknown3 = reader.ReadUInt32();
			var unknown4 = result.Unknown4 = reader.ReadUInt32();
			var mappingCount = reader.ReadUInt32();
			var unknown5 = result.Unknown5 = reader.ReadUInt32();
			var unknown6 = result.Unknown6 = reader.ReadUInt32();
			Debug.Assert(unknown1 == 0, $"Unknown1={unknown1}");
			Debug.Assert(unknown2 == 0, $"Unknown2={unknown2}");
			Debug.Assert(unknown3 == 0, $"Unknown3={unknown3}");
			Debug.Assert(unknown4 == 0, $"Unknown4={unknown4}");

			Debug.Assert(unknown5 == result.OutputRegisterOffset,
				$"Unknown5 ({unknown5}) and OutputRegisterOffset ({result.OutputRegisterOffset}) differ");
			//Debug.Assert(unk6 == outputRegisterCount,
			//	$"unk6 ({unk6}) and OutputRegisterCount ({outputRegisterCount}) differ");
			for (int i = 0; i < mappingCount; i++)
			{
				result.Mapping.Add(new Tuple<uint, uint>(
					reader.ReadUInt32(),
					reader.ReadUInt32()));
			}
			return result;
		}
		public string Dump()
		{
			var sb = new StringBuilder();
			sb.AppendLine($"PRSI");
			sb.AppendLine($"  OutputRegisterOffset: {OutputRegisterOffset}");
			sb.AppendLine($"  Unknown1: {Unknown1}");
			sb.AppendLine($"  Unknown2: {Unknown2}");
			sb.AppendLine($"  OutputRegisterCount: {OutputRegisterCount}");
			sb.AppendLine($"  Unknown3: {Unknown3}");
			sb.AppendLine($"  Unknown4: {Unknown4}");
			sb.AppendLine($"  Unknown5: {Unknown5}");
			sb.AppendLine($"  Unknown6: {Unknown6}");
			sb.AppendLine($"  Mappings: {Mapping.Count}");
			for(int i = 0; i < Mapping.Count; i++)
			{
				var pair = Mapping[i];
				sb.AppendLine($"    {i} - ConstOutput: {pair.Item1} ConstInput {pair.Item2}");
			}
			return sb.ToString();
		}
	}
}
