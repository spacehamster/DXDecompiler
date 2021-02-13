using DXDecompiler.Chunks.Fx10;


namespace DXDecompiler.Decompiler.IR
{
	public class IrEffect
	{
		internal EffectChunk EffectChunk;
		public IrEffect(EffectChunk effect)
		{
			EffectChunk = effect;
		}
	}
}
