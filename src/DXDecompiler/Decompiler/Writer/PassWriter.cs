using DXDecompiler.Chunks.Shex.Tokens;
using DXDecompiler.Decompiler.IR;

namespace DXDecompiler.Decompiler.Writer
{
	public class PassWriter : BaseWriter
	{
		public PassWriter(DecompileContext context) : base(context)
		{

		}

		public void WritePass(IrPass pass)
		{
			foreach(var attribute in pass.Attributes)
			{
				WriteAttribute(attribute);
			}
			WriteIndent();
			WriteLineFormat("void {0}()", pass.Name);
			WriteIndent();
			WriteLine("{");
			IncreaseIndent();
			foreach(var declaration in pass.Declarations)
			{
				WriteDeclrations(declaration);
			}
			foreach(var instruction in pass.Instructions)
			{
				Context.InstructionWriter.WriteInstruction(instruction);
			}
			DecreaseIndent();
			WriteLine("}");
		}

		private void WriteAttribute(IrAttribute attribute)
		{
			WriteIndent();
			WriteFormat("[{0}(", attribute.Name);
			Join(", ", attribute.Arguments, (arg) => Write(arg.ToString()));
			WriteLine(")]");
		}
		private void WriteDeclrations(DeclarationToken decl)
		{
			if(decl is TempRegisterDeclarationToken temps)
			{
				WriteIndent();
				Write("float4 ");
				for(int i = 0; i < temps.TempCount; i++)
				{
					WriteFormat("r{0}", i);
					if(i < temps.TempCount - 1) Write(", ");
				}
				Write(";");
				WriteLineFormat(" // {0}", decl.ToString());
			}
			else
			{
				WriteIndent();
				WriteLineFormat("// Not implemented [{0}]: {1}", decl.Header.OpcodeType, decl.ToString());
			}
		}
	}
}
