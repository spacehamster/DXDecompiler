using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;
using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Shex.Tokens
{
	public class DebugInstructionToken : DebugOpcodeToken
	{
		public ResInfoReturnType ResInfoReturnType;
		public bool Saturate;
		public InstructionTestBoolean TestBoolean;
		public ComponentMask PreciseValueMask;
		public SyncFlags SyncFlags;
		public List<InstructionTokenExtendedType> ExtendedTypes;
		public sbyte[] SampleOffsets;
		public ResourceDimension ResourceTarget;
		public ushort ResourceStride;
		public ResourceReturnType[] ResourceReturnTypes;
		public int LinkedInstructionOffset;
		public uint FunctionIndex;
		public List<DebugOperand> Operands;

		public DebugInstructionToken()
		{
			ExtendedTypes = new List<InstructionTokenExtendedType>();
			SampleOffsets = new sbyte[3];
			ResourceReturnTypes = new ResourceReturnType[4];
			Operands = new List<DebugOperand>();
		}

		public static DebugInstructionToken Parse(DebugBytecodeReader reader, DebugOpcodeHeader header)
		{
			var instructionToken = new DebugInstructionToken();

			// Advance to next token.
			var instructionEnd = reader.CurrentPosition + (header.Length * sizeof(uint));
			var token0 = reader.ReadUInt32("token0");
			DebugOpcodeHeader.AddNotes(reader, token0);

			if (header.OpcodeType == OpcodeType.Sync)
			{
				instructionToken.SyncFlags = token0.DecodeValue<SyncFlags>(11, 14);
				reader.AddNote("SyncFlags", instructionToken.SyncFlags);
			}
			else
			{
				instructionToken.ResInfoReturnType = token0.DecodeValue<ResInfoReturnType>(11, 12);
				instructionToken.Saturate = (token0.DecodeValue(13, 13) == 1);
				instructionToken.TestBoolean = token0.DecodeValue<InstructionTestBoolean>(18, 18);
				instructionToken.PreciseValueMask = token0.DecodeValue<ComponentMask>(19, 22);
				reader.AddNote("ResInfoReturnType", instructionToken.ResInfoReturnType);
				reader.AddNote("Saturate", instructionToken.Saturate);
				reader.AddNote("TestBoolean", instructionToken.TestBoolean);
				reader.AddNote("PreciseValueMask", instructionToken.PreciseValueMask);
			}

			bool extended = header.IsExtended;
			while (extended)
			{
				uint extendedToken = reader.ReadUInt32("extendedToken");
				var extendedType = extendedToken.DecodeValue<InstructionTokenExtendedType>(0, 5);
				reader.AddNote("extendedType", extendedType);
				instructionToken.ExtendedTypes.Add(extendedType);
				extended = (extendedToken.DecodeValue(31, 31) == 1);
				reader.AddNote("extended", extended);

				switch (extendedType)
				{
					case InstructionTokenExtendedType.SampleControls:
						instructionToken.SampleOffsets[0] = extendedToken.DecodeSigned4BitValue(09, 12);
						instructionToken.SampleOffsets[1] = extendedToken.DecodeSigned4BitValue(13, 16);
						instructionToken.SampleOffsets[2] = extendedToken.DecodeSigned4BitValue(17, 20);
						reader.AddNote("SampleOffsets[0]", instructionToken.SampleOffsets[0]);
						reader.AddNote("SampleOffsets[1]", instructionToken.SampleOffsets[1]);
						reader.AddNote("SampleOffsets[2]", instructionToken.SampleOffsets[2]);
						break;
					case InstructionTokenExtendedType.ResourceDim:
						instructionToken.ResourceTarget = extendedToken.DecodeValue<ResourceDimension>(6, 10);
						instructionToken.ResourceStride = extendedToken.DecodeValue<ushort>(11, 22);
						reader.AddNote("ResourceTarget", instructionToken.ResourceTarget);
						reader.AddNote("ResourceStride", instructionToken.ResourceStride);
						break;
					case InstructionTokenExtendedType.ResourceReturnType:
						instructionToken.ResourceReturnTypes[0] = extendedToken.DecodeValue<ResourceReturnType>(06, 09);
						instructionToken.ResourceReturnTypes[1] = extendedToken.DecodeValue<ResourceReturnType>(10, 13);
						instructionToken.ResourceReturnTypes[2] = extendedToken.DecodeValue<ResourceReturnType>(14, 17);
						instructionToken.ResourceReturnTypes[3] = extendedToken.DecodeValue<ResourceReturnType>(18, 21);
						reader.AddNote("ResourceReturnTypes[0]", instructionToken.ResourceReturnTypes[0]);
						reader.AddNote("ResourceReturnTypes[1]", instructionToken.ResourceReturnTypes[1]);
						reader.AddNote("ResourceReturnTypes[2]", instructionToken.ResourceReturnTypes[2]);
						reader.AddNote("ResourceReturnTypes[3]", instructionToken.ResourceReturnTypes[3]);
						break;
					default:
						throw new ParseException("Unrecognised extended type: " + extendedType);
				}
			}

			if (header.OpcodeType == OpcodeType.InterfaceCall)
			{
				instructionToken.FunctionIndex = reader.ReadUInt32("FunctionIndex");
			}

			while (reader.CurrentPosition < instructionEnd)
			{
				reader.AddIndent($"Operand{instructionToken.Operands.Count}");
				var operand = DebugOperand.Parse(reader, header.OpcodeType);
				if (operand != null)
					instructionToken.Operands.Add(operand);
				reader.RemoveIndent();
			}
			return instructionToken;
		}
	}
}
