using DXDecompiler.Chunks.Fx10.Assignemnt;
using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.Fx10
{
	public class EffectAssignment
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
		public static EffectAssignment Parse(BytecodeReader reader, BytecodeReader annotationReader)
		{
			//MemberType type, such as AddressV or Filter
			var memberType = (EffectAssignmentType)annotationReader.ReadUInt32();
			//	$"EffectStateAnnotationType is {memberType}");
			//MemberIndex is 1 for BlendEnable[1] = TRUE;
			var memberIndex = annotationReader.ReadUInt32();
			var assignmentType = (EffectCompilerAssignmentType)annotationReader.ReadUInt32();
			var valueOffset = annotationReader.ReadUInt32();
			var typeSpecificReader = reader.CopyAtOffset((int)valueOffset);
			EffectAssignment result;
			switch (assignmentType)
			{
				case EffectCompilerAssignmentType.Constant:
					result = EffectConstantAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.Variable:
					result = EffectVariableAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.ConstantIndex:
					result = EffectConstantIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.VariableIndex:
					result = EffectVariableIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.ExpressionIndex:
					result = EffectExpressionIndexAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.Expression:
					result = EffectExpressionAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.InlineShader:
					result = EffectInlineShaderAssignment.Parse(reader, typeSpecificReader);
					break;
				case EffectCompilerAssignmentType.InlineShader5:
					result = EffectInlineShader5Assignment.Parse(reader, typeSpecificReader);
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
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(MemberType.ToString());
			sb.Append(" = TODO;");
			return sb.ToString();
		}
	}
}
