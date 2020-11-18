using System.Collections.Generic;

namespace DXDecompiler.DebugParser.FX9
{
	public class DebugEffectChunk
	{
		public uint VariableCount;
		public uint TechniqueCount;
		public uint PassCount;
		public uint ShaderCount;
		public uint VariableBlobCount;
		public uint StateBlobCount;

		List<DebugVariable> Variables = new List<DebugVariable>();
		List<DebugTechnique> Techniques = new List<DebugTechnique>();
		List<DebugBinaryData> VariableBlobs = new List<DebugBinaryData>();
		List<DebugStateBlob> StateBlobs = new List<DebugStateBlob>();
		public static DebugEffectChunk Parse(DebugBytecodeReader reader, uint length)
		{
			var result = new DebugEffectChunk();
			var footerOffset = reader.ReadUInt32("footerOffset");
			var bodyReader = reader.CopyAtCurrentPosition("BodyReader", reader);
			var dummyReader = bodyReader.CopyAtCurrentPosition("Dummy", bodyReader);
			dummyReader.ReadUInt32("Zero");
			var footerReader = bodyReader.CopyAtOffset("FooterReader", reader, (int)footerOffset);
			var variableCount = result.VariableCount = footerReader.ReadUInt32("VariableCount");
			var techniqueCount = result.TechniqueCount = footerReader.ReadUInt32("TechniqueCount");
			result.PassCount = footerReader.ReadUInt32("PassCount");
			result.ShaderCount = footerReader.ReadUInt32("ShaderCount");

			for(int i = 0; i < variableCount; i++)
			{
				footerReader.AddIndent($"Variable {i}");
				result.Variables.Add(DebugVariable.Parse(bodyReader, footerReader));
				footerReader.RemoveIndent();
			}
			for(int i = 0; i < techniqueCount; i++)
			{
				footerReader.AddIndent($"Technique {i}");
				result.Techniques.Add(DebugTechnique.Parse(bodyReader, footerReader));
				footerReader.RemoveIndent();
			}

			result.VariableBlobCount = footerReader.ReadUInt32("VariableBlobCount");
			result.StateBlobCount = footerReader.ReadUInt32("StateBlobCount");
			for(int i = 0; i < result.VariableBlobCount; i++)
			{
				footerReader.AddIndent($"VariableBlob {i}");
				result.VariableBlobs.Add(DebugBinaryData.Parse(bodyReader, footerReader));
				footerReader.RemoveIndent();
			}
			for(int i = 0; i < result.StateBlobCount; i++)
			{
				footerReader.AddIndent($"StateBlob {i}");
				result.StateBlobs.Add(DebugStateBlob.Parse(bodyReader, footerReader));
				footerReader.RemoveIndent();
			}
			return result;

		}
	}
}
