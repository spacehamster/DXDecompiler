using DXDecompiler.Chunks;
using DXDecompiler.Chunks.Ifce;
using DXDecompiler.Chunks.Rdef;
using DXDecompiler.Chunks.Shex;
using DXDecompiler.Chunks.Shex.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DXDecompiler.Decompiler.IR
{
	public class IrInterfaceManger
	{
		internal InterfacesChunk Chunk;
		public List<IrClass> Classes = new List<IrClass>();
		public List<IrInterface> Interfaces = new List<IrInterface>();
		public Dictionary<string, IrPass> Passes = new Dictionary<string, IrPass>();
		internal Dictionary<string, List<string>> FunctionTables = new Dictionary<string, List<string>>();
		public IrInterfaceManger(InterfacesChunk interfaces)
		{
			Chunk = interfaces;
		}
		public void Parse(IrShader shader, BytecodeContainer container)
		{
			FunctionTables = new Dictionary<string, List<string>>();
			foreach(var dcl in container.Shader.DeclarationTokens)
			{
				if(dcl is FunctionTableDeclarationToken ft)
				{
					FunctionTables[$"ft{ft.Identifier}"] = ft.FunctionBodyIndices
									.Select(i => $"fb{i}")
									.ToList();
				}
			}
			var passes = shader.Passes
				.Where(p => p.Type == IrPass.PassType.FunctionBody)
				.ToDictionary(p => (p.Instructions.First().Token as InstructionToken).Operands.First().ToString());
			foreach(var kv in passes)
			{
				AddLabelPass(kv.Value);
			}
			foreach(var type in Chunk.AvailableClassTypes)
			{
				var @class = new IrClass();
				@class.Name = type.Name;
				var keys = FunctionTables[$"ft{type.ID}"];
				foreach(var key in keys)
				{
					var pass = passes[key];
					@class.Passes.Add(pass);
				}
				Classes.Add(@class);
			}
			var rdef = container.ResourceDefinition;
			var cbVariables = rdef.ConstantBuffers
				.Where(cb => cb.BufferType == ConstantBufferType.InterfacePointers)
				.Single()
				.Variables;
			foreach(var type in cbVariables)
			{
				var @interface = new IrInterface();
				@interface.Name = GetInterfaceShaderTypeName(type.ShaderType);
			}
		}

		string GetInterfaceShaderTypeName(ShaderType variable)
		{
			if(!string.IsNullOrEmpty(variable.BaseTypeName)) // BaseTypeName is only populated in SM 5.0
			{
				return variable.BaseTypeName;
			}
			else
			{
				return string.Format("{0}{1}",
					variable.VariableClass.GetDescription(),
					variable.VariableType.GetDescription());
			}
		}
		public void AddLabelPass(IrPass pass)
		{
			var inst = pass.Instructions[0].Token as InstructionToken;
			var operand = inst.Operands[0];
			Passes[operand.ToString()] = pass;
		}
		public void AddFunctionDeclaration(FunctionTableDeclarationToken dcl)
		{
			FunctionTables[$"ft{dcl.Identifier}"] = dcl.FunctionBodyIndices
								.Select(i => $"fb{i}")
								.ToList();
		}
	}
}
