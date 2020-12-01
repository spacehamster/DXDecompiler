namespace DXDecompiler.DX9Shader
{
	/// <summary>
	/// Comment Token
	/// https://docs.microsoft.com/en-us/windows-hardware/drivers/display/comment-token
	/// 
	/// A comment token describes the length of the comment that follows and is composed of the following bits:
	/// 
	/// Bits
	/// [15:00] Bits 0 through 15 indicate that the token is a comment token.This value is 0xFFFE.
	/// 
	/// [30:16] Bits 16 through 30 specify the length in DWORDs of the comment that follows. A comment can be up to 2^15 DWORDs in length, which equals 128 KB of video memory or system memory.
	/// 
	/// [31] Bit 31 is zero (0x0).
	/// </summary>
	public class CommentToken : Token
	{
		public CommentToken(Opcode opcode, int length, ShaderModel shaderModel) : base(opcode, length, shaderModel)
		{

		}
	}
}
