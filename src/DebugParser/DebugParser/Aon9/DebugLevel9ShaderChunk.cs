using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DebugParser.Aon9
{
	public class DebugLevel9ShaderChunk : DebugBytecodeChunk
	{
		public List<DebugConstantBufferMapping> ConstantBufferMappings { get; private set; }
		public List<DebugLoopRegisterMapping> LoopRegisterMappings { get; private set; }
		public List<DebugUnknown1Mapping> Unknown0Mappings { get; private set; }
		public List<DebugSamplerMapping> SamplerMappings { get; private set; }
		public List<DebugRuntimeConstantMapping> RuntimeConstantMappings { get; private set; }
		public DebugShaderVersion Version;
		public DebugLevel9ShaderChunk()
		{
			ConstantBufferMappings = new List<DebugConstantBufferMapping>();
			LoopRegisterMappings = new List<DebugLoopRegisterMapping>();
			Unknown0Mappings = new List<DebugUnknown1Mapping>();
			SamplerMappings = new List<DebugSamplerMapping>();
			RuntimeConstantMappings = new List<DebugRuntimeConstantMapping>();
		}
		public static DebugLevel9ShaderChunk Parse(DebugBytecodeReader chunkContentReader, uint chunkSize)
		{
			var result = new DebugLevel9ShaderChunk();
			uint chunkSize2 = chunkContentReader.ReadUInt32("chunkSize2");
			result.Version = DebugShaderVersion.ParseAon9(chunkContentReader);
			uint shaderSize = chunkContentReader.ReadUInt32("shaderSize");
			var shaderOffset = chunkContentReader.ReadUInt32("shaderOffset");
			var cbMappingCount = chunkContentReader.ReadUInt16("cbMappingCount");
			var cbMappingOffset = chunkContentReader.ReadUInt16("cbMappingOffset");
			var loopRegisterMappingCount = chunkContentReader.ReadUInt16("loopRegisterMappingCount");
			var loopRegisterMappingOffset = chunkContentReader.ReadUInt16("loopRegisterMappingOffset");
			var unk0MappingCount = chunkContentReader.ReadUInt16("unk0MappingCount");
			var unk0MappingOffset = chunkContentReader.ReadUInt16("unk0MappingOffset");
			var samplerMappingCount = chunkContentReader.ReadUInt16("samplerMappingCount");
			var samplerMappingOffset = chunkContentReader.ReadUInt16("samplerMappingOffset");
			var runtimeConstantMappingCount = chunkContentReader.ReadUInt16("runtimeConstantMappingCount");
			var runtimeConstantMappingOffset = chunkContentReader.ReadUInt16("runtimeConstantMappingOffset");
			if(cbMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset("mappingReader", chunkContentReader, cbMappingOffset);
				for(int i = 0; i < cbMappingCount; i++)
				{
					result.ConstantBufferMappings.Add(DebugConstantBufferMapping.Parse(mappingReader));
				}
			}
			if(loopRegisterMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset("mappingReader", chunkContentReader, loopRegisterMappingOffset);
				for(int i = 0; i < loopRegisterMappingCount; i++)
				{
					result.LoopRegisterMappings.Add(DebugLoopRegisterMapping.Parse(mappingReader));
				}
			}
			if(unk0MappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset("mappingReader", chunkContentReader, unk0MappingOffset);
				for(int i = 0; i < unk0MappingCount; i++)
				{
					result.Unknown0Mappings.Add(DebugUnknown1Mapping.Parse(mappingReader));
				}
			}
			if(samplerMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset("mappingReader", chunkContentReader, samplerMappingOffset);
				for(int i = 0; i < samplerMappingCount; i++)
				{
					result.SamplerMappings.Add(DebugSamplerMapping.Parse(mappingReader));
				}
				// FXC dissassembly sorts sampler mappings 
				result.SamplerMappings = result.SamplerMappings
					.OrderBy(s => s.TargetSampler)
					.ToList();
			}
			if(runtimeConstantMappingCount > 0)
			{
				var mappingReader = chunkContentReader.CopyAtOffset("mappingReader", chunkContentReader, runtimeConstantMappingOffset);
				for(int i = 0; i < runtimeConstantMappingCount; i++)
				{
					result.RuntimeConstantMappings.Add(DebugRuntimeConstantMapping.Parse(mappingReader));
				}
			}
			var shaderChunkReader = chunkContentReader.CopyAtOffset("shaderChunkReader", chunkContentReader, (int)shaderOffset);
			var byteCode = shaderChunkReader.ReadBytes("bytecode", (int)shaderSize);
			return result;
		}
	}
}
