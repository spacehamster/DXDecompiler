using System;

namespace DXDecompiler.Chunks.Fx10.Assignemnt
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AssignmentTypeAttribute : Attribute
	{
		public Type Type { get; }

		public AssignmentTypeAttribute(Type type)
		{
			Type = type;
		}
	}
}
