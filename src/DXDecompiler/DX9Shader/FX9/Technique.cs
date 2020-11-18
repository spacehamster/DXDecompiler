using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.FX9
{
	/*
	 * Refer https://docs.microsoft.com/en-us/windows/win32/direct3d9/techniques-and-passes
	 * Syntax:
	 * technique [ id ]  [< annotation(s) >] 
	 *    { pass(es) }
	 * 
	 */
	public class Technique
	{
		public string Name { get; private set; }
		public List<Annotation> Annotations = new List<Annotation>();
		public List<Pass> Passes = new List<Pass>();

		public static Technique Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Technique();
			var nameOffset = variableReader.ReadUInt32();
			var annotationCount = variableReader.ReadUInt32();
			var passCount = variableReader.ReadUInt32();
			for(int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			for (int i = 0; i < passCount; i++)
			{
				result.Passes.Add(Pass.Parse(reader, variableReader));
			}
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.TryReadString();
			return result;
		}
	}
}
