using DXDecompiler.DebugParser.DX9;
using DXDecompiler.DX9Shader.FX9;
using System.Text;

namespace DXDecompiler.DebugParser.FX9
{
	/*
Format:
	Pass.ShaderAttribute.Index=Technique index

	Pass.Unknown2=Index of sampler in variable list if entry is a texture
	Pass.Unknown2=Index of pass if entry is a shader (both inline and variable)
Example
pass P0
{
    VertexShader = compile vs_2_0 VertScene();
    PixelShader  = compile ps_2_0 PixScene();
}
//Snip pass
pass P3
{
    VertexShader = compile vs_2_0 VertScene();
    PixelShader  = compile ps_2_0 PixScene();
}
// InlineShader0: Unk1 00000000 Unk2 00000003 Unk3 FFFFFFFF Index? 1
//                IsVariable: 0 Size 220 Name:  Version: Pixel_2_0
// InlineShader1: Unk1 00000000 Unk2 00000003 Unk3 FFFFFFFF Index? 0
//                IsVariable: 0 Size 896 Name:  Version: Vertex_2_0
// InlineShader6: Unk1 00000000 Unk2 00000000 Unk3 FFFFFFFF Index? 1
//                IsVariable: 0 Size 220 Name:  Version: Pixel_2_0
// InlineShader7: Unk1 00000000 Unk2 00000000 Unk3 FFFFFFFF Index? 0
//                IsVariable: 0 Size 896 Name:  Version: Vertex_2_0
Result:
    pass P0
    {
        VertexShader = 9;
        PixelShader = 10;
    }
	pass P3
    {
        VertexShader = 15;
        PixelShader = 16;
    }
Example:
	texture g_MeshTexture;
	sampler MeshTextureSampler = 
	sampler_state
	{
		Texture = <g_MeshTexture>;
	}
	texture g_MeshTexture;
	sampler MeshTextureSampler = 
	sampler_state
	{
		Texture = <g_MeshTexture>;
	}
// BinaryData0: Index? 3 Size: 0 Note index is assigned to texture variable
// BinaryData1: Index? 1 Size: 0 Note index is assigned to texture variable
// InlineShader2: Unk1 FFFFFFFF Unk2 0000000B Unk3 00000000 Index? 0
//                IsVariable: 1 Size 14 Name: g_MeshTexture Version: 
// InlineShader3: Unk1 FFFFFFFF Unk2 0000000A Unk3 00000000 Index? 0
//                IsVariable: 1 Size 22 Name: g_RenderTargetTexture Version:

Example
VertexShader Test1VS = compile vs_2_0 VertScene();
PixelShader  Test2PS = compile ps_2_0 PixScene();
VertexShader Test3VS = compile vs_2_0 VertScene();
PixelShader  Test4PS = compile ps_2_0 PixScene();
pass P1 {
	VertexShader = Test1VS;
	PixelShader = Test2PS;
}
pass P2 {
	VertexShader = Test3VS;
	PixelShader = Test4PS;
}
// BinaryData0: Index? 8 Size: 220 
// BinaryData1: Index? 7 Size: 896 
// BinaryData2: Index? 6 Size: 220 
// BinaryData3: Index? 5 Size: 896 
// InlineShader0: Unk1 00000000 Unk2 00000002 Unk3 FFFFFFFF Index? 1
//                IsVariable: 1 Size 8 Name: Test4PS Version: 
// InlineShader1: Unk1 00000000 Unk2 00000002 Unk3 FFFFFFFF Index? 0
//                IsVariable: 1 Size 8 Name: Test3VS Version: 
// InlineShader2: Unk1 00000000 Unk2 00000001 Unk3 FFFFFFFF Index? 1
//                IsVariable: 1 Size 8 Name: Test2PS Version: 
// InlineShader3: Unk1 00000000 Unk2 00000001 Unk3 FFFFFFFF Index? 0
//                IsVariable: 1 Size 8 Name: Test1VS Version:
result:
	vertexshader Test1VS = { 5 };
	pixelshader Test2PS = { 6 };
	vertexshader Test3VS = { 7 };
	pixelshader Test4PS = { 8 };
	pass P1
	{
		VertexShader = 11;
		PixelShader = 12;
	}
	pass P2
	{
		VertexShader = 13;
		PixelShader = 14;
	}
//How does the pass entry reference the inline shader entry?
 */
	public class DebugStateBlob
	{
		public uint TechniqueIndex;
		public uint PassIndex;
		public uint SamplerStateIndex;
		public uint AssignmentIndex;
		public StateBlobType BlobType;
		public uint ShaderSize;
		public string VariableName { get; private set; }
		public byte[] Data = new byte[0];
		public DebugShaderModel Shader;
		public static DebugStateBlob Parse(DebugBytecodeReader reader, DebugBytecodeReader blobReader)
		{
			var result = new DebugStateBlob();
			result.TechniqueIndex = blobReader.ReadUInt32("TechniqueIndex");
			result.PassIndex = blobReader.ReadUInt32("PassIndex");
			result.SamplerStateIndex = blobReader.ReadUInt32("SamplerStateIndex");
			result.AssignmentIndex = blobReader.ReadUInt32("AssignmentIndex");
			result.BlobType = blobReader.ReadEnum32< StateBlobType>("BlobType");
			if (result.BlobType == StateBlobType.Shader)
			{
				result.ShaderSize = blobReader.ReadUInt32("BlobSize");
				var startPosition = blobReader._reader.BaseStream.Position;
				var shaderReader = blobReader.CopyAtCurrentPosition("ShaderReader", blobReader);
				result.Shader = DebugShaderModel.Parse(shaderReader);
				blobReader._reader.BaseStream.Position = startPosition + result.ShaderSize;
			}
			if (result.BlobType == StateBlobType.Variable)
			{
				result.VariableName = blobReader.TryReadString("VariableName");
			}
			else if(result.BlobType == StateBlobType.IndexShader)
			{
				result.ShaderSize = blobReader.ReadUInt32("BlobSize");
				var startPosition = blobReader._reader.BaseStream.Position;
				var variableSize = blobReader.ReadUInt32("VariableNameSize");
				var variableData = blobReader.ReadBytes("VariableData", (int)variableSize);
				result.VariableName = Encoding.UTF8.GetString(variableData, 0, variableData.Length - 1);
				var shaderReader = blobReader.CopyAtCurrentPosition("ShaderReader", blobReader);
				result.Shader = DebugShaderModel.Parse(shaderReader);
				blobReader._reader.BaseStream.Position = startPosition + result.ShaderSize;
			}
			return result;
		}
	}
}