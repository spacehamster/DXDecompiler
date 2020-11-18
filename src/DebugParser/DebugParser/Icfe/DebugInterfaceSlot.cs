using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Icfe
{
	public class DebugInterfaceSlot
	{
		public uint StartSlot { get; set; }
		public uint SlotSpan { get; private set; }
		public List<uint> TypeIDs { get; private set; }
		public List<uint> TableIDs { get; private set; }

		public DebugInterfaceSlot()
		{
			TypeIDs = new List<uint>();
			TableIDs = new List<uint>();
		}

		public static DebugInterfaceSlot Parse(DebugBytecodeReader reader, DebugBytecodeReader interfaceSlotReader)
		{
			var slotSpan = interfaceSlotReader.ReadUInt32("slotSpan");

			var count = interfaceSlotReader.ReadUInt32("count");

			var typeIDsOffset = interfaceSlotReader.ReadUInt32("typeIDsOffset");
			var typeIDsReader = reader.CopyAtOffset("typeIDsReader", interfaceSlotReader, (int)typeIDsOffset);

			var tableIDsOffset = interfaceSlotReader.ReadUInt32("tableIDsOffset");
			var tableIDsReader = reader.CopyAtOffset("tableIDsReader", interfaceSlotReader, (int)tableIDsOffset);

			var result = new DebugInterfaceSlot
			{
				SlotSpan = slotSpan
			};

			for (int i = 0; i < count; i++)
			{
				result.TypeIDs.Add(typeIDsReader.ReadUInt16($"TypeIDs[{i}]"));
				result.TableIDs.Add(tableIDsReader.ReadUInt32($"TableIDs[{i}]"));
			}

			return result;
		}
	}
}