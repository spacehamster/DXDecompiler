using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Aon9
{
	public class Level9ShaderChunk : BytecodeChunk
	{
		public DX9Shader.ShaderModel ShaderModel { get; private set; }
		public List<ConstantBufferMapping> ConstantBufferMappings { get; private set; }
		public List<LoopRegisterMapping> LoopRegisterMappings { get; private set; }
		public List<Unknown1Mapping> Unknown0Mappings { get; private set; }
		public List<SamplerMapping> SamplerMappings { get; private set; }
		public List<RuntimeConstantMapping> RuntimeConstantMappings { get; private set; }
		public ShaderVersion Version { get; private set; }

		public Level9ShaderChunk()
		{
			ConstantBufferMappings = new List<ConstantBufferMapping>();
			LoopRegisterMappings = new List<LoopRegisterMapping>();
			Unknown0Mappings = new List<Unknown1Mapping>();
			SamplerMappings = new List<SamplerMapping>();
			RuntimeConstantMappings = new List<RuntimeConstantMapping>();
	}
		internal static BytecodeChunk Parse(BytecodeReader chunkContentReader, uint chunkSize)
		{
			var result = new Level9ShaderChunk();
			uint chunkSize2 = chunkContentReader.ReadUInt32();
			result.Version = ShaderVersion.ParseAon9(chunkContentReader);
			uint shaderSize = chunkContentReader.ReadUInt32();
			var shaderOffset = chunkContentReader.ReadUInt32();
			var cbMappingCount = chunkContentReader.ReadUInt16();
			var cbMappingOffset = chunkContentReader.ReadUInt16();
			var loopRegisterMappingCount = chunkContentReader.ReadUInt16();
			var loopRegisterMappingOffset = chunkContentReader.ReadUInt16();
			var unk0MappingCount = chunkContentReader.ReadUInt16();
			var unk0MappingOffset = chunkContentReader.ReadUInt16();
			var samplerMappingCount = chunkContentReader.ReadUInt16();
			var samplerMappingOffset = chunkContentReader.ReadUInt16();
			var runtimeConstantMappingCount = chunkContentReader.ReadUInt16();
			var runtimeConstantMappingOffset = chunkContentReader.ReadUInt16();
			if (cbMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset(cbMappingOffset);
				for (int i = 0; i < cbMappingCount; i++)
				{
					result.ConstantBufferMappings.Add(ConstantBufferMapping.Parse(mappingReader));
				}
			}
			if (loopRegisterMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset(loopRegisterMappingOffset);
				for (int i = 0; i < loopRegisterMappingCount; i++)
				{
					result.LoopRegisterMappings.Add(LoopRegisterMapping.Parse(mappingReader));
				}
			}
			if (unk0MappingCount > 0)
			{
				Debug.Assert(false, "Unknown Level9 Mapping");
				var mappingReader = chunkContentReader.CopyAtOffset(unk0MappingOffset);
				for (int i = 0; i < unk0MappingCount; i++)
				{
					result.Unknown0Mappings.Add(Unknown1Mapping.Parse(mappingReader));
				}
			}
			if (samplerMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset(samplerMappingOffset);
				for (int i = 0; i < samplerMappingCount; i++)
				{
					result.SamplerMappings.Add(SamplerMapping.Parse(mappingReader));
				}
				// FXC dissassembly sorts sampler mappings 
				result.SamplerMappings = result.SamplerMappings
					.OrderBy(s => s.TargetSampler)
					.ToList();
			}
			if (runtimeConstantMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset(runtimeConstantMappingOffset);
				for (int i = 0; i < runtimeConstantMappingCount; i++)
				{
					result.RuntimeConstantMappings.Add(RuntimeConstantMapping.Parse(mappingReader));
				}
			}
			var shaderChunkReader = chunkContentReader.CopyAtOffset((int)shaderOffset);
			var byteCode = shaderChunkReader.ReadBytes((int)shaderSize);
			result.ShaderModel = DX9Shader.ShaderReader.ReadShader(byteCode);
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			if (ConstantBufferMappings.Count > 0 || LoopRegisterMappings.Count > 0)
			{
				sb.AppendLine("// Constant buffer to DX9 shader constant mappings:");
				sb.AppendLine("//");
				if (LoopRegisterMappings.Count > 0)
				{
					sb.AppendLine("// Target Reg Buffer  Source Reg Component");
					sb.AppendLine("// ---------- ------- ---------- ---------");
					foreach (var mapping in LoopRegisterMappings)
					{
						sb.AppendLine(mapping.ToString());
					}
					sb.AppendLine("//");
				}
				if (ConstantBufferMappings.Count > 0)
				{
					sb.AppendLine("// Target Reg Buffer  Start Reg # of Regs        Data Conversion");
					sb.AppendLine("// ---------- ------- --------- --------- ----------------------");
					foreach (var mapping in ConstantBufferMappings)
					{
						sb.AppendLine(mapping.ToString());
					}
					sb.AppendLine("//");
				}
				sb.AppendLine("//");
			}
			if (Unknown0Mappings.Count > 0)
			{
				sb.AppendLine("// Unknown0Mappings:");
				sb.AppendLine("//");
				foreach (var mapping in Unknown0Mappings)
				{
					sb.AppendLine(mapping.ToString());
				}
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			if (SamplerMappings.Count > 0)
			{
				sb.AppendLine("// Sampler/Resource to DX9 shader sampler mappings:");
				sb.AppendLine("//");
				sb.AppendLine("// Target Sampler Source Sampler  Source Resource");
				sb.AppendLine("// -------------- --------------- ----------------");
				foreach(var mapping in SamplerMappings)
				{
					sb.AppendLine(mapping.ToString());
				}
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			if (RuntimeConstantMappings.Count > 0)
			{
				sb.AppendLine("// Runtime generated constant mappings:");
				sb.AppendLine("//");
				sb.AppendLine("// Target Reg                               Constant Description");
				sb.AppendLine("// ---------- --------------------------------------------------");
				foreach (var mapping in RuntimeConstantMappings)
				{
					sb.AppendLine(mapping.ToString());
				}
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			switch (ChunkType)
			{
				case ChunkType.Aon9:
					sb.AppendLine($"// Level9 shader bytecode:");
					break;
				case ChunkType.Xnas:
					sb.AppendLine($"// XNA shader bytecode:");
					break;
				case ChunkType.Xnap:
					sb.AppendLine($"// XNA Prepass shader bytecode:");
					break;
				default:
					sb.AppendLine($"// {ChunkType} shader bytecode:");
					break;
			}
			sb.AppendLine($"//");
			using (var stream = new MemoryStream())
			{
				var decompiledAssmembly = DX9Shader.AsmWriter.Disassemble(ShaderModel);
				decompiledAssmembly = decompiledAssmembly
					.Replace("ps_2_1", "ps_2_x")
					.Replace("vs_2_1", "vs_2_x");
				sb.AppendLine(decompiledAssmembly);
			}
			return sb.ToString();
		}
	}
}
