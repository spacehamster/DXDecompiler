using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXDecompiler.DebugParser.Icfe
{
	public class DebugInterfacesChunk : DebugBytecodeChunk
	{
		public uint InterfaceSlotCount { get; private set; }
		public List<DebugClassType> AvailableClassTypes { get; private set; }
		public List<DebugClassInstance> AvailableClassInstances { get; private set; }
		public List<DebugInterfaceSlot> InterfaceSlots { get; private set; }

		public DebugInterfacesChunk()
		{
			AvailableClassTypes = new List<DebugClassType>();
			AvailableClassInstances = new List<DebugClassInstance>();
			InterfaceSlots = new List<DebugInterfaceSlot>();
		}

		public static DebugInterfacesChunk Parse(DebugBytecodeReader reader, uint sizeInBytes)
		{
			var headerReader = reader.CopyAtCurrentPosition("InterfaceHeaderReader", reader);

			var result = new DebugInterfacesChunk();

			var classInstanceCount = headerReader.ReadUInt32("classInstanceCount");
			var classTypeCount = headerReader.ReadUInt32("classTypeCount");
			var interfaceSlotRecordCount = headerReader.ReadUInt32("interfaceSlotRecordCount");

			// Will be same as interfaceSlotRecordCount unless there are interface arrays.
			result.InterfaceSlotCount = headerReader.ReadUInt32("InterfaceSlotCount");

			headerReader.ReadUInt32("Think this is offset to start of interface slot info, but we don't need it."); // Think this is offset to start of interface slot info, but we don't need it.

			var classTypeOffset = headerReader.ReadUInt32("classTypeOffset");
			var availableClassReader = reader.CopyAtOffset("availableClassReader", headerReader, (int)classTypeOffset);

			var interfaceSlotOffset = headerReader.ReadUInt32("interfaceSlotOffset");
			var interfaceSlotReader = reader.CopyAtOffset("interfaceSlotReader", headerReader, (int)interfaceSlotOffset);

			var unknown1 = headerReader.ReadBytes("InterfaceChunkUnknown1", 4);
			var unknown2 = headerReader.ReadUInt16("InterfaceChunkUnknown2");
			var unknown3 = headerReader.ReadUInt16("InterfaceChunkUnknown3");

			for (uint i = 0; i < classTypeCount; i++)
			{
				var classType = DebugClassType.Parse(reader, availableClassReader);
				classType.ID = i; // Really??
				result.AvailableClassTypes.Add(classType);
			}

			for (uint i = 0; i < classInstanceCount; i++)
			{
				var classInstance = DebugClassInstance.Parse(reader, availableClassReader);
				result.AvailableClassInstances.Add(classInstance);
			}

			uint startSlot = 0;
			for (uint i = 0; i < interfaceSlotRecordCount; i++)
			{
				var interfaceSlot = DebugInterfaceSlot.Parse(reader, interfaceSlotReader);
				interfaceSlot.StartSlot = startSlot; // Really??
				result.InterfaceSlots.Add(interfaceSlot);

				startSlot += interfaceSlot.SlotSpan;
			}

			return result;
		}
	}
}
