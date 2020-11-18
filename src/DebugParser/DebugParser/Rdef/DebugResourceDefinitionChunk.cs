using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Diagnostics;

namespace DXDecompiler.DebugParser.Rdef
{
	public class DebugResourceDefinitionChunk : DebugBytecodeChunk
	{
		public List<DebugConstantBuffer> ConstantBuffers { get; private set; }
		public List<DebugResourceBinding> ResourceBindings { get; private set; }
		public DebugShaderVersion Target { get; private set; }
		public ShaderFlags Flags { get; private set; }
		public string Creator { get; private set; }
		public uint InterfaceSlotCount { get; private set; }
		public DebugResourceDefinitionChunk()
		{
			ConstantBuffers = new List<DebugConstantBuffer>();
			ResourceBindings = new List<DebugResourceBinding>();
		}
		static int CalculateResourceBindingStride(DebugShaderVersion version)
		{
			if(version.MajorVersion == 5 && version.MinorVersion == 1)
			{
				return 40;
			}
			else
			{
				return 32;
			}
		}
		public static DebugResourceDefinitionChunk Parse(DebugBytecodeReader reader)
		{
			var headerReader = reader.CopyAtCurrentPosition("RDefHeader", reader);

			uint constantBufferCount = headerReader.ReadUInt32("constantBufferCount");
			uint constantBufferOffset = headerReader.ReadUInt32("constantBufferOffset");
			uint resourceBindingCount = headerReader.ReadUInt32("resourceBindingCount");
			uint resourceBindingOffset = headerReader.ReadUInt32("resourceBindingOffset");
			var target = DebugShaderVersion.ParseRdef(headerReader);
			var flags = headerReader.ReadEnum32<ShaderFlags>("flags");

			var creatorOffset = headerReader.ReadUInt32("creatorOffset");
			var creatorReader = reader.CopyAtOffset("CreatorString", headerReader, (int)creatorOffset);
			var creator = creatorReader.ReadString("creator");

			var result = new DebugResourceDefinitionChunk
			{
				Target = target,
				Flags = flags,
				Creator = creator
			};

			if(target.MajorVersion == 5 || target.ProgramType == ProgramType.LibraryShader)
			{
				var isVersion5_1 = target.MajorVersion == 5 && target.MinorVersion == 1;
				if(isVersion5_1)
				{
					var unknown0 = headerReader.ReadUInt32("unkRdefHeader");
				}
				else
				{
					string rd11 = headerReader.ReadUInt32("rd11").ToFourCcString();
					if(rd11 != "RD11")
						throw new ParseException("Expected RD11.");
				}

				var unkStride1 = headerReader.ReadUInt32("unkStride1"); // TODO
				var constantBufferStride = headerReader.ReadUInt32("constantBufferStride");
				var resourceBindingStride = headerReader.ReadUInt32("resourceBindingStride");
				//Shader Variable Stride?
				var unkStride2 = headerReader.ReadUInt32("unkStride2");
				var unkStride3 = headerReader.ReadUInt32("unkStride3");
				//Shader Type Member Stride?
				var unkStride4 = headerReader.ReadUInt32("unkStride4");

				Debug.Assert(unkStride1 == 60, $"unkStride1 is {unkStride1}");
				Debug.Assert(constantBufferStride == 24, $"constantBufferStride is {constantBufferStride}");
				Debug.Assert(resourceBindingStride == CalculateResourceBindingStride(target),
					$"resourceBindingStride is {resourceBindingStride}");
				Debug.Assert(unkStride2 == 40, $"unkStride2 is {unkStride2}");
				Debug.Assert(unkStride3 == 36, $"unkStride3 is {unkStride3}");
				Debug.Assert(unkStride4 == 12, $"unkStride4 is {unkStride4}");

				result.InterfaceSlotCount = headerReader.ReadUInt32("InterfaceSlotCount");
			}

			var resourceBindingReader = reader.CopyAtOffset("ResourceBindings", headerReader, (int)resourceBindingOffset);
			for(int i = 0; i < resourceBindingCount; i++)
				result.ResourceBindings.Add(DebugResourceBinding.Parse(reader, resourceBindingReader, target));

			var constantBufferReader = reader.CopyAtOffset("ContantBuffers", headerReader, (int)constantBufferOffset);
			constantBufferReader.Indent++;
			for(int i = 0; i < constantBufferCount; i++)
				result.ConstantBuffers.Add(DebugConstantBuffer.Parse(reader, constantBufferReader, result.Target));

			return result;
		}

	}
}
