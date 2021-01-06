using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;
using System;
using System.Linq;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugOperand
	{
		internal readonly OpcodeType ParentType;
		public byte NumComponents;
		public Operand4ComponentSelectionMode SelectionMode;
		public ComponentMask ComponentMask;
		public Operand4ComponentName[] Swizzles;
		public OperandType OperandType;
		public OperandIndexDimension IndexDimension;
		public bool IsExtended;
		public OperandModifier Modifier;
		public DebugOperandIndex[] Indices;
		public DebugNumber4 ImmediateValues;
		public OperandMinPrecision MinPrecision;
		public bool NonUniform;

		public DebugOperand(OpcodeType parentType)
		{
			ParentType = parentType;
			Swizzles = new[]
			{
				Operand4ComponentName.X,
				Operand4ComponentName.Y,
				Operand4ComponentName.Z,
				Operand4ComponentName.W
			};
			Indices = new DebugOperandIndex[3];
		}
		public static DebugOperand Parse(DebugBytecodeReader reader, OpcodeType parentType)
		{
			uint token0 = reader.ReadUInt32("operandToken");
			if(token0 == 0)
				return null;
			var member = reader.LocalMembers.Last();
			var operand = new DebugOperand(parentType);

			var numComponents = token0.DecodeValue<OperandNumComponents>(0, 1);
			reader.AddNote("numComponents", numComponents);
			switch(numComponents)
			{
				case OperandNumComponents.Zero:
					{
						operand.NumComponents = 0;
						break;
					}
				case OperandNumComponents.One:
					{
						operand.NumComponents = 1;
						break;
					}
				case OperandNumComponents.Four:
					{
						operand.NumComponents = 4;
						operand.SelectionMode = token0.DecodeValue<Operand4ComponentSelectionMode>(2, 3);
						member.AddNote("SelectionMode", operand.SelectionMode);
						switch(operand.SelectionMode)
						{
							case Operand4ComponentSelectionMode.Mask:
								{
									operand.ComponentMask = token0.DecodeValue<ComponentMask>(4, 7);
									member.AddNote("ComponentMask", operand.ComponentMask);
									break;
								}
							case Operand4ComponentSelectionMode.Swizzle:
								{
									var swizzle = token0.DecodeValue(4, 11);
									Func<uint, byte, Operand4ComponentName> swizzleDecoder = (s, i) =>
										(Operand4ComponentName)((s >> (i * 2)) & 3);
									operand.Swizzles[0] = swizzleDecoder(swizzle, 0);
									operand.Swizzles[1] = swizzleDecoder(swizzle, 1);
									operand.Swizzles[2] = swizzleDecoder(swizzle, 2);
									operand.Swizzles[3] = swizzleDecoder(swizzle, 3);
									member.AddNote("Swizzles[0]", operand.Swizzles[0]);
									member.AddNote("Swizzles[1]", operand.Swizzles[1]);
									member.AddNote("Swizzles[2]", operand.Swizzles[2]);
									member.AddNote("Swizzles[3]", operand.Swizzles[3]);
									break;
								}
							case Operand4ComponentSelectionMode.Select1:
								{
									var swizzle = token0.DecodeValue<Operand4ComponentName>(4, 5);
									operand.Swizzles[0] = operand.Swizzles[1] = operand.Swizzles[2] = operand.Swizzles[3] = swizzle;
									member.AddNote("Swizzles[0]", operand.Swizzles[0]);
									member.AddNote("Swizzles[1]", operand.Swizzles[1]);
									member.AddNote("Swizzles[2]", operand.Swizzles[2]);
									member.AddNote("Swizzles[3]", operand.Swizzles[3]);
									break;
								}
							default:
								{
									throw new ParseException("Unrecognized selection method: " + operand.SelectionMode);
								}
						}
						break;
					}
				case OperandNumComponents.N:
					{
						throw new ParseException("OperandNumComponents.N is not currently supported.");
					}
			}

			operand.OperandType = token0.DecodeValue<OperandType>(12, 19);
			member.AddNote("OperandType", operand.OperandType);
			operand.IndexDimension = token0.DecodeValue<OperandIndexDimension>(20, 21);
			member.AddNote("IndexDimension", operand.IndexDimension);

			operand.IsExtended = token0.DecodeValue(31, 31) == 1;
			member.AddNote("IsExtended", operand.IsExtended);
			if(operand.IsExtended)
				ReadExtendedOperand(operand, reader);

			Func<uint, byte, OperandIndexRepresentation> indexRepresentationDecoder = (t, i) =>
				(OperandIndexRepresentation)t.DecodeValue((byte)(22 + (i * 3)), (byte)(22 + (i * 3) + 2));

			for(byte i = 0; i < (byte)operand.IndexDimension; i++)
			{
				operand.Indices[i] = new DebugOperandIndex();

				var indexRepresentation = indexRepresentationDecoder(token0, i);
				operand.Indices[i].Representation = indexRepresentation;
				member.AddNote($"Indices[{i}].Representation", operand.Indices[i].Representation);
				switch(indexRepresentation)
				{
					case OperandIndexRepresentation.Immediate32:
						operand.Indices[i].Value = reader.ReadUInt32($"Indices[{i}].Value");
						break;
					case OperandIndexRepresentation.Immediate64:
						operand.Indices[i].Value = reader.ReadUInt64($"Indices[{i}].Value");
						goto default;
					case OperandIndexRepresentation.Relative:
						operand.Indices[i].Register = Parse(reader, parentType);
						break;
					case OperandIndexRepresentation.Immediate32PlusRelative:
						operand.Indices[i].Value = reader.ReadUInt32($"Indices[{i}].Value");
						goto case OperandIndexRepresentation.Relative;
					case OperandIndexRepresentation.Immediate64PlusRelative:
						operand.Indices[i].Value = reader.ReadUInt64($"Indices[{i}].Value");
						goto case OperandIndexRepresentation.Relative;
					default:
						throw new ParseException("Unrecognised index representation: " + indexRepresentation);
				}
			}

			var numberType = parentType.GetNumberType();
			switch(operand.OperandType)
			{
				case OperandType.Immediate32:
					{
						var immediateValues = new DebugNumber4();
						for(var i = 0; i < operand.NumComponents; i++)
							immediateValues.SetNumber(i, DebugNumber.Parse(reader));
						operand.ImmediateValues = immediateValues;
						break;
					}
				case OperandType.Immediate64:
					{
						var immediateValues = new DebugNumber4();
						for(var i = 0; i < operand.NumComponents; i++)
							immediateValues.SetDouble(i, reader.ReadDouble($"ImmediateValues[{i}]"));
						operand.ImmediateValues = immediateValues;
						break;
					}
			}
			return operand;
		}

		private static void ReadExtendedOperand(DebugOperand operand, DebugBytecodeReader reader)
		{
			uint token1 = reader.ReadUInt32("token1");
			var type = token1.DecodeValue<ExtendedOperandType>(0, 5);
			reader.AddNote("type", type);
			switch(type)
			{
				case ExtendedOperandType.Modifier:
					operand.Modifier = token1.DecodeValue<OperandModifier>(6, 13);
					operand.MinPrecision = token1.DecodeValue<OperandMinPrecision>(14, 16);
					operand.NonUniform = token1.DecodeValue<bool>(17, 17);
					reader.AddNote("Modifier", operand.Modifier);
					reader.AddNote("MinPrecision", operand.MinPrecision);
					reader.AddNote("NonUniform", operand.NonUniform);
					break;
				default:
					throw new Exception($"Unknown Extended modifier {type}");
			}
		}
	}
}
