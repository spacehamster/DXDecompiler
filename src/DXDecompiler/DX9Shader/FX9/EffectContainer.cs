using DXDecompiler.Util;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.DX9Shader.FX9
{
	/// <summary>
	/// Effects are a collection of techniques and variables and states.
	/// DirectX 9 effects are compiled with the fx_2_0 profile
	/// </summary>
	public class EffectContainer
	{
		public List<Variable> Variables = new List<Variable>();
		public List<Technique> Techniques = new List<Technique>();
		public List<VariableBlob> VariableBlobs = new List<VariableBlob>();
		public List<StateBlob> StateBlobs = new List<StateBlob>();

		public Dictionary<Parameter, List<VariableBlob>> VariableBlobLookup = new Dictionary<Parameter, List<VariableBlob>>();
		public Dictionary<Assignment, StateBlob> StateBlobLookup = new Dictionary<Assignment, StateBlob>();
		public static EffectContainer Parse(BytecodeReader reader, uint length)
		{
			var result = new EffectContainer();
			var chunkReader = reader.CopyAtCurrentPosition();
			var footerOffset = chunkReader.ReadUInt32() + 4;
			var bodyReader = chunkReader.CopyAtCurrentPosition();
			var footerReader = reader.CopyAtOffset((int)footerOffset);
			var variableCount = footerReader.ReadUInt32();
			var techniqueCount = footerReader.ReadUInt32();
			var passCount = footerReader.ReadUInt32();
			var shaderCount = footerReader.ReadUInt32();
			for (int i = 0; i < variableCount; i++)
			{
				result.Variables.Add(Variable.Parse(bodyReader, footerReader));
			}
			for (int i = 0; i < techniqueCount; i++)
			{
				result.Techniques.Add(Technique.Parse(bodyReader, footerReader));
			}
			var variableBlobCount = footerReader.ReadUInt32();
			var stateBlobCount = footerReader.ReadUInt32();
			for (int i = 0; i < variableBlobCount; i++)
			{
				var data = VariableBlob.Parse(bodyReader, footerReader);
				result.VariableBlobs.Add(data);

			}
			for (int i = 0; i < stateBlobCount; i++)
			{
				var data = StateBlob.Parse(bodyReader, footerReader);
				result.StateBlobs.Add(data);
			}
			result.BuildBlobLookup();
			return result;
		}
		private void AnnotationBlobLookup(List<Annotation> annotations)
		{
			foreach (var annotation in annotations)
			{
				if (annotation.Parameter.ParameterType.HasVariableBlob())
				{
					var blobs = new List<VariableBlob>();
					var elementCount = annotation.Parameter.ElementCount == 0 ? 1 : annotation.Parameter.ElementCount;
					for (int i = 0; i < elementCount; i++)
					{
						var index = annotation.Value[i].UInt;
						var blob = VariableBlobs.FirstOrDefault(b => b.Index == index);
						blobs.Add(blob);
					}
					VariableBlobLookup[annotation.Parameter] = blobs;
				}
			}
		}
		public void BuildBlobLookup()
		{
			for(int i = 0; i < Variables.Count; i++)
			{
				var variable = Variables[i];
				if(variable.Parameter.ParameterType.HasVariableBlob())
				{
					var blobs = new List<VariableBlob>();
					var elementCount = variable.Parameter.ElementCount == 0 ? 1 : variable.Parameter.ElementCount;
					for (int j = 0; j < elementCount; j++)
					{
						var index = variable.DefaultValue[j].UInt;
						var blob = VariableBlobs.FirstOrDefault(b => b.Index == index);
						blobs.Add(blob);
					}
					VariableBlobLookup[variable.Parameter] = blobs;
				}
				AnnotationBlobLookup(variable.Annotations);
				if (variable.Parameter.ParameterType.IsSampler())
				{
					for(int j = 0; j < variable.SamplerStates.Count; j++)
					{
						var samplerState = variable.SamplerStates[j];
						for (int k = 0; k < samplerState.Assignments.Count; k++)
						{
							var assignment = samplerState.Assignments[k];
							if (assignment.Type.HasStateBlob())
							{
								StateBlobLookup[assignment] = StateBlobs.FirstOrDefault(b => 
									b.PassIndex == i &&
									b.SamplerStateIndex == j &&
									b.AssignmentIndex == k);
							}
						}
					}
				}
			}
			for (int i = 0; i < Techniques.Count; i++)
			{
				var technique = Techniques[i];
				AnnotationBlobLookup(technique.Annotations);
				for (int j = 0; j < technique.Passes.Count; j++)
				{
					var pass = technique.Passes[j];
					AnnotationBlobLookup(pass.Annotations);
					for (int k = 0; k < pass.Assignments.Count; k++)
					{
						var assignment = pass.Assignments[k];
						var blob = StateBlobs.FirstOrDefault(b => 
								b.TechniqueIndex == i &&
								b.PassIndex == j &&
								b.AssignmentIndex == k);
						if(blob != null || assignment.Type.HasStateBlob())
						{
							StateBlobLookup[assignment] = blob;
						}
					}
				}
			}
		}
	}
}
