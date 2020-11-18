using NUnit.Framework;
using SharpDX.D3DCompiler;
using System.IO;

namespace DXDecompiler.Tests
{
	/// <summary>
	/// Refer https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/using-shader-linking
	/// https://github.com/microsoftarchive/msdn-code-gallery-microsoft/blob/master/Official%20Windows%20Platform%20Sample/HLSL%20shader%20compiler%20sample/%5BC++%5D-HLSL%20shader%20compiler%20sample/C++/DirectXPage.xaml.cpp
	/// </summary>


	[TestFixture]
	public class TestLibraryShader
	{
		private static string OutputDir => $@"{TestContext.CurrentContext.TestDirectory}/TestLibraryShader";
		private static string ShaderDirectory => $"{TestContext.CurrentContext.TestDirectory}/TestLibraryShader";

		static ParameterDescription ParamDesc(string name, string semanticName, ShaderVariableType type, ShaderVariableClass @class,
				int rows, int columns, SharpDX.Direct3D.InterpolationMode interpolationMode, 
				ParameterFlags flags, int firstInRegister, int firstInComponent, 
				int firstOutRegister, int firstOutComponent)
		{
			return new ParameterDescription()
			{ 
				Name = name, 
				SemanticName = semanticName,
				Type = type, 
				Class = @class,
				Rows = rows,
				Columns = columns,
				InterpolationMode = interpolationMode,
				Flags = flags,
				FirstInRegister = firstInRegister,
				FirstInComponent = firstInComponent,
				FirstOutRegister = firstOutRegister,
				FirstOutComponent = firstOutComponent
			};
		}
		[Test]
		public void LinkLibrary()
		{
			var testShader = @"
// This file defines the defaults for the editor.

// This is the default code in the fixed header section.
// @@@ Begin Header
Texture2D<float3> Texture : register(t0);
SamplerState Anisotropic : register(s0);
cbuffer CameraData : register(b0)
{
    float4x4 Model;
    float4x4 View;
    float4x4 Projection;
};
cbuffer TimeVariantSignals : register(b1)
{
    float SineWave;
    float SquareWave;
    float TriangleWave;
    float SawtoothWave;
};
// @@@ End Header

// This is the default code in the source section.
// @@@ Begin Source
export void VertexFunction(
    inout float4 position, inout float2 texcoord, inout float4 normal)
{
    position = mul(position, Model);
    position = mul(position, View);
    position = mul(position, Projection);
    normal = mul(normal, Model);
}

export float3 ColorFunction(float2 texcoord, float3 normal)
{
    return Texture.Sample(Anisotropic, texcoord);
}
// @@@ End Source

// This code is not displayed, but is used as part of the linking process.
// @@@ Begin Hidden
cbuffer HiddenBuffer : register(b2)
{
    float3 LightDirection;
};

export float3 AddLighting(float3 color, float3 normal)
{
    static const float ambient = 0.2f;
    float brightness = ambient + (1.0f - ambient) * saturate(dot(normal, LightDirection));
    return color * brightness;
}

export float3 AddDepthFog(float3 color, float depth)
{
    float3 fogColor = float3(0.4f, 0.9f, 0.5f); // Greenish.
    return lerp(color, fogColor, exp2(-depth));
}

export float3 AddGrayscale(float3 color)
{
    float luminance = 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    return float3(luminance, luminance, luminance);
}

export float4 Float3ToFloat4SetW1(float3 value)
{
    // Convert a float3 value to a float4 value with the w component set to 1.0f.
    // Used for initializing homogeneous 3D coordinates and generating fully opaque color values.
    return float4(value, 1.0f);
}
// @@@ End Hidden
";
			//1. Compiling your shader code
			var compilationResult = ShaderBytecode.Compile(testShader, "lib_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None);
			var bytecode = compilationResult.Bytecode;
			// 2.Load the compiled code into a shader library
			var shaderLibrary = new Module(bytecode);
			//3. Bind resources from source slots to destination slots
			var shaderLibraryInstance = new ModuleInstance("", shaderLibrary);
			shaderLibraryInstance.BindResource(0, 0, 1);
			shaderLibraryInstance.BindSampler(0, 0, 1);
			shaderLibraryInstance.BindConstantBuffer(0, 0, 0);
			shaderLibraryInstance.BindConstantBuffer(1, 1, 0);
			shaderLibraryInstance.BindConstantBuffer(2, 2, 0);

			//1. Construct a function-linking-graph for the vertex shader.
			var vertexShaderGraph = new FunctionLinkingGraph();
			var vertexShaderInputNode = vertexShaderGraph.SetInputSignature(
				ParamDesc("inputPos", "POSITION0", ShaderVariableType.Float, ShaderVariableClass.Vector, 
						1, 3, SharpDX.Direct3D.InterpolationMode.Linear, ParameterFlags.In, 0, 0, 0, 0),
				ParamDesc("inputTex", "TEXCOORD0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 2, SharpDX.Direct3D.InterpolationMode.Linear, ParameterFlags.In, 0, 0, 0, 0),
				ParamDesc("inputNorm", "NORMAL0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 3, SharpDX.Direct3D.InterpolationMode.Linear, ParameterFlags.In, 0, 0, 0, 0));

			var homogenizeCallNodeForPos = vertexShaderGraph.CallFunction("", shaderLibrary, "Float3ToFloat4SetW1");
			var homogenizeCallNodeForNorm = vertexShaderGraph.CallFunction("", shaderLibrary, "Float3ToFloat4SetW1");
			vertexShaderGraph.PassValue(vertexShaderInputNode, 0, homogenizeCallNodeForPos, 0);
			vertexShaderGraph.PassValue(vertexShaderInputNode, 2, homogenizeCallNodeForNorm, 0);

			var vertexFunctionCallNode = vertexShaderGraph.CallFunction("", shaderLibrary, "VertexFunction");
			vertexShaderGraph.PassValue(homogenizeCallNodeForPos, -1, vertexFunctionCallNode, 0);
			vertexShaderGraph.PassValue(vertexShaderInputNode, 1, vertexFunctionCallNode, 1);
			vertexShaderGraph.PassValue(homogenizeCallNodeForNorm, -1, vertexFunctionCallNode, 2);

			var vertexShaderOutputNode = vertexShaderGraph.SetOutputSignature(
				ParamDesc("outputTex", "TEXCOORD0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 2, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.Out, 0, 0, 0, 0),
				ParamDesc("outputNorm", "NORMAL0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 3, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.Out, 0, 0, 0, 0),
				ParamDesc("outputPos", "SV_POSITION", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 4, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.Out, 0, 0, 0, 0));

			vertexShaderGraph.PassValue(vertexFunctionCallNode, 0, vertexShaderOutputNode, 2);
			vertexShaderGraph.PassValue(vertexFunctionCallNode, 1, vertexShaderOutputNode, 0);
			vertexShaderGraph.PassValue(vertexFunctionCallNode, 2, vertexShaderOutputNode, 1);

			var vertexShaderGraphInstance = vertexShaderGraph.CreateModuleInstance();

			// 2. Link the vertex shader
			var vertexLinker = new Linker();
			vertexLinker.UseLibrary(shaderLibraryInstance);
			var vertexShaderBlob = vertexLinker.Link(vertexShaderGraphInstance, "main", "vs_5_0", 0);

			Directory.CreateDirectory($"{OutputDir}");

			File.WriteAllBytes($"{OutputDir}/TestLinkedVertexShader.o", vertexShaderBlob.Data);
			File.WriteAllText($"{OutputDir}/TestLinkedVertexShader.asm", vertexShaderBlob.Disassemble());
			File.WriteAllText($"{OutputDir}/TestLinkedVertexShader.hlsl", vertexShaderGraph.GenerateHlsl(0));
			var vertexContainer = new BytecodeContainer(vertexShaderBlob.Data);
			File.WriteAllText($"{OutputDir}/TestLinkedVertexShader.d.asm", vertexContainer.ToString());

			bool enableLighting = true;
			bool enableDepthFog = true;
			bool enableGreyScale = true;
			//1. Construct a function-linking-graph for the vertex shader.
			var pixelShaderGraph = new FunctionLinkingGraph();
			var pixelShaderInputNode = pixelShaderGraph.SetInputSignature(
				ParamDesc("inputTex", "TEXCOORD0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 2, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.In, 0, 0, 0, 0),
				ParamDesc("inputNorm", "NORMAL0", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 3, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.In, 0, 0, 0, 0),
				ParamDesc("inputPos", "SV_POSITION", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 4, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.In, 0, 0, 0, 0));

			var colorValueNode = pixelShaderGraph.CallFunction("", shaderLibrary, "ColorFunction");
			pixelShaderGraph.PassValue(pixelShaderInputNode, 0, colorValueNode, 0);
			pixelShaderGraph.PassValue(pixelShaderInputNode, 1, colorValueNode, 1);

			if (enableLighting)
			{
				var tempNode = pixelShaderGraph.CallFunction("", shaderLibrary, "AddLighting");
				pixelShaderGraph.PassValue(colorValueNode, -1, tempNode, 0);
				pixelShaderGraph.PassValue(pixelShaderInputNode, 1, tempNode, 1);
				colorValueNode = tempNode;
			}
			if (enableDepthFog)
			{
				var tempNode = pixelShaderGraph.CallFunction("", shaderLibrary, "AddDepthFog");
				pixelShaderGraph.PassValue(colorValueNode, -1, tempNode, 0);
				pixelShaderGraph.PassValueWithSwizzle(pixelShaderInputNode, 2, "z",  tempNode, 1, "x");
				colorValueNode = tempNode;
			}
			if (enableGreyScale)
			{
				var tempNode = pixelShaderGraph.CallFunction("", shaderLibrary, "AddGrayscale");
				pixelShaderGraph.PassValue(colorValueNode, -1, tempNode, 0);
				colorValueNode = tempNode;
			}

			var fillAlphaCallNode = pixelShaderGraph.CallFunction("", shaderLibrary, "Float3ToFloat4SetW1");
			pixelShaderGraph.PassValue(colorValueNode, -1, fillAlphaCallNode, 0);

			var pixelShaderOutputNode = pixelShaderGraph.SetOutputSignature(
				ParamDesc("outputColor", "SV_TARGET", ShaderVariableType.Float, ShaderVariableClass.Vector,
						1, 4, SharpDX.Direct3D.InterpolationMode.Undefined, ParameterFlags.Out, 0, 0, 0, 0));

			pixelShaderGraph.PassValue(fillAlphaCallNode, -1, pixelShaderOutputNode, 0);

			var pixelShaderGraphInstance = pixelShaderGraph.CreateModuleInstance();

			// 2. Link the vertex shader
			var pixelLinker = new Linker();
			pixelLinker.UseLibrary(shaderLibraryInstance);
			var pixelShaderBlob = pixelLinker.Link(pixelShaderGraphInstance, "main", "ps_5_0", 5);

			Directory.CreateDirectory($"{OutputDir}");

			File.WriteAllBytes($"{OutputDir}/TestLinkedPixelShader.o", pixelShaderBlob.Data);
			File.WriteAllText($"{OutputDir}/TestLinkedPixelShader.asm", pixelShaderBlob.Disassemble());
			File.WriteAllText($"{OutputDir}/TestLinkedPixelShader.hlsl", pixelShaderGraph.GenerateHlsl(0));

			var pixelContainer = new BytecodeContainer(pixelShaderBlob.Data);
			File.WriteAllText($"{OutputDir}/TestLinkedPixelShader.d.asm", pixelContainer.ToString());
		}

		[Test]
		public void BuildLibrary()
		{
			var testShader = @"
export float3 TestFunction(float3 input)
{
	return input * 2.0f;
}";
			//1. Compiling your shader code
			var compilationResult = ShaderBytecode.Compile(testShader, "lib_5_0", ShaderFlags.OptimizationLevel3, EffectFlags.None);
			var libraryBytecode = compilationResult.Bytecode;

			Directory.CreateDirectory($"{OutputDir}");

			File.WriteAllBytes($"{OutputDir}/TestLibraryShader.o", libraryBytecode.Data);
			File.WriteAllText($"{OutputDir}/TestLibraryShader.asm", libraryBytecode.Disassemble());

			var container = new BytecodeContainer(libraryBytecode.Data);
			File.WriteAllText($"{OutputDir}/TestLibraryShader.d.asm", container.ToString());
		}
	}
}
