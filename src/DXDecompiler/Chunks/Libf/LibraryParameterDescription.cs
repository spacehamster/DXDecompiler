using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Util;
using System.Text;

namespace DXDecompiler.Chunks.Libf
{
	/// <summary>
	/// Describes a library function's input/output parameter
	/// Based on D3D12_PARAMETER_DESC
	/// </summary>
	public class LibraryParameterDescription
	{
		/// <summary>
		/// Parameter name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Parameter semantic name (+index).
		/// </summary>
		public string SemanticName { get; private set; }

		/// <summary>
		/// Element type.
		/// </summary>
		public ShaderVariableType VariableType { get; private set; }

		/// <summary>
		/// Scalar/Vector/Matrix.
		/// </summary>
		public ShaderVariableClass VariableClass { get; private set; }

		/// <summary>
		/// Rows are for matrix parameters.
		/// </summary>
		public uint Rows { get; private set; }

		/// <summary>
		/// Components or Columns in matrix.
		/// </summary>
		public uint Column { get; private set; }

		/// <summary>
		/// Interpolation mode.
		/// </summary>
		public InterpolationMode InterpolationMode { get; private set; }

		/// <summary>
		/// Parameter modifiers.
		/// </summary>
		public ParameterFlags Flags { get; private set; }

		/// <summary>
		/// The first input register for this parameter.
		/// </summary>
		public uint FirstInRegister { get; private set; }

		/// <summary>
		/// The first input register component for this parameter.
		/// </summary>
		public uint FirstInComponent { get; private set; }

		/// <summary>
		/// The first output register for this parameter.
		/// </summary>
		public uint FirstOutRegister { get; private set; }

		/// <summary>
		/// The first output register component for this parameter.
		/// </summary>
		public uint FirstOutComponent { get; private set; }

		public string InputDescription
		{
			get
			{
				if(FirstInRegister != uint.MaxValue)
				{
					return $"v{FirstInRegister}";
				}
				else
				{
					return "";
				}
			}
		}
		public string OutputDescription
		{
			get
			{
				if(FirstOutRegister != uint.MaxValue)
				{
					return $"o{FirstOutRegister}";
				}
				else
				{
					return "";
				}
			}
		}
		public string TypeName
		{
			get
			{
				if(VariableType == ShaderVariableType.Void)
				{
					return "void";
				}
				var sb = new StringBuilder();
				if(Flags.HasFlag(ParameterFlags.In))
				{
					sb.Append("in");
				}
				if(Flags.HasFlag(ParameterFlags.Out))
				{
					sb.Append("out");
				}
				if(Flags.HasFlag(ParameterFlags.In) || Flags.HasFlag(ParameterFlags.Out))
				{
					sb.Append(" ");
				}
				if(VariableClass == ShaderVariableClass.MatrixRows)
				{
					sb.Append("row_major ");
				}
				sb.Append(VariableType.GetDescription());
				switch(VariableClass)
				{
					case ShaderVariableClass.MatrixRows:
						sb.AppendFormat("{0}x{1}", Rows, Column);
						break;
					case ShaderVariableClass.MatrixColumns:
						sb.AppendFormat("{0}x{1}", Column, Rows);
						break;
					case ShaderVariableClass.Vector:
						sb.Append(Column);
						break;
				}
				return sb.ToString();
			}
		}
		public static LibraryParameterDescription Parse(BytecodeReader reader, BytecodeReader paramReader)
		{
			var nameOffset = paramReader.ReadUInt32();
			var semanticNameOffset = paramReader.ReadUInt32();
			var result = new LibraryParameterDescription()
			{
				VariableType = (ShaderVariableType)paramReader.ReadUInt32(),
				VariableClass = (ShaderVariableClass)paramReader.ReadUInt32(),
				Rows = paramReader.ReadUInt32(),
				Column = paramReader.ReadUInt32(),
				InterpolationMode = (InterpolationMode)paramReader.ReadUInt32(),
				Flags = (ParameterFlags)paramReader.ReadUInt32(),
				FirstInRegister = paramReader.ReadUInt32(),
				FirstInComponent = paramReader.ReadUInt32(),
				FirstOutRegister = paramReader.ReadUInt32(),
				FirstOutComponent = paramReader.ReadUInt32(),
			};
			var nameReader = reader.CopyAtOffset((int)nameOffset);
			result.Name = nameReader.ReadString();

			if(semanticNameOffset != 0)
			{
				var semanticNameReader = reader.CopyAtOffset((int)semanticNameOffset);
				result.SemanticName = semanticNameReader.ReadString();
			}
			else
			{
				result.SemanticName = "";
			}
			return result;
		}
		string GetMask()
		{
			switch(Column)
			{
				case 1:
					return "x";
				case 2:
					return "xy";
				case 3:
					return "xyz";
				case 4:
					return "xyzw";
			}
			return "";
		}
		public override string ToString()
		{
			// For example:
			// Name                 SemanticName         In 1st,Num,Mask Out 1st,Num,Mask Type                           
			// -------------------- -------------------- --------------- ---------------- ------------------------------ 
			// TestFunction3                                               o0    4   xyzw out float4x4
			// input                                      v0    1   x                     in uint
			string inDesc = "";
			string inNum = "";
			string inMask = "";
			string outDesc = "";
			string outNum = "";
			string outMask = "";
			if(FirstInRegister != uint.MaxValue)
			{
				inDesc = InputDescription;
				inNum = Rows.ToString();
				inMask = GetMask();
			}
			if(FirstOutRegister != uint.MaxValue)
			{
				outDesc = OutputDescription;
				outNum = Rows.ToString();
				outMask = GetMask();
			}
			var result = string.Format("// {0,-20} {1,-19}   {2,-3}{3,4}   {4,-4} {5,4} {6,4}   {7,-4} {8}",
				Name, SemanticName, inDesc, inNum, inMask, outDesc, outNum, outMask, TypeName);
			return result;
		}
	}
}
