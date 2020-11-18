using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	/// <summary>
	/// Based on D3D10_PASS_DESC
	/// </summary>
	public class EffectPass
	{
		public string Name { get; private set; }
		public List<EffectAssignment> Assignments { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public EffectPass()
		{
			Assignments = new List<EffectAssignment>();
			Annotations = new List<EffectAnnotation>();
		}
		public static EffectPass Parse(BytecodeReader reader, BytecodeReader passReader, ShaderVersion version)
		{
			var result = new EffectPass();
			var nameOffset = passReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var assignmentCount = passReader.ReadUInt32();
			var annotationCount = passReader.ReadUInt32();
			for (int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, passReader, version));
			}
			for (int i = 0; i < assignmentCount; i++)
			{
				result.Assignments.Add(EffectAssignment.Parse(reader, passReader));
			}
			return result;
		}
		public string ToString(int indent)
		{
			var indentString = new string(' ', indent * 4);
			var sb = new StringBuilder();
			sb.AppendLine(string.Format("{0}pass {1}", indentString, Name));
			sb.AppendLine(string.Format("{0}{{", indentString));
			foreach(var assignment in Assignments)
			{
				if (assignment is EffectInlineShaderAssignment || assignment is EffectInlineShader5Assignment)
				{
					var shaderText = assignment.ToString()
						.Replace(Environment.NewLine, $"{Environment.NewLine}{indentString}");
					sb.AppendLine(string.Format("{0}    {1}", indentString, shaderText));
				}
				else
				{
					sb.AppendLine(string.Format("{0}    {1}", indentString, assignment.ToString()));
				}
			}
			sb.AppendLine(string.Format("{0}}}", indentString));
			sb.AppendLine();
			return sb.ToString();
		}
		public override string ToString()
		{
			return ToString(0);
		}
	}
}
