using DXDecompiler.Chunks.Common;
using DXDecompiler.Util;

namespace DXDecompiler.DebugParser
{
	public class DebugShaderVersion
	{
		public byte MajorVersion { get; private set; }
		public byte MinorVersion { get; private set; }
		public ProgramType ProgramType { get; private set; }
		public bool IsSM51 => MajorVersion == 5 && MinorVersion == 1;

		internal static DebugShaderVersion ParseRdef(DebugBytecodeReader reader)
		{
			byte minorVersion = reader.ReadByte("minorVersion");
			byte majorVersion = reader.ReadByte("majorVersion");
			ushort programTypeValue = reader.ReadUInt16("programTypeValue");
			ProgramType programType;
			switch (programTypeValue)
			{
				case 0xFFFF:
					programType = ProgramType.PixelShader;
					break;
				case 0xFFFE:
					programType = ProgramType.VertexShader;
					break;
				case 0x4853:
					programType = ProgramType.HullShader;
					break;
				case 0x4753:
					programType = ProgramType.GeometryShader;
					break;
				case 0x4453:
					programType = ProgramType.DomainShader;
					break;
				case 0x4353:
					programType = ProgramType.ComputeShader;
					break;
				case 0x4C46:
					programType = ProgramType.LibraryShader;
					break;
				default:
					throw new ParseException(string.Format("Unknown program type: 0x{0:X}", programTypeValue));
			}
			return new DebugShaderVersion
			{
				MajorVersion = majorVersion,
				MinorVersion = minorVersion,
				ProgramType = programType
			};
		}

		public static DebugShaderVersion FromShexToken(uint versionToken)
		{
			return new DebugShaderVersion
			{
				MinorVersion = versionToken.DecodeValue<byte>(0, 3),
				MajorVersion = versionToken.DecodeValue<byte>(4, 7),
				ProgramType = versionToken.DecodeValue<ProgramType>(16, 31)
			};
		}
		public static DebugShaderVersion ParseShex(DebugBytecodeReader reader)
		{
			uint versionToken = reader.ReadUInt32("Version");
			return FromShexToken(versionToken);
		}

		public static DebugShaderVersion ParseAon9(DebugBytecodeReader reader)
		{
			byte minor = reader.ReadByte("minorVersion");
			byte major = reader.ReadByte("majorVersion");
			ushort shaderType = reader.ReadUInt16("programType");
			return new DebugShaderVersion
			{
				MinorVersion = minor,
				MajorVersion = major,
				ProgramType = shaderType == 0xFFFF ? ProgramType.PixelShader : ProgramType.VertexShader
			};
		}

		private static ProgramType ParseProgramType(ushort programTypeValue)
		{
			switch (programTypeValue)
			{
				case 0xFFFF:
					return ProgramType.PixelShader;
				case 0xFFFE:
					return ProgramType.VertexShader;
				case 0x4853:
					return ProgramType.HullShader;
				case 0x4753:
					return ProgramType.GeometryShader;
				case 0x4453:
					return ProgramType.DomainShader;
				case 0x4353:
					return ProgramType.ComputeShader;
				case 0x4C46:
					return ProgramType.LibraryShader;
				case 0xFEFF:
					return ProgramType.EffectsShader;
				default:
					throw new ParseException(string.Format("Unknown program type: 0x{0:X}", programTypeValue));
			}
		}
		public static DebugShaderVersion ParseFX(DebugBytecodeReader reader)
		{
			uint target = reader.ReadUInt16("Target");
			var programTypeValue = reader.ReadUInt16("ProgramType");
			ProgramType programType = ParseProgramType(programTypeValue);
			byte majorVersion;
			byte minorVersion;
			switch (target)
			{
				case 0x1001:
					majorVersion = 4;
					minorVersion = 0;
					break;
				case 0x1011:
					majorVersion = 4;
					minorVersion = 1;
					break;
				case 0x2001:
					majorVersion = 5;
					minorVersion = 0;
					break;
				case 0x0901:
					majorVersion = 2;
					minorVersion = 0;
					break;
				default:
					throw new ParseException(string.Format("Unknown program version: 0x{0:X}", target));
			}
			return new DebugShaderVersion
			{
				MajorVersion = majorVersion,
				MinorVersion = minorVersion,
				ProgramType = programType
			};
		}
	}
}
