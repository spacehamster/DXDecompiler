using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DX9Shader.FX9
{
	/*
	 * Refer https://docs.microsoft.com/en-us/windows/win32/direct3d9/techniques-and-passes
	 * Syntax:
	 * pass  [ id ]  [< annotation(s) >] 
	 *    { state assignment(s) }
	 * 
	 */
	public class Pass
	{
		public string Name { get; private set; }
		public List<Annotation> Annotations = new List<Annotation>();
		public List<Assignment> Assignments = new List<Assignment>();

		public static Pass Parse(BytecodeReader reader, BytecodeReader variableReader)
		{
			var result = new Pass();
			var nameOffset = variableReader.ReadUInt32();
			var annotationCount = variableReader.ReadUInt32();
			var assignmentCount = variableReader.ReadUInt32();
			for(int i = 0; i < annotationCount; i++)
			{
				result.Annotations.Add(Annotation.Parse(reader, variableReader));
			}
			for(int i = 0; i < assignmentCount; i++)
			{
				result.Assignments.Add(Assignment.Parse(reader, variableReader));
			}
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.TryReadString();
			return result;
		}
	}
}
