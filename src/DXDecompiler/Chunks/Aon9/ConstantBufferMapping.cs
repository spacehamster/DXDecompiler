using SlimShader.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SlimShader.Chunks.Aon9
{
	public class ConstantBufferMapping
	{
		public ushort TargetReg { get; private set; }
		public ushort Buffer { get; private set; }
		public ushort StartReg { get; private set; }
		public ushort RegCount { get; private set; }
		public DataConversionType[] DataConversion { get; private set; }

		ConstantBufferMapping()
		{
			DataConversion = new DataConversionType[4];
		}

		public static ConstantBufferMapping Parse(BytecodeReader reader)
		{
			var result = new ConstantBufferMapping();
			result.Buffer = reader.ReadUInt16();
			result.StartReg = reader.ReadUInt16();
			result.RegCount = reader.ReadUInt16();
			result.TargetReg = reader.ReadUInt16();
			for (int i = 0; i < 4; i++)
			{
				var type = (DataConversionType)reader.ReadByte();
				result.DataConversion[i] = type;
				Debug.Assert(Enum.IsDefined(typeof(DataConversionType), type),
					$"Invalid DataConversionType {type}");
			}
			return result;
		}
		public override string ToString()
		{
			return string.Format("// c{0,-9} cb{1,-5} {2,9} {3,9}  ({4,4},{5,4},{6,4},{7,4})",
					TargetReg, Buffer, StartReg, RegCount, 
					DataConversion[0].GetDescription(),
					DataConversion[1].GetDescription(),
					DataConversion[2].GetDescription(),
					DataConversion[3].GetDescription());
		}
	}
}
