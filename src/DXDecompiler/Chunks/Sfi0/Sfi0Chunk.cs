using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Util;

namespace DXDecompiler.Chunks.Sfi0
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
		public ShaderRequiresFlags Flags;
		private ShaderVersion _version;
		public static Sfi0Chunk Parse(BytecodeReader reader, ShaderVersion version, uint chunkSize)
		{
			var flags = (ShaderRequiresFlags)reader.ReadInt32();
			var result = new Sfi0Chunk();
			result.Flags = flags;
			Debug.Assert(chunkSize == 8,
				$"Unexpected Sfi0 Chunk Size");
			Debug.Assert((flags & (ShaderRequiresFlags.UavsAtEveryStage)) == 0,
				$"Unexpected SfiFlags {Convert.ToString((int)flags, 2)} {flags.ToString()}");
			Debug.Assert(!int.TryParse(flags.ToString(), out _),
				$"Unexpected SfiFlags {Convert.ToString((int)flags, 2)} {flags.ToString()}");
			var unknown0 = reader.ReadUInt32();
			Debug.Assert(unknown0 == 0, "Sfi0 unknown value");
			result._version = version;
			return result;
		}
		public static string RequireFlagsToString(ShaderRequiresFlags flags)
		{
			var sb = new StringBuilder();
			if (flags != ShaderRequiresFlags.None)
			{
				sb.AppendLine("// Note: shader requires additional functionality:");
				if (flags.HasFlag(ShaderRequiresFlags.Doubles))
				{
					sb.AppendLine("//       Double-precision floating point");
				}
				if (flags.HasFlag(ShaderRequiresFlags.EarlyDepthStencil))
				{
					sb.AppendLine("//       Early depth-stencil");
				}
				if (flags.HasFlag(ShaderRequiresFlags.Requires64Uavs))
				{
					sb.AppendLine("//       64 UAV slots");
				}
				if (flags.HasFlag(ShaderRequiresFlags.MinimumPrecision))
				{
					sb.AppendLine("//       Minimum-precision data types");
				}
				if (flags.HasFlag(ShaderRequiresFlags.DoubleExtensionsFor11Point1))
				{
					sb.AppendLine("//       Double-precision extensions for 11.1");
				}
				if (flags.HasFlag(ShaderRequiresFlags.ShaderExtensionsFor11Point1))
				{
					sb.AppendLine("//       Shader extensions for 11.1");
				}
				if (flags.HasFlag(ShaderRequiresFlags.Level9ComparisonFiltering))
				{
					sb.AppendLine("//       Comparison filtering for feature level 9");
				}
				if (flags.HasFlag(ShaderRequiresFlags.TiledResources))
				{
					sb.AppendLine("//       Tiled resources");
				}
				if (flags.HasFlag(ShaderRequiresFlags.StencilRef))
				{
					sb.AppendLine("//       PS Output Stencil Ref");
				}
				if (flags.HasFlag(ShaderRequiresFlags.InnerCoverage))
				{
					sb.AppendLine("//       PS Inner Coverage");
				}
				if (flags.HasFlag(ShaderRequiresFlags.TypedUAVLoadAdditionalFormats))
				{
					sb.AppendLine("//       Typed UAV Load Additional Formats");
				}
				if (flags.HasFlag(ShaderRequiresFlags.Rovs))
				{
					sb.AppendLine("//       Raster Ordered UAVs");
				}
				if (flags.HasFlag(ShaderRequiresFlags.SVArrayIndexFromFeedingRasterizer))
				{
					sb.AppendLine("//       SV_RenderTargetArrayIndex or SV_ViewportArrayIndex from any shader feeding rasterizer");
				}
				sb.AppendLine("//");
				sb.AppendLine("//");
			}
			return sb.ToString();
		}
		public static ShaderRequiresFlags GlobalFlagsToRequireFlags(GlobalFlags flags)
		{
			ShaderRequiresFlags result = ShaderRequiresFlags.None;
			if (flags.HasFlag(GlobalFlags.ForceEarlyDepthStencilTest))
			{
				result |= ShaderRequiresFlags.EarlyDepthStencil;
			}
			return result;
		}
		public static string GlobalFlagsToString(GlobalFlags flags)
		{
			var requireFlags = GlobalFlagsToRequireFlags(flags);
			return RequireFlagsToString(requireFlags);
		}
		public override string ToString()
		{
			if (_version.MajorVersion >= 5)
			{
				return RequireFlagsToString(Flags);
			}
			return string.Empty;
		}
	}
}