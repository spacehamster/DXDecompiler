using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;
using System.Collections.Generic;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	/// <summary>
	/// 
	/// Based on D3D10_TECHNIQUE_DESC
	/// </summary>
	public class EffectTechnique
	{
		public string Name { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }
		public List<EffectPass> Passes { get; private set; }

		private ShaderVersion m_Version;

		public EffectTechnique(ShaderVersion version)
		{
			Annotations = new List<EffectAnnotation>();
			Passes = new List<EffectPass>();
			m_Version = version;
		}
		public static EffectTechnique Parse(BytecodeReader reader, BytecodeReader techniqueReader, ShaderVersion version)
		{
			var result = new EffectTechnique(version);
			var nameOffset = techniqueReader.ReadUInt32();
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();
			var passCount = techniqueReader.ReadUInt32();
			var annotationCount = techniqueReader.ReadUInt32();

			for (int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, techniqueReader, version));
			}
			for (int i = 0; i < passCount; i++)
			{
				result.Passes.Add(EffectPass.Parse(reader, techniqueReader, version));
			}
			return result;
		}
		public string ToString(int indent)
		{
			var techniqueVersion = m_Version.MajorVersion == 5 ? "11" : "10";
			var sb = new StringBuilder();
			var indentString = new string(' ', indent * 4);
			sb.Append(indentString);
			sb.AppendLine(string.Format("technique{0} {1}", techniqueVersion, Name));
			sb.Append(indentString);
			sb.AppendLine("{");
			foreach(var pass in Passes)
			{
				sb.Append(pass.ToString(indent + 1));
			}
			sb.Append(indentString);
			sb.AppendLine("}");
			sb.AppendLine();
			return sb.ToString();
		}
		public override string ToString()
		{
			return ToString(0);
		}
	}
}
