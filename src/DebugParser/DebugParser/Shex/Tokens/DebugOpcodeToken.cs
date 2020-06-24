using SlimShader.Chunks;

namespace SlimShader.DebugParser.Shex.Tokens
{
	public abstract class DebugOpcodeToken
	{
		public DebugOpcodeHeader Header { get; internal set; }

		protected string TypeDescription
		{
			get { return Header.OpcodeType.GetDescription(); }
		}

		protected DebugOpcodeToken()
		{
			Header = new DebugOpcodeHeader();
		}

		public override string ToString()
		{
			return TypeDescription;
		}
	}
}