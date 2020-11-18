using System.Collections.Generic;

namespace DXDecompiler.DebugParser.Chunks.Fx10
{
	/// <summary>
	/// Looks vaguely similar to D3D10_EFFECT_DESC and D3D10_STATE_BLOCK_MASK
	/// Some Notes:
	/// Cbuffer has a stride of 24 bytes
	/// Effect Variables have a stride of 28 (32 for sampler variables) bytes without annotations
	/// Effect Variables Types has as stride of 28 bytes
	/// Effect Annotation has a stride of 3 bytes
	/// Sampler Annotations have a stride of 4 bytes
	/// Sampler Annotation types have a stride of 12 bytes?
	/// Effect Techniques with 0 pass 0 shaders have a stride of 12 bytes
	/// Effect Techniques with 1 pass 0 shaders have a stride of 24 bytes
	/// Effect Techniques with 1 pass 2 shaders(1 vs, 1 ps) have a stride of 56 bytes
	/// Effect Pass with 0 shaders have a stride of 12 bytes
	/// Effect shaders have a stride of 12 bytes
	/// 
	/// /// </summary>
	public class DebugEffectChunk : DebugBytecodeChunk
	{
		public byte[] HeaderData;
		public byte[] BodyData;
		public byte[] FooterData;
		uint Size;
		public DebugEffectHeader Header;
		public IEnumerable<DebugEffectBuffer> AllBuffers
		{
			get
			{
				foreach(var buffer in LocalBuffers)
				{
					yield return buffer;
				}
				foreach(var buffer in SharedBuffers)
				{
					yield return buffer;
				}
			}
		}
		public IEnumerable<IDebugEffectVariable> AllVariables
		{
			get
			{
				foreach(var buffer in LocalBuffers)
				{
					foreach(var variable in buffer.Variables)
					{
						yield return variable;
					}
				}
				foreach(var variable in LocalVariables)
				{
					yield return variable;
				}
				foreach(var buffer in SharedBuffers)
				{
					foreach(var variable in buffer.Variables)
					{
						yield return variable;
					}
				}
				foreach(var variable in SharedVariables)
				{
					yield return variable;
				}
			}
		}
		public bool IsChildEffect => SharedBuffers.Count > 0 || SharedVariables.Count > 0;
		public List<DebugEffectBuffer> LocalBuffers { get; private set; }
		public List<DebugEffectBuffer> SharedBuffers { get; private set; }
		public List<DebugEffectObjectVariable> LocalVariables { get; private set; }
		public List<DebugEffectObjectVariable> SharedVariables { get; private set; }
		public List<DebugEffectInterfaceVariable> InterfaceVariables { get; private set; }
		public List<DebugEffectTechnique> Techniques { get; private set; }
		/// <summary>
		/// Only used in fx_5_0
		/// </summary>
		public List<DebugEffectGroup> Groups { get; private set; }
		public DebugEffectChunk()
		{
			LocalBuffers = new List<DebugEffectBuffer>();
			SharedBuffers = new List<DebugEffectBuffer>();
			LocalVariables = new List<DebugEffectObjectVariable>();
			SharedVariables = new List<DebugEffectObjectVariable>();
			InterfaceVariables = new List<DebugEffectInterfaceVariable>();
			Techniques = new List<DebugEffectTechnique>();
			Groups = new List<DebugEffectGroup>();
		}
		public static DebugEffectChunk Parse(DebugBytecodeReader reader, uint size)
		{
			var headerReader = reader.CopyAtCurrentPosition("Header", reader);
			var result = new DebugEffectChunk();
			result.Size = size;
			var header = result.Header = DebugEffectHeader.Parse(headerReader);
			var bodyOffset = header.Version.MajorVersion < 5 ?
				0x4C : 0x60;
			var footerOffset = (int)(result.Header.FooterOffset + bodyOffset);
			var bodyReader = reader.CopyAtOffset("Body", reader, (int)bodyOffset);
			var dummyReader = bodyReader.CopyAtCurrentPosition("DummyReader", bodyReader);
			dummyReader.ReadUInt32("Zero");
			var footerReader = reader.CopyAtOffset("Footer", reader, footerOffset);
			var version = header.Version;
			for(int i = 0; i < header.ConstantBuffers; i++)
			{
				footerReader.AddIndent($"ConstantBuffer {i}");
				result.LocalBuffers.Add(DebugEffectBuffer.Parse(bodyReader, footerReader, version, false));
				footerReader.RemoveIndent();
			}
			for(int i = 0; i < header.ObjectCount; i++)
			{
				footerReader.AddIndent($"Variable {i}");
				result.LocalVariables.Add(DebugEffectObjectVariable.Parse(bodyReader, footerReader, version, false));
				footerReader.RemoveIndent();
			}
			for(int i = 0; i < header.SharedConstantBuffers; i++)
			{
				footerReader.AddIndent($"SharedConstantBuffer {i}");
				result.SharedBuffers.Add(DebugEffectBuffer.Parse(bodyReader, footerReader, version, true));
				footerReader.RemoveIndent();
			}
			for(int i = 0; i < header.SharedObjectCount; i++)
			{
				footerReader.AddIndent($"SharedVariable {i}");
				result.SharedVariables.Add(DebugEffectObjectVariable.Parse(bodyReader, footerReader, version, true));
				footerReader.RemoveIndent();
			}
			if(header.Version.MajorVersion >= 5)
			{
				for(int i = 0; i < header.InterfaceVariableCount; i++)
				{
					footerReader.AddIndent($"Interface {i}");
					result.InterfaceVariables.Add(DebugEffectInterfaceVariable.Parse(bodyReader, footerReader, version));
					footerReader.RemoveIndent();
				}
				for(int i = 0; i < header.GroupCount; i++)
				{
					footerReader.AddIndent($"Group {i}");
					result.Groups.Add(DebugEffectGroup.Parse(bodyReader, footerReader, header.Version));
					footerReader.RemoveIndent();
				}
			}
			else
			{
				for(int i = 0; i < header.Techniques; i++)
				{
					footerReader.AddIndent($"Technique {i}");
					result.Techniques.Add(DebugEffectTechnique.Parse(bodyReader, footerReader, header.Version));
					footerReader.RemoveIndent();
				}
			}

			return result;
		}
	}
}
