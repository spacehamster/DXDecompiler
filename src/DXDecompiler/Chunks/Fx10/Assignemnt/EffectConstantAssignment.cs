using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimShader.Chunks.Fx10
{
	public class EffectConstantAssignment : EffectAssignment
	{
		public List<EffectScalarType> Types { get; private set; }
		public List<Number> Values { get; private set; }
		public EffectConstantAssignment()
		{
			Types = new List<EffectScalarType>();
			Values = new List<Number>();
		}
		public new static EffectConstantAssignment Parse(BytecodeReader reader, BytecodeReader assignmentReader)
		{
			var result = new EffectConstantAssignment();
			var assignmentCount = assignmentReader.ReadUInt32();
			for (int i = 0; i < assignmentCount; i++)
			{
				result.Types.Add((EffectScalarType)assignmentReader.ReadUInt32());
				result.Values.Add(Number.Parse(assignmentReader));
			}
			return result;
		}
		public string FormatValue(Number value, EffectScalarType scalarType)
		{
			var type = MemberType.GetAssignmentType();
			if(type == null)
			{
				return value.UInt.ToString();
			}
			var numberType = scalarType.ToNumberType();
			if (type.IsEnum && scalarType == EffectScalarType.Int)
			{
				var enumValue = (Enum)Enum.ToObject(type, value.UInt);
				var description = EnumExtensions.GetDescription(enumValue);
				return string.Format("{0} /* {1} */", description, value.UInt);
			}
			else if(type.IsEnum)
			{
				return value.ToString(numberType);
			}
			if (type == typeof(float))
			{
				return value.ToString();
			}
			if (type == typeof(byte))
			{
				if (numberType == Shex.NumberType.UInt || numberType == Shex.NumberType.Int)
				{
					return string.Format("0x{0:x2}", value.Byte0);
				}
				else
				{
					return value.ToString(numberType);
				}
			}
			if (type == typeof(bool))
			{
				if (scalarType == EffectScalarType.Bool)
				{
					return string.Format("{0} /* {1} */",
						value.ToString(Shex.NumberType.Bool).ToUpper(), value.ToString(Shex.NumberType.Bool));
				} else if (scalarType == EffectScalarType.Int)
				{
					return string.Format("{0} /* {1} */",
						value.ToString(Shex.NumberType.Bool).ToUpper(), value.UInt);
				}
				else
				{
					return value.ToString(numberType);
				}
			}
			if (type == typeof(uint))
			{
				if (scalarType == EffectScalarType.UInt)
				{
					if (value.UInt > 10000)
						return "0x" + value.UInt.ToString("x8");
					return value.UInt.ToString();
				}
				else
				{
					return value.ToString(numberType);
				}
			}
			return value.ToString(numberType);
		}
		public string FormatValue()
		{
			var type = MemberType.GetAssignmentType();
			var formatedValues = Values.Zip(Types, (x, y) => FormatValue(x, y));
			string value = string.Join(", ", formatedValues);
			string typeName = "unknown";
			if (type == null)
			{
				return "NULL";
			}
			else if (type.IsEnum)
			{
				typeName = "uint";
			}
			else if (type == typeof(float))
			{
				typeName = "float";
			}
			else if (type == typeof(byte))
			{
				typeName = "byte";
			}
			else if (type == typeof(bool))
			{
				typeName = "bool";
			}
			else if (type == typeof(uint))
			{
				typeName = "uint";
			}
			else if (type == typeof(int))
			{
				typeName = "uint";
			}
			if (Values.Count > 1)
			{
				typeName += Values.Count;
			}
			return string.Format("{0}({1})", typeName, value);
		}
		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append(string.Format("{0, -8}", MemberName));
			sb.Append(" = ");
			sb.Append(FormatValue());
			sb.Append(";");
			return sb.ToString();
		}
	}
}
