using DXDecompiler.Chunks.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser.Stat
{
	public class DebugStatisticsChunk : DebugBytecodeChunk
	{
		public uint InstructionCount { get; private set; }
		public uint TempRegisterCount { get; private set; }
		public uint TempArrayCount { get; private set; }
		public uint DefineCount { get; private set; }
		public uint DeclarationCount { get; private set; }
		public uint TextureNormalInstructions { get; private set; }
		public uint TextureLoadInstructions { get; private set; }
		public uint TextureCompInstructions { get; private set; }
		public uint TextureBiasInstructions { get; private set; }
		public uint TextureGradientInstructions { get; private set; }
		public uint FloatInstructionCount { get; private set; }
		public uint IntInstructionCount { get; private set; }
		public uint UIntInstructionCount { get; private set; }
		public uint StaticFlowControlCount { get; private set; }
		public uint DynamicFlowControlCount { get; private set; }
		public uint MacroInstructionCount { get; private set; }
		public uint ArrayInstructionCount { get; private set; }
		public uint CutInstructionCount { get; private set; }
		public uint EmitInstructionCount { get; private set; }
		public PrimitiveTopology GeometryShaderOutputTopology { get; private set; }
		public uint GeometryShaderMaxOutputVertexCount { get; private set; }
		public bool IsSampleFrequencyShader { get; private set; }
		public Primitive InputPrimitive { get; private set; }
		public uint GeometryShaderInstanceCount { get; private set; }
		public uint ControlPoints { get; private set; }
		public TessellatorOutputPrimitive HullShaderOutputPrimitive { get; private set; }
		public TessellatorPartitioning HullShaderPartitioning { get; private set; }
		public TessellatorDomain TessellatorDomain { get; private set; }
		public uint BarrierInstructions { get; private set; }
		public uint InterlockedInstructions { get; private set; }
		public uint TextureStoreInstructions { get; private set; }
		public uint MovInstructionCount { get; private set; }
		public uint MovCInstructionCount { get; private set; }
		public uint ConversionInstructionCount { get; private set; }

		public static DebugStatisticsChunk Parse(DebugBytecodeReader reader, uint chunkSize)
		{
			var size = chunkSize / sizeof(uint);

			var result = new DebugStatisticsChunk
			{
				InstructionCount = reader.ReadUInt32("InstructionCount"),
				TempRegisterCount = reader.ReadUInt32("TempRegisterCount"),
				DefineCount = reader.ReadUInt32("DefineCount"),
				DeclarationCount = reader.ReadUInt32("DeclarationCount"),
				FloatInstructionCount = reader.ReadUInt32("FloatInstructionCount"),
				IntInstructionCount = reader.ReadUInt32("IntInstructionCount"),
				UIntInstructionCount = reader.ReadUInt32("UIntInstructionCount"),
				StaticFlowControlCount = reader.ReadUInt32("StaticFlowControlCount"),
				DynamicFlowControlCount = reader.ReadUInt32("DynamicFlowControlCount"),
				MacroInstructionCount = reader.ReadUInt32("MacroInstructionCount"), // Guessed
				TempArrayCount = reader.ReadUInt32("TempArrayCount"),
				ArrayInstructionCount = reader.ReadUInt32("ArrayInstructionCount"),
				CutInstructionCount = reader.ReadUInt32("CutInstructionCount"),
				EmitInstructionCount = reader.ReadUInt32("EmitInstructionCount"),
				TextureNormalInstructions = reader.ReadUInt32("TextureNormalInstructions"),
				TextureLoadInstructions = reader.ReadUInt32("TextureLoadInstructions"),
				TextureCompInstructions = reader.ReadUInt32("TextureCompInstructions"),
				TextureBiasInstructions = reader.ReadUInt32("TextureBiasInstructions"),
				TextureGradientInstructions = reader.ReadUInt32("TextureGradientInstructions"),
				MovInstructionCount = reader.ReadUInt32("MovInstructionCount"),
				MovCInstructionCount = reader.ReadUInt32("MovCInstructionCount"),
				ConversionInstructionCount = reader.ReadUInt32("ConversionInstructionCount")
			};

			//TODO
			var unknown0 = reader.ReadUInt32("StatisticsChunkUnknown0");
			result.InputPrimitive = (Primitive)reader.ReadUInt32("InputPrimitive");
			result.GeometryShaderOutputTopology = reader.ReadEnum32<PrimitiveTopology>("GeometryShaderOutputTopology");
			result.GeometryShaderMaxOutputVertexCount = reader.ReadUInt32("GeometryShaderMaxOutputVertexCount");

			var unknown1 = reader.ReadUInt32("StatisticsChunkUnknown1");
			//if (unknown1 == 0 || unknown1 == 1 || unknown1 == 3) throw new System.Exception($"unknown1 is {unknown1}");
			//TODO: CheckAccessFullyMapped textures have large unknown1
			//Texture_Texture2D, Texture_Texture2DArray, Texture_TextureCube, Texture_TextureCubeArray

			var unknown2 = reader.ReadUInt32("StatisticsChunkUnknown2");
			//if (unknown2 != 0 && unknown2 != 2) throw new System.Exception($"unknown2 is {unknown2}");

			result.IsSampleFrequencyShader = (reader.ReadUInt32("IsSampleFrequencyShader") == 1);

			// DX10 stat size
			if (size == 29)
				return result;

			result.GeometryShaderInstanceCount = reader.ReadUInt32("GeometryShaderInstanceCount");
			result.ControlPoints = reader.ReadUInt32("ControlPoints");
			result.HullShaderOutputPrimitive = reader.ReadEnum32<TessellatorOutputPrimitive>("HullShaderOutputPrimitive");
			result.HullShaderPartitioning = reader.ReadEnum32<TessellatorPartitioning>("HullShaderPartitioning");
			result.TessellatorDomain = reader.ReadEnum32<TessellatorDomain>("TessellatorDomain");

			result.BarrierInstructions = reader.ReadUInt32("BarrierInstructions");
			result.InterlockedInstructions = reader.ReadUInt32("InterlockedInstructions");
			result.TextureStoreInstructions = reader.ReadUInt32("TextureStoreInstructions");

			// DX11 stat size.
			if (size == 37)
				return result;

			throw new ParseException("Unhandled stat size: " + chunkSize);
		}
	}
}