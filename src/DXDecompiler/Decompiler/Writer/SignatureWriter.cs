using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Common;
using DXDecompiler.Chunks.Xsgn;
using DXDecompiler.Decompiler.IR;
using System;

namespace DXDecompiler.Decompiler.Writer
{
	public class SignatureWriter : BaseWriter
	{
		public SignatureWriter(DecompileContext context) : base(context)
		{

		}

		public void WriteSignature(IrSignature signature)
		{
			WriteIndent();
			WriteLineFormat("struct {0} {{", signature.Name);
			IncreaseIndent();
			foreach(var param in signature.Chunk.Parameters)
			{
				WriteParameter(param);
			}
			DecreaseIndent();
			WriteLine("};");
		}
		void WriteParameter(SignatureParameterDescription param)
		{
			WriteIndent();
			var fieldType = GetFieldType(param);
			Write($"{fieldType} {param.GetName()} : {GetSemanticName(param)};");
			DebugSignatureParamater(param);
		}
		void DebugSignatureParamater(SignatureParameterDescription param)
		{
			Write(" // ");
			WriteLine(param.ToString());
		}
		string GetFieldType(SignatureParameterDescription param)
		{
			string fieldType = param.ComponentType.GetDescription();
			if(param.MinPrecision != MinPrecision.None)
			{
				fieldType = param.MinPrecision.GetTypeName();
			}
			int componentCount = 0;
			if(param.Mask.HasFlag(ComponentMask.X)) componentCount += 1;
			if(param.Mask.HasFlag(ComponentMask.Y)) componentCount += 1;
			if(param.Mask.HasFlag(ComponentMask.Z)) componentCount += 1;
			if(param.Mask.HasFlag(ComponentMask.W)) componentCount += 1;
			switch(componentCount)
			{
				case 1:
					return $"{fieldType}";
				case 2:
					return $"{fieldType}2";
				case 3:
					return $"{fieldType}3";
				case 4:
					return $"{fieldType}4";
				default:
					throw new Exception($"Invalid ComponentMask {param.Mask}");
			}
		}

		string GetSemanticName(SignatureParameterDescription param)
		{
			if(param.SemanticName == "TEXCOORD")
			{
				return $"{param.SemanticName}{param.SemanticIndex}";
			}
			return param.SemanticName;
		}
	}
}
