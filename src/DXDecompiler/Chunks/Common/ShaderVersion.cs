using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Common
{
	/// <summary>
	/// Version Token (VerTok)
	/// [07:00] minor version number (0-255)
	/// [15:08] major version number (0-255)
	/// [31:16] D3D10_SB_TOKENIZED_PROGRAM_TYPE
	/// </summary>
	public class ShaderVersion
	{
		public byte MajorVersion { get; private set; }
		public byte MinorVersion { get; private set; }
		public ProgramType ProgramType { get; private set; }

		public static ShaderVersion ParseShex(BytecodeReader reader)
		{
			uint versionToken = reader.ReadUInt32();
			return FromShexToken(versionToken);
		}

		public static ShaderVersion FromShexToken(uint versionToken)
		{
			return new ShaderVersion
			{
				MinorVersion = versionToken.DecodeValue<byte>(0, 3),
				MajorVersion = versionToken.DecodeValue<byte>(4, 7),
				ProgramType = versionToken.DecodeValue<ProgramType>(16, 31)
			};
		}

		public static ShaderVersion ParseAon9(BytecodeReader reader)
		{
			byte minor = reader.ReadByte();
			byte major = reader.ReadByte();
			ushort shaderType = reader.ReadUInt16();
			return new ShaderVersion
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
		public static ShaderVersion ParseRdef(BytecodeReader reader)
		{
			uint target = reader.ReadUInt32();
			var programTypeValue = target.DecodeValue<ushort>(16, 31);
			ProgramType programType = ParseProgramType(programTypeValue);
			return new ShaderVersion
			{
				MajorVersion = target.DecodeValue<byte>(8, 15),
				MinorVersion = target.DecodeValue<byte>(0, 7),
				ProgramType = programType
			};
		}
		public static ShaderVersion ParseFX(BytecodeReader reader)
		{
			uint target = reader.ReadUInt16();
			var programTypeValue = reader.ReadUInt16();
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
			return new ShaderVersion
			{
				MajorVersion = majorVersion,
				MinorVersion = minorVersion,
				ProgramType = programType
			};
		}
		public bool IsSM51 => MajorVersion == 5 && MinorVersion == 1;

		public override string ToString()
		{
			return $"{ProgramType.GetDescription()}_{MajorVersion}_{MinorVersion}";
		}
	}
}