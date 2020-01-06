using System;
using SlimShader.Chunks.Common;
using SlimShader.Chunks.Shex;

namespace SlimShader.Chunks.Xsgn
{
	public static class EnumExtensions
	{
		public static string GetDescription(this ComponentMask value)
		{
			string result = string.Empty;

			result += (value.HasFlag(ComponentMask.X)) ? "x" : " ";
			result += (value.HasFlag(ComponentMask.Y)) ? "y" : " ";
			result += (value.HasFlag(ComponentMask.Z)) ? "z" : " ";
			result += (value.HasFlag(ComponentMask.W)) ? "w" : " ";

			return result;
		}

		public static bool RequiresMask(this Name value)
		{
			switch (value)
			{
				case Name.Coverage:
				case Name.Depth:
				case Name.DepthGreaterEqual :
				case Name.DepthLessEqual :
				default :
					return true;
			}
		}

		public static string GetRegisterName(this Name value)
		{
			switch (value)
			{
				case Name.DepthGreaterEqual:
					return OperandType.OutputDepthGreaterEqual.GetDescription();
				case Name.DepthLessEqual:
					return OperandType.OutputDepthLessEqual.GetDescription();
				case Name.Coverage:
					return OperandType.OutputCoverageMask.GetDescription();
				case Name.Depth:
					return OperandType.OutputDepth.GetDescription();
				case Name.PrimitiveID:
					return "primID";
				default:
					throw new ArgumentOutOfRangeException("value", "Unrecognised name: " + value);
			}
		}
	}
}