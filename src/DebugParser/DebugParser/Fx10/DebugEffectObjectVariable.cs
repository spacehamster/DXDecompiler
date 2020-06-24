using SlimShader.Chunks.Fx10;
using SlimShader.Chunks.Rdef;
using SlimShader.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// Type bits
	/// Shader Variable Type
	//	Texture1D = 6,
	//	Texture2D = 7,
	//	Texture3D = 8,
	//  TextureCube = 9,
	/// SamplerState:
	/// SamplerComparisonState:
	/// Based on D3D10_EFFECT_VARIABLE_DESC
	/// </summary>
	public class DebugEffectObjectVariable : IDebugEffectVariable
	{
		public uint NameOffset { get; private set; }
		public string Name { get; private set; }
		public string Semantic { get; private set; }
		public uint TypeOffset { get; private set; }
		public DebugEffectType Type { get; private set; }
		public uint SemanticOffset { get; private set; }
		public uint BufferOffset { get; private set; }
		public uint AssignmentCount { get; private set; }
		public uint AnnotationCount { get; private set; }
		public List<DebugEffectAnnotation> Annotations { get; private set; }
		public List<List<DebugEffectAssignment>> Assignments { get; private set; }
		public List<string> Strings { get; private set; }
		public List<DebugEffectShaderData5> ShaderData5 { get; private set; }
		public List<DebugEffectShaderData> ShaderData { get; private set; }
		public List<DebugEffectGSSOInitializer> GSSOInitializers { get; private set; }

		//TODO
		public uint Flags => 0;
		public uint ExplicitBindPoint => 0;
		IList<IDebugEffectVariable> IDebugEffectVariable.Annotations => 
			Annotations.Cast<IDebugEffectVariable>().ToList();

		private uint ElementCount => Type.ElementCount == 0 ? 1 : Type.ElementCount;
		public DebugEffectObjectVariable()
		{
			Annotations = new List<DebugEffectAnnotation>();
			Assignments = new List<List<DebugEffectAssignment>>();
			Strings = new List<string>();
			ShaderData5 = new List<DebugEffectShaderData5>();
			ShaderData = new List<DebugEffectShaderData>();
			GSSOInitializers = new List<DebugEffectGSSOInitializer>();
	}
		private static bool IfHasAssignments(DebugEffectType type)
		{
			switch (type.VariableType)
			{
				case ShaderVariableType.Sampler:
				case ShaderVariableType.DepthStencil:
				case ShaderVariableType.Blend:
				case ShaderVariableType.Rasterizer:
					return true;
			}
			return false;
		}
		private static bool IsShader(DebugEffectType type)
		{
			switch (type.VariableType)
			{
				case ShaderVariableType.VertexShader:
				case ShaderVariableType.PixelShader:
				case ShaderVariableType.GeometryShader:
				case ShaderVariableType.ComputeShader:
				case ShaderVariableType.HullShader:
				case ShaderVariableType.DomainShader:
					return true;
			}
			return false;
		}
		private static bool IsShader5(DebugEffectType type)
		{
			switch (type.ObjectType)
			{
				case EffectObjectType.VertexShader5:
				case EffectObjectType.PixelShader5:
				case EffectObjectType.GeometryShader5:
				case EffectObjectType.ComputeShader5:
				case EffectObjectType.HullShader5:
				case EffectObjectType.DomainShader5:
					return true;
			}
			return false;
		}
		public static DebugEffectObjectVariable Parse(DebugBytecodeReader reader, DebugBytecodeReader variableReader,
			DebugShaderVersion version, bool isShared = false)
		{
			var result = new DebugEffectObjectVariable();
			var nameOffset = result.NameOffset = variableReader.ReadUInt32("NameOffset");
			var nameReader = reader.CopyAtOffset("NameReader", variableReader, (int)nameOffset);
			result.Name = nameReader.ReadString("Name");
			result.TypeOffset = variableReader.ReadUInt32("TypeOffset");
			var typeReader = reader.CopyAtOffset("TypeReader", variableReader, (int)result.TypeOffset);
			result.Type = DebugEffectType.Parse(reader, typeReader, version);
			var semanticOffset = result.SemanticOffset = variableReader.ReadUInt32("SemanticOffset");
			if (semanticOffset != 0)
			{
				var semanticReader = reader.CopyAtOffset("SemanticReader", variableReader, (int)semanticOffset);
				result.Semantic = semanticReader.ReadString("Semantic");
			}
			else
			{
				result.Semantic = "";
			}
			result.BufferOffset = variableReader.ReadUInt32("BufferOffset");
			if (isShared)
			{
				return result;
			}
			// Initializer data
			if (result.Type.ObjectType == EffectObjectType.String)
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					var stringValueOffset = variableReader.ReadUInt32($"StringValueOffset{i}");
					var stringValueReader = reader.CopyAtOffset($"StringValueReader{i}", variableReader, (int)stringValueOffset);
					result.Strings.Add(stringValueReader.ReadString($"StringValue{i}"));
				}
			}
			if (IfHasAssignments(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					var assignmentCount = variableReader.ReadUInt32($"AssignmentCount{i}");
					var assignments = new List<DebugEffectAssignment>();
					result.Assignments.Add(assignments);
					for (int j = 0; j < assignmentCount; j++)
					{
						variableReader.AddIndent($"Assignment {i}");
						assignments.Add(DebugEffectAssignment.Parse(reader, variableReader));
						variableReader.RemoveIndent();
					}
				}
			}
			if (result.Type.ObjectType == EffectObjectType.GeometryShaderWithStream)
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					variableReader.AddIndent($"GSSOInitializer {i}");
					result.GSSOInitializers.Add(DebugEffectGSSOInitializer.Parse(reader, variableReader));
					variableReader.RemoveIndent();
				}
			}
			else if (IsShader5(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					variableReader.AddIndent($"ShaderData5 {i}");
					result.ShaderData5.Add(DebugEffectShaderData5.Parse(reader, variableReader));
					variableReader.RemoveIndent();
				}
			}
			else if (IsShader(result.Type))
			{
				for (int i = 0; i < result.ElementCount; i++)
				{
					variableReader.AddIndent($"ShaderData {i}");
					result.ShaderData.Add(DebugEffectShaderData.Parse(reader, variableReader));
					variableReader.RemoveIndent();
				}
			}
			result.AnnotationCount = variableReader.ReadUInt32("AnnotationCount");
			for (int i = 0; i < result.AnnotationCount; i++)
			{
				variableReader.AddIndent($"Annotation {i}");
				result.Annotations.Add(DebugEffectAnnotation.Parse(reader, variableReader, version));
				variableReader.RemoveIndent();
			}
			return result;
		}
	}
}
