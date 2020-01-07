using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SlimShader.Chunks.Common;
using SlimShader.Util;

namespace SlimShader.Chunks.Sfi0
{
	/// <summary>
	/// TODO: No idea, but this is present in ParticleDrawVS.asm, which is unique for including
	/// the enableRawAndStructuredBuffers global flag. I think this is because it includes both a normal
	/// Texture2D, and a StructuredBuffer.
	/// 
	/// When shader includes enableDoublePrecisionFloatOps global flag, then value is 1.
	/// </summary>
	public class Sfi0Chunk : BytecodeChunk
	{
		public SfiFlags Flags;
		private ShaderVersion _version;
		public static Sfi0Chunk Parse(BytecodeReader reader, ShaderVersion version)
		{
			var flags = (SfiFlags)reader.ReadInt32();
			var result = new Sfi0Chunk();
			result.Flags = flags;
			if(int.TryParse(flags.ToString(), out int discard))
			{
				throw new Exception($"Invalid SfiFlags {Convert.ToString((int)flags, 2)} {flags.ToString()}");
			}
			result._version = version;
			return result;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			if (Flags != SfiFlags.None && _version.MajorVersion >= 5)
			{
				sb.AppendLine("// Note: shader requires additional functionality:");
				if (Flags.HasFlag(SfiFlags.RequiresDoublePrecisionFloatingPoint)){
					sb.AppendLine("//       Double-precision floating point");
				}
				if (Flags.HasFlag(SfiFlags.RequiresEarlyDepthStencil))
				{
					sb.AppendLine("//       Early depth-stencil");
				}
				if (Flags.HasFlag(SfiFlags.RequiresUAVSlots64)){
					sb.AppendLine("//       64 UAV slots");
				}
				if (Flags.HasFlag(SfiFlags.RequiresMinimumPrecisionDataTypes))
				{
					sb.AppendLine("//       Minimum-precision data types");
				}
				if (Flags.HasFlag(SfiFlags.RequiresDoublePrecisionExtensions)){
					sb.AppendLine("//       Double-precision extensions for 11.1");
				}
				if (Flags.HasFlag(SfiFlags.RequiresShaderExtensionsFor11_1))
				{
					sb.AppendLine("//       Shader extensions for 11.1");
				}
				if (Flags.HasFlag(SfiFlags.TypedUAVLoadAdditionalFormats))
				{
					sb.AppendLine("//       Typed UAV Load Additional Formats");
				}
				if (Flags.HasFlag(SfiFlags.SVArrayIndexFromFeedingRasterizer))
				{
					sb.AppendLine("//       SV_RenderTargetArrayIndex or SV_ViewportArrayIndex from any shader feeding rasterizer");
				}
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			return sb.ToString();
		}
	}
}