using SlimShader.Chunks.Common;
using SlimShader.Util;
using System.Collections.Generic;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectGroup
	{
		public string Name { get; private set; }
		public List<EffectTechnique> Techniques { get; private set; }
		public List<EffectAnnotation> Annotations { get; private set; }

		public EffectGroup()
		{
			Techniques = new List<EffectTechnique>();
			Annotations = new List<EffectAnnotation>();
		}
		public static EffectGroup Parse(BytecodeReader reader, BytecodeReader groupReader, ShaderVersion version)
		{
			var result = new EffectGroup();
			var nameOffset = groupReader.ReadUInt32();
			if(nameOffset != 0)
			{
				var nameReader = reader.CopyAtOffset((int)nameOffset);
				result.Name = nameReader.ReadString();
			} else
			{
				result.Name = "";
			}
			var techniqueCount = groupReader.ReadUInt32();
			for (int i = 0; i < techniqueCount; i++)
			{
				result.Techniques.Add(EffectTechnique.Parse(reader, groupReader, version));
			}
			var annotationCount = groupReader.ReadUInt32();
			for (int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(EffectAnnotation.Parse(reader, groupReader, version));
			}
			return result;
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("fxgroup");
			if (!string.IsNullOrEmpty(Name))
			{
				sb.Append(" ");
				sb.Append(Name);
			}
			sb.AppendLine();
			sb.AppendLine("{");
			sb.AppendLine("    //");
			sb.AppendLine(string.Format("    // {0} technique(s)",
				Techniques.Count));
			sb.AppendLine("    //");
			foreach(var technique in Techniques)
			{
				sb.Append(technique.ToString(1));
			}
			sb.Append("}");
			return sb.ToString();

		}
	}
}
