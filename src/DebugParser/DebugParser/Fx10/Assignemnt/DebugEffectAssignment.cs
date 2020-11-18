using DXDecompiler.Chunks.Fx10;
using DXDecompiler.DebugParser.Chunks.Fx10.Assignemnt;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	public class DebugEffectAssignment
	{
		public EffectAssignmentType MemberType;
		public uint MemberIndex;
		public EffectCompilerAssignmentType AssignmentType;
		public uint ValueOffset;
		public string MemberName {
			get
			{
				if (MemberType.IsArrayAssignemnt())
				{
					return string.Format("{0}[{1}]", MemberType, MemberIndex);
				}
				return MemberType.ToString();
			}
		}
		public static DebugEffectAssignment Parse(DebugBytecodeReader reader, DebugBytecodeReader annotationReader)
		{
			//MemberType type, such as AddressV or Filter
			var memberType = (EffectAssignmentType)annotationReader.ReadUInt32("MemberType");
			//	$"EffectStateAnnotationType is {memberType}");
			//MemberIndex is 1 for BlendEnable[1] = TRUE;
			var memberIndex = annotationReader.ReadUInt32("MemberIndex");
			var assignmentType = (EffectCompilerAssignmentType)annotationReader.ReadUInt32("AssignmentType");
			var valueOffset = annotationReader.ReadUInt32("ValueOffset");
			var typeSpecificReader = reader.CopyAtOffset("TypeSpecificReader", annotationReader, (int)valueOffset);
			DebugEffectAssignment result;
			switch (assignmentType)
			{
				case EffectCompilerAssignmentType.Constant:
					result = DebugEffectConstantAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.Variable:
					result = DebugEffectVariableAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.ConstantIndex:
					result = DebugEffectConstantIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.VariableIndex:
					result = DebugEffectVariableIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.ExpressionIndex:
					result = DebugEffectExpressionIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.Expression:
					result = DebugEffectExpressionAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.InlineShader:
					result = DebugEffectInlineShaderAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.InlineShader5:
					result = DebugEffectInlineShader5Assignment.Parse(reader, typeSpecificReader);
					break;
				default:
					throw new ParseException($"Unsupported EffectCompilerAssignmentType {assignmentType}");
			}
			result.MemberType = memberType;
			result.MemberIndex = memberIndex;
			result.AssignmentType = assignmentType;
			result.ValueOffset = valueOffset;
			return result;
		}
	}
}
