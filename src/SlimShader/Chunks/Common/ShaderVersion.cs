using SlimShader.Util;

namespace SlimShader.Chunks.Common
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

		public static ShaderVersion ParseRdef(BytecodeReader reader)
		{
			uint target = reader.ReadUInt32();

			var programTypeValue = target.DecodeValue<ushort>(16, 31);
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
				case 0x4c46:
					programType = ProgramType.LibraryShader;
					break;
				default:
					throw new ParseException(string.Format("Unknown program type: 0x{0:X}", programTypeValue));
			}

			return new ShaderVersion
			{
				MajorVersion = target.DecodeValue<byte>(8, 15),
				MinorVersion = target.DecodeValue<byte>(0, 7),
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